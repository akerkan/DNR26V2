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
    public decimal FakturierteMenge { get; set; }   // initial = Menge; später Teilfakturierung
    public decimal Preis            { get; set; }
    public decimal MwstProzent      { get; set; } = 7.00m;
    public decimal Gesamtpreis      { get; set; }   // FakturierteMenge * Preis
    public string? Notiz            { get; set; }
    public decimal Gewicht          { get; set; }

    public InvoiceHeader  Rechnung     { get; set; } = null!;
    public DeliveryHeader Lieferschein { get; set; } = null!;
    public DeliveryLine   DeliveryLine { get; set; } = null!;
    public Product        Artikel      { get; set; } = null!;
}