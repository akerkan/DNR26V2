using System.Threading.Tasks;
using DNR26V2.Data.Context;
using DNR26V2.Domain.Entities.System;
using Microsoft.EntityFrameworkCore;

namespace DNR26V2.Services.System;

public class AppSetupService : IAppSetupService
{
    private readonly AppDbContext _db;

    public AppSetupService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<AppSetup> GetAsync()
    {
        // AppSetup ist als Singleton-Row (Id = 1) angelegt via Migration/Seeder.
        var setup = await _db.Set<AppSetup>()
                             .AsNoTracking()
                             .FirstOrDefaultAsync(s => s.Id == 1);

        if (setup is null)
            throw new InvalidOperationException("AppSetup fehlt in der Datenbank (Id = 1). Bitte Migration/Seed prüfen.");

        return setup;
    }

    public async Task SaveAsync(AppSetup setup)
    {
        _db.Update(setup);
        await _db.SaveChangesAsync();
    }
}