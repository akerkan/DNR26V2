namespace DNR26V2.Domain.DTOs;

public sealed class CustomerProductDto
{
    public int     Id           { get; set; }
    public int     KundeId      { get; set; }
    public int     ArtikelId    { get; set; }
    public string  Artikelnummer { get; set; } = string.Empty;
    public string  Produktname  { get; set; } = string.Empty;
    public decimal Preis        { get; set; }
    public decimal Menge        { get; set; }
    public decimal Gewicht      { get; set; }
}