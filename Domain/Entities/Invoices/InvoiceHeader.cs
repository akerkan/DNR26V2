using DNR26V2.Domain.Entities;
using DNR26V2.Domain.Entities.Deliveries;
using DNR26V2.Domain.Entities.MasterData;

public class InvoiceHeader : AuditableEntity
{
    public int      Id              { get; set; }
    public string   Rechnungsnummer { get; set; } = string.Empty;
    public int      KundeId         { get; set; }
    public int?     LieferscheinId  { get; set; }  // nullable — Nachlieferungs-Rechnung ohne LS
    public DateTime Rechnungsdatum  { get; set; }
    public DateTime? Faelligkeitsdatum { get; set; }
    //public InvoiceStatus Status     { get; set; } = InvoiceStatus.Offen;

    // Snapshot fields (frozen at time of invoice creation)
    public string  KundenName       { get; set; } = string.Empty;
    public string? KundenAdresse    { get; set; }
    public string? KundenPLZ        { get; set; }
    public string? KundenOrt        { get; set; }
    public string? KundenSteuernummer { get; set; }

    // Payment terms
    public decimal Gesamtbetrag     { get; set; }
    public decimal MwstBetrag       { get; set; }
    public decimal Nettobetrag      { get; set; }
    public decimal OffenerBetrag    { get; set; }  // for partial payments

    public string? Notiz            { get; set; }

    // Navigation
    public Customer          Kunde        { get; set; } = null!;
    public DeliveryHeader?   Lieferschein { get; set; }
    //public ICollection<InvoiceLine> Zeilen { get; set; } = [];
}