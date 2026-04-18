using DNR26V2.Domain.Entities.Deliveries;
using DNR26V2.Domain.Entities.Orders;

namespace DNR26V2.Services.Deliveries;

public interface IDeliveryService
{
    Task<DeliveryHeader> CreateFromOrderAsync(Order order);
}