namespace DNR26V2.Domain.DTOs;

public sealed class CustomerProductAttributeDto
{
    public int     Id                { get; set; }
    public int     CustomerProductId { get; set; }
    public int     AttributId        { get; set; }
    public string  AttributName      { get; set; } = string.Empty;
    public int     Feldtyp           { get; set; }  // 0=Lookup, 1=FreeText
    public int?    AttributWertId    { get; set; }
    public string? WertBezeichnung   { get; set; }
    public string? FreierText        { get; set; }
}