namespace DNR26V2.Domain.DTOs;

public sealed class KundenFactBoxDto
{
    public string    Kundenname      { get; set; } = string.Empty;
    public bool      PreisAusblenden { get; set; }
    public string?   Tour            { get; set; }
    public decimal   Saldo           { get; set; }   // populated in Module 7
    public DateTime? LetzterAuftrag  { get; set; }
    public int       OffeneAuftraege { get; set; }
}