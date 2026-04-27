using Dapper;
using DNR26V2.Data;
using DNR26V2.Data.Context;
using DNR26V2.Domain.DTOs;
using DNR26V2.Domain.Entities.Deliveries;
using DNR26V2.Domain.Entities.Invoices;
using DNR26V2.Domain.Entities.Orders;
using DNR26V2.Domain.Enums;
using DNR26V2.Domain.Exceptions;
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

    // -- helpers --------------------------------------------------------------

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

    private async Task<string> GenerateGutschriftnummerAsync()
    {
        var setup   = await _db.AppSetup.AsNoTracking().FirstAsync();
        var praefix = setup.GutschriftPraefix;
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

    // -- FrmRechnungErfassung --------------------------------------------------

    public async Task<IReadOnlyList<KundeOffeneLsDto>> GetKundenMitOffenenLsAsync(
        DateTime von, DateTime bis)
    {
        // Filter by open invoiceable quantity at line level (MengeFakturiert < Menge).
        // DeliveryHeader.Status is NOT used as sole criterion — a Fakturiert delivery
        // can become re-invoiceable after invoice storno without resetting its status.
        // Storniert deliveries (Status = 3) are excluded as they represent canceled shipments.
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
               AND dh.Status <> 3
               AND CAST(dh.LieferDatum AS date) BETWEEN @Von AND @Bis
            INNER JOIN DeliveryLines dl
                ON dl.LieferscheinId = dh.Id
               AND dl.Menge > 0
               AND dl.MengeFakturiert < dl.Menge
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
        // Filter by open invoiceable quantity at line level (MengeFakturiert < Menge).
        // Storniert deliveries (Status = 3) are excluded. All other statuses are valid
        // candidates as long as at least one line has open invoiceable quantity.
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
                AND dl.Menge > 0
                AND dl.MengeFakturiert < dl.Menge
            WHERE dh.KundeId   = @KundeId
              AND dh.Status <> 3
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

        // Load delivery lines — accept all non-storniert deliveries.
        // A Fakturiert delivery can be re-invoiced after invoice storno if MengeFakturiert < Menge.
        // The per-line guard below (verbleibend check) prevents over-invoicing.
        var deliveries = await _db.DeliveryHeader
            .Include(d => d.Zeilen)
            .Where(d => lsIds.Contains(d.Id) && d.KundeId == kundeId && d.Status != DeliveryStatus.Storniert)
            .ToListAsync();

        if (deliveries.Count == 0)
            throw new InvalidOperationException("Keine fakturierbaren Lieferscheine gefunden.");

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

        // Collect (deliveryLineId, invoicedMenge, auftragZeileId) per InvoiceLine for explicit
        // tracking update below. Mirrors StornierenAsync: load entities directly via _db.DeliveryLine /
        // _db.OrderLine instead of relying on Include-based navigation collection change tracking,
        // which does not reliably generate UPDATE statements for MengeFakturiert.
        var pendingUpdates = new List<(int DeliveryLineId, decimal Menge, int? AuftragZeileId)>();

        foreach (var ls in deliveries)
        {
            // Only process positive original lines with open invoiceable quantity.
            // Storno lines (Menge < 0) and already fully-invoiced lines are skipped.
            foreach (var dl in ls.Zeilen.Where(z => z.Menge > 0 && z.MengeFakturiert < z.Menge))
            {
                // Open quantity: Menge - already invoiced.
                // Do NOT use MengeGeliefert — it is always 0 on DeliveryLines and unused.
                var fakturiert = dl.Menge - dl.MengeFakturiert;
                var gewicht    = dl.Menge > 0
                                     ? Math.Round(dl.Gewicht * fakturiert / dl.Menge, 3)
                                     : dl.Gewicht;

                var formel   = produktFormelMap.GetValueOrDefault(dl.ArtikelId, DNR26V2.Domain.Enums.PreisFormel.MengeXPreis);
                var gross    = InvoiceCalculator.CalcGrossAmount(fakturiert, gewicht, dl.Preis, formel);
                var discount = InvoiceCalculator.CalcDiscountAmount(gross, dl.DiscountProzent);
                var line     = InvoiceCalculator.CalcLineAmount(gross, discount);
                var vat      = InvoiceCalculator.CalcVatAmount(line, dl.MwstProzent);
                var incl     = InvoiceCalculator.CalcAmountInclVat(line, vat);

                var il = new InvoiceLine
                {
                    LieferscheinId   = ls.Id,
                    DeliveryLineId   = dl.Id,
                    ArtikelId        = dl.ArtikelId,
                    Menge            = fakturiert,
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
                };
                header.Zeilen.Add(il);

                // Guard — never invoice beyond available open quantity
                if (il.Menge > fakturiert)
                    throw new ValidationException(
                        $"Überfakturierung nicht erlaubt: DeliveryLine {dl.Id} — " +
                        $"fakturierbar: {fakturiert:0.###}, angefragt: {il.Menge:0.###}.");

                // Queue for explicit tracking update after the loop
                pendingUpdates.Add((dl.Id, il.Menge, dl.AuftragZeileId));
            }

            // Mark delivery as Fakturiert
            ls.Status       = DeliveryStatus.Fakturiert;
            ls.GeaendertAm  = DateTime.Now;
            ls.GeaendertVon = Environment.UserName;
        }

        // Explicitly update DeliveryLine.MengeFakturiert.
        // Direct _db.DeliveryLine load ensures EF generates the correct UPDATE statement
        // regardless of what is already in the identity map from the Include above.
        var dlIds = pendingUpdates.Select(u => u.DeliveryLineId).Distinct().ToList();
        if (dlIds.Count > 0)
        {
            var dlsToUpdate = await _db.DeliveryLine
                .Where(d => dlIds.Contains(d.Id))
                .ToDictionaryAsync(d => d.Id);

            foreach (var (dlId, menge, _) in pendingUpdates)
            {
                if (dlsToUpdate.TryGetValue(dlId, out var trackedDl))
                    trackedDl.MengeFakturiert += menge;
            }
        }

        // Explicitly update OrderLine.MengeFakturiert
        var auftragZeileIds = pendingUpdates
            .Where(u => u.AuftragZeileId.HasValue)
            .Select(u => u.AuftragZeileId!.Value)
            .Distinct()
            .ToList();

        if (auftragZeileIds.Count > 0)
        {
            var olsToUpdate = await _db.OrderLine
                .Where(ol => auftragZeileIds.Contains(ol.Id))
                .ToDictionaryAsync(ol => ol.Id);

            foreach (var (_, menge, auftragZeileId) in pendingUpdates.Where(u => u.AuftragZeileId.HasValue))
            {
                if (olsToUpdate.TryGetValue(auftragZeileId!.Value, out var trackedOl))
                    trackedOl.MengeFakturiert += menge;
            }
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

    // -- FrmSammelRechnung -----------------------------------------------------

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
                result.RechnungIds.Add(header.Id);
            }
            catch (Exception ex)
            {
                result.Fehler.Add($"KundeId {kundeId}: {ex.Message}");
            }
        }

        return result;
    }

    // -- FrmRechnungList -------------------------------------------------------

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
                i.BelegArt,
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
                i.Status, i.BelegArt, i.IstSammelrechnung,
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

    public async Task<InvoiceHeader> CreateGutschriftAsync(int rechnungId)
    {
        var original = await _db.Invoices
            .Include(i => i.Zeilen)
            .FirstOrDefaultAsync(i => i.Id == rechnungId)
            ?? throw new InvalidOperationException("Rechnung nicht gefunden.");

        if (original.Status != InvoiceStatus.Gebucht)
            throw new InvalidOperationException(
                $"Gutschrift nur für gebuchte Rechnungen möglich (aktuell: {original.Status}).");

        if (original.BelegArt != InvoiceDocumentType.Rechnung)
            throw new InvalidOperationException(
                "Gutschriften können nicht erneut gutgeschrieben werden.");

        var gutschriftnr = await GenerateGutschriftnummerAsync();

        var gutschrift = new InvoiceHeader
        {
            Rechnungsnummer    = gutschriftnr,
            KundeId            = original.KundeId,
            Rechnungsdatum     = DateTime.Today,
            Von                = original.Von,
            Bis                = original.Bis,
            Status             = InvoiceStatus.Gebucht,
            BelegArt           = InvoiceDocumentType.Gutschrift,
            OriginalRechnungId = original.Id,
            Notiz              = $"Gutschrift für {original.Rechnungsnummer}",
            IstSammelrechnung  = false,
            ErstelltAm         = DateTime.Now,
            ErstelltVon        = Environment.UserName,
        };

        // Copy lines from original — all values stay POSITIVE.
        // BelegArt = Gutschrift carries the financial reversal meaning.
        // Presentation layer (UI / print) must render a minus sign for Gutschrift documents.
        // Storing negative amounts in DB is intentionally avoided: it would break
        // InvoiceCalculator grouping, VAT aggregation, and future ledger movement logic.
        foreach (var origLine in original.Zeilen)
        {
            gutschrift.Zeilen.Add(new InvoiceLine
            {
                LieferscheinId   = origLine.LieferscheinId,
                DeliveryLineId   = origLine.DeliveryLineId,
                ArtikelId        = origLine.ArtikelId,
                Menge            = origLine.Menge,
                FakturierteMenge = origLine.FakturierteMenge,
                Gewicht          = origLine.Gewicht,
                Preis            = origLine.Preis,
                MwstProzent      = origLine.MwstProzent,
                DiscountProzent  = origLine.DiscountProzent,
                GrossAmount      = origLine.GrossAmount,
                DiscountAmount   = origLine.DiscountAmount,
                LineAmount       = origLine.LineAmount,
                VatAmount        = origLine.VatAmount,
                AmountInclVat    = origLine.AmountInclVat,
                ErstelltAm       = DateTime.Now,
                ErstelltVon      = Environment.UserName,
            });
        }

        // Header totals stay POSITIVE — BelegArt = Gutschrift signals the negative financial effect.
        gutschrift.Gesamtnetto  = original.Gesamtnetto;
        gutschrift.Gesamtmwst   = original.Gesamtmwst;
        gutschrift.Gesamtbrutto = original.Gesamtbrutto;

        // Roll back DeliveryLine.MengeFakturiert — same explicit-load pattern as StornierenAsync
        var deliveryLineIds = original.Zeilen.Select(z => z.DeliveryLineId).ToList();
        var deliveryLines = await _db.DeliveryLine
            .Where(dl => deliveryLineIds.Contains(dl.Id))
            .ToListAsync();

        foreach (var origLine in original.Zeilen)
        {
            var dl = deliveryLines.FirstOrDefault(d => d.Id == origLine.DeliveryLineId);
            if (dl is not null)
                dl.MengeFakturiert = Math.Max(0, dl.MengeFakturiert - origLine.Menge);
        }

        // Roll back OrderLine.MengeFakturiert
        var auftragZeileIds = deliveryLines
            .Where(dl => dl.AuftragZeileId.HasValue)
            .Select(dl => dl.AuftragZeileId!.Value)
            .Distinct()
            .ToList();

        if (auftragZeileIds.Count > 0)
        {
            var orderLines = await _db.OrderLine
                .Where(ol => auftragZeileIds.Contains(ol.Id))
                .ToDictionaryAsync(ol => ol.Id);

            foreach (var origLine in original.Zeilen)
            {
                var dl = deliveryLines.FirstOrDefault(d => d.Id == origLine.DeliveryLineId);
                if (dl?.AuftragZeileId.HasValue == true &&
                    orderLines.TryGetValue(dl.AuftragZeileId.Value, out var ol))
                    ol.MengeFakturiert = Math.Max(0, ol.MengeFakturiert - origLine.Menge);
            }
        }

        // Mark original invoice as Gutgeschrieben — it stays in DB, audit trail intact
        original.Status       = InvoiceStatus.Gutgeschrieben;
        original.GeaendertAm  = DateTime.Now;
        original.GeaendertVon = Environment.UserName;

        _db.Invoices.Add(gutschrift);
        await _db.SaveChangesAsync();

        return gutschrift;
    }

    [Obsolete("Use CreateGutschriftAsync instead. StornierenAsync will be removed in a future version.")]
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
                dl.MengeFakturiert = Math.Max(0, dl.MengeFakturiert - zeile.Menge);
        }

        // Part B: roll back OrderLine.MengeFakturiert
        var auftragZeileIds = deliveryLines
            .Where(dl => dl.AuftragZeileId.HasValue)
            .Select(dl => dl.AuftragZeileId!.Value)
            .Distinct()
            .ToList();

        if (auftragZeileIds.Count > 0)
        {
            var orderLines = await _db.OrderLine
                .Where(ol => auftragZeileIds.Contains(ol.Id))
                .ToDictionaryAsync(ol => ol.Id);

            foreach (var zeile in header.Zeilen)
            {
                var dl = deliveryLines.FirstOrDefault(d => d.Id == zeile.DeliveryLineId);
                if (dl?.AuftragZeileId.HasValue == true &&
                    orderLines.TryGetValue(dl.AuftragZeileId.Value, out var ol))
                    ol.MengeFakturiert = Math.Max(0, ol.MengeFakturiert - zeile.Menge);
            }
        }

        // DeliveryHeader.Status is intentionally NOT changed here.
        // Invoice storno reverses invoicing effects only (MengeFakturiert).
        // Physical delivery remains intact — the goods were still shipped.
        // Re-invoiceability is determined by MengeFakturiert < Menge at line level.

        header.Status       = InvoiceStatus.Storniert;
        header.GeaendertAm  = DateTime.Now;
        header.GeaendertVon = Environment.UserName;

        await _db.SaveChangesAsync();
    }

    // -- Vorschau --------------------------------------------------------------

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
