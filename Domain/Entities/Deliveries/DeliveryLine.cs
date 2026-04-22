using DNR26V2.Domain.Entities.Orders;

namespace DNR26V2.Domain.Entities.Deliveries;

public class DeliveryLine : AuditableEntity
{
    public int     Id             { get; set; }
    public int     LieferscheinId { get; set; }
    public int     ArtikelId      { get; set; }
    public decimal Menge          { get; set; }
    public decimal MengeGeliefert { get; set; }
    public decimal Gewicht        { get; set; }
    public decimal Preis          { get; set; }
    public string? Notiz          { get; set; }

    // Navigation
    public DeliveryHeader         Lieferschein  { get; set; } = null!;
    public MasterData.Product     Artikel       { get; set; } = null!;

    public int?        AuftragZeileId { get; set; }   // nullable FK → OrderLine
    public OrderLine?  AuftragZeile   { get; set; }   // navigation

    // Bestehende Properties beibehalten — NUR HINZUFÜGEN:
    public decimal MengeFakturiert { get; set; } = 0m;   // akkumuliert über mehrere Rechnungen

    // ── Calculated amount fields ──────────────────────────────────────────────
    public decimal GrossAmount     { get; set; }
    public decimal DiscountProzent { get; set; }
    public decimal DiscountAmount  { get; set; }
    public decimal LineAmount      { get; set; }
    public decimal MwstProzent     { get; set; } = 7.00m;   // ← NEU: pro Zeile, nie vom Header
    public decimal VatAmount       { get; set; }
    public decimal AmountInclVat   { get; set; }
}