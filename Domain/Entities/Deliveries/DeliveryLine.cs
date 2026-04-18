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
    public DeliveryHeader         Lieferschein { get; set; } = null!;
    public MasterData.Product     Artikel      { get; set; } = null!;
}