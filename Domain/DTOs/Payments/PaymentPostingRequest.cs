namespace DNR26V2.Domain.DTOs.Payments;

/// <summary>
/// One row from the Zahlungseingänge grid: maps to one ledger entry
/// with user-entered Bar and Bank amounts.
/// </summary>
public class PaymentPostingRowItem
{
    public int     ReferenceId   { get; set; }   // FK into the source document line
    public int     ReferenceType { get; set; }   // 0 = InvoiceLine, 1 = DeliveryLine
    public string  BelegArt      { get; set; } = string.Empty;   // display only
    public decimal OffenerBetrag { get; set; }   // open amount as shown in UI
    public decimal Bar           { get; set; }
    public decimal Bank          { get; set; }
    public string? Notiz         { get; set; }
}

/// <summary>
/// Full posting request sent from the Zahlungseingänge form to the service.
/// One request = one PaymentHeader + N PaymentLines.
/// </summary>
public class PaymentPostingRequest
{
    public int      KundeId       { get; set; }
    public DateTime Buchungsdatum { get; set; }
    public string?  Notiz         { get; set; }
    public List<PaymentPostingRowItem> Rows { get; set; } = [];
}
