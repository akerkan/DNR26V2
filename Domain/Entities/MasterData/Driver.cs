namespace DNR26V2.Domain.Entities.MasterData;

/// <summary>Fahrer-Stammdaten. Wird Lieferungen/Touren zugewiesen.</summary>
public class Driver : AuditableEntity
{
    public int     Id            { get; set; }
    public string  Fahrercode    { get; set; } = string.Empty;
    public string  Vorname       { get; set; } = string.Empty;
    public string  Nachname      { get; set; } = string.Empty;
    public string? Telefonnummer { get; set; }
    public bool    Aktiv         { get; set; } = true;
}