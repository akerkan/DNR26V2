using DNR26V2.Domain.Entities.MasterData;
using DNR26V2.Domain.Enums;

namespace DNR26V2.Domain.Entities.Deliveries;

public class DeliveryHeader : AuditableEntity
{
    public int            Id                 { get; set; }
    public string         Lieferscheinnummer { get; set; } = string.Empty;
    public int            KundeId            { get; set; }
    public int?           AuftragId          { get; set; }
    public DateTime       LieferDatum        { get; set; }
    public DeliveryStatus Status             { get; set; } = DeliveryStatus.Offen;
    public string?        Notiz              { get; set; }

    // Navigation
    public MasterData.Customer            Kunde   { get; set; } = null!;
    public Orders.Order?                  Auftrag { get; set; }
    public ICollection<DeliveryLine>      Zeilen  { get; set; } = [];

    // ── Header totals ─────────────────────────────────────────────────────────
    public decimal Gesamtnetto   { get; set; }
    public decimal Gesamtmwst    { get; set; }
    public decimal Gesamtbrutto  { get; set; }

    public int? TurWertId { get; set; }
    public ProductAttributeValue? TurWert { get; set; }
    public int? RoutenFolgeWertId { get; set; }
    public ProductAttributeValue? RoutenFolgeWert { get; set; }
}