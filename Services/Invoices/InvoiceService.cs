using Dapper;
using DNR26V2.Data;
using DNR26V2.Data.Context;
using DNR26V2.Domain.DTOs;
using DNR26V2.Domain.Entities.Invoices;
using DNR26V2.Domain.Enums;
using DNR26V2.Domain.Helpers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DNR26V2.Services.Invoices;

public class InvoiceService : IInvoiceService
{
    private readonly AppDbContext _db;
    private readonly string       _connectionString;

    public InvoiceService(AppDbContext db, string connectionString)
    {
        _db               = db;
        _connectionString = connectionString;
    }

    // ── helpers ──────────────────────────────────────────────────────────────

    private SqlConnection CreateConnection() => new(_connectionString);

    private async Task<string> GenerateRechnungsnummerAsync()
    {
        var setup = await _db.AppSetup.AsNoTracking().FirstAsync();
        var praefix = setup.RechnungPraefix;
        var year    = DateTime.Today.Year;

        var last = await _db.Invoices
            .Where(i => i.Rechnungsnummer.StartsWith($"{praefix}{year}"))
            .OrderByDescending(i => i.Rechnungsnummer)
            .Select(i => i.Rechnungsnummer)
            .FirstOrDefaultAsync();

        int seq = 1;
        if (last is not null)
        {
            var suffix = last[$"{praefix}{year}".Length..];
            if (int.TryParse(suffix, out int prev))
                seq = prev + 1;
        }

        return $"{praefix}{year}{seq:D4}";
    }

    // ── FrmRechnungErfassung ──────────────────────────────────────────────────

    public async Task<IReadOnlyList<KundeOffeneLsDto>> GetKundenMitOffenenLsAsync(
        DateTime von, DateTime bis)
    {
        const string sql = """
            SELECT
                c.Id                                          AS KundeId,
                c.Kundennummer,
                c.Kundenname                                  AS Kundenname,
                COUNT(DISTINCT dh.Id)                         AS AnzahlOffeneLs,
                ISNULL(SUM(dl.LineAmount), 0)  AS GesamtbetragOffen
            FROM Customer c
            INNER JOIN Deliveries dh
                ON dh.KundeId = c.Id
               AND dh.Status  = 0
               AND CAST(dh.LieferDatum AS date) BETWEEN @Von AND @Bis
            INNER JOIN DeliveryLines dl
                ON dl.LieferscheinId = dh.Id
            GROUP BY c.Id, c.Kundennummer, c.Kundenname
            ORDER BY c.Kundenname
            """;

        using var con = CreateConnection();
        var rows = await con.QueryAsync<KundeOffeneLsDto>(sql,
            new { Von = von.Date, Bis = bis.Date });
        return rows.AsList();
    }

    public async Task<IReadOnlyList<LieferscheinFuerRechnungDto>> GetOffeneLieferscheineAsync(
        int kundeId, DateTime von, DateTime bis)
    {
        const string sql = """
            SELECT
                dh.Id                                AS LieferscheinId,
                dh.Lieferscheinnummer,
                dh.LieferDatum                       AS Lieferdatum,
                COUNT(dl.Id)                         AS AnzahlPositionen,
                ISNULL(SUM(dl.LineAmount), 0)     AS Gesamtbetrag,
                ISNULL(SUM(dl.AmountInclVat), 0)  AS Gesamtbrutto
            FROM Deliveries dh
            INNER JOIN DeliveryLines dl ON dl.LieferscheinId = dh.Id
            WHERE dh.KundeId   = @KundeId
              AND dh.Status    = 0
              AND CAST(dh.LieferDatum AS date) BETWEEN @Von AND @Bis
            GROUP BY dh.Id, dh.Lieferscheinnummer, dh.LieferDatum
            ORDER BY dh.LieferDatum
            """;

        using var con = CreateConnection();
        var rows = await con.QueryAsync<LieferscheinFuerRechnungDto>(sql,
            new { KundeId = kundeId, Von = von.Date, Bis = bis.Date });
        return rows.AsList();
    }

