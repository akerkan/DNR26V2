using DNR26V2.Data.Context;
using DNR26V2.Domain.DTOs.Reports;
using Microsoft.EntityFrameworkCore;

namespace DNR26V2.Services.Reports;

public class OrderReportDataService : IOrderReportDataService
{
    private readonly AppDbContext _db;

    public OrderReportDataService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<OrderReportData> GetOrderReportDataAsync(int orderId)
    {
        var order = await _db.Order
            .Include(o => o.Kunde)
            .Include(o => o.Zeilen)
                .ThenInclude(z => z.Artikel)
            .FirstOrDefaultAsync(o => o.Id == orderId)
            ?? throw new InvalidOperationException($"Auftrag {orderId} nicht gefunden.");

        var setup = await _db.AppSetup.FirstOrDefaultAsync()
            ?? throw new InvalidOperationException("AppSetup nicht gefunden.");

        var kundenadresseText = string.Join(Environment.NewLine,
            new[]
            {
                order.Kunde.Kundenname,
                order.Kunde.Adresse,
                $"{order.Kunde.PLZ} {order.Kunde.Ort}".Trim(),
                order.Kunde.Land
            }.Where(x => !string.IsNullOrWhiteSpace(x)));

        return new OrderReportData
        {
            Header = new OrderPrintHeaderDto
            {
                Auftragsnummer = order.Auftragsnummer,
                Lieferdatum = order.LieferDatum,
                KundeName = order.Kunde.Kundenname,
                KundenNr = order.Kunde.Kundennummer,
                KundenadresseText = kundenadresseText,
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
                Gesamtnetto = order.Gesamtnetto,
                Gesamtmwst = order.Gesamtmwst,
                Gesamtbrutto = order.Gesamtbrutto
            },
            Lines = order.Zeilen
                .OrderBy(z => z.Id)
                .Select(z => new OrderPrintLineDto
                {
                    Auftragsnummer = order.Auftragsnummer,
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
            SuggestedFileName = $"Auftragsbestaetigung_{order.Auftragsnummer}.pdf"
        };
    }
}
