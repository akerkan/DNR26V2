namespace DNR26V2.Domain.DTOs;

/// <summary>Filter-Parameter für die Kundenliste.</summary>
public sealed class CustomerListFilter
{
    public string? Suche    { get; init; }
    public bool?   NurAktiv { get; init; }
    public int?    RouteId  { get; init; }
    public int?    FilterId { get; init; }
}