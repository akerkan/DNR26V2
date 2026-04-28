using DNR26V2.Domain.DTOs.Reports;

namespace DNR26V2.Services.Reports;

public interface IKundenkontoReportDataService
{
    Task<KundenkontoReportData> GetKundenkontoReportDataAsync(int kundeId, DateTime von, DateTime bis);
}
