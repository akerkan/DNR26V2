using DNR26V2.Domain.DTOs.Reports;

namespace DNR26V2.Services.Reports;

public interface IOrderReportDataService
{
    Task<OrderReportData> GetOrderReportDataAsync(int orderId);
}
