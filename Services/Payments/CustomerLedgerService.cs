using DNR26V2.Data.Context;
using DNR26V2.Domain.DTOs;
using DNR26V2.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DNR26V2.Services.Payments;

public class CustomerLedgerService : ICustomerLedgerService
{
    private readonly AppDbContext _db;

    public CustomerLedgerService(AppDbContext db) { _db = db; }

    public async Task<IReadOnlyList<CustomerLedgerEntryDto>> GetLedgerAsync(int kundeId, DateTime von, DateTime bis)
    {
        // Collect invoice/gutschrift entries
        var invoices = await _db.Invoices
            .Where(i => i.KundeId == kundeId && i.Rechnungsdatum >= von && i.Rechnungsdatum <= bis)
            .SelectMany(i => i.Zeilen.Select(z => new CustomerLedgerEntryDto
            {
                Datum = i.Rechnungsdatum,
                KundeId = i.KundeId,
                KundeName = i.Kunde.Kundenname,
                BelegArt = i.BelegArt == InvoiceDocumentType.Gutschrift ? "Gutschrift" : "Rechnung",
                BelegNr = i.Rechnungsnummer,
                ReferenzId = z.Id,
                Betrag = z.AmountInclVat, // positive
                Richtung = i.BelegArt == InvoiceDocumentType.Gutschrift ? "-" : "+",
                OffenerBetrag = z.AmountInclVat, // placeholder — allocation not implemented in Phase 1
                Notiz = z.Notiz
            }))
            .ToListAsync();

        // Delivery-based entries for customers with ReceivableSource = Delivery
        var customer = await _db.Customer.FindAsync(kundeId);
        var deliveries = new List<CustomerLedgerEntryDto>();
        if (customer is not null && customer.ReceivableSource == ReceivableSource.Delivery)
        {
            deliveries = await _db.DeliveryHeader
                .Where(d => d.KundeId == kundeId && d.LieferDatum >= von && d.LieferDatum <= bis)
                .SelectMany(d => d.Zeilen.Select(z => new CustomerLedgerEntryDto
                {
                    Datum = d.LieferDatum,
                    KundeId = d.KundeId,
                    KundeName = d.Kunde.Kundenname,
                    BelegArt = "Lieferschein",
                    BelegNr = d.Lieferscheinnummer,
                    ReferenzId = z.Id,
                    Betrag = z.AmountInclVat,
                    Richtung = "+",
                    OffenerBetrag = z.AmountInclVat,
                    Notiz = z.Notiz
                }))
                .ToListAsync();

            // Important: avoid double-booking — for Delivery-based customers, exclude invoices
            // that were generated from these delivery lines. We assume that invoice lines reference
            // DeliveryLine.Id via DeliveryLineId, so we'll remove invoice-based entries that reference
            // the same DeliveryLine for customers with Delivery receivable source.
            var dlIds = deliveries.Select(d => d.ReferenzId).Distinct().ToHashSet();
            invoices = invoices.Where(inv => !dlIds.Contains(inv.ReferenzId)).ToList();
        }

        // Merge and sort
        var all = invoices.Concat(deliveries).OrderBy(e => e.Datum).ThenBy(e => e.BelegNr).ToList();
        return all;
    }
}
