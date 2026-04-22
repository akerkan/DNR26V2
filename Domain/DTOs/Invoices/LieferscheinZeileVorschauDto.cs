namespace DNR26V2.Domain.DTOs;

public class LieferscheinZeileVorschauDto
{
    public string  Artikelnummer    { get; set; } = string.Empty;
    public string  Bezeichnung      { get; set; } = string.Empty;
    public decimal Menge            { get; set; }
    public decimal Gewicht          { get; set; }
    public decimal FakturierteMenge { get; set; }
    public decimal Preis            { get; set; }
    public decimal LineAmount       { get; set; }   // renamed from Gesamtpreis
}