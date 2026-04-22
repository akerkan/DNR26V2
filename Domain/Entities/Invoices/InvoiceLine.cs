using DNR26V2.Domain.Entities.Deliveries;
using DNR26V2.Domain.Entities.MasterData;

namespace DNR26V2.Domain.Entities.Invoices;

public class InvoiceLine : AuditableEntity
{
    public int     Id               { get; set; }
    public int     RechnungId       { get; set; }
    public int     LieferscheinId   { get; set; }
    public int     DeliveryLineId   { get; set; }   // Teilfakturierung vorbereitet
    public int     ArtikelId        { get; set; }
    public decimal Menge            { get; set; }   // gelieferte Menge aus DeliveryLine
    public decimal FakturierteMenge { get; set; }
    public decimal Preis            { get; set; }
    public decimal MwstProzent      { get; set; } = 7.00m;
    public decimal Gewicht          { get; set; }

    // ── Calculated amount fields ──────────────────────────────────────────────
    public decimal GrossAmount     { get; set; }
    public decimal DiscountProzent { get; set; }
    public decimal DiscountAmount  { get; set; }
    public decimal LineAmount      { get; set; }   // renamed from Gesamtpreis
    public decimal VatAmount       { get; set; }
    public decimal AmountInclVat   { get; set; }

    public string? Notiz            { get; set; }

    public InvoiceHeader  Rechnung     { get; set; } = null!;
    public DeliveryHeader Lieferschein { get; set; } = null!;
    public DeliveryLine   DeliveryLine { get; set; } = null!;
    public Product        Artikel      { get; set; } = null!;
}