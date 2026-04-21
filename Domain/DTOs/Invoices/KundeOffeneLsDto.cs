namespace DNR26V2.Domain.DTOs;

public class KundeOffeneLsDto
{
    public int    KundeId          { get; set; }
    public string Kundennummer     { get; set; } = string.Empty;
    public string Kundenname       { get; set; } = string.Empty;
    public int    AnzahlOffeneLs   { get; set; }
    public decimal GesamtbetragOffen { get; set; }
}