using DNR26V2.Domain.DTOs;
using DNR26V2.Domain.Entities.Deliveries;
using DNR26V2.Domain.Entities.Orders;
using DNR26V2.Domain.Enums;

namespace DNR26V2.Services.Deliveries;

public interface IDeliveryService
{
    Task<DeliveryHeader>                     CreateFromOrderAsync(Order order);
    Task<IReadOnlyList<LieferscheinListDto>> GetLieferscheinListeAsync(
        DateTime? von, DateTime? bis, string? kunde, DeliveryStatus? status);
    Task<IReadOnlyList<OrderLineDto>>        GetPositionenByLieferscheinIdAsync(int lieferscheinId);

    /// <summary>Sets Lieferschein status to Storniert. Auftrag status is NOT changed.</summary>
    Task StornierenAsync(int lieferscheinId);

    /// <summary>Creates a minus line for the given DeliveryLine and sets header to TeilStorniert.</summary>
    Task StornierenZeileAsync(int lieferscheinId, int lineId);

    Task FakturierenAsync(int lieferscheinId);
}