using DNR26V2.Domain.Entities.MasterData;

namespace DNR26V2.Services.MasterData;

public interface IRouteService
{
    Task<IReadOnlyList<Route>> GetAllActiveAsync();
    Task SaveAsync(Route route);
}