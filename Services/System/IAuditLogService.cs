namespace DNR26V2.Services.System;

public interface IAuditLogService
{
    /// <summary>
    /// Protokolliert eine kritische Aktion (Storno, Statuswechsel, Korrektur).
    /// Wirft keine Exception – bei Fehler wird still ignoriert.
    /// </summary>
    Task LogAsync(
        string  tabellenname,
        int     datensatzId,
        string  aktion,
        string? belegnummer = null,
        string? grund       = null,
        string? alterWert   = null,
        string? neuerWert   = null);
}