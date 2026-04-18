using DNR26V2.Domain.Enums;

namespace DNR26V2.Domain.DTOs;

public sealed class OrderKundeListDto
{
    public int           Id             { get; set; }
    public string        Kundennummer   { get; set; } = string.Empty;
    public string        Kundenname     { get; set; } = string.Empty;
    public string?       Tur            { get; set; }
    public int           Routenfolge    { get; set; }
    public int?          AuftragId      { get; set; }
    public OrderStatus?  AuftragStatus  { get; set; }
}