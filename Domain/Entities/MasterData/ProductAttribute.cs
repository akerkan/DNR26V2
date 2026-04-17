using DNR26V2.Domain.Enums;

namespace DNR26V2.Domain.Entities.MasterData;

public class ProductAttribute : AuditableEntity
{
    public int               Id          { get; set; }
    public string            Bezeichnung { get; set; } = string.Empty;
    public AttributeFieldType Feldtyp    { get; set; } = AttributeFieldType.Lookup;
    public int?              MaxLaenge   { get; set; }
    public bool              Aktiv       { get; set; } = true;

    public ICollection<ProductAttributeValue>   Werte    { get; set; } = [];
    public ICollection<ProductAttributeMapping> Mappings { get; set; } = [];
}