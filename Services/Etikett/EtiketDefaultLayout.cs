using DNR26V2.Domain.Entities.Etikett;
using DNR26V2.Domain.Enums;

namespace DNR26V2.Services.Etikett;

/// <summary>
/// Factory for the built-in default label layout (10 × 15 cm canvas = 378 × 567 px at 96 DPI).
/// </summary>
public static class EtiketDefaultLayout
{
    public static IReadOnlyList<EtiketLayoutField> Build(string layoutName = "Default")
    {
        return new List<EtiketLayoutField>
        {
            Field(layoutName, EtiketFeld.Logo,
                x:5, y:5, w:190, h:80,
                fontName:"Arial", fontSize:9f,
                backHex:"#FFFFFF", foreHex:"#000000", align:1),

            Field(layoutName, EtiketFeld.BarcodeObenLinks,
                x:5, y:3, w:140, h:14,
                fontName:"Arial", fontSize:7f,
                backHex:"#FFFFFF", foreHex:"#000000", align:0),

            Field(layoutName, EtiketFeld.BarcodeObenRechts,
                x:230, y:3, w:143, h:14,
                fontName:"Arial", fontSize:7f,
                backHex:"#FFFFFF", foreHex:"#000000", align:2),

            Field(layoutName, EtiketFeld.SchockGefroren,
                x:140, y:3, w:90, h:14,
                fontName:"Arial", fontSize:7f,
                backHex:"#FFFFFF", foreHex:"#000000", align:1),

            Field(layoutName, EtiketFeld.UrunAdi,
                x:5, y:90, w:368, h:36,
                fontName:"Arial", fontSize:14f, bold:true,
                backHex:"#0078D7", foreHex:"#FFFFFF", align:1),

            Field(layoutName, EtiketFeld.Untertitel,
                x:5, y:130, w:368, h:20,
                fontName:"Arial", fontSize:9f,
                backHex:"#FFFFFF", foreHex:"#000000", align:1),

            Field(layoutName, EtiketFeld.Zutaten,
                x:5, y:154, w:368, h:115,
                fontName:"Arial", fontSize:7.5f,
                backHex:"#FFFFFF", foreHex:"#000000", align:0),

            Field(layoutName, EtiketFeld.Hinweis,
                x:5, y:273, w:368, h:70,
                fontName:"Arial", fontSize:7.5f,
                backHex:"#FFFFFF", foreHex:"#000000", align:1),

            Field(layoutName, EtiketFeld.HaltbarBis,
                x:5, y:348, w:180, h:30,
                fontName:"Arial", fontSize:8f,
                backHex:"#FFFFFF", foreHex:"#000000", align:0),

            Field(layoutName, EtiketFeld.Wochentag,
                x:155, y:350, w:100, h:26,
                fontName:"Arial", fontSize:11f, bold:true,
                backHex:"#FFFFFF", foreHex:"#CC0000", align:1),

            Field(layoutName, EtiketFeld.ChargeEingefrorenAm,
                x:263, y:348, w:110, h:30,
                fontName:"Arial", fontSize:8f,
                backHex:"#FFFFFF", foreHex:"#000000", align:2),

            Field(layoutName, EtiketFeld.Kundenname,
                x:5, y:388, w:368, h:36,
                fontName:"Arial", fontSize:14f, bold:true,
                backHex:"#FFFFFF", foreHex:"#000000", align:1),

            Field(layoutName, EtiketFeld.MengeGewicht,
                x:5, y:428, w:368, h:45,
                fontName:"Arial", fontSize:20f, bold:true,
                backHex:"#FFFFFF", foreHex:"#000000", align:1),

            Field(layoutName, EtiketFeld.FirmaFooter,
                x:5, y:480, w:368, h:50,
                fontName:"Arial", fontSize:7.5f,
                backHex:"#FFFFFF", foreHex:"#000000", align:1),

            // ?? Resim alanlar? 2-5 ????????????????????????????????????????????
            Field(layoutName, EtiketFeld.Bild2,
                x:200, y:5, w:173, h:80,
                fontName:"Arial", fontSize:9f,
                backHex:"#FFFFFF", foreHex:"#000000", align:1),

            Field(layoutName, EtiketFeld.Bild3,
                x:5, y:540, w:120, h:22,
                fontName:"Arial", fontSize:9f,
                backHex:"#FFFFFF", foreHex:"#000000", align:1),

            Field(layoutName, EtiketFeld.Bild4,
                x:130, y:540, w:120, h:22,
                fontName:"Arial", fontSize:9f,
                backHex:"#FFFFFF", foreHex:"#000000", align:1),

            Field(layoutName, EtiketFeld.Bild5,
                x:255, y:540, w:118, h:22,
                fontName:"Arial", fontSize:9f,
                backHex:"#FFFFFF", foreHex:"#000000", align:1),
        };
    }

    private static EtiketLayoutField Field(
        string layoutName, EtiketFeld feld,
        int x, int y, int w, int h,
        string fontName = "Arial", float fontSize = 9f,
        bool bold = false, bool italic = false,
        string foreHex = "#000000", string backHex = "#FFFFFF",
        int align = 0, bool visible = true)
        => new()
        {
            LayoutName  = layoutName,
            Feld        = feld,
            X           = x,
            Y           = y,
            Width       = w,
            Height      = h,
            FontName    = fontName,
            FontSize    = fontSize,
            Bold        = bold,
            Italic      = italic,
            ForeColorHex= foreHex,
            BackColorHex= backHex,
            TextAlignH  = align,
            Visible     = visible,
        };
}
