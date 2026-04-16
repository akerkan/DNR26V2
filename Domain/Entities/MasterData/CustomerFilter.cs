namespace DNR26V2.Domain.Entities.MasterData;

/// <summary>Kundengruppe / Filterbezeichnung (z.B. „Nord", „Süd", „Industrie").</summary>
public class CustomerFilter : AuditableEntity
{
    public int    Id            { get; set; }
    /// <summary>Anzeige-Name des Filters – muss eindeutig sein.</summary>
    public string Kundenfilter  { get; set; } = string.Empty;

    // Navigation
    public ICollection<Customer> Kunden { get; set; } = [];
}