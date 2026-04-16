namespace DNR26V2.Domain.DTOs;

/// <summary>
/// Schlankes DTO für die Kundenliste (Dapper-Query).
/// Enthält nur die für die Listenansicht benötigten Felder.
/// </summary>
public sealed record CustomerListDto(
    int     Id,
    string  Kundennummer,
    string  Kundenname,
    string? Routencode,
    string? Kundenfilter,
    bool    Aktiv,
    decimal Limit,
    bool    Wochenendtour);