    public async Task<InvoiceHeader> BuchenAsync(
        int kundeId, DateTime von, DateTime bis,
        IEnumerable<int> lieferscheinIds,
        string? notiz)
    {
        var lsIds = lieferscheinIds.ToList();
        if (lsIds.Count == 0)
            throw new InvalidOperationException("Keine Lieferscheine ausgewählt.");

        // Load delivery lines with product prices
        var deliveries = await _db.DeliveryHeader
            .Include(d => d.Zeilen)
            .Where(d => lsIds.Contains(d.Id) && d.KundeId == kundeId && d.Status == DeliveryStatus.Offen)
            .ToListAsync();

        if (deliveries.Count == 0)
            throw new InvalidOperationException("Keine aktiven Lieferscheine gefunden.");

        var setup       = await _db.AppSetup.AsNoTracking().FirstAsync();
        var rechnungsnr = await GenerateRechnungsnummerAsync();

        // Load PreisFormel per product — each product has its own formula
        var artikelIds     = deliveries.SelectMany(d => d.Zeilen).Select(z => z.ArtikelId).Distinct().ToList();
        var produktFormelMap = await _db.Product
            .Where(p => artikelIds.Contains(p.Id))
            .Select(p => new { p.Id, p.PreisFormel })
            .ToDictionaryAsync(p => p.Id, p => p.PreisFormel);

        var header = new InvoiceHeader
        {
            Rechnungsnummer   = rechnungsnr,
            KundeId           = kundeId,
            Rechnungsdatum    = DateTime.Today,
            Von               = von.Date,
            Bis               = bis.Date,
            Status            = InvoiceStatus.Gebucht,
            Notiz             = notiz,
            IstSammelrechnung = false,
            ErstelltAm        = DateTime.Now,
            ErstelltVon       = Environment.UserName,
        };

        foreach (var ls in deliveries)
        {
            foreach (var dl in ls.Zeilen)
            {
                // Use delivered quantity; fall back to ordered quantity if not yet recorded
                var fakturiert = dl.MengeGeliefert > 0 ? dl.MengeGeliefert : dl.Menge;
                var gewicht    = fakturiert > 0 && dl.Menge > 0
                                     ? Math.Round(dl.Gewicht * fakturiert / dl.Menge, 3)
                                     : dl.Gewicht;

                var formel   = produktFormelMap.GetValueOrDefault(dl.ArtikelId, DNR26V2.Domain.Enums.PreisFormel.MengeXPreis);
                var gross    = InvoiceCalculator.CalcGrossAmount(fakturiert, gewicht, dl.Preis, formel);
                var discount = InvoiceCalculator.CalcDiscountAmount(gross, dl.DiscountProzent);
                var line     = InvoiceCalculator.CalcLineAmount(gross, discount);
                var vat      = InvoiceCalculator.CalcVatAmount(line, dl.MwstProzent);
                var incl     = InvoiceCalculator.CalcAmountInclVat(line, vat);

                header.Zeilen.Add(new InvoiceLine
                {
                    LieferscheinId   = ls.Id,
                    DeliveryLineId   = dl.Id,
                    ArtikelId        = dl.ArtikelId,
                    Menge            = dl.MengeGeliefert,
                    FakturierteMenge = fakturiert,
                    Gewicht          = gewicht,
                    Preis            = dl.Preis,
                    MwstProzent      = dl.MwstProzent,
                    DiscountProzent  = dl.DiscountProzent,
                    GrossAmount      = gross,
                    DiscountAmount   = discount,
                    LineAmount       = line,
                    VatAmount        = vat,
                    AmountInclVat    = incl,
                    ErstelltAm       = DateTime.Now,
                    ErstelltVon      = Environment.UserName,
                });

                dl.MengeFakturiert += fakturiert;
            }

            // Mark delivery as Fakturiert
            ls.Status      = DeliveryStatus.Fakturiert;
            ls.GeaendertAm = DateTime.Now;
            ls.GeaendertVon = Environment.UserName;
        }

        _db.Invoices.Add(header);
        await _db.SaveChangesAsync();

        // Header totals
        await _db.Entry(header).Collection(r => r.Zeilen).LoadAsync();
        var (netto, mwstTotal, brutto) = InvoiceCalculator.CalcHeader(
            header.Zeilen.Select(z => (z.LineAmount, z.MwstProzent)));
        header.Gesamtnetto = netto;
        header.Gesamtmwst = mwstTotal;
        header.Gesamtbrutto = brutto;
        await _db.SaveChangesAsync();
        return header;
    }

