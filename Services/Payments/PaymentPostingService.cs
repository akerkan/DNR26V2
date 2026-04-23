using DNR26V2.Data.Context;
using DNR26V2.Domain.DTOs.Payments;
using DNR26V2.Domain.Entities.Payments;
using DNR26V2.Domain.Exceptions;

namespace DNR26V2.Services.Payments;

public class PaymentPostingService : IPaymentPostingService
{
    private readonly AppDbContext _db;

    public PaymentPostingService(AppDbContext db)
    {
        _db = db;
    }

    /// <inheritdoc/>
    public async Task<PaymentHeader> PostAsync(PaymentPostingRequest request)
    {
        // ?? Validate request ?????????????????????????????????????????????????

        if (request.KundeId <= 0)
            throw new ValidationException("Kein Kunde ausgewählt.");

        if (request.Buchungsdatum == default)
            throw new ValidationException("Buchungsdatum fehlt.");

        // Filter to active rows only (skip Bar=0 / Bank=0 rows silently)
        var activeRows = request.Rows
            .Where(r => r.Bar > 0 || r.Bank > 0)
            .ToList();

        if (activeRows.Count == 0)
            throw new ValidationException("Keine Zahlungsbeträge eingegeben.");

        foreach (var row in activeRows)
        {
            // Rule 1 — no negative amounts
            if (row.Bar < 0)
                throw new ValidationException(
                    $"Negativer Bar-Betrag nicht erlaubt (ReferenzId {row.ReferenceId}).");

            if (row.Bank < 0)
                throw new ValidationException(
                    $"Negativer Bank-Betrag nicht erlaubt (ReferenzId {row.ReferenceId}).");

            // Rule 5 — valid source reference
            if (row.ReferenceId <= 0)
                throw new ValidationException(
                    $"Ungültige ReferenzId: {row.ReferenceId}.");

            if (row.OffenerBetrag <= 0)
                throw new ValidationException(
                    $"Position {row.ReferenceId} hat keinen offenen Betrag — Zahlung nicht möglich.");

            // Rule 3 — payment must not exceed open amount
            // Phase 1: validates against UI-supplied OffenerBetrag.
            // Future improvement: re-query CustomerLedgerService.GetLedgerAsync and compute
            // true remaining open = Betrag - SUM(existing PaymentLines.Amount for same ReferenceId)
            // to guard against stale UI data or concurrent postings.
            var total = row.Bar + row.Bank;
            if (total > row.OffenerBetrag)
                throw new ValidationException(
                    $"Zahlung {total:N2} € überschreitet den offenen Betrag von {row.OffenerBetrag:N2} € " +
                    $"(ReferenzId {row.ReferenceId}).");
        }

        // ?? Create PaymentHeader ??????????????????????????????????????????????

        var header = new PaymentHeader
        {
            KundeId       = request.KundeId,
            Buchungsdatum = request.Buchungsdatum,
            Notiz         = request.Notiz,
            ErstelltVon   = Environment.UserName,
            ErstelltAm    = DateTime.Now,
        };

        // ?? Create PaymentLines (row-based direct allocation) ?????????????????
        //
        // Phase-1 strategy: each UI row maps directly to the referenced document line.
        // One PaymentLine per payment method per row.
        // ReferenceId + ReferenceType are stored on every line so that future
        // reconciliation can group payments by source document without schema changes.

        foreach (var row in activeRows)
        {
            if (row.Bar > 0)
            {
                header.Lines.Add(new PaymentLine
                {
                    ReferenceId   = row.ReferenceId,
                    ReferenceType = row.ReferenceType,
                    PaymentMethod = PaymentMethod.Bar,
                    Amount        = row.Bar,
                    Notiz         = row.Notiz,
                });
            }

            if (row.Bank > 0)
            {
                header.Lines.Add(new PaymentLine
                {
                    ReferenceId   = row.ReferenceId,
                    ReferenceType = row.ReferenceType,
                    PaymentMethod = PaymentMethod.Bank,
                    Amount        = row.Bank,
                    Notiz         = row.Notiz,
                });
            }
        }

        _db.PaymentHeaders.Add(header);
        await _db.SaveChangesAsync();

        return header;
    }
}
