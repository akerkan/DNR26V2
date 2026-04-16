namespace DNR26V2.Domain.Entities;

/// <summary>
/// Basisklasse für alle auditierbaren Entitäten.
/// Alle Stamm- und Transaktionsdaten erben von dieser Klasse.
/// </summary>
public abstract class AuditableEntity
{
    public DateTime  ErstelltAm  { get; set; }
    public string    ErstelltVon { get; set; } = string.Empty;
    public DateTime? GeaendertAm  { get; set; }
    public string?   GeaendertVon { get; set; }
}