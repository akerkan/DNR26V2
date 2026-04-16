using Dapper;
using DNR26V2.Data.Context;
using DNR26V2.Domain.DTOs;
using DNR26V2.Domain.Entities.MasterData;
using DNR26V2.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DNR26V2.Services.MasterData;

public class CustomerService : ICustomerService
{
    private readonly AppDbContext  _db;
    private readonly DapperContext _dapper;

    public CustomerService(AppDbContext db, DapperContext dapper)
    {
        _db     = db;
        _dapper = dapper;
    }

    // ── Liste via Dapper (performant für DataGridView) ────────────────────────

    public async Task<IReadOnlyList<CustomerListDto>> GetListAsync(CustomerListFilter filter)
    {
        const string sql = """
            SELECT c.Id,
                   c.Kundennummer,
                   c.Kundenname,
                   r.Routencode,
                   cf.Kundenfilter,
                   c.Aktiv,
                   c.Limit,
                   c.Wochenendtour
            FROM   Customer c
            LEFT   JOIN Route          r  ON r.Id  = c.RouteId
            LEFT   JOIN CustomerFilter cf ON cf.Id = c.KundenfilterId
            WHERE  (@Suche    IS NULL
                    OR c.Kundenname   LIKE '%' + @Suche + '%'
                    OR c.Kundennummer LIKE '%' + @Suche + '%')
            AND    (@NurAktiv IS NULL OR c.Aktiv         = @NurAktiv)
            AND    (@RouteId  IS NULL OR c.RouteId        = @RouteId)
            AND    (@FilterId IS NULL OR c.KundenfilterId = @FilterId)
            ORDER  BY c.Kundenname
            """;

        using var conn = _dapper.CreateConnection();
        var result = await conn.QueryAsync<CustomerListDto>(sql, new
        {
            Suche    = filter.Suche,
            NurAktiv = filter.NurAktiv,
            RouteId  = filter.RouteId,
            FilterId = filter.FilterId
        });
        return result.AsList();
    }

    // ── Einzelkunde via EF Core ───────────────────────────────────────────────

    public async Task<Customer?> GetByIdAsync(int id)
        => await _db.Customer
                    .Include(c => c.Route)
                    .Include(c => c.KundenfilterNavigation)
                    .FirstOrDefaultAsync(c => c.Id == id);

    // ── Speichern ─────────────────────────────────────────────────────────────

    public async Task SaveAsync(Customer customer)
    {
        await ValidateAsync(customer);

        if (customer.Id == 0) _db.Customer.Add(customer);
        else                  _db.Customer.Update(customer);

        await _db.SaveChangesAsync();
    }

    // ── Soft-Delete ───────────────────────────────────────────────────────────

    public async Task SetActiveAsync(int id, bool aktiv)
    {
        var customer = await _db.Customer.FindAsync(id)
            ?? throw new InvalidOperationException("Kunde nicht gefunden.");
        customer.Aktiv = aktiv;
        await _db.SaveChangesAsync();
    }

    // ── Validierung ───────────────────────────────────────────────────────────

    private async Task ValidateAsync(Customer customer)
    {
        if (string.IsNullOrWhiteSpace(customer.Kundennummer))
            throw new ValidationException("Kundennummer ist ein Pflichtfeld.");

        if (string.IsNullOrWhiteSpace(customer.Kundenname))
            throw new ValidationException("Kundenname ist ein Pflichtfeld.");

        bool duplicate = await _db.Customer
            .AnyAsync(c => c.Kundennummer == customer.Kundennummer
                        && c.Id != customer.Id);
        if (duplicate)
            throw new ValidationException(
                $"Kundennummer '{customer.Kundennummer}' ist bereits vergeben.");
    }
}