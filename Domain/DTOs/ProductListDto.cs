namespace DNR26V2.Domain.DTOs;

public sealed class ProductListDto
{
    public int     Id            { get; set; }
    public string  Artikelnummer { get; set; } = string.Empty;
    public string  Bezeichnung   { get; set; } = string.Empty;
    public string  Einheit       { get; set; } = string.Empty;
    public decimal VKPreis       { get; set; }
    public decimal MwstProzent   { get; set; }
    public string? Feld1         { get; set; }
    public string? Feld2         { get; set; }
    public string? Feld3         { get; set; }
    public string? Feld4         { get; set; }
    public string? Printfarbe    { get; set; }
    public bool    Aktiv         { get; set; }
}