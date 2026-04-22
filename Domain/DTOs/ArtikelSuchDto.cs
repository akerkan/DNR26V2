using DNR26V2.Domain.Enums;

namespace DNR26V2.Domain.DTOs;

public sealed class ArtikelSuchDto
{
    public int        ArtikelId     { get; set; }
    public string     Artikelnummer { get; set; } = string.Empty;
    public string     Produktname   { get; set; } = string.Empty;
    public decimal    VKPreis       { get; set; }
    public PreisFormel PreisFormel  { get; set; } = PreisFormel.MengeXPreis;
}