using DNR26V2.Domain.Enums;

namespace DNR26V2.Domain.Entities.Etikett;

/// <summary>
/// Persisted position/style for one named field on the label canvas.
/// Canvas is 378 × 567 px  (10 × 15 cm at 96 DPI).
/// </summary>
public class EtiketLayoutField
{
    public int       Id          { get; set; }
    public string    LayoutName  { get; set; } = "Default";
    public EtiketFeld Feld       { get; set; }

    // Position on canvas (pixels at 96 DPI)
    public int X      { get; set; }
    public int Y      { get; set; }
    public int Width  { get; set; }
    public int Height { get; set; }

    // Font
    public string FontName { get; set; } = "Arial";
    public float  FontSize { get; set; } = 9f;
    public bool   Bold     { get; set; }
    public bool   Italic   { get; set; }

    // Colors (hex, e.g. "#000000")
    public string ForeColorHex { get; set; } = "#000000";
    public string BackColorHex { get; set; } = "#FFFFFF";

    // 0 = Left, 1 = Center, 2 = Right
    public int TextAlignH { get; set; } = 0;

    public bool Visible { get; set; } = true;

    // ?? Hintergrundbild (nur für Resim-Felder) ????????????????????????????????
    /// <summary>Rohe Bilddaten (PNG/JPG), max. 100 KB. Null = kein Bild.</summary>
    public byte[]? ImageData { get; set; }

    /// <summary>0 = Strecken, 1 = Anpassen (Zoom), 2 = Zentrieren</summary>
    public int ImageSizeMode { get; set; } = 1;
}
