using DNR26V2.Data.Context;
using DNR26V2.Domain.Entities.Payments;
using DNR26V2.Domain.Exceptions;
using DNR26V2.Domain.DTOs.Payments;
using DNR26V2.Services.System;
using Microsoft.EntityFrameworkCore;

namespace DNR26V2.Services.Payments;

public class PaymentPostingService : IPaymentPostingService
{
    private const string ZahlungSeriencode = "ZA";

    private readonly AppDbContext           _db;
    private readonly ICustomerLedgerService _customerLedgerService;
    private readonly INoSeriesService       _noSeries;

    private static readonly HashSet<string> _allowedBelegArten =
        new(StringComparer.OrdinalIgnoreCase) { "Rechnung", "Gutschrift", "Lieferschein" };

    public PaymentPostingService(
        AppDbContext           db,
        ICustomerLedgerService customerLedgerService,
        INoSeriesService       noSeries)
    {
        _db                    = db;
        _customerLedgerService = customerLedgerService;
        _noSeries              = noSeries;
    }

    public async Task<int> BuchenAsync(PaymentPostingRequest request)
    {
        // --- Validation Phase ---

        if (request.KundeId <= 0)
            throw new ValidationException("KundeId ist ungültig.");

        if (!request.Rows.Any())
            throw new ValidationException("Es wurden keine Zeilen übergeben.");

        // Customer consistency: all rows must match the header KundeId.
        // In phase 1 this is implicit (UI sends one customer at a time),
        // but we guard against future multi-row multi-customer calls.
        // Stronger revalidation point: load open amounts from CustomerLedgerService
        // per ReferenceId and compare — not done in Phase 1 to avoid N+1 queries.

        var activeRows = new List<PaymentPostingRowItem>();

        foreach (var row in request.Rows)
        {
            // Rule 1 — Negative values forbidden
            if (row.Bar < 0)
                throw new ValidationException(
                    $"Bar-Betrag darf nicht negativ sein (ReferenceId={row.ReferenceId}).");

            if (row.Bank < 0)
                throw new ValidationException(
                    $"Bank-Betrag darf nicht negativ sein (ReferenceId={row.ReferenceId}).");

            // Rule 2 — Empty rows ignored
            if (row.Bar == 0 && row.Bank == 0)
                continue;

            // Rule 5 — Invalid ReferenceId
            if (row.ReferenceId <= 0)
                throw new ValidationException(
                    $"Ungültige ReferenceId ({row.ReferenceId}).");

            // Rule 5 — Unsupported BelegArt
            if (!_allowedBelegArten.Contains(row.BelegArt))
                throw new ValidationException(
                    $"Nicht unterstützte BelegArt '{row.BelegArt}' (ReferenceId={row.ReferenceId}).");

            // Rule 5 — Closed / zero-open entries
            if (row.OffenerBetrag <= 0)
                throw new ValidationException(
                    $"Kein offener Betrag vorhanden (ReferenceId={row.ReferenceId}, BelegArt={row.BelegArt}).");

            // Rule 3 — Payment must not exceed open amount
            var total = row.Bar + row.Bank;
            if (total > row.OffenerBetrag)
                throw new ValidationException(
                    $"Zahlung ({total:F2}) überschreitet den offenen Betrag ({row.OffenerBetrag:F2}) " +
                    $"für ReferenceId={row.ReferenceId} ({row.BelegArt}).");

            activeRows.Add(row);
        }

        if (!activeRows.Any())
            throw new ValidationException("Alle Zeilen sind leer (Bar = 0, Bank = 0).");

        // --- Posting Phase ---

        var zahlungsnummer = await _noSeries.GetNextNumberAsync(
            ZahlungSeriencode, request.Buchungsdatum);

        var header = new PaymentHeader
        {
            Zahlungsnummer = zahlungsnummer,
            KundeId        = request.KundeId,
            Buchungsdatum  = request.Buchungsdatum,
            Notiz          = request.Notiz,
            ErstelltVon    = Environment.UserName,
            ErstelltAm     = DateTime.Now
        };

        _db.PaymentHeaders.Add(header);

        foreach (var row in activeRows)
        {
            // Map BelegArt string to ReferenceType int (matches PaymentLine.ReferenceType convention)
            var refType = row.ReferenceType;

            if (row.ReferenceType != 0 && row.ReferenceType != 1)
                throw new ValidationException(
                    $"Ungültiger ReferenceType ({row.ReferenceType}) für ReferenceId={row.ReferenceId}.");

            if (row.Bar > 0)
            {
                header.Lines.Add(new PaymentLine
                {
                    ReferenceType = refType,
                    ReferenceId = row.ReferenceId,
                    PaymentMethod = PaymentMethod.Bar,
                    Amount = row.Bar,
                    Notiz = row.Notiz
                });
            }

            if (row.Bank > 0)
            {
                header.Lines.Add(new PaymentLine
                {
                    ReferenceType = refType,
                    ReferenceId = row.ReferenceId,
                    PaymentMethod = PaymentMethod.Bank,
                    Amount = row.Bank,
                    Notiz = row.Notiz
                });
            }
        }

        await _db.SaveChangesAsync();

        return header.Id;
    }

