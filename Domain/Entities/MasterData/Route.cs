namespace DNR26V2.Domain.Entities.MasterData;

/// <summary>Liefer-Tour / Route für die Auslieferung.</summary>
public class Route : AuditableEntity
{
    public int    Id               { get; set; }
    public string Routencode       { get; set; } = string.Empty;
    public string Bezeichnung      { get; set; } = string.Empty;
    public bool   IstWochenendtour { get; set; } = false;
    public bool   Aktiv            { get; set; } = true;

    // Navigation
    public ICollection<Customer> Kunden { get; set; } = [];
}