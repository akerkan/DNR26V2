namespace DNR26V2.Domain.DTOs.Reports;

public class KundenkontoReportData
{
    public KundenkontoPrintHeaderDto Header { get; set; } = null!;
    public List<KundenkontoPrintLineDto> Lines { get; set; } = new();
}
