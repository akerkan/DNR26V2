namespace DNR26V2.Domain.Entities.System;

/// <summary>
/// Protokoll kritischer Geschäftsvorgänge (Storno, Statuswechsel, Korrekturen).
/// NIEMALS löschen oder ändern – Append-only.
/// </summary>
public class AuditLog
{
    public long     Id           { get; set; }
    public string   Tabellenname { get; set; } = string.Empty;
    public int      DatensatzId  { get; set; }
    public string?  Belegnummer  { get; set; }

    /// <summary>z.B. STORNIERT, STATUS_GEAENDERT, ERSTELLT, KORREKTUR</summary>
    public string   Aktion       { get; set; } = string.Empty;
    public string?  AlterWert    { get; set; }
    public string?  NeuerWert    { get; set; }
    public string?  Grund        { get; set; }
    public string   Benutzer     { get; set; } = string.Empty;
    public DateTime Zeitstempel  { get; set; } = DateTime.Now;
    public string?  IPAdresse    { get; set; }
}