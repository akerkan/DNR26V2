namespace DNR26V2.Domain.Entities.Etikett;

/// <summary>
/// Stores the physical paper dimensions (cm) for a named label layout.
/// </summary>
public class EtiketPaperConfig
{
    public int    Id         { get; set; }
    public string LayoutName { get; set; } = "Default";
    public float  WidthCm   { get; set; } = 10f;
    public float  HeightCm  { get; set; } = 15f;
}