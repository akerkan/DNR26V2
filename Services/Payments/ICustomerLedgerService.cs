using DNR26V2.Domain.DTOs;

namespace DNR26V2.Services.Payments;

public interface ICustomerLedgerService
{
    Task<IReadOnlyList<CustomerLedgerEntryDto>> GetLedgerAsync(int kundeId, DateTime von, DateTime bis);
}
