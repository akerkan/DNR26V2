namespace DNR26V2.Domain.DTOs.Payments;

/// <summary>
/// One already-posted payment line, shown in the Zahlungseingänge history grid.
/// </summary>
public class PaymentHistoryDto
{
    public int      PaymentHeaderId { get; set; }
    public int      PaymentLineId   { get; set; }   // for storno reference
    public string   Zahlungsnummer  { get; set; } = string.Empty;
    public DateTime Buchungsdatum   { get; set; }
    public int      ReferenceType   { get; set; }   // 0 = InvoiceLine, 1 = DeliveryLine
    public int      ReferenceId     { get; set; }
    public string   BelegNr         { get; set; } = string.Empty;   // best-effort join
    public string   Zahlungsart     { get; set; } = string.Empty;   // "Bar" / "Bank"
    public decimal  Amount          { get; set; }
    public bool     IsStorno        { get; set; }   // true when Amount < 0
    public string?  Notiz           { get; set; }
    public string   ErstelltVon     { get; set; } = string.Empty;
}
