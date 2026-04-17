namespace DNR26V2.Domain.Entities.MasterData;

public class ProductAttributeMapping
{
    public int     Id             { get; set; }
    public int     ArtikelId      { get; set; }
    public int     AttributId     { get; set; }
    public int?    AttributWertId { get; set; }
    public string? FreierText     { get; set; }

    public Product                Artikel      { get; set; } = null!;
    public ProductAttribute       Attribut     { get; set; } = null!;
    public ProductAttributeValue? AttributWert { get; set; }
}