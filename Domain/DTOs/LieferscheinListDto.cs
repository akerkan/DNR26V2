using DNR26V2.Domain.Enums;

namespace DNR26V2.Domain.DTOs;

public sealed class LieferscheinListDto
{
    public int            Id               { get; set; }
    public string         LieferscheinNr   { get; set; } = string.Empty;
    public DateTime       Lieferdatum      { get; set; }
    public string         Kundenname       { get; set; } = string.Empty;
    public string?        Tour             { get; set; }
    public string?        AuftragNr        { get; set; }
    public DeliveryStatus Status           { get; set; }
    public int            AnzahlPositionen { get; set; }
    public decimal        Gesamtbetrag     { get; set; }
    public int            KundeId          { get; set; }
}