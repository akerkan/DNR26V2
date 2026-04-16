using DNR26V2.Data.Context;
using DNR26V2.Domain.Entities.MasterData;
using DNR26V2.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DNR26V2.Services.MasterData;

public class DriverService : IDriverService
{
    private readonly AppDbContext _db;
    public DriverService(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<Driver>> GetAllActiveAsync()
        => await _db.Driver
                    .Where(d => d.Aktiv)
                    .OrderBy(d => d.Nachname)
                    .ThenBy(d => d.Vorname)
                    .ToListAsync();

    public async Task SaveAsync(Driver driver)
    {
        if (string.IsNullOrWhiteSpace(driver.Fahrercode))
            throw new ValidationException("Fahrercode ist ein Pflichtfeld.");
        if (string.IsNullOrWhiteSpace(driver.Nachname))
            throw new ValidationException("Nachname ist ein Pflichtfeld.");

        if (driver.Id == 0) _db.Driver.Add(driver);
        else                _db.Driver.Update(driver);

        await _db.SaveChangesAsync();
    }

    public async Task SetActiveAsync(int id, bool aktiv)
    {
        var driver = await _db.Driver.FindAsync(id)
            ?? throw new InvalidOperationException("Fahrer nicht gefunden.");
        driver.Aktiv = aktiv;
        await _db.SaveChangesAsync();
    }
}