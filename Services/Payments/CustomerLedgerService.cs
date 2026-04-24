using DNR26V2.Data.Context;
using DNR26V2.Domain.DTOs;
using DNR26V2.Domain.DTOs.Payments;
using DNR26V2.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DNR26V2.Services.Payments;

public class CustomerLedgerService : ICustomerLedgerService
{
    private readonly AppDbContext _db;

    public CustomerLedgerService(AppDbContext db) { _db = db; }

    public async Task<IReadOnlyList<CustomerLedgerEntryDto>> GetLedgerAsync(int kundeId, DateTime von, DateTime bis)
    {
        // ── 1. Invoice / Gutschrift entries — ONE ROW PER INVOICEHEADER ──────────
        var invoiceRaw = await _db.Invoices
            .Where(i => i.KundeId == kundeId
                     && i.Rechnungsdatum >= von
                     && i.Rechnungsdatum <= bis)
            .Select(i => new
            {
                Datum      = i.Rechnungsdatum,
                KundeId    = i.KundeId,
                KundeName  = i.Kunde.Kundenname,
                BelegArt   = i.BelegArt == InvoiceDocumentType.Gutschrift ? "Gutschrift" : "Rechnung",
                BelegNr    = i.Rechnungsnummer,
                ReferenzId = i.Id,              // InvoiceHeader.Id — 1 row per Beleg
                Betrag     = i.Gesamtbrutto,    // header total — no line explosion
                Richtung   = i.BelegArt == InvoiceDocumentType.Gutschrift ? "-" : "+"
            })
            .ToListAsync();

        var invoices = invoiceRaw.Select(x => new CustomerLedgerEntryDto
        {
            Datum         = x.Datum,
            KundeId       = x.KundeId,
            KundeName     = x.KundeName,
            BelegArt      = x.BelegArt,
            BelegNr       = x.BelegNr,
            ReferenzId    = x.ReferenzId,
            Betrag        = x.Betrag,
            Richtung      = x.Richtung,
            OffenerBetrag = x.Betrag   // recalculated in step 4
        }).ToList();

        // ── 2. Delivery entries — ONE ROW PER DELIVERYHEADER ─────────────────────
        var customer    = await _db.Customer.FindAsync(kundeId);
        var deliveries  = new List<CustomerLedgerEntryDto>();

        if (customer is not null && customer.ReceivableSource == ReceivableSource.Delivery)
        {
            deliveries = await _db.DeliveryHeader
                .Where(d => d.KundeId == kundeId
                         && d.LieferDatum >= von
                         && d.LieferDatum <= bis)
                .Select(d => new CustomerLedgerEntryDto
                {
                    Datum         = d.LieferDatum,
                    KundeId       = d.KundeId,
                    KundeName     = d.Kunde.Kundenname,
                    BelegArt      = "Lieferschein",
                    BelegNr       = d.Lieferscheinnummer,
                    ReferenzId    = d.Id,           // DeliveryHeader.Id — 1 row per Beleg
                    Betrag        = d.Gesamtbrutto, // header total
                    Richtung      = "+",
                    OffenerBetrag = d.Gesamtbrutto  // recalculated in step 4
                })
                .ToListAsync();

            // For Delivery-source customers the delivery IS the receivable.
            // Exclude Rechnung rows entirely to avoid double-counting.
            // Gutschrift rows are kept as informational entries (OffenerBetrag will be 0).
            invoices = invoices.Where(i => i.BelegArt != "Rechnung").ToList();
        }

        // ── 3. Paid amounts — keyed by InvoiceHeader.Id or DeliveryHeader.Id ─────
        //    ReferenceType 0 = InvoiceHeader.Id, 1 = DeliveryHeader.Id
        //    Storno-Zeilen (ReferenceType = -1) müssen auf die originale Zahlungszeile
        //    zurückgemappt werden, damit Saldo/Offen korrekt aktualisiert wird.
        var paidInvoiceBase = await _db.PaymentLines
            .Where(pl => pl.Header.KundeId == kundeId && pl.ReferenceType == 0)
            .GroupBy(pl => pl.ReferenceId)
            .Select(g => new { ReferenceId = g.Key, Paid = g.Sum(pl => pl.Amount) })
            .ToListAsync();

        var paidDeliveryBase = await _db.PaymentLines
            .Where(pl => pl.Header.KundeId == kundeId && pl.ReferenceType == 1)
            .GroupBy(pl => pl.ReferenceId)
            .Select(g => new { ReferenceId = g.Key, Paid = g.Sum(pl => pl.Amount) })
            .ToListAsync();

        var stornoInvoiceAdjustments = await _db.PaymentLines
            .Where(st => st.Header.KundeId == kundeId && st.ReferenceType == -1)
            .Join(
                _db.PaymentLines,
                st => st.ReferenceId,   // points to original PaymentLine.Id
                org => org.Id,
                (st, org) => new
                {
                    org.ReferenceType,
                    org.ReferenceId,
                    StornoAmount = st.Amount
                })
            .Where(x => x.ReferenceType == 0)
            .GroupBy(x => x.ReferenceId)
            .Select(g => new { ReferenceId = g.Key, Paid = g.Sum(x => x.StornoAmount) })
            .ToListAsync();

        var stornoDeliveryAdjustments = await _db.PaymentLines
            .Where(st => st.Header.KundeId == kundeId && st.ReferenceType == -1)
            .Join(
                _db.PaymentLines,
                st => st.ReferenceId,   // points to original PaymentLine.Id
                org => org.Id,
                (st, org) => new
                {
                    org.ReferenceType,
                    org.ReferenceId,
                    StornoAmount = st.Amount
                })
            .Where(x => x.ReferenceType == 1)
            .GroupBy(x => x.ReferenceId)
            .Select(g => new { ReferenceId = g.Key, Paid = g.Sum(x => x.StornoAmount) })
            .ToListAsync();

        var paidInvoiceMap = paidInvoiceBase.ToDictionary(x => x.ReferenceId, x => x.Paid);
        var paidDeliveryMap = paidDeliveryBase.ToDictionary(x => x.ReferenceId, x => x.Paid);

        foreach (var a in stornoInvoiceAdjustments)
            paidInvoiceMap[a.ReferenceId] = paidInvoiceMap.GetValueOrDefault(a.ReferenceId, 0m) + a.Paid;

        foreach (var a in stornoDeliveryAdjustments)
            paidDeliveryMap[a.ReferenceId] = paidDeliveryMap.GetValueOrDefault(a.ReferenceId, 0m) + a.Paid;

        // ── 4. Apply OffenerBetrag ────────────────────────────────────────────────
        foreach (var entry in invoices)
        {
            // Gutschrift = credit to customer — not a payable receivable.
            if (entry.BelegArt == "Gutschrift") { entry.OffenerBetrag = 0m; continue; }

            var paid = paidInvoiceMap.GetValueOrDefault(entry.ReferenzId, 0m);
            entry.OffenerBetrag = Math.Max(0m, entry.Betrag - paid);
        }

        foreach (var entry in deliveries)
        {
            var paid = paidDeliveryMap.GetValueOrDefault(entry.ReferenzId, 0m);
            entry.OffenerBetrag = Math.Max(0m, entry.Betrag - paid);
        }

        // ── 5. Merge and sort ─────────────────────────────────────────────────────
        return invoices.Concat(deliveries)
            .OrderBy(e => e.Datum)
            .ThenBy(e => e.BelegNr)
            .ToList();
    }

