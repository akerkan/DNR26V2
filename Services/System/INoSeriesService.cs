namespace DNR26V2.Services.System;

public interface INoSeriesService
{
    /// <summary>
    /// Gibt die nächste Belegnummer für den angegebenen Seriencode zurück.
    /// Thread-safe: verwendet UPDLOCK für gleichzeitige Multi-User-Anfragen.
    /// Format: {Praefix}-{YY}-{MMDD}-{NNN} z.B. RE-26-0416-001
    /// </summary>
    Task<string> GetNextNumberAsync(string seriencode, DateTime belegdatum);
}