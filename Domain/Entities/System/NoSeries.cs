namespace DNR26V2.Domain.Entities.System;

/// <summary>
/// Nummernserie für automatische Belegnummern-Vergabe.
/// Format: {Praefix}{Trennzeichen}{YY}{Trennzeichen}{MMDD}{Trennzeichen}{NNN}
/// Beispiel: RE-26-0416-001
/// </summary>
public class NoSeries : AuditableEntity
{
    public int      Id                      { get; set; }
    public string   Seriencode              { get; set; } = string.Empty;
    public string?  Beschreibung            { get; set; }
    public string   Praefix                 { get; set; } = string.Empty;
    public string?  LetzteVerwendeteNr      { get; set; }
    public DateTime? LetztesVerwendetesDatum { get; set; }
    public string   Nummernformat           { get; set; } = "000";
    public char     Trennzeichen            { get; set; } = '-';
    public bool     Aktiv                   { get; set; } = true;
}