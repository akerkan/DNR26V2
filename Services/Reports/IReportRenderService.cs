namespace DNR26V2.Services.Reports;

public interface IReportRenderService
{
    Task PreviewInvoiceAsync(int invoiceId);
    Task PreviewInvoicesAsync(IEnumerable<int> invoiceIds);

    Task PrintInvoiceAsync(int invoiceId, string? printerName = null);
    Task PrintInvoicesAsync(IEnumerable<int> invoiceIds, string? printerName = null);

    Task<byte[]> RenderInvoicePdfAsync(int invoiceId);
    Task<byte[]> RenderInvoicesPdfAsync(IEnumerable<int> invoiceIds);
}
