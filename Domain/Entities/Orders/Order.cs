using DNR26V2.Domain.Entities.MasterData;
using DNR26V2.Domain.Enums;

namespace DNR26V2.Domain.Entities.Orders;

public class Order : AuditableEntity
{
    public int         Id             { get; set; }
    public string      Auftragsnummer { get; set; } = string.Empty;
    public int         KundeId        { get; set; }
    public DateTime    LieferDatum    { get; set; }
    public OrderStatus Status         { get; set; } = OrderStatus.Offen;
    public string?     Notiz          { get; set; }

    // ── Header totals ─────────────────────────────────────────────────────────
    public decimal Gesamtnetto   { get; set; }
    public decimal Gesamtmwst    { get; set; }
    public decimal Gesamtbrutto  { get; set; }

    // Navigation
    public Customer                   Kunde  { get; set; } = null!;
    public ICollection<OrderLine>     Zeilen { get; set; } = [];
}