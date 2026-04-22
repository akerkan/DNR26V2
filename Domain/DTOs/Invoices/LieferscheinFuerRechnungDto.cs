namespace DNR26V2.Domain.DTOs;

public class LieferscheinFuerRechnungDto
{
    public int      LieferscheinId     { get; set; }
    public string   Lieferscheinnummer { get; set; } = string.Empty;
    public DateTime Lieferdatum        { get; set; }
    public int      AnzahlPositionen   { get; set; }
    public decimal  Gesamtbetrag       { get; set; }   // netto = SUM(LineAmount)
    public decimal  Gesamtbrutto       { get; set; }   // brutto = SUM(AmountInclVat)
}