    /// <summary>
    /// Maps BelegArt string to the ReferenceType integer stored on PaymentLine.
    /// 0 = InvoiceLine (Rechnung/Gutschrift), 1 = DeliveryLine (Lieferschein).
    /// </summary>
    private static int ResolveReferenceType(string belegArt) =>
        belegArt.Equals("Lieferschein", StringComparison.OrdinalIgnoreCase) ? 1 : 0;

    // ?? Storno ???????????????????????????????????????????????????????????????

    public async Task<int> StornierenAsync(int paymentLineId, string stornoNotiz)
    {
        var originalLine = await _db.PaymentLines
            .Include(pl => pl.Header)
            .FirstOrDefaultAsync(pl => pl.Id == paymentLineId)
            ?? throw new ValidationException(
                $"Zahlungszeile {paymentLineId} nicht gefunden.");

        // Guard: already a storno line (negative) cannot be storniert again
        if (originalLine.Amount < 0)
            throw new ValidationException(
                "Storno-Zeilen können nicht erneut storniert werden.");

        // Guard: already has a counter-line
        bool alreadyStorniert = await _db.PaymentLines
            .AnyAsync(pl => pl.ReferenceId    == originalLine.Id
                         && pl.ReferenceType  == -1);   // -1 = storno back-reference
        if (alreadyStorniert)
            throw new ValidationException(
                "Diese Zahlungszeile wurde bereits storniert.");

        var stornoNummer = await _noSeries.GetNextNumberAsync(
            ZahlungSeriencode, DateTime.Today);

        var stornoHeader = new PaymentHeader
        {
            Zahlungsnummer = stornoNummer,
            KundeId        = originalLine.Header.KundeId,
            Buchungsdatum  = DateTime.Today,
            Notiz          = $"STORNO: {originalLine.Header.Zahlungsnummer} — {stornoNotiz}",
            ErstelltVon    = Environment.UserName,
            ErstelltAm     = DateTime.Now
        };

        stornoHeader.Lines.Add(new PaymentLine
        {
            ReferenceType = -1,                    // -1 = storno back-reference
            ReferenceId   = originalLine.Id,       // points to the original PaymentLine
            PaymentMethod = originalLine.PaymentMethod,
            Amount        = -originalLine.Amount,  // negative = reversal
            Notiz         = $"STORNO: {originalLine.Header.Zahlungsnummer} — {stornoNotiz}"
        });

        _db.PaymentHeaders.Add(stornoHeader);
        await _db.SaveChangesAsync();

        return stornoHeader.Id;
    }
}
