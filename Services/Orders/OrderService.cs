using Dapper;
using DNR26V2.Data;
using DNR26V2.Data.Context;
using DNR26V2.Domain.DTOs;
using DNR26V2.Domain.Entities.Orders;
using DNR26V2.Domain.Enums;
using DNR26V2.Domain.Exceptions;
using DNR26V2.Domain.Helpers;
using DNR26V2.Services.Deliveries;
using DNR26V2.Services.System;
using Microsoft.EntityFrameworkCore;

namespace DNR26V2.Services.Orders;

public class OrderService : IOrderService
{
    private readonly AppDbContext     _db;
    private readonly DapperContext    _dapper;
    private readonly INoSeriesService _noSeries;
    private readonly IDeliveryService _deliveryService;
    private readonly IAuditLogService _auditLog;  // ← ADD in Konstruktor

    public OrderService(
        AppDbContext      db,
        DapperContext     dapper,
        INoSeriesService  noSeries,
        IDeliveryService  deliveryService,
        IAuditLogService  auditLogService) // ← ADD
    {
        _db              = db;
        _dapper          = dapper;
        _noSeries        = noSeries;
        _deliveryService = deliveryService;
        _auditLog        = auditLogService; // ← ADD
    }

    // ── Customer list for order entry ─────────────────────────────────────────

    public async Task<IReadOnlyList<OrderKundeListDto>> GetKundenListeAsync(DateTime datum, DayOfWeek? tag)
    {
        string dayFilter = tag switch
        {
            DayOfWeek.Monday    => "AND c.LiefertMo = 1",
            DayOfWeek.Tuesday   => "AND c.LiefertDi = 1",
            DayOfWeek.Wednesday => "AND c.LiefertMi = 1",
            DayOfWeek.Thursday  => "AND c.LiefertDo = 1",
            DayOfWeek.Friday    => "AND c.LiefertFr = 1",
            DayOfWeek.Saturday  => "AND c.LiefertSa = 1",
            DayOfWeek.Sunday    => "AND c.LiefertSo = 1",
            _                   => string.Empty
        };

        // Exclude Storniert(3) and Geloescht(4) — show Offen, Freigegeben and Gebucht
        var sql = $"""
            SELECT c.Id,
                   c.Kundennummer,
                   c.Kundenname,
                   tv.Bezeichnung   AS Tur,
                   c.Routenfolge,
                   c.PreisAusblenden,
                   o.Id             AS AuftragId,
                   o.Status         AS AuftragStatus
            FROM   Customer c
            LEFT   JOIN ProductAttributeValue tv ON tv.Id = c.TurWertId
            LEFT   JOIN Orders o ON o.KundeId     = c.Id
                                AND CAST(o.LieferDatum AS date) = CAST(@Datum AS date)
                                AND o.Status NOT IN (3, 4)
            WHERE  c.Aktiv = 1
            {dayFilter}
            ORDER  BY tv.Bezeichnung, c.Routenfolge, c.Kundenname
            """;

        using var conn = _dapper.CreateConnection();
        return (await conn.QueryAsync<OrderKundeListDto>(sql, new { Datum = datum })).AsList();
    }

    // ── FactBox data for selected customer ────────────────────────────────────

    public async Task<KundenFactBoxDto?> GetKundenFactBoxAsync(int kundeId)
    {
        const string sql = """
            SELECT c.Kundenname,
                   c.PreisAusblenden,
                   tv.Bezeichnung  AS Tour,
                   0.00            AS Saldo,
                   (
                       SELECT MAX(o.LieferDatum)
                       FROM   Orders o
                       WHERE  o.KundeId = @KundeId
                       AND    o.Status  NOT IN (3, 4)
                   )               AS LetzterAuftrag,
                   (
                       SELECT COUNT(*)
                       FROM   Orders o
                       WHERE  o.KundeId = @KundeId
                       AND    o.Status  IN (0, 1)
                   )               AS OffeneAuftraege
            FROM   Customer c
            LEFT   JOIN ProductAttributeValue tv ON tv.Id = c.TurWertId
            WHERE  c.Id = @KundeId
            """;

        using var conn = _dapper.CreateConnection();
        return await conn.QuerySingleOrDefaultAsync<KundenFactBoxDto>(sql, new { KundeId = kundeId });
    }

