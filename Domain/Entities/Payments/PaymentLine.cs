namespace DNR26V2.Domain.Entities.Payments;

public enum PaymentMethod { Bar = 0, Bank = 1 }

public class PaymentLine : AuditableEntity
{
    public int Id { get; set; }
    public int PaymentHeaderId { get; set; }
    public int ReferenceType { get; set; } // 0=InvoiceLine,1=DeliveryLine etc. (simple)
    public int ReferenceId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public decimal Amount { get; set; }
    public string? Notiz { get; set; }

    public PaymentHeader Header { get; set; } = null!;
}
