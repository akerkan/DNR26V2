using DNR26V2.Data.Context;
using DNR26V2.Domain.DTOs.Reports;
using DNR26V2.Services.MasterData;
using DNR26V2.Services.Payments;
using Microsoft.EntityFrameworkCore;

namespace DNR26V2.Services.Reports;

public class KundenkontoReportDataService : IKundenkontoReportDataService
{
    private readonly AppDbContext _db;
    private readonly ICustomerService _customerService;
    private readonly ICustomerAccountService _accountService;

    public KundenkontoReportDataService(AppDbContext db, ICustomerService customerService, ICustomerAccountService accountService)
    {
        _db = db;
        _customerService = customerService;
        _accountService = accountService;
    }

    public async Task<KundenkontoReportData> GetKundenkontoReportDataAsync(int kundeId, DateTime von, DateTime bis)
    {
        var kunde = await _db.Customer.AsNoTracking().FirstOrDefaultAsync(x => x.Id == kundeId)
                    ?? throw new InvalidOperationException("Kunde nicht gefunden.");
        var appSetup = await _db.AppSetup.AsNoTracking().FirstOrDefaultAsync();
        var entries = await _accountService.GetEntriesAsync(kundeId, von, bis);

        var header = new KundenkontoPrintHeaderDto
        {
            KundeId = kunde.Id,
            KundenNr = kunde.Kundennummer,
            KundeName = kunde.Kundenname,
            ZeitraumVon = von,
            ZeitraumBis = bis,
            Anfangssaldo = 0m, // Phase 1: 0, can be improved later
            Endsaldo = entries.LastOrDefault()?.Saldo ?? 0m,
            Firmenname = appSetup?.Firmenname ?? string.Empty,
            FirmaAdresse = appSetup?.Firmenadresse ?? string.Empty,
            FirmaPLZ = appSetup?.FirmenPLZ ?? string.Empty,
            FirmaOrt = appSetup?.FirmenOrt ?? string.Empty,
            FirmaLand = appSetup?.FirmenLand ?? string.Empty,
            FirmaTelefon = appSetup?.FirmenTelefon ?? string.Empty,
            FirmaEmail = appSetup?.FirmenEmail ?? string.Empty,
            FirmaUstIdNr = appSetup?.FirmenUStIdNr ?? string.Empty,
            LogoVerwenden = appSetup?.LogoVerwenden ?? false,
            LogoPfad = appSetup?.LogoPfad,
            BriefpapierVerwenden = appSetup?.BriefpapierVerwenden ?? false
        };

        var lines = entries.Select(e => new KundenkontoPrintLineDto
        {
            Datum = e.Datum,
            Belegart = e.Belegart,
            BelegNr = e.BelegNr,
            Beschreibung = e.Beschreibung ?? string.Empty,
            Soll = e.Soll,
            Haben = e.Haben,
            Saldo = e.Saldo,
            Notiz = e.Notiz
        }).ToList();

        return new KundenkontoReportData
        {
            Header = header,
            Lines = lines
        };
    }
}
