namespace DNR26V2.Domain.DTOs.Etikett;

/// <summary>
/// All data required to render one label.
/// </summary>
public class EtiketDruckData
{
    public string  Kundenname       { get; set; } = string.Empty;
    public string  Produktname      { get; set; } = string.Empty;
    public string? Untertitel       { get; set; }
    public string? Zutaten          { get; set; }   // HTML stripped
    public string? Hinweis          { get; set; }
    public string? Printfarbe       { get; set; }
    public string? Barcode          { get; set; }
    public DateTime LieferDatum     { get; set; }   // Lieferungsdatum = Charge/eingefroren = MHD basis
    public DateTime HerstellDatum   { get; set; }   // Herstellungsdatum (shown separately)
    public decimal Menge            { get; set; }
    public decimal Gewicht          { get; set; }
    public int     Kopien           { get; set; } = 1;

    // From AppSetup
    public string  Firmenname       { get; set; } = string.Empty;
    public string? FirmenAdresse    { get; set; }
    public string? FirmenTelefon    { get; set; }
    public string? FirmenEmail      { get; set; }
    public string? LogoPfad         { get; set; }
}
