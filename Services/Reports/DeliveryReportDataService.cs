using DNR26V2.Data.Context;
using DNR26V2.Domain.DTOs.Reports;
using Microsoft.EntityFrameworkCore;

namespace DNR26V2.Services.Reports;

public class DeliveryReportDataService : IDeliveryReportDataService
{
    private readonly AppDbContext _db;

    public DeliveryReportDataService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<DeliveryReportData> GetDeliveryReportDataAsync(int deliveryId)
    {
        var delivery = await _db.DeliveryHeader
            .Include(d => d.Kunde)
            .Include(d => d.Auftrag)
            .Include(d => d.Zeilen)
                .ThenInclude(z => z.Artikel)
            .FirstOrDefaultAsync(d => d.Id == deliveryId)
            ?? throw new InvalidOperationException($"Lieferschein {deliveryId} nicht gefunden.");

        var setup = await _db.AppSetup.FirstOrDefaultAsync()
            ?? throw new InvalidOperationException("AppSetup nicht gefunden.");

        var kunde = delivery.Kunde;
        var lieferadresseText = string.Join(Environment.NewLine,
            new[]
            {
                kunde.AbweichendeLieferadresse ? (kunde.ALName2 ?? kunde.Kundenname) : kunde.Kundenname,
                kunde.AbweichendeLieferadresse ? kunde.ALAdresse : kunde.Adresse,
                kunde.AbweichendeLieferadresse
                    ? $"{kunde.ALPLZ} {kunde.ALOrt}".Trim()
                    : $"{kunde.PLZ} {kunde.Ort}".Trim(),
                kunde.AbweichendeLieferadresse ? kunde.ALLand : kunde.Land
            }.Where(x => !string.IsNullOrWhiteSpace(x)));

        return new DeliveryReportData
        {
            Header = new DeliveryPrintHeaderDto
            {
                Lieferscheinnummer = delivery.Lieferscheinnummer,
                Lieferdatum = delivery.LieferDatum,
                AuftragNr = delivery.Auftrag?.Auftragsnummer,
                KundeName = kunde.Kundenname,
                KundenNr = kunde.Kundennummer,
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
                LogoVerwenden = setup.LogoVerwenden,
                LogoPfad = setup.LogoPfad,
                BriefpapierVerwenden = setup.BriefpapierVerwenden,
                Gesamtnetto = delivery.Gesamtnetto,
                Gesamtmwst = delivery.Gesamtmwst,
                Gesamtbrutto = delivery.Gesamtbrutto
            },
            Lines = delivery.Zeilen
                .OrderBy(z => z.Id)
                .Select(z => new DeliveryPrintLineDto
                {
                    Lieferscheinnummer = delivery.Lieferscheinnummer,
                    Artikelnummer = z.Artikel?.Artikelnummer ?? string.Empty,
                    Bezeichnung = z.Artikel?.Bezeichnung ?? string.Empty,
                    Menge = z.Menge,
                    Gewicht = z.Gewicht,
                    Preis = z.Preis,
                    MwstProzent = z.MwstProzent,
                    LineAmount = z.LineAmount,
                    VatAmount = z.VatAmount,
                    AmountInclVat = z.AmountInclVat
                })
                .ToList(),
            SuggestedFileName = $"Lieferschein_{delivery.Lieferscheinnummer}.pdf"
        };
    }

    public async Task<IReadOnlyList<DeliveryReportData>> GetDeliveryReportDataAsync(IEnumerable<int> deliveryIds)
    {
        var result = new List<DeliveryReportData>();
        foreach (var deliveryId in deliveryIds.Distinct())
            result.Add(await GetDeliveryReportDataAsync(deliveryId));
        return result;
    }
}
