namespace DNR26V2.Domain.DTOs;

public class SammelrechnungResultDto
{
    public int           Erstellt { get; set; }
    public List<int>     RechnungIds { get; set; } = [];
    public List<string>  Fehler   { get; set; } = [];
}