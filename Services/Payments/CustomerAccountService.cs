using DNR26V2.Data.Context;
using DNR26V2.Domain.DTOs.Payments;
using DNR26V2.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DNR26V2.Services.Payments;

public class CustomerAccountService : ICustomerAccountService
{
    private readonly AppDbContext _db;

    public CustomerAccountService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<CustomerAccountEntryDto>> GetEntriesAsync(int kundeId, DateTime von, DateTime bis)
    {
        var customer = await _db.Customer
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == kundeId);

        if (customer is null)
            return [];

        var rows = new List<RawEntry>();

        if (customer.ReceivableSource == ReceivableSource.Delivery)
        {
            var deliveries = await _db.DeliveryHeader
                .AsNoTracking()
                .Where(d => d.KundeId == kundeId
                         && d.LieferDatum >= von
                         && d.LieferDatum <= bis)
                .Select(d => new RawEntry
                {
                    Datum = d.LieferDatum,
                    Belegart = "Lieferschein",
                    BelegNr = d.Lieferscheinnummer,
                    Beschreibung = $"Lieferschein {d.Lieferscheinnummer}",
                    Soll = d.Gesamtbrutto,
                    Haben = 0m,
                    Notiz = d.Notiz,
                    SortReferenz = d.Id
                })
                .ToListAsync();

            rows.AddRange(deliveries);
        }
        else
        {
            var invoices = await _db.Invoices
                .AsNoTracking()
                .Where(i => i.KundeId == kundeId
                         && i.Rechnungsdatum >= von
                         && i.Rechnungsdatum <= bis
                         && i.BelegArt == InvoiceDocumentType.Rechnung)
                .Select(i => new RawEntry
                {
                    Datum = i.Rechnungsdatum,
                    Belegart = "Rechnung",
                    BelegNr = i.Rechnungsnummer,
                    Beschreibung = $"Rechnung {i.Rechnungsnummer}",
                    Soll = i.Gesamtbrutto,
                    Haben = 0m,
                    Notiz = i.Notiz,
                    SortReferenz = i.Id
                })
                .ToListAsync();

            rows.AddRange(invoices);
        }

        var credits = await _db.Invoices
            .AsNoTracking()
            .Where(i => i.KundeId == kundeId
                     && i.Rechnungsdatum >= von
                     && i.Rechnungsdatum <= bis
                     && i.BelegArt == InvoiceDocumentType.Gutschrift)
            .Select(i => new RawEntry
            {
                Datum = i.Rechnungsdatum,
                Belegart = "Gutschrift",
                BelegNr = i.Rechnungsnummer,
                Beschreibung = $"Gutschrift {i.Rechnungsnummer}",
                Soll = 0m,
                Haben = i.Gesamtbrutto,
                Notiz = i.Notiz,
                SortReferenz = i.Id
            })
            .ToListAsync();

        rows.AddRange(credits);

        var payments = await _db.PaymentLines
            .AsNoTracking()
            .Where(pl => pl.Header.KundeId == kundeId
                      && pl.Header.Buchungsdatum >= von
                      && pl.Header.Buchungsdatum <= bis)
            .Select(pl => new
            {
                pl.Id,
                pl.Amount,
                pl.PaymentMethod,
                pl.Notiz,
                HeaderId = pl.PaymentHeaderId,
                pl.Header.Buchungsdatum,
                pl.Header.Zahlungsnummer,
                HeaderNotiz = pl.Header.Notiz
            })
            .ToListAsync();

        foreach (var payment in payments)
        {
            var zahlungsnummer = string.IsNullOrWhiteSpace(payment.Zahlungsnummer)
                ? $"ZA-{payment.HeaderId}"
                : payment.Zahlungsnummer;

            var isStorno = payment.Amount < 0m;
            var amountAbs = Math.Abs(payment.Amount);

            rows.Add(new RawEntry
            {
                Datum = payment.Buchungsdatum,
                Belegart = isStorno ? "Zahlungsstorno" : "Zahlung",
                BelegNr = zahlungsnummer,
                Beschreibung = $"{(isStorno ? "Zahlungsstorno" : "Zahlung")} ({payment.PaymentMethod})",
                Soll = isStorno ? amountAbs : 0m,
                Haben = isStorno ? 0m : amountAbs,
                Notiz = CombineNotes(payment.HeaderNotiz, payment.Notiz),
                SortReferenz = payment.Id
            });
        }

        var ordered = rows
            .OrderBy(x => x.Datum)
            .ThenBy(x => x.BelegNr)
            .ThenBy(x => x.Belegart)
            .ThenBy(x => x.SortReferenz)
            .ToList();

        decimal saldo = 0m;
        var result = new List<CustomerAccountEntryDto>(ordered.Count);

        foreach (var item in ordered)
        {
            saldo += item.Soll - item.Haben;
            result.Add(new CustomerAccountEntryDto
            {
                Datum = item.Datum,
                Belegart = item.Belegart,
                BelegNr = item.BelegNr,
                Beschreibung = item.Beschreibung,
                Soll = item.Soll,
                Haben = item.Haben,
                Saldo = saldo,
                Notiz = item.Notiz,
                SortReferenz = item.SortReferenz
            });
        }

        return result;
    }

    private static string? CombineNotes(string? headerNote, string? lineNote)
    {
        if (string.IsNullOrWhiteSpace(headerNote) && string.IsNullOrWhiteSpace(lineNote))
            return null;

        if (string.IsNullOrWhiteSpace(headerNote))
            return lineNote;

        if (string.IsNullOrWhiteSpace(lineNote))
            return headerNote;

        return $"{headerNote} | {lineNote}";
    }

    private sealed class RawEntry
    {
        public DateTime Datum { get; set; }
        public string Belegart { get; set; } = string.Empty;
        public string BelegNr { get; set; } = string.Empty;
        public string Beschreibung { get; set; } = string.Empty;
        public decimal Soll { get; set; }
        public decimal Haben { get; set; }
        public string? Notiz { get; set; }
        public int SortReferenz { get; set; }
    }
}
