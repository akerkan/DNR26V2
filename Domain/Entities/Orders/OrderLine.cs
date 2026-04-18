namespace DNR26V2.Domain.Entities.Orders;

public class OrderLine : AuditableEntity
{
    public int     Id        { get; set; }
    public int     AuftragId { get; set; }
    public int     ArtikelId { get; set; }
    public decimal Menge     { get; set; }
    public decimal Gewicht   { get; set; }
    public decimal Preis     { get; set; }
    public string? Notiz     { get; set; }

    // Navigation
    public Order                      Auftrag { get; set; } = null!;
    public MasterData.Product         Artikel { get; set; } = null!;
}