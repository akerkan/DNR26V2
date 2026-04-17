namespace DNR26V2.Domain.DTOs;

public sealed class CustomerListFilter
{
    public string? Suche              { get; set; }
    public bool?   NurAktiv           { get; set; }
    public int?    TourWertId         { get; set; }  // war: RouteId
    public int?    KundenGruppeWertId { get; set; }  // war: FilterId
}