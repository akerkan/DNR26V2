using DNR26V2.Domain.Enums;

namespace DNR26V2.Domain.Entities.MasterData;

public class ProductAttribute : AuditableEntity
{
    public int Id { get; set; }
    public string Bezeichnung { get; set; } = string.Empty;
    public AttributeFieldType Feldtyp { get; set; } = AttributeFieldType.Lookup;
    public AttributeEntityType EntityType { get; set; } = AttributeEntityType.Product;
    public int? MaxLaenge { get; set; }
    public bool Aktiv { get; set; } = true;

    /// <summary>
    /// Wenn true: In der Kundenvorlage darf der Benutzer einen Freitext
    /// eingeben (DropDown statt DropDownList). Gilt für den gesamten Attribut-Typ.
    /// </summary>
    public bool IstVorlage { get; set; } = false;

    public ICollection<ProductAttributeValue> Werte { get; set; } = [];
    public ICollection<ProductAttributeMapping> Mappings { get; set; } = [];
    public ICollection<CustomerProductAttributeMapping> KundenMappings { get; set; } = [];
}