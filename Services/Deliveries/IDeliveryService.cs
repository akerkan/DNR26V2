using DNR26V2.Domain.DTOs;
using DNR26V2.Domain.Entities.Deliveries;
using DNR26V2.Domain.Entities.Orders;
using DNR26V2.Domain.Enums;

namespace DNR26V2.Services.Deliveries;

public interface IDeliveryService
{
    Task<DeliveryHeader> CreateFromOrderAsync(Order order);

    Task<IReadOnlyList<LieferscheinListDto>> GetLieferscheinListeAsync(
        DateTime? von, DateTime? bis, string? kunde, DeliveryStatus? status);

    /// <summary>Setzt einen offenen Lieferschein auf Abgeschlossen.</summary>
    Task AbschliessenAsync(int lieferscheinId);

    /// <summary>Storniert einen Lieferschein und setzt den verknüpften Auftrag zurück auf Freigegeben.</summary>
    Task StornierenAsync(int lieferscheinId);
}