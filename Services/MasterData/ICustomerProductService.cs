using DNR26V2.Domain.DTOs;
using DNR26V2.Domain.Entities.MasterData;
using DNR26V2.Domain.Enums;

namespace DNR26V2.Services.MasterData;

public interface ICustomerProductService
{
    Task<IReadOnlyList<CustomerProductDto>>          GetByCustomerAsync(int kundeId);
    Task<IReadOnlyList<CustomerProductAttributeDto>> GetAttributesAsync(int customerProductId);
    Task<IReadOnlyList<ProductAttribute>>            GetApplicableAttributesAsync();
    Task<IReadOnlyList<ProductAttributeValue>>       GetAttributeValuesAsync(int attributId);

    /// <summary>initialPreis = Kundenpreis vor dem Speichern (aus oberem Grid).</summary>
    Task<CustomerProduct> AddProductAsync(int kundeId, int artikelId, decimal initialPreis = 0);

    Task SaveAsync(CustomerProduct entity);
    Task SaveAttributeAsync(CustomerProductAttributeMapping mapping);
    Task RemoveAsync(int id);
    Task<CustomerProduct?> GetByIdAsync(int id);
}