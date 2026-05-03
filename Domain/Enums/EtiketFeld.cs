namespace DNR26V2.Domain.Enums;

/// <summary>
/// Named fields on the 10×15 cm label.
/// </summary>
public enum EtiketFeld
{
    Logo               = 0,
    BarcodeObenLinks   = 1,
    BarcodeObenRechts  = 2,
    SchockGefroren     = 3,
    UrunAdi            = 4,   // Product.Bezeichnung, colored background
    Untertitel         = 5,   // Product.Feld1
    Zutaten            = 6,   // Product.Feld2 (HTML stripped)
    Hinweis            = 7,   // Product.Feld3
    HaltbarBis         = 8,   // HerstellDatum + 6 months
    Wochentag          = 9,   // Day name (SONNTAG …)
    ChargeEingefrorenAm = 10,
    Kundenname         = 11,
    MengeGewicht       = 12,
    FirmaFooter        = 13,
}
