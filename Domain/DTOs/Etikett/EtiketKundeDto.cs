namespace DNR26V2.Domain.DTOs.Etikett;

public class EtiketKundeDto
{
    public int    Id          { get; set; }
    public string Kundenname  { get; set; } = string.Empty;
    public string Kundennummer{ get; set; } = string.Empty;
    public string? Tur        { get; set; }
}
