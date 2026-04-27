namespace DNR26V2.Domain.DTOs.Reports;

/// <summary>
/// One invoice line for print. All amounts are positive (DB truth).
/// </summary>
public class InvoicePrintLineDto
{
    public string Rechnungsnummer { get; set; } = string.Empty;
    public string Artikelnummer { get; set; } = string.Empty;
    public string Bezeichnung { get; set; } = string.Empty;
    public decimal Menge { get; set; }
    public decimal Gewicht { get; set; }
    public decimal Preis { get; set; }
    public decimal MwstProzent { get; set; }
    public decimal LineAmount { get; set; }
    public decimal VatAmount { get; set; }
    public decimal AmountInclVat { get; set; }
    public string? Lieferscheinnummer { get; set; }
    public DateTime? Lieferdatum { get; set; }
    public string? GruppierungsRechnungsnummer { get; set; } // New field for grouping invoice numbers
}