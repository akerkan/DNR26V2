namespace DNR26V2.Services.Reports;

public interface IReportRenderService
{
    Task PreviewInvoiceAsync(int invoiceId);
    Task PreviewInvoicesAsync(IEnumerable<int> invoiceIds);

    Task PrintInvoiceAsync(int invoiceId, string? printerName = null);
    Task PrintInvoicesAsync(IEnumerable<int> invoiceIds, string? printerName = null);

    Task<byte[]> RenderInvoicePdfAsync(int invoiceId);
    Task<byte[]> RenderInvoicesPdfAsync(IEnumerable<int> invoiceIds);

    Task PreviewOrderAsync(int orderId);
    Task PrintOrderAsync(int orderId, string? printerName = null);
    Task<byte[]> RenderOrderPdfAsync(int orderId);

    Task PreviewDeliveryAsync(int deliveryId);
    Task PreviewDeliveriesAsync(IEnumerable<int> deliveryIds);
    Task PrintDeliveryAsync(int deliveryId, string? printerName = null);
    Task PrintDeliveriesAsync(IEnumerable<int> deliveryIds, string? printerName = null);
    Task<byte[]> RenderDeliveryPdfAsync(int deliveryId);
}
