using DNR26V2.Domain.DTOs.Reports;

namespace DNR26V2.Services.Reports;

public interface IDeliveryReportDataService
{
    Task<DeliveryReportData> GetDeliveryReportDataAsync(int deliveryId);
    Task<IReadOnlyList<DeliveryReportData>> GetDeliveryReportDataAsync(IEnumerable<int> deliveryIds);
}
