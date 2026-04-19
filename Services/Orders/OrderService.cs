using Dapper;
using DNR26V2.Data;
using DNR26V2.Data.Context;
using DNR26V2.Domain.DTOs;
using DNR26V2.Domain.Entities.Orders;
using DNR26V2.Domain.Enums;
using DNR26V2.Domain.Exceptions;
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

    public OrderService(
        AppDbContext      db,
        DapperContext     dapper,
        INoSeriesService  noSeries,
        IDeliveryService  deliveryService)
    {
        _db              = db;
        _dapper          = dapper;
        _noSeries        = noSeries;
        _deliveryService = deliveryService;
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
                   p.VKPreis
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
                       ol.Notiz
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
                   NULL          AS Notiz
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

        foreach (var pos in positionenList)
        {
            _db.OrderLine.Add(new OrderLine
            {
                AuftragId = trackedOrder.Id,
                ArtikelId = pos.ArtikelId,
                Menge     = pos.Menge,
                Gewicht   = pos.Gewicht,
                Preis     = pos.Preis,
                Notiz     = pos.Notiz
            });
        }

        await SaveChangesAsync();
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

        order.Status = OrderStatus.Gebucht;
        await SaveChangesAsync();

        await _deliveryService.CreateFromOrderAsync(order);
        return order;
    }

    // ── Freigeben ────────────────────────────────────────────────────────────

    public async Task FreigebenAsync(int auftragId)
    {
        // ExecuteUpdateAsync bypasses EF tracking → no stale-cache issues
        var affected = await _db.Order
            .Where(o => o.Id == auftragId && o.Status == OrderStatus.Offen)
            .ExecuteUpdateAsync(s => s.SetProperty(o => o.Status, OrderStatus.Freigegeben));

        if (affected == 0)
            throw new ValidationException("Nur offene Aufträge können freigegeben werden.");
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

    // ── Stornieren ────────────────────────────────────────────────────────────

    public async Task StornierenAsync(int auftragId)
    {
        // ExecuteUpdateAsync reads directly from DB — avoids stale EF tracking
        var affected = await _db.Order
            .Where(o => o.Id == auftragId
                     && o.Status != OrderStatus.Storniert
                     && o.Status != OrderStatus.Geloescht)
            .ExecuteUpdateAsync(s => s.SetProperty(o => o.Status, OrderStatus.Storniert));

        if (affected == 0)
            throw new ValidationException("Auftrag ist bereits storniert oder nicht gefunden.");
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
}