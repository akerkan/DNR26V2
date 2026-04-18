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
    private readonly AppDbContext      _db;
    private readonly DapperContext     _dapper;
    private readonly INoSeriesService  _noSeries;
    private readonly IDeliveryService  _deliveryService;

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

        var sql = $"""
            SELECT c.Id,
                   c.Kundennummer,
                   c.Kundenname,
                   tv.Bezeichnung  AS Tur,
                   c.Routenfolge,
                   o.Id            AS AuftragId,
                   o.Status        AS AuftragStatus
            FROM   Customer c
            LEFT   JOIN ProductAttributeValue tv ON tv.Id = c.TurWertId
            LEFT   JOIN Orders o ON o.KundeId     = c.Id
                                AND CAST(o.LieferDatum AS date) = CAST(@Datum AS date)
                                AND o.Status      <> 2
            WHERE  c.Aktiv = 1
            {dayFilter}
            ORDER  BY tv.Bezeichnung, c.Routenfolge, c.Kundenname
            """;

        using var conn = _dapper.CreateConnection();
        return (await conn.QueryAsync<OrderKundeListDto>(sql, new { Datum = datum })).AsList();
    }

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
                    .Include(o => o.Zeilen)
                    .FirstOrDefaultAsync(o => o.KundeId          == kundeId
                                           && o.LieferDatum.Date == datum.Date
                                           && o.Status           != OrderStatus.Storniert);

    public async Task<Order> SaveAuftragAsync(
        int kundeId, DateTime datum,
        IEnumerable<(int ArtikelId, decimal Menge, decimal Gewicht, decimal Preis, string? Notiz)> positionen)
    {
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
            await _db.SaveChangesAsync();
        }
        else if (order.Status != OrderStatus.Offen)
        {
            throw new ValidationException($"Auftrag '{order.Auftragsnummer}' ist bereits '{order.Status}'.");
        }

        // Replace all lines
        var existingLines = await _db.OrderLine.Where(l => l.AuftragId == order.Id).ToListAsync();
        _db.OrderLine.RemoveRange(existingLines);

        foreach (var pos in positionen)
        {
            _db.OrderLine.Add(new OrderLine
            {
                AuftragId = order.Id,
                ArtikelId = pos.ArtikelId,
                Menge     = pos.Menge,
                Gewicht   = pos.Gewicht,
                Preis     = pos.Preis,
                Notiz     = pos.Notiz
            });
        }

        await _db.SaveChangesAsync();
        return order;
    }

    public async Task<Order> BuchenAsync(int auftragId)
    {
        var order = await _db.Order
            .Include(o => o.Zeilen)
            .FirstOrDefaultAsync(o => o.Id == auftragId)
            ?? throw new InvalidOperationException("Auftrag nicht gefunden.");

        if (order.Status != OrderStatus.Offen)
            throw new ValidationException($"Auftrag ist bereits '{order.Status}'.");

        if (!order.Zeilen.Any(z => z.Menge > 0))
            throw new ValidationException("Mindestens eine Position mit Menge > 0 erforderlich.");

        order.Status = OrderStatus.Bestaetigt;
        await _db.SaveChangesAsync();

        await _deliveryService.CreateFromOrderAsync(order);
        return order;
    }

    public async Task StornierenAsync(int auftragId)
    {
        var order = await _db.Order.FindAsync(auftragId)
            ?? throw new InvalidOperationException("Auftrag nicht gefunden.");

        if (order.Status == OrderStatus.Storniert)
            throw new ValidationException("Auftrag ist bereits storniert.");

        order.Status = OrderStatus.Storniert;
        await _db.SaveChangesAsync();
    }
}