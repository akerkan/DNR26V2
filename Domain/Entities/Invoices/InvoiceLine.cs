using DNR26V2.Domain.Entities;
using DNR26V2.Domain.Entities.MasterData;

public class InvoiceLine : AuditableEntity
{
    public int     Id           { get; set; }
    public int     RechnungId   { get; set; }
    public int     ArtikelId    { get; set; }
    public decimal Menge        { get; set; }
    public decimal Preis        { get; set; }
    public decimal MwstProzent  { get; set; } = 7.00m;
    public decimal Gesamtpreis  { get; set; }  // calculated: Menge * Preis
    public string? Notiz        { get; set; }

    // Navigation
    public InvoiceHeader Rechnung { get; set; } = null!;
    public Product       Artikel  { get; set; } = null!;
}