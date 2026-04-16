using DNR26V2.Data.Context;
using DNR26V2.Domain.Entities.MasterData;
using DNR26V2.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DNR26V2.Services.MasterData;

public class RouteService : IRouteService
{
    private readonly AppDbContext _db;
    public RouteService(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<Route>> GetAllActiveAsync()
        => await _db.Route
                    .Where(r => r.Aktiv)
                    .OrderBy(r => r.Routencode)
                    .ToListAsync();

    public async Task SaveAsync(Route route)
    {
        if (string.IsNullOrWhiteSpace(route.Routencode))
            throw new ValidationException("Routencode ist ein Pflichtfeld.");

        bool duplicate = await _db.Route
            .AnyAsync(r => r.Routencode == route.Routencode && r.Id != route.Id);
        if (duplicate)
            throw new ValidationException($"Routencode '{route.Routencode}' existiert bereits.");

        if (route.Id == 0) _db.Route.Add(route);
        else               _db.Route.Update(route);

        await _db.SaveChangesAsync();
    }
}