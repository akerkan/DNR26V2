namespace DNR26V2.Domain.DTOs.Reports;

public class KundenkontoPrintHeaderDto
{
    public int KundeId { get; set; }
    public string KundenNr { get; set; } = string.Empty;
    public string KundeName { get; set; } = string.Empty;
    public DateTime ZeitraumVon { get; set; }
    public DateTime ZeitraumBis { get; set; }
    public decimal Anfangssaldo { get; set; }
    public decimal Endsaldo { get; set; }
    public string Firmenname { get; set; } = string.Empty;
    public string FirmaAdresse { get; set; } = string.Empty;
    public string FirmaPLZ { get; set; } = string.Empty;
    public string FirmaOrt { get; set; } = string.Empty;
    public string FirmaLand { get; set; } = string.Empty;
    public string FirmaTelefon { get; set; } = string.Empty;
    public string FirmaEmail { get; set; } = string.Empty;
    public string FirmaUstIdNr { get; set; } = string.Empty;
    public bool LogoVerwenden { get; set; }
    public string? LogoPfad { get; set; }
    public bool BriefpapierVerwenden { get; set; }
}
