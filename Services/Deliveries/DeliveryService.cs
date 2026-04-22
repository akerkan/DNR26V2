using Dapper;
using DNR26V2.Data;
using DNR26V2.Data.Context;
using DNR26V2.Domain.DTOs;
using DNR26V2.Domain.Entities.Deliveries;
using DNR26V2.Domain.Entities.Orders;
using DNR26V2.Domain.Enums;
using DNR26V2.Domain.Exceptions;
using DNR26V2.Domain.Helpers;
using DNR26V2.Services.Deliveries;
using DNR26V2.Services.System;
using Microsoft.EntityFrameworkCore;

namespace DNR26V2.Services.Deliveries;

public class DeliveryService : IDeliveryService
{
    private readonly AppDbContext     _db;
    private readonly INoSeriesService _noSeries;
    private readonly DapperContext    _dapper;
    private readonly IAuditLogService _auditLog;  // ← ADD

    public DeliveryService(
        AppDbContext db, 
        INoSeriesService noSeries, 
        DapperContext dapper,
        IAuditLogService auditLog)  // ← ADD
    {
        _db       = db;
        _noSeries = noSeries;
        _dapper   = dapper;
        _auditLog = auditLog;  // ← ADD
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
                LieferscheinId  = lieferschein.Id,
                ArtikelId       = zeile.ArtikelId,
                Menge           = zeile.Menge,
                MengeGeliefert  = 0,
                Gewicht         = zeile.Gewicht,
                Preis           = zeile.Preis,
                Notiz           = zeile.Notiz,
                GrossAmount     = zeile.GrossAmount,
                DiscountProzent = zeile.DiscountProzent,
                DiscountAmount  = zeile.DiscountAmount,
                LineAmount      = zeile.LineAmount,
                MwstProzent     = zeile.MwstProzent,
                VatAmount       = zeile.VatAmount,
                AmountInclVat   = zeile.AmountInclVat
            });
        }

        await _db.SaveChangesAsync();

        // Load saved lines — MwstProzent copied from OrderLine, no recalculation
        await _db.Entry(lieferschein).Collection(l => l.Zeilen).LoadAsync();

        var (netto, mwstTotal, brutto) = InvoiceCalculator.CalcHeader(
            lieferschein.Zeilen.Select(z => (z.LineAmount, z.MwstProzent)));
        lieferschein.Gesamtnetto  = netto;
        lieferschein.Gesamtmwst   = mwstTotal;
        lieferschein.Gesamtbrutto = brutto;

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
                       COUNT(*)        AS AnzahlPositionen,
                       SUM(LineAmount) AS Gesamtbetrag
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

    // ── Stornieren (Header) ───────────────────────────────────────────────────

    public async Task StornierenAsync(int lieferscheinId)
    {
        var lieferschein = await _db.DeliveryHeader
            .Include(d => d.Auftrag)
            .FirstOrDefaultAsync(d => d.Id == lieferscheinId)
            ?? throw new InvalidOperationException("Lieferschein nicht gefunden.");

        if (lieferschein.Status == DeliveryStatus.Storniert)
            throw new ValidationException("Lieferschein ist bereits storniert.");

        if (lieferschein.Status == DeliveryStatus.Fakturiert)
            throw new ValidationException("Fakturierte Lieferscheine können nicht storniert werden.");

        var oldStatus = lieferschein.Status;
        lieferschein.Status = DeliveryStatus.Storniert;

        if (lieferschein.Auftrag is not null)
        {
            var oldAuftragStatus = lieferschein.Auftrag.Status;
            lieferschein.Auftrag.Status = OrderStatus.Offen;
            
            await _db.SaveChangesAsync();

            // ← ADD: AuditLog für Lieferschein
            await _auditLog.LogAsync(
                tabellenname: "Deliveries",
                datensatzId: lieferscheinId,
                belegnummer: lieferschein.Lieferscheinnummer,
                aktion: "Stornieren",
                alterWert: oldStatus.ToString(),
                neuerWert: DeliveryStatus.Storniert.ToString()
            );

            // ← ADD: AuditLog für Auftrag (Rückfluss)
            await _auditLog.LogAsync(
                tabellenname: "Orders",
                datensatzId: lieferschein.Auftrag.Id,
                belegnummer: lieferschein.Auftrag.Auftragsnummer,
                aktion: "Öffnen (Lieferschein storniert)",
                alterWert: oldAuftragStatus.ToString(),
                neuerWert: OrderStatus.Offen.ToString()
            );
        }
        else
        {
            await _db.SaveChangesAsync();

            await _auditLog.LogAsync(
                tabellenname: "Deliveries",
                datensatzId: lieferscheinId,
                belegnummer: lieferschein.Lieferscheinnummer,
                aktion: "Stornieren",
                alterWert: oldStatus.ToString(),
                neuerWert: DeliveryStatus.Storniert.ToString()
            );
        }
    }

    // ── Stornieren (Zeile → Minus-Zeile) ─────────────────────────────────────

    public async Task StornierenZeileAsync(int lieferscheinId, int lineId)
    {
        var lieferschein = await _db.DeliveryHeader
            .FirstOrDefaultAsync(d => d.Id == lieferscheinId)
            ?? throw new InvalidOperationException("Lieferschein nicht gefunden.");

        if (lieferschein.Status == DeliveryStatus.Fakturiert)
            throw new ValidationException("Positionen eines fakturierten Lieferscheins können nicht storniert werden.");

        if (lieferschein.Status == DeliveryStatus.Storniert)
            throw new ValidationException("Der gesamte Lieferschein ist bereits storniert.");

        var zeile = await _db.DeliveryLine
            .FirstOrDefaultAsync(l => l.Id == lineId && l.LieferscheinId == lieferscheinId)
            ?? throw new InvalidOperationException("Lieferzeile nicht gefunden.");

        if (zeile.Menge < 0)
            throw new ValidationException("Storno-Zeilen können nicht erneut storniert werden.");

        bool stornoExists = await _db.DeliveryLine
            .AnyAsync(l => l.LieferscheinId == lieferscheinId
                        && l.Notiz != null
                        && l.Notiz.StartsWith($"Storno #{lineId}"));

        if (stornoExists)
            throw new ValidationException("Für diese Zeile wurde bereits eine Storno-Zeile erstellt.");

        // Create minus line
        _db.DeliveryLine.Add(new DeliveryLine
        {
            LieferscheinId = lieferscheinId,
            ArtikelId      = zeile.ArtikelId,
            Menge          = -zeile.Menge,
            MengeGeliefert = 0,
            Gewicht        = -zeile.Gewicht,
            Preis          = zeile.Preis,
            Notiz          = $"Storno #{lineId}"
        });

        // Update header status → TeilStorniert (unless already Storniert)
        if (lieferschein.Status != DeliveryStatus.Storniert)
            lieferschein.Status = DeliveryStatus.TeilStorniert;

        await _db.SaveChangesAsync();
    }

    // ── Positionen nach Lieferschein-ID (für Detail-Panel) ───────────────────

    public async Task<IReadOnlyList<OrderLineDto>> GetPositionenByLieferscheinIdAsync(int lieferscheinId)
    {
        const string sql = """
            SELECT dl.Id          AS OrderLineId,
                   dl.ArtikelId,
                   p.Artikelnummer,
                   p.Bezeichnung  AS Produktname,
                   dl.Menge,
                   dl.Gewicht,
                   dl.Preis,
                   dl.Notiz
            FROM   DeliveryLines dl
            LEFT   JOIN Product p ON p.Id = dl.ArtikelId
            WHERE  dl.LieferscheinId = @LieferscheinId
            ORDER  BY p.Bezeichnung
            """;
        using var conn = _dapper.CreateConnection();
        return (await conn.QueryAsync<OrderLineDto>(sql, new { LieferscheinId = lieferscheinId })).AsList();
    }

    public async Task FakturierenAsync(int lieferscheinId)
    {
        var affected = await _db.DeliveryHeader
            .Where(d => d.Id == lieferscheinId && d.Status == DeliveryStatus.Offen)
            .ExecuteUpdateAsync(s => s.SetProperty(d => d.Status, DeliveryStatus.Fakturiert));

        if (affected == 0)
            throw new ValidationException(
                "Nur offene Lieferscheine können fakturiert werden.");
    }
}