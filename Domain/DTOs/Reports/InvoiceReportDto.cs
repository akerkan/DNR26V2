namespace DNR26V2.Domain.DTOs.Reports;
public class InvoiceReportDto
{
    public int RechnungId { get; set; }
    public string Rechnungsnummer { get; set; }
    public DateTime Rechnungsdatum { get; set; }

    public DateTime ZeitraumVon { get; set; }
    public DateTime ZeitraumBis { get; set; }

    public int BelegArt { get; set; }
    public string BelegArtText { get; set; }

    public string Kundenname { get; set; }
    public string Kundennummer { get; set; }
    public string KundeAdresse { get; set; }
    public string KundePLZ { get; set; }
    public string KundeOrt { get; set; }

    public decimal Gesamtnetto { get; set; }
    public decimal Gesamtmwst { get; set; }
    public decimal Gesamtbrutto { get; set; }

    public string Artikelnummer { get; set; }
    public string Bezeichnung { get; set; }

    public decimal Menge { get; set; }
    public decimal Gewicht { get; set; }
    public decimal Preis { get; set; }
    public decimal MwstProzent { get; set; }

    public decimal LineAmount { get; set; }
    public decimal VatAmount { get; set; }
    public decimal AmountInclVat { get; set; }

    public string Lieferscheinnummer { get; set; }
    public DateTime Lieferdatum { get; set; }
}