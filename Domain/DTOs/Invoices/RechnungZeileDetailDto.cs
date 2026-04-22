namespace DNR26V2.Domain.DTOs;

public class RechnungZeileDetailDto
{
    public int      Id                 { get; set; }
    public string   Artikelnummer      { get; set; } = string.Empty;
    public string   Bezeichnung        { get; set; } = string.Empty;
    public string   Lieferscheinnummer { get; set; } = string.Empty;
    public DateTime Lieferdatum        { get; set; }
    public decimal  Menge              { get; set; }
    public decimal  FakturierteMenge   { get; set; }
    public decimal  Preis              { get; set; }
    public decimal  MwstProzent        { get; set; }
    public decimal  LineAmount        { get; set; }
    public string?  Notiz              { get; set; }
    public decimal  Gewicht            { get; set; }
}