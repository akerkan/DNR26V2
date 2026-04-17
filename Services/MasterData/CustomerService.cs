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

    // ── Liste via Dapper ──────────────────────────────────────────────────────

    public async Task<IReadOnlyList<CustomerListDto>> GetListAsync(CustomerListFilter filter)
    {
        const string sql = """
            SELECT c.Id,
                   c.Kundennummer,
                   c.Kundenname,
                   c.Name2,
                   c.Inhaber,
                   c.Telefonnummer,
                   c.Handynummer,
                   c.EMail,
                   c.PLZ,
                   c.Ort,
                   c.Adresse,
                   r1.Routencode                          AS Tur,
                   r2.Routencode                          AS AusnahmeTur,
                   cf.Kundenfilter,
                   c.Routenfolge,
                   c.Limit,
                   c.LiefertMo, c.LiefertDi, c.LiefertMi,
                   c.LiefertDo, c.LiefertFr, c.LiefertSa, c.LiefertSo,
                   c.PreisAusblenden,
                   c.Offen,
                   c.Aktiv
            FROM   Customer c
            LEFT JOIN Route          r1 ON r1.Id = c.TurId
            LEFT JOIN Route          r2 ON r2.Id = c.AusnahmeTurId
            LEFT JOIN CustomerFilter cf ON cf.Id = c.KundenfilterId
            WHERE  (@Suche    IS NULL
                    OR c.Kundenname    LIKE '%' + @Suche + '%'
                    OR c.Kundennummer  LIKE '%' + @Suche + '%'
                    OR c.Telefonnummer LIKE '%' + @Suche + '%')
            AND    (@NurAktiv IS NULL OR c.Aktiv = @NurAktiv)
            AND    (@RouteId  IS NULL OR c.TurId = @RouteId)
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
        => await _db.Customer.FirstOrDefaultAsync(c => c.Id == id);

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
            .AnyAsync(c => c.Kundennummer == customer.Kundennummer && c.Id != customer.Id);
        if (duplicate)
            throw new ValidationException($"Kundennummer '{customer.Kundennummer}' ist bereits vergeben.");
    }
}