    // ── FrmSammelRechnung ─────────────────────────────────────────────────────

    public async Task<SammelrechnungResultDto> SammelBuchenAsync(
        IEnumerable<int> kundeIds, DateTime von, DateTime bis)
    {
        var result = new SammelrechnungResultDto();

        foreach (var kundeId in kundeIds)
        {
            try
            {
                var offeneLs = await GetOffeneLieferscheineAsync(kundeId, von, bis);
                if (offeneLs.Count == 0) continue;

                var lsIds = offeneLs.Select(l => l.LieferscheinId);

                var header = await BuchenAsync(kundeId, von, bis, lsIds, notiz: null);
                // Flag as Sammelrechnung
                header.IstSammelrechnung = true;
                await _db.SaveChangesAsync();

                result.Erstellt++;
            }
            catch (Exception ex)
            {
                result.Fehler.Add($"KundeId {kundeId}: {ex.Message}");
            }
        }

        return result;
    }

    // ── FrmRechnungList ───────────────────────────────────────────────────────

    public async Task<IReadOnlyList<RechnungListDto>> GetRechnungListeAsync(
        DateTime? von, DateTime? bis, string? kunde, InvoiceStatus? status)
    {
        const string sql = """
            SELECT
                i.Id,
                i.Rechnungsnummer,
                i.Rechnungsdatum,
                i.Von,
                i.Bis,
                c.Kundenname                          AS Kundenname,
                i.Status,
                i.IstSammelrechnung,
                i.Gesamtnetto,
                i.Gesamtbrutto,
                COUNT(il.Id)                          AS AnzahlPositionen
            FROM Invoices i
            INNER JOIN Customer c      ON c.Id  = i.KundeId
            LEFT  JOIN InvoiceLines il ON il.RechnungId = i.Id
            WHERE (@Von    IS NULL OR i.Rechnungsdatum >= @Von)
              AND (@Bis    IS NULL OR i.Rechnungsdatum <= @Bis)
              AND (@Kunde  IS NULL OR c.Kundenname LIKE '%' + @Kunde + '%'
                               OR c.Kundennummer = @Kunde)
              AND (@Status IS NULL OR i.Status = @Status)
            GROUP BY
                i.Id, i.Rechnungsnummer, i.Rechnungsdatum,
                i.Von, i.Bis, c.Kundenname,
                i.Status, i.IstSammelrechnung,
                i.Gesamtnetto, i.Gesamtbrutto
            ORDER BY i.Rechnungsdatum DESC, i.Rechnungsnummer DESC
            """;

        using var con = CreateConnection();
        var rows = await con.QueryAsync<RechnungListDto>(sql, new
        {
            Von    = von?.Date,
            Bis    = bis?.Date,
            Kunde  = string.IsNullOrWhiteSpace(kunde) ? null : kunde.Trim(),
            Status = status.HasValue ? (int?)status.Value : null,
        });
        return rows.AsList();
    }

