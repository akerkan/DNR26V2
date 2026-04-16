namespace DNR26V2.Domain.Entities.MasterData;

/// <summary>
/// Produktstammdaten.
/// Soft-Delete via Aktiv=false — physisches Löschen ist verboten.
/// </summary>
public class Product : AuditableEntity
{
    public int    Id            { get; set; }
    public string Artikelnummer { get; set; } = string.Empty;  // Eindeutiger Geschäftsschlüssel
    public string Bezeichnung   { get; set; } = string.Empty;
    public string? Bezeichnung2 { get; set; }

    // Mengen / Einheit
    public string  Einheit      { get; set; } = "STK";

    // Preise
    public decimal VKPreis      { get; set; } = 0m;
    public decimal EKPreis      { get; set; } = 0m;
    public decimal MwstProzent  { get; set; } = 7m;

    // Logistik
    public string? Barcode      { get; set; }

    // Sonstiges
    public string? Notizen      { get; set; }
    public bool    Aktiv        { get; set; } = true;
}