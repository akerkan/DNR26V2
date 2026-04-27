namespace DNR26V2.Domain.DTOs.Reports;

public class DeliveryPrintLineDto
{
    public string Lieferscheinnummer { get; set; } = string.Empty;
    public string Artikelnummer { get; set; } = string.Empty;
    public string Bezeichnung { get; set; } = string.Empty;
    public decimal Menge { get; set; }
    public decimal Gewicht { get; set; }
    public decimal Preis { get; set; }
    public decimal MwstProzent { get; set; }
    public decimal LineAmount { get; set; }
    public decimal VatAmount { get; set; }
    public decimal AmountInclVat { get; set; }
}
