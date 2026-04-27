namespace DNR26V2.Domain.DTOs.Reports;

public class OrderReportData
{
    public OrderPrintHeaderDto Header { get; set; } = null!;
    public IReadOnlyList<OrderPrintLineDto> Lines { get; set; } = [];
    public string SuggestedFileName { get; set; } = string.Empty;
}
