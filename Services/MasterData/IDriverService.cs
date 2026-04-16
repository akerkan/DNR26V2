using DNR26V2.Domain.Entities.MasterData;

namespace DNR26V2.Services.MasterData;

public interface IDriverService
{
    Task<IReadOnlyList<Driver>> GetAllActiveAsync();
    Task SaveAsync(Driver driver);
    Task SetActiveAsync(int id, bool aktiv);
}