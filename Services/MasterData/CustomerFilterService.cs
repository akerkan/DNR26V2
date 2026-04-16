using DNR26V2.Data.Context;
using DNR26V2.Domain.Entities.MasterData;
using DNR26V2.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DNR26V2.Services.MasterData;

public class CustomerFilterService : ICustomerFilterService
{
    private readonly AppDbContext _db;
    public CustomerFilterService(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<CustomerFilter>> GetAllAsync()
        => await _db.CustomerFilter.OrderBy(f => f.Kundenfilter).ToListAsync();

    public async Task SaveAsync(CustomerFilter filter)
    {
        if (string.IsNullOrWhiteSpace(filter.Kundenfilter))
            throw new ValidationException("Filterbezeichnung darf nicht leer sein.");

        bool duplicate = await _db.CustomerFilter
            .AnyAsync(f => f.Kundenfilter == filter.Kundenfilter && f.Id != filter.Id);
        if (duplicate)
            throw new ValidationException($"Filter '{filter.Kundenfilter}' existiert bereits.");

        if (filter.Id == 0) _db.CustomerFilter.Add(filter);
        else                _db.CustomerFilter.Update(filter);

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        bool inUse = await _db.Customer.AnyAsync(c => c.KundenfilterId == id);
        if (inUse)
            throw new ValidationException("Filter wird von Kunden verwendet und kann nicht gelöscht werden.");

        var entity = await _db.CustomerFilter.FindAsync(id);
        if (entity is not null)
        {
            _db.CustomerFilter.Remove(entity);
            await _db.SaveChangesAsync();
        }
    }
}