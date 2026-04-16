using DNR26V2.Domain.DTOs;
using DNR26V2.Domain.Entities.MasterData;

namespace DNR26V2.Services.MasterData;

public interface ICustomerService
{
    /// <summary>Schnelle Listenabfrage via Dapper.</summary>
    Task<IReadOnlyList<CustomerListDto>> GetListAsync(CustomerListFilter filter);

    /// <summary>Einzelkunde mit Navigation-Properties via EF Core.</summary>
    Task<Customer?> GetByIdAsync(int id);

    /// <summary>Neu anlegen oder aktualisieren (inkl. Validierung).</summary>
    Task SaveAsync(Customer customer);

    /// <summary>Soft-Delete: setzt Aktiv = false/true.</summary>
    Task SetActiveAsync(int id, bool aktiv);
}