    // ── Saldo ─────────────────────────────────────────────────────────────────

    public async Task<decimal> GetSaldoAsync(int kundeId, DateTime von, DateTime bis)
    {
        var entries = await GetLedgerAsync(kundeId, von, bis);

        // Rechnung / Lieferschein = + (receivable)
        // Gutschrift               = - (credit)
        // OffenerBetrag already has payments subtracted — use it directly.
        decimal saldo = entries.Sum(e =>
            e.BelegArt == "Gutschrift"
                ? -e.Betrag          // Gutschrift reduces balance
                : e.OffenerBetrag);  // open receivable (after payments)

        return saldo;
    }

    // ── Payment history ───────────────────────────────────────────────────────

    public async Task<IReadOnlyList<PaymentHistoryDto>> GetPaymentHistoryAsync(
        int kundeId, DateTime von, DateTime bis)
    {
        var lines = await _db.PaymentLines
            .Where(pl => pl.Header.KundeId == kundeId
                      && pl.Header.Buchungsdatum >= von
                      && pl.Header.Buchungsdatum <= bis)
            .Select(pl => new
            {
                pl.Id,
                pl.PaymentHeaderId,
                pl.Header.Zahlungsnummer,
                pl.Header.Buchungsdatum,
                pl.ReferenceType,
                pl.ReferenceId,
                pl.PaymentMethod,
                pl.Amount,
                pl.Notiz,
                pl.Header.ErstelltVon,
                InvoiceBelegNr  = pl.ReferenceType == 0
                    ? _db.Invoices
                        .Where(i => i.Id == pl.ReferenceId)
                        .Select(i => i.Rechnungsnummer)
                        .FirstOrDefault()
                    : null,
                DeliveryBelegNr = pl.ReferenceType == 1
                    ? _db.DeliveryHeader
                        .Where(d => d.Id == pl.ReferenceId)
                        .Select(d => d.Lieferscheinnummer)
                        .FirstOrDefault()
                    : null
            })
            .OrderBy(x => x.Buchungsdatum)
            .ThenBy(x => x.PaymentHeaderId)
            .ToListAsync();

        return lines.Select(x => new PaymentHistoryDto
        {
            PaymentLineId   = x.Id,
            PaymentHeaderId = x.PaymentHeaderId,
            Zahlungsnummer  = x.Zahlungsnummer,
            Buchungsdatum   = x.Buchungsdatum,
            ReferenceType   = x.ReferenceType,
            ReferenceId     = x.ReferenceId,
            BelegNr         = x.InvoiceBelegNr ?? x.DeliveryBelegNr
                              ?? (x.ReferenceType == -1 ? $"STORNO#{x.ReferenceId}" : $"Ref#{x.ReferenceId}"),
            Zahlungsart     = x.PaymentMethod == Domain.Entities.Payments.PaymentMethod.Bar
                              ? "Bar" : "Bank",
            Amount          = x.Amount,
            IsStorno        = x.Amount < 0,
            Notiz           = x.Notiz,
            ErstelltVon     = x.ErstelltVon
        }).ToList();
    }
}
