namespace DNR26V2.Domain.DTOs;

public sealed class ProductListFilter
{
    public string? Suche    { get; set; }
    public bool?   NurAktiv { get; set; }
}