    // ── Product search for Hinzufügen dialog ─────────────────────────────────

    public async Task<IReadOnlyList<ArtikelSuchDto>> GetArtikelListeAsync(string? suche = null)
    {
        const string sql = """
            SELECT p.Id           AS ArtikelId,
                   p.Artikelnummer,
                   p.Bezeichnung  AS Produktname,
                   p.VKPreis,
                   p.PreisFormel
            FROM   Product p
            WHERE  p.Aktiv = 1
            AND    (@Suche IS NULL
                    OR p.Bezeichnung   LIKE '%' + @Suche + '%'
                    OR p.Artikelnummer LIKE '%' + @Suche + '%')
            ORDER  BY p.Bezeichnung
            """;

        using var conn = _dapper.CreateConnection();
        return (await conn.QueryAsync<ArtikelSuchDto>(sql, new { Suche = suche })).AsList();
    }

    // ── Order lines ───────────────────────────────────────────────────────────

    public async Task<IReadOnlyList<OrderLineDto>> GetPositionenAsync(int kundeId, DateTime datum)
    {
        var existing = await GetAuftragAsync(kundeId, datum);

        if (existing is not null)
        {
            const string sql = """
                SELECT ol.Id          AS OrderLineId,
                       ol.ArtikelId,
                       p.Artikelnummer,
                       p.Bezeichnung  AS Produktname,
                       ol.Menge,
                       ol.Gewicht,
                       ol.Preis,
                       ol.Notiz,
                       p.PreisFormel
                FROM   OrderLines ol
                INNER  JOIN Product p ON p.Id = ol.ArtikelId
                WHERE  ol.AuftragId = @AuftragId
                ORDER  BY p.Bezeichnung
                """;
            using var conn = _dapper.CreateConnection();
            return (await conn.QueryAsync<OrderLineDto>(sql, new { AuftragId = existing.Id })).AsList();
        }

        // Load from CustomerProduct template
        const string tplSql = """
            SELECT 0             AS OrderLineId,
                   cp.ArtikelId,
                   p.Artikelnummer,
                   p.Bezeichnung AS Produktname,
                   cp.Menge,
                   cp.Gewicht,
                   cp.Preis,
                   NULL          AS Notiz,
                   p.PreisFormel
            FROM   CustomerProduct cp
            INNER  JOIN Product p ON p.Id = cp.ArtikelId
            WHERE  cp.KundeId = @KundeId
            AND    cp.Aktiv   = 1
            ORDER  BY p.Bezeichnung
            """;
        using var conn2 = _dapper.CreateConnection();
        return (await conn2.QueryAsync<OrderLineDto>(tplSql, new { KundeId = kundeId })).AsList();
    }

    public async Task<Order?> GetAuftragAsync(int kundeId, DateTime datum)
        => await _db.Order
                    .AsNoTracking()
                    .Include(o => o.Zeilen)
                    .FirstOrDefaultAsync(o => o.KundeId          == kundeId
                                           && o.LieferDatum.Date == datum.Date
                                           && o.Status           != OrderStatus.Storniert
                                           && o.Status           != OrderStatus.Geloescht);

    // ── Save ─────────────────────────────────────────────────────────────────

