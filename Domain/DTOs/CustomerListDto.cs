namespace DNR26V2.Domain.DTOs;

/// <summary>
/// Schlankes DTO für die Kundenliste (Dapper-Query).
/// Enthält nur die für die Listenansicht benötigten Felder.
/// Die Parameter-Namen müssen mit den Spalten/Aliases der SQL-Abfrage übereinstimmen.
/// </summary>
public sealed record CustomerListDto(
    int     Id,
    string  Kundennummer,
    string  Kundenname,
    string? Tur,
    string? Kundenfilter,
    bool    Aktiv,
    decimal Limit);