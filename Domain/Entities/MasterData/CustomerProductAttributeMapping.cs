namespace DNR26V2.Domain.Entities.MasterData;

/// <summary>
/// Dynamische Produktspezifikation pro Kundenprodukt-Schablone.
/// Beispiel: Rohrtyp = "Demir Boru", Form = "Üçgen", Länge = "60 Cm"
/// </summary>
public class CustomerProductAttributeMapping : AuditableEntity
{
    public int     Id                { get; set; }
    public int     CustomerProductId { get; set; }
    public int     AttributId        { get; set; }

    /// <summary>Gewählter Lookup-Wert (nullable — nur bei Feldtyp = Lookup).</summary>
    public int?    AttributWertId    { get; set; }

    /// <summary>Freitext-Wert (nur bei Feldtyp = FreeText).</summary>
    public string? FreierText        { get; set; }

    // Navigation
    public CustomerProduct        CustomerProduct { get; set; } = null!;
    public ProductAttribute       Attribut        { get; set; } = null!;
    public ProductAttributeValue? AttributWert    { get; set; }
}