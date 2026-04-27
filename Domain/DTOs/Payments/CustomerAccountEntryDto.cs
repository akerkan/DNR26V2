namespace DNR26V2.Domain.DTOs.Payments;

public class CustomerAccountEntryDto
{
    public DateTime Datum { get; set; }
    public string Belegart { get; set; } = string.Empty;
    public string BelegNr { get; set; } = string.Empty;
    public string Beschreibung { get; set; } = string.Empty;
    public decimal Soll { get; set; }
    public decimal Haben { get; set; }
    public decimal Saldo { get; set; }
    public string? Notiz { get; set; }

    public int SortReferenz { get; set; }
}
