using DNR26V2.Data.Context;
using DNR26V2.Domain.Entities.Deliveries;
using DNR26V2.Domain.Entities.Orders;
using DNR26V2.Domain.Enums;
using DNR26V2.Services.System;
using Microsoft.EntityFrameworkCore;

namespace DNR26V2.Services.Deliveries;

public class DeliveryService : IDeliveryService
{
    private readonly AppDbContext     _db;
    private readonly INoSeriesService _noSeries;

    public DeliveryService(AppDbContext db, INoSeriesService noSeries)
    {
        _db       = db;
        _noSeries = noSeries;
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
}