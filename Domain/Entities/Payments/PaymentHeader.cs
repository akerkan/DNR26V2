namespace DNR26V2.Domain.Entities.Payments;


public class PaymentHeader : AuditableEntity
{
    public int Id { get; set; }
    public string Zahlungsnummer { get; set; } = string.Empty;
    public int KundeId { get; set; }
    public DateTime Buchungsdatum { get; set; }
    public string? Notiz { get; set; }

    public ICollection<PaymentLine> Lines { get; set; } = new List<PaymentLine>();
}
