using DNR26V2.Data.Context;
using DNR26V2.Domain.DTOs.Reports;
using DNR26V2.Domain.Entities.Invoices;
using DNR26V2.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DNR26V2.Services.Reports;

public class InvoiceReportDataService : IInvoiceReportDataService
{
    private readonly AppDbContext _db;

    public InvoiceReportDataService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<InvoiceReportData> GetInvoiceReportDataAsync(int invoiceId)
    {
        var invoice = await _db.Invoices
            .Include(i => i.Kunde)
            .Include(i => i.Zeilen)
            .FirstOrDefaultAsync(i => i.Id == invoiceId)
            ?? throw new InvalidOperationException($"Rechnung {invoiceId} nicht gefunden.");

        var setup = await _db.AppSetup.FirstOrDefaultAsync()
            ?? throw new InvalidOperationException("AppSetup nicht gefunden.");

        var header = BuildHeader(invoice, setup);
        var lines  = await BuildLinesAsync(invoice);

        return new InvoiceReportData
        {
            Header = header,
            Lines = lines,
            SuggestedFileName = $"{header.BelegArtText}_{header.Rechnungsnummer}.pdf"
        };
    }

    public async Task<IReadOnlyList<InvoiceReportData>> GetInvoiceReportDataAsync(IEnumerable<int> invoiceIds)
    {
        var result = new List<InvoiceReportData>();
        foreach (var invoiceId in invoiceIds.Distinct())
            result.Add(await GetInvoiceReportDataAsync(invoiceId));
        return result;
    }

    private InvoicePrintHeaderDto BuildHeader(InvoiceHeader invoice, Domain.Entities.System.AppSetup setup)
    {
        var kunde = invoice.Kunde;
        var rechnungsadresseText = string.Join(Environment.NewLine,
            new[]
            {
                kunde.Kundenname,
                kunde.Adresse,
                $"{kunde.PLZ} {kunde.Ort}".Trim(),
                kunde.Land
            }.Where(x => !string.IsNullOrWhiteSpace(x)));

        var lieferadresseText = string.Join(Environment.NewLine,
            new[]
            {
                kunde.ALName2,
                kunde.ALAdresse,
                $"{kunde.ALPLZ} {kunde.ALOrt}".Trim(),
                kunde.ALLand
            }.Where(x => !string.IsNullOrWhiteSpace(x)));

        return new InvoicePrintHeaderDto
        {
            BelegArtText = invoice.BelegArt == InvoiceDocumentType.Gutschrift ? "Gutschrift" : "Rechnung",
            Rechnungsnummer = invoice.Rechnungsnummer,
            Rechnungsdatum = invoice.Rechnungsdatum,
            ZeitraumVon = invoice.Von,
            ZeitraumBis = invoice.Bis,
            KundeName = kunde.Kundenname,
            KundenNr = kunde.Kundennummer,
            KundeAdresse = kunde.Adresse,
            KundePLZ = kunde.PLZ,
            KundeOrt = kunde.Ort,
            KundeLand = kunde.Land,
            RechnungsadresseText = rechnungsadresseText,
            HasAbweichendeLieferadresse = kunde.AbweichendeLieferadresse,
            LieferName = kunde.ALName2,
            LieferAdresse = kunde.ALAdresse,
            LieferPLZ = kunde.ALPLZ,
            LieferOrt = kunde.ALOrt,
            LieferLand = kunde.ALLand,
            LieferadresseText = lieferadresseText,
            Firmenname = setup.Firmenname,
            FirmaAdresse = setup.Firmenadresse,
            FirmaPLZ = setup.FirmenPLZ,
            FirmaOrt = setup.FirmenOrt,
            FirmaLand = setup.FirmenLand,
            FirmaTelefon = setup.FirmenTelefon,
            FirmaEmail = setup.FirmenEmail,
            FirmaSteuernummer = setup.FirmenSteuernummer,
            FirmaUstIdNr = setup.FirmenUStIdNr,
            FirmaBankName = setup.BankName,
            FirmaIBAN = setup.IBAN,
            FirmaBIC = setup.BIC,
            LogoVerwenden = setup.LogoVerwenden,
            LogoPfad = setup.LogoPfad,
            BriefpapierVerwenden = setup.BriefpapierVerwenden,
            Gesamtnetto = invoice.Gesamtnetto,
            Gesamtmwst = invoice.Gesamtmwst,
            Gesamtbrutto = invoice.Gesamtbrutto,
            MwstText = invoice.Zeilen.Select(z => z.MwstProzent).Distinct().Count() == 1
                ? $"{invoice.Zeilen.First().MwstProzent:N2} % MwSt."
                : "MwSt.",
            ZahlungszielTage = setup.ZahlungszielTage,
            SkontoProzent = setup.SkontoProzent,
            SkontoTage = setup.SkontoTage
        };
    }

    private async Task<IReadOnlyList<InvoicePrintLineDto>> BuildLinesAsync(InvoiceHeader invoice)
    {
        var result = new List<InvoicePrintLineDto>();

        foreach (var line in invoice.Zeilen.OrderBy(z => z.Id))
        {
            var artikel = await _db.Product.FindAsync(line.ArtikelId);
            var delivery = await _db.DeliveryLine
                .Include(d => d.Lieferschein)
                .FirstOrDefaultAsync(d => d.Id == line.DeliveryLineId);

            result.Add(new InvoicePrintLineDto
            {
                Artikelnummer = artikel?.Artikelnummer ?? string.Empty,
                Bezeichnung = artikel?.Bezeichnung ?? string.Empty,
                Menge = line.Menge,
                Gewicht = line.Gewicht,
                Preis = line.Preis,
                MwstProzent = line.MwstProzent,
                LineAmount = line.LineAmount,
                VatAmount = line.VatAmount,
                AmountInclVat = line.AmountInclVat,
                Lieferscheinnummer = delivery?.Lieferschein?.Lieferscheinnummer,
                Lieferdatum = delivery?.Lieferschein?.LieferDatum
            });
        }

        return result;
    }
}
