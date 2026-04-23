using DNR26V2.Domain.Enums;

namespace DNR26V2.Domain.DTOs;

public sealed class OrderLineDto
{
    public int        OrderLineId   { get; set; }
    public int        ArtikelId     { get; set; }
    public string     Artikelnummer { get; set; } = string.Empty;
    public string     Produktname   { get; set; } = string.Empty;
    public decimal    Menge         { get; set; }
    public decimal    MengeGeliefert  { get; set; }   // cumulative delivered
    public decimal    MengeFakturiert { get; set; }   // cumulative invoiced
    public decimal    Offen           => Menge - MengeGeliefert;  // display only
    public decimal    Gewicht       { get; set; }
    public decimal    Preis         { get; set; }
    public string?    Notiz         { get; set; }
    public PreisFormel PreisFormel  { get; set; } = PreisFormel.MengeXPreis;
}