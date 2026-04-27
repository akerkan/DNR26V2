using DNR26V2.Domain.DTOs.Payments;

namespace DNR26V2.Services.Payments;

public interface ICustomerAccountService
{
    Task<IReadOnlyList<CustomerAccountEntryDto>> GetEntriesAsync(int kundeId, DateTime von, DateTime bis);
}
