namespace DNR26V2.Domain.Entities.MasterData;

public class ProductAttributeValue : AuditableEntity
{
    public int    Id          { get; set; }
    public int    AttributId  { get; set; }
    public string Bezeichnung { get; set; } = string.Empty;
    public int    Sortierung  { get; set; } = 0;
    public bool   Aktiv       { get; set; } = true;

    public ProductAttribute                     Attribut { get; set; } = null!;
    public ICollection<ProductAttributeMapping> Mappings { get; set; } = [];
}