using Dapper;
using DNR26V2.Data;
using DNR26V2.Data.Context;
using DNR26V2.Domain.DTOs;
using DNR26V2.Domain.Entities.Deliveries;
using DNR26V2.Domain.Entities.Orders;
using DNR26V2.Domain.Enums;
using DNR26V2.Domain.Exceptions;
using DNR26V2.Services.Deliveries;
using DNR26V2.Services.System;
using Microsoft.EntityFrameworkCore;

namespace DNR26V2.Services.Deliveries;

public class DeliveryService : IDeliveryService
{
    private readonly AppDbContext     _db;
    private readonly INoSeriesService _noSeries;
    private readonly DapperContext    _dapper;

    public DeliveryService(AppDbContext db, INoSeriesService noSeries, DapperContext dapper)
    {
        _db      = db;
        _noSeries = noSeries;
        _dapper  = dapper;
    }

    public async Task<DeliveryHeader> CreateFromOrderAsync(Order order)
    {
        // Ensure order lines are loaded
        if (!order.Zeilen.Any())
            await _db.Entry(order).Collection(o => o.Zeilen).LoadAsync();

        var nummer = await _noSeries.GetNextNumberAsync("LS", order.LieferDatum);

        var lieferschein = new DeliveryHeader
        {
            Lieferscheinnummer = nummer,
            KundeId            = order.KundeId,
            AuftragId          = order.Id,
            LieferDatum        = order.LieferDatum,
            Status             = DeliveryStatus.Offen
        };
        _db.DeliveryHeader.Add(lieferschein);
        await _db.SaveChangesAsync();

        foreach (var zeile in order.Zeilen)
        {
            _db.DeliveryLine.Add(new DeliveryLine
            {
                LieferscheinId = lieferschein.Id,
                ArtikelId      = zeile.ArtikelId,
                Menge          = zeile.Menge,
                MengeGeliefert = 0,
                Gewicht        = zeile.Gewicht,
                Preis          = zeile.Preis,
                Notiz          = zeile.Notiz
            });
        }

        await _db.SaveChangesAsync();
        return lieferschein;
    }

    // ── Lieferscheinübersicht ─────────────────────────────────────────────────

    public async Task<IReadOnlyList<LieferscheinListDto>> GetLieferscheinListeAsync(
        DateTime? von, DateTime? bis, string? kunde, DeliveryStatus? status)
    {
        const string sql = """
            SELECT d.Id,
                   d.Lieferscheinnummer              AS LieferscheinNr,
                   d.LieferDatum                     AS Lieferdatum,
                   c.Kundenname,
                   tv.Bezeichnung                    AS Tour,
                   o.Auftragsnummer                  AS AuftragNr,
                   d.Status,
                   ISNULL(pos.AnzahlPositionen, 0)   AS AnzahlPositionen,
                   ISNULL(pos.Gesamtbetrag, 0)       AS Gesamtbetrag,
                   d.KundeId
            FROM   Deliveries d
            INNER  JOIN Customer c ON c.Id = d.KundeId
            LEFT   JOIN ProductAttributeValue tv ON tv.Id = c.TurWertId
            LEFT   JOIN Orders o ON o.Id = d.AuftragId
            LEFT   JOIN (
                SELECT LieferscheinId,
                       COUNT(*)            AS AnzahlPositionen,
                       SUM(Menge * Preis)  AS Gesamtbetrag
                FROM   DeliveryLines
                GROUP  BY LieferscheinId
            ) pos ON pos.LieferscheinId = d.Id
            WHERE  (@Von    IS NULL OR CAST(d.LieferDatum AS date) >= CAST(@Von AS date))
            AND    (@Bis    IS NULL OR CAST(d.LieferDatum AS date) <= CAST(@Bis AS date))
            AND    (@Kunde  IS NULL OR c.Kundenname LIKE '%' + @Kunde + '%')
            AND    (@Status IS NULL OR d.Status = @Status)
            ORDER  BY d.LieferDatum DESC, c.Kundenname
            """;

        using var conn = _dapper.CreateConnection();
        return (await conn.QueryAsync<LieferscheinListDto>(sql, new
        {
            Von    = von,
            Bis    = bis,
            Kunde  = kunde,
            Status = status
        })).AsList();
    }

    // ── Abschliessen ─────────────────────────────────────────────────────────

    public async Task AbschliessenAsync(int lieferscheinId)
    {
        var affected = await _db.DeliveryHeader
            .Where(d => d.Id == lieferscheinId && d.Status == DeliveryStatus.Offen)
            .ExecuteUpdateAsync(s => s.SetProperty(d => d.Status, DeliveryStatus.Abgeschlossen));

        if (affected == 0)
            throw new ValidationException(
                "Nur offene Lieferscheine können abgeschlossen werden.");
    }

    // ── Stornieren ────────────────────────────────────────────────────────────

    public async Task StornierenAsync(int lieferscheinId)
    {
        var lieferschein = await _db.DeliveryHeader
            .FirstOrDefaultAsync(d => d.Id == lieferscheinId)
            ?? throw new InvalidOperationException("Lieferschein nicht gefunden.");

        if (lieferschein.Status == DeliveryStatus.Storniert)
            throw new ValidationException("Lieferschein ist bereits storniert.");

        if (lieferschein.Status == DeliveryStatus.Fakturiert)
            throw new ValidationException("Fakturierte Lieferscheine können nicht storniert werden.");

        lieferschein.Status = DeliveryStatus.Storniert;

        // Linked Auftrag → back to Freigegeben so it can be re-booked
        if (lieferschein.AuftragId.HasValue)
        {
            await _db.Order
                .Where(o => o.Id == lieferschein.AuftragId.Value
                         && o.Status == OrderStatus.Gebucht)
                .ExecuteUpdateAsync(s => s.SetProperty(o => o.Status, OrderStatus.Freigegeben));
        }

        await _db.SaveChangesAsync();
    }

    Task<DeliveryHeader> IDeliveryService.CreateFromOrderAsync(Order order)
    {
        throw new NotImplementedException();
    }
}