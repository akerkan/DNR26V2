using DNR26V2.Domain.DTOs;
using DNR26V2.Domain.Entities.Orders;

namespace DNR26V2.Services.Orders;

public interface IOrderService
{
    Task<IReadOnlyList<OrderKundeListDto>> GetKundenListeAsync(DateTime datum, DayOfWeek? tag);
    Task<IReadOnlyList<OrderLineDto>> GetPositionenAsync(int kundeId, DateTime datum);
    Task<Order?> GetAuftragAsync(int kundeId, DateTime datum);
    Task<Order> SaveAuftragAsync(int kundeId, DateTime datum, IEnumerable<(int ArtikelId, decimal Menge, decimal Gewicht, decimal Preis, string? Notiz)> positionen);
    Task<Order> BuchenAsync(int auftragId);
    Task StornierenAsync(int auftragId);
}