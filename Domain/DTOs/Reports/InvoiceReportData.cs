namespace DNR26V2.Domain.DTOs.Reports;

/// <summary>
/// Complete data package for one invoice print job.
/// Returned by IInvoiceReportDataService.
/// </summary>
public class InvoiceReportData
{
    public InvoicePrintHeaderDto Header { get; set; } = null!;
    public IReadOnlyList<InvoicePrintLineDto> Lines { get; set; } = [];

    /// <summary>Suggested filename for PDF output, e.g. Rechnung_RE20260001.pdf</summary>
    public string SuggestedFileName { get; set; } = string.Empty;
}