    public async Task<Order> SaveAuftragAsync(
        int kundeId, DateTime datum,
        IEnumerable<(int ArtikelId, decimal Menge, decimal Gewicht, decimal Preis, string? Notiz)> positionen)
    {
        var positionenList = positionen.ToList();

        // Validate: at least one line required
        if (positionenList.Count == 0)
            throw new ValidationException("Mindestens eine Position ist erforderlich.");

        var order = await GetAuftragAsync(kundeId, datum);

        if (order is null)
        {
            var nummer = await _noSeries.GetNextNumberAsync("AUF", datum);
            order = new Order
            {
                Auftragsnummer = nummer,
                KundeId        = kundeId,
                LieferDatum    = datum.Date,
                Status         = OrderStatus.Offen
            };
            _db.Order.Add(order);
            await SaveChangesAsync();
        }
        else if (order.Status != OrderStatus.Offen)
        {
            throw new ValidationException($"Auftrag '{order.Auftragsnummer}' ist bereits '{order.Status}'.");        }

        // Replace all lines — reload tracked entity by ID
        var trackedOrder = await _db.Order.FindAsync(order.Id) ?? order;

        var existingLines = await _db.OrderLine.Where(l => l.AuftragId == trackedOrder.Id).ToListAsync();
        _db.OrderLine.RemoveRange(existingLines);

        var user = Environment.UserName;

        // Skip lines with Menge = 0 — do not persist them
        positionenList = positionenList.Where(p => p.Menge > 0).ToList();

        if (positionenList.Count == 0)
            throw new ValidationException("Mindestens eine Position mit Menge > 0 ist erforderlich.");

        // Load MwstProzent and PreisFormel from Product once — per-product, never global
        var artikelIds = positionenList.Select(p => p.ArtikelId).ToList();
        var productMap = await _db.Product
            .Where(p => artikelIds.Contains(p.Id))
            .Select(p => new { p.Id, p.MwstProzent, p.PreisFormel })
            .ToDictionaryAsync(p => p.Id, p => (p.MwstProzent, p.PreisFormel));

        foreach (var pos in positionenList)
        {
            if (!productMap.TryGetValue(pos.ArtikelId, out var product))
                throw new InvalidOperationException(
                    $"Product data missing for ArtikelId {pos.ArtikelId}");

            var formel = product.PreisFormel;

            var line = new OrderLine
            {
                AuftragId       = trackedOrder.Id,
                ArtikelId       = pos.ArtikelId,
                ErstelltVon     = user,
                Menge           = pos.Menge,
                Gewicht         = pos.Gewicht,
                Preis           = pos.Preis,
                Notiz           = pos.Notiz,
                MwstProzent     = product.MwstProzent,
                DiscountProzent = 0   // future: pass from caller
            };

            line.GrossAmount   = InvoiceCalculator.CalcGrossAmount(line.Menge, line.Gewicht, line.Preis, formel);
            line.DiscountAmount = InvoiceCalculator.CalcDiscountAmount(line.GrossAmount, line.DiscountProzent);
            line.LineAmount    = InvoiceCalculator.CalcLineAmount(line.GrossAmount, line.DiscountAmount);
            line.VatAmount     = InvoiceCalculator.CalcVatAmount(line.LineAmount, line.MwstProzent);
            line.AmountInclVat = InvoiceCalculator.CalcAmountInclVat(line.LineAmount, line.VatAmount);

            _db.OrderLine.Add(line);
        }

        await _db.SaveChangesAsync();

        // Reload lines — CalcHeader uses per-line MwstProzent (UStG §14 compliant)
        await _db.Entry(trackedOrder).Collection(o => o.Zeilen).LoadAsync();
        var (netto, mwstTotal, brutto) = InvoiceCalculator.CalcHeader(
            trackedOrder.Zeilen.Select(z => (z.LineAmount, z.MwstProzent)));
        trackedOrder.Gesamtnetto  = netto;
        trackedOrder.Gesamtmwst   = mwstTotal;
        trackedOrder.Gesamtbrutto = brutto;
        await _db.SaveChangesAsync();

        return trackedOrder;
    }

    // ── Buchen → creates Delivery ─────────────────────────────────────────────

