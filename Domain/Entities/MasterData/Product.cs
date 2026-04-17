using DNR26V2.Domain.Entities;

namespace DNR26V2.Domain.Entities.MasterData;

/// <summary>
/// Produktstammdaten.
/// Soft-Delete via Aktiv=false — physisches Löschen ist verboten.
/// </summary>
public class Product : AuditableEntity
{
    public int     Id            { get; set; }
    public string  Artikelnummer { get; set; } = string.Empty;
    public string  Bezeichnung   { get; set; } = string.Empty;
    public string? Bezeichnung2  { get; set; }
    public string  Einheit       { get; set; } = "STK";

    public decimal VKPreis      { get; set; } = 0m;
    public decimal EKPreis      { get; set; } = 0m;
    public decimal MwstProzent  { get; set; } = 7m;

    // Etikett-Felder (werden auf dem Label gedruckt)
    public string? Feld1       { get; set; }
    public string? Feld2       { get; set; }
    public string? Feld3       { get; set; }
    public string? Feld4       { get; set; }
    public string? Printfarbe  { get; set; }

    public string? Barcode     { get; set; }
    public string? Notizen     { get; set; }
    public bool    Aktiv       { get; set; } = true;
}