using DNR26V2.Domain.DTOs;
using DNR26V2.Domain.Entities.Orders;
using DNR26V2.Domain.Enums;

namespace DNR26V2.Services.Orders;

public interface IOrderService
{
    Task<IReadOnlyList<OrderKundeListDto>> GetKundenListeAsync(DateTime datum, DayOfWeek? tag);
    Task<IReadOnlyList<OrderLineDto>>      GetPositionenAsync(int kundeId, DateTime datum);
    Task<Order?>                           GetAuftragAsync(int kundeId, DateTime datum);
    Task<KundenFactBoxDto?>                GetKundenFactBoxAsync(int kundeId);
    Task<IReadOnlyList<ArtikelSuchDto>>    GetArtikelListeAsync(string? suche = null);
    Task<Order> SaveAuftragAsync(int kundeId, DateTime datum,
        IEnumerable<(int ArtikelId, decimal Menge, decimal Gewicht, decimal Preis, string? Notiz)> positionen);
    Task<Order> BuchenAsync(int auftragId);
    Task        FreigebenAsync(int auftragId);

    /// <summary>Setzt einen freigegebenen Auftrag zurück auf Offen.</summary>
    Task        OeffnenAsync(int auftragId);

    Task        LoeschenAsync(int auftragId);
    Task<Order> NachlieferungAsync(int kundeId, DateTime lieferdatum);
    Task        StornierenAsync(int auftragId);

    // ── Auftragsübersicht ─────────────────────────────────────────────────────
    Task<IReadOnlyList<AuftragListDto>> GetAuftragListeAsync(
        DateTime? von, DateTime? bis, string? kunde, OrderStatus? status);
}