    public async Task<Order> BuchenAsync(int auftragId)
    {
        var order = await _db.Order
         .Include(o => o.Zeilen)
         .FirstOrDefaultAsync(o => o.Id == auftragId)
         ?? throw new InvalidOperationException("Auftrag nicht gefunden.");

        if (order.Status != OrderStatus.Offen && order.Status != OrderStatus.Freigegeben)
            throw new ValidationException($"Auftrag kann nicht gebucht werden (Status: '{order.Status}').");

        if (!order.Zeilen.Any(z => z.Menge > 0))
            throw new ValidationException("Mindestens eine Position mit Menge > 0 erforderlich.");

        var oldStatus = order.Status;

        using var tx = await _db.Database.BeginTransactionAsync();
        try
        {
            order.Status = OrderStatus.Gebucht;
            await SaveChangesAsync();

            await _auditLog.LogAsync(
                tabellenname: "Orders",
                datensatzId: auftragId,
                belegnummer: order.Auftragsnummer,
                aktion: "Buchen",
                alterWert: oldStatus.ToString(),
                neuerWert: OrderStatus.Gebucht.ToString()
            );

            // CreateFromOrderAsync erwartet das Order-Objekt (oder Id) — vorhandene Signatur beibehalten
            await _deliveryService.CreateFromOrderAsync(order);

            await tx.CommitAsync();
        }
        catch
        {
            await tx.RollbackAsync();
            _db.ChangeTracker.Clear();
            throw;
        }

        return order;
    }

    // ── Freigeben ────────────────────────────────────────────────────────────

    public async Task FreigebenAsync(int auftragId)
    {
        var order = await _db.Order.FindAsync(auftragId);
        if (order is null || order.Status != OrderStatus.Offen)
            throw new ValidationException("Nur offene Aufträge können freigegeben werden.");

        var oldStatus = order.Status;
        order.Status = OrderStatus.Freigegeben;
        await _db.SaveChangesAsync();

        // ← ADD
        await _auditLog.LogAsync(
            tabellenname: "Orders",
            datensatzId: auftragId,
            belegnummer: order.Auftragsnummer,
            aktion: "Freigeben",
            alterWert: oldStatus.ToString(),
            neuerWert: OrderStatus.Freigegeben.ToString()
        );
    }

    // ── Löschen (soft-delete, nur Offen) ─────────────────────────────────────

    public async Task LoeschenAsync(int auftragId)
    {
        var affected = await _db.Order
            .Where(o => o.Id == auftragId && o.Status == OrderStatus.Offen)
            .ExecuteUpdateAsync(s => s.SetProperty(o => o.Status, OrderStatus.Geloescht));

        if (affected == 0)
            throw new ValidationException("Nur offene Aufträge können gelöscht werden.");
    }

    // ── Nachlieferung ────────────────────────────────────────────────────────

    public async Task<Order> NachlieferungAsync(int kundeId, DateTime lieferdatum)
    {
        var nummer = await _noSeries.GetNextNumberAsync("AUF", lieferdatum);
        var order  = new Order
        {
            Auftragsnummer = nummer,
            KundeId        = kundeId,
            LieferDatum    = lieferdatum.Date,
            Status         = OrderStatus.Freigegeben
        };
        _db.Order.Add(order);
        await SaveChangesAsync();
        return order;
    }

   
    // ── Auftragsübersicht (für FrmOrderList) ──────────────────────────────────

