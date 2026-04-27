namespace DNR26V2.Domain.DTOs.Reports;

public class DeliveryReportData
{
    public DeliveryPrintHeaderDto Header { get; set; } = null!;
    public IReadOnlyList<DeliveryPrintLineDto> Lines { get; set; } = [];
    public string SuggestedFileName { get; set; } = string.Empty;
}
