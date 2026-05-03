namespace DNR26V2.Domain.DTOs.Etikett;

public class EtiketProduktDto
{
    public int     KundeId       { get; set; }
    public string  Kundenname    { get; set; } = string.Empty;
    public int     ArtikelId     { get; set; }
    public string  Artikelnummer { get; set; } = string.Empty;
    public string  Produktname   { get; set; } = string.Empty;
    public decimal Menge         { get; set; }
    public decimal Gewicht       { get; set; }
    public string? Printfarbe    { get; set; }
    public string? Feld1         { get; set; }
    public string? Feld2         { get; set; }
    public string? Feld3         { get; set; }
    public string? Barcode       { get; set; }
}
