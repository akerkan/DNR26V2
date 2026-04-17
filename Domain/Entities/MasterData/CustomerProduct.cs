namespace DNR26V2.Domain.Entities.MasterData;

/// <summary>
/// Kundenprodukt-Schablone: Standard-Menge, Gewicht und Preis pro Kunde/Artikel.
/// Produktspezifische Felder (z.B. Rohrtyp, Form, Länge) werden dynamisch
/// über CustomerProductAttributeMapping abgebildet.
/// </summary>
public class CustomerProduct : AuditableEntity
{
    public int     Id        { get; set; }
    public int     KundeId   { get; set; }
    public int     ArtikelId { get; set; }

    /// <summary>Kundenspezifischer Preis (überschreibt VKPreis aus Product).</summary>
    public decimal Preis     { get; set; }

    /// <summary>Standard-Bestellmenge für diesen Kunden.</summary>
    public decimal Menge     { get; set; }

    /// <summary>Standard-Gewicht für diesen Kunden.</summary>
    public decimal Gewicht   { get; set; }

    public bool    Aktiv     { get; set; } = true;

    // Navigation
    public Customer  Customer  { get; set; } = null!;
    public Product   Product   { get; set; } = null!;

    public ICollection<CustomerProductAttributeMapping> Spezifikationen { get; set; } = [];
}