    public async Task<IReadOnlyList<AuftragListDto>> GetAuftragListeAsync(
        DateTime? von, DateTime? bis, string? kunde, OrderStatus? status)
    {
        const string sql = """
            SELECT o.Id,
                   o.Auftragsnummer                  AS AuftragNr,
                   o.LieferDatum                     AS Lieferdatum,
                   c.Kundenname,
                   tv.Bezeichnung                    AS Tour,
                   o.Status,
                   dh.Lieferscheinnummer             AS LieferscheinNr,
                   ISNULL(pos.AnzahlPositionen, 0)   AS AnzahlPositionen,
                   ISNULL(pos.Gesamtbetrag, 0)       AS Gesamtbetrag,
                   o.KundeId
            FROM   Orders o
            INNER  JOIN Customer c ON c.Id = o.KundeId
            LEFT   JOIN ProductAttributeValue tv ON tv.Id = c.TurWertId
            LEFT   JOIN (
                SELECT AuftragId,
                       COUNT(*)        AS AnzahlPositionen,
                       SUM(LineAmount) AS Gesamtbetrag
                FROM   OrderLines
                GROUP  BY AuftragId
            ) pos ON pos.AuftragId = o.Id
            LEFT   JOIN (
                SELECT AuftragId, MIN(Lieferscheinnummer) AS Lieferscheinnummer
                FROM   Deliveries
                WHERE  Status <> 3   -- not Storniert
                GROUP  BY AuftragId
            ) dh ON dh.AuftragId = o.Id
            WHERE  o.Status <> 4   -- never show Geloescht
            AND    (@Von    IS NULL OR CAST(o.LieferDatum AS date) >= CAST(@Von AS date))
            AND    (@Bis    IS NULL OR CAST(o.LieferDatum AS date) <= CAST(@Bis AS date))
            AND    (@Kunde  IS NULL OR c.Kundenname LIKE '%' + @Kunde + '%')
            AND    (@Status IS NULL OR o.Status = @Status)
            ORDER  BY o.LieferDatum DESC, c.Kundenname
            """;

        using var conn = _dapper.CreateConnection();
        return (await conn.QueryAsync<AuftragListDto>(sql, new
        {
            Von    = von,
            Bis    = bis,
            Kunde  = kunde,
            Status = status
        })).AsList();
    }

    // ── Helper: SaveChanges with automatic tracker cleanup on failure ─────────

    private async Task SaveChangesAsync()
    {
        try
        {
            await _db.SaveChangesAsync();
        }
        catch
        {
            // Reset EF in-memory state so subsequent calls work on clean context
            _db.ChangeTracker.Clear();
            throw;
        }
    }

    // ── Öffnen (Freigegeben → Offen zurücksetzen) ─────────────────────────────

    public async Task OeffnenAsync(int auftragId)
    {
        var order = await _db.Order.FindAsync(auftragId)
        ?? throw new InvalidOperationException("Auftrag nicht gefunden.");

        if (order.Status != OrderStatus.Freigegeben)
            throw new ValidationException("Nur freigegebene Aufträge können zurück auf Offen gesetzt werden.");

        var oldStatus = order.Status;
        order.Status = OrderStatus.Offen;
        await SaveChangesAsync();

        await _auditLog.LogAsync(
            tabellenname: "Orders",
            datensatzId: auftragId,
            belegnummer: order.Auftragsnummer,
            aktion: "Öffnen",
            alterWert: oldStatus.ToString(),
            neuerWert: OrderStatus.Offen.ToString()
        );
    }

    // ── Positionen nach Auftrag-ID (für Detail-Panel) ─────────────────────────

    public async Task<IReadOnlyList<OrderLineDto>> GetPositionenByAuftragIdAsync(int auftragId)
    {
        const string sql = """
            SELECT ol.Id          AS OrderLineId,
                   ol.ArtikelId,
                   p.Artikelnummer,
                   p.Bezeichnung  AS Produktname,
                   ol.Menge,
                   ol.Gewicht,
                   ol.Preis,
                   ol.Notiz
            FROM   OrderLines ol
            LEFT   JOIN Product p ON p.Id = ol.ArtikelId
            WHERE  ol.AuftragId = @AuftragId
            ORDER  BY p.Bezeichnung
            """;
        using var conn = _dapper.CreateConnection();
        return (await conn.QueryAsync<OrderLineDto>(sql, new { AuftragId = auftragId })).AsList();
    }
}