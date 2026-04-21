using DNR26V2.Domain.Entities.MasterData;
using DNR26V2.Domain.Enums;

namespace DNR26V2.Domain.Entities.Invoices;

public class InvoiceHeader : AuditableEntity
{
    public int           Id                { get; set; }
    public string        Rechnungsnummer   { get; set; } = string.Empty;
    public int           KundeId           { get; set; }
    public DateTime      Rechnungsdatum    { get; set; }
    public DateTime      Von               { get; set; }
    public DateTime      Bis               { get; set; }
    public InvoiceStatus Status            { get; set; } = InvoiceStatus.Offen;
    public string?       Notiz             { get; set; }
    public bool          IstSammelrechnung { get; set; } = false;

    // Navigation
    public Customer                  Kunde  { get; set; } = null!;
    public ICollection<InvoiceLine>  Zeilen { get; set; } = [];
}