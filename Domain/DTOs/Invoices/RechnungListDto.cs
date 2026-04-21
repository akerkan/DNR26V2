using DNR26V2.Domain.Enums;

namespace DNR26V2.Domain.DTOs;

public class RechnungListDto
{
    public int           Id                { get; set; }
    public string        Rechnungsnummer   { get; set; } = string.Empty;
    public DateTime      Rechnungsdatum    { get; set; }
    public DateTime      Von               { get; set; }
    public DateTime      Bis               { get; set; }
    public string        Kundenname        { get; set; } = string.Empty;
    public decimal       Gesamtnetto       { get; set; }
    public decimal       Gesamtbrutto      { get; set; }
    public InvoiceStatus Status            { get; set; }
    public bool          IstSammelrechnung { get; set; }
    public int           AnzahlPositionen  { get; set; }
}