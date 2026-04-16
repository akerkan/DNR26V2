using DNR26V2.Domain.Entities.MasterData;

namespace DNR26V2.Services.MasterData;

public interface ICustomerFilterService
{
    Task<IReadOnlyList<CustomerFilter>> GetAllAsync();
    Task SaveAsync(CustomerFilter filter);
    Task DeleteAsync(int id);
}