    public async Task<IReadOnlyList<RechnungZeileDetailDto>> GetZeilenByRechnungIdAsync(int rechnungId)
    {
        // GetZeilenByRechnungIdAsync — SQL: il.Gewicht ergänzen
        const string sql = """
            SELECT
                il.Id,
                p.Artikelnummer,
                p.Bezeichnung,
                dh.Lieferscheinnummer,
                dh.LieferDatum                AS Lieferdatum,
                il.Menge,
                il.Gewicht,
                il.FakturierteMenge,
                il.Preis,
                il.MwstProzent,
                il.LineAmount,
                il.Notiz
            FROM InvoiceLines il
            INNER JOIN Deliveries dh ON dh.Id = il.LieferscheinId
            INNER JOIN Product    p  ON p.Id  = il.ArtikelId
            WHERE il.RechnungId = @RechnungId
            ORDER BY dh.LieferDatum, p.Bezeichnung
            """;

        using var con = CreateConnection();
        var rows = await con.QueryAsync<RechnungZeileDetailDto>(sql,
            new { RechnungId = rechnungId });
        return rows.AsList();
    }

    public async Task StornierenAsync(int rechnungId)
    {
        var header = await _db.Invoices
            .Include(i => i.Zeilen)
            .FirstOrDefaultAsync(i => i.Id == rechnungId)
            ?? throw new InvalidOperationException("Rechnung nicht gefunden.");

        if (header.Status != InvoiceStatus.Gebucht)
            throw new InvalidOperationException(
                $"Nur gebuchte Rechnungen können storniert werden (aktuell: {header.Status}).");

        // Collect unique LS IDs from lines
        var lsIds = header.Zeilen.Select(z => z.LieferscheinId).Distinct().ToList();

        // Roll back MengeFakturiert on DeliveryLines
        var deliveryLineIds = header.Zeilen.Select(z => z.DeliveryLineId).ToList();
        var deliveryLines   = await _db.DeliveryLine
            .Where(dl => deliveryLineIds.Contains(dl.Id))
            .ToListAsync();

        foreach (var zeile in header.Zeilen)
        {
            var dl = deliveryLines.FirstOrDefault(d => d.Id == zeile.DeliveryLineId);
            if (dl is not null)
                dl.MengeFakturiert = Math.Max(0, dl.MengeFakturiert - zeile.FakturierteMenge);
        }

        // Roll back delivery status → Aktiv
        var deliveries = await _db.DeliveryHeader
            .Where(d => lsIds.Contains(d.Id))
            .ToListAsync();

        foreach (var ls in deliveries)
        {
            ls.Status       = DeliveryStatus.Offen;
            ls.GeaendertAm  = DateTime.Now;
            ls.GeaendertVon = Environment.UserName;
        }

        header.Status       = InvoiceStatus.Storniert;
        header.GeaendertAm  = DateTime.Now;
        header.GeaendertVon = Environment.UserName;

        await _db.SaveChangesAsync();
    }

    // ── Vorschau ──────────────────────────────────────────────────────────────

    public async Task<IReadOnlyList<LieferscheinZeileVorschauDto>> GetZeilenVorschauAsync(
        int lieferscheinId)
    {
        // GetZeilenVorschauAsync — SQL: dl.Gewicht ergänzen
        const string sql = """
            SELECT
                p.Artikelnummer,
                p.Bezeichnung,
                dl.Menge,
                dl.Gewicht,
                COALESCE(NULLIF(dl.MengeGeliefert, 0), dl.Menge) AS FakturierteMenge,
                dl.Preis,
                dl.LineAmount
            FROM DeliveryLines dl
            INNER JOIN Product p ON p.Id = dl.ArtikelId
            WHERE dl.LieferscheinId = @LieferscheinId
            ORDER BY p.Bezeichnung
            """;

        using var con = CreateConnection();
        var rows = await con.QueryAsync<LieferscheinZeileVorschauDto>(sql,
            new { LieferscheinId = lieferscheinId });
        return rows.AsList();
    }
}