using DNR26V2.Domain.Enums;

namespace DNR26V2.Domain.DTOs;

public sealed class AuftragListDto
{
    public int         Id               { get; set; }
    public string      AuftragNr        { get; set; } = string.Empty;
    public DateTime    Lieferdatum      { get; set; }
    public string      Kundenname       { get; set; } = string.Empty;
    public string?     Tour             { get; set; }
    public OrderStatus Status           { get; set; }
    public string?     LieferscheinNr   { get; set; }
    public int         AnzahlPositionen { get; set; }
    public decimal     Gesamtbetrag     { get; set; }
    public int         KundeId          { get; set; }   // for double-click navigation
}