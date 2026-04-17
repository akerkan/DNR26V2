using DNR26V2.Domain.Entities.MasterData;

namespace DNR26V2.Services.MasterData;

public interface IProductAttributeService
{
    // ── Attribute-Definitionen ─────────────────────────────────────────────
    Task<IReadOnlyList<ProductAttribute>> GetAllAttributesAsync(bool nurAktiv = false);
    Task<ProductAttribute?> GetAttributeByIdAsync(int id);
    Task SaveAttributeAsync(ProductAttribute attribute);
    Task SetAttributeActiveAsync(int id, bool aktiv);

    // ── Attribute-Werte ────────────────────────────────────────────────────
    Task<IReadOnlyList<ProductAttributeValue>> GetValuesByAttributeAsync(int attributId);
    Task SaveValueAsync(ProductAttributeValue value);
    Task SetValueActiveAsync(int id, bool aktiv);
    Task DeleteValueAsync(int id);

    // ── Produkt-Zuweisungen ────────────────────────────────────────────────
    Task<IReadOnlyList<ProductAttributeMapping>> GetMappingsByProductAsync(int produktId);
    Task SaveMappingsAsync(int produktId, IEnumerable<ProductAttributeMapping> mappings);
    Task DeleteMappingAsync(int mappingId);
}