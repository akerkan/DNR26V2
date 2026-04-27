using DNR26V2.Domain.DTOs.Reports;

namespace DNR26V2.Services.Reports;

public interface IInvoiceReportDataService
{
    Task<InvoiceReportData> GetInvoiceReportDataAsync(int invoiceId);
    Task<IReadOnlyList<InvoiceReportData>> GetInvoiceReportDataAsync(IEnumerable<int> invoiceIds);
}
