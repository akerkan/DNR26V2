namespace DNR26V2.Domain.DTOs.Reports;

public class OrderPrintHeaderDto
{
    public string Auftragsnummer { get; set; } = string.Empty;
    public DateTime Lieferdatum { get; set; }
    public string KundeName { get; set; } = string.Empty;
    public string KundenNr { get; set; } = string.Empty;
    public string KundenadresseText { get; set; } = string.Empty;

    public string Firmenname { get; set; } = string.Empty;
    public string? FirmaAdresse { get; set; }
    public string? FirmaPLZ { get; set; }
    public string? FirmaOrt { get; set; }
    public string? FirmaLand { get; set; }
    public string? FirmaTelefon { get; set; }
    public string? FirmaEmail { get; set; }
    public string? FirmaSteuernummer { get; set; }
    public string? FirmaUstIdNr { get; set; }
    public bool LogoVerwenden { get; set; }
    public string? LogoPfad { get; set; }
    public bool BriefpapierVerwenden { get; set; }

    public decimal Gesamtnetto { get; set; }
    public decimal Gesamtmwst { get; set; }
    public decimal Gesamtbrutto { get; set; }
}
