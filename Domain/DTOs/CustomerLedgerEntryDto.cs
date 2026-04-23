namespace DNR26V2.Domain.DTOs;

public class CustomerLedgerEntryDto
{
    public DateTime Datum { get; set; }
    public int KundeId { get; set; }
    public string KundeName { get; set; } = string.Empty;
    public string BelegArt { get; set; } = string.Empty; // Rechnung / Gutschrift / Lieferschein
    public string BelegNr { get; set; } = string.Empty;
    public int ReferenzId { get; set; }
    public decimal Betrag { get; set; }
    public string Richtung { get; set; } = "+"; // "+" or "-"
    public decimal OffenerBetrag { get; set; }
    public string? Notiz { get; set; }
}
