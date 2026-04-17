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
                   vTur.Bezeichnung           AS Tur,
                   vAus.Bezeichnung           AS AusnahmeTur,
                   vGrp.Bezeichnung           AS KundenGruppe,
                   c.Routenfolge,
                   c.Limit,
                   c.LiefertMo, c.LiefertDi, c.LiefertMi,
                   c.LiefertDo, c.LiefertFr, c.LiefertSa, c.LiefertSo,
                   c.PreisAusblenden,
                   c.Offen,
                   c.Aktiv
            FROM   Customer c
            LEFT JOIN ProductAttributeValue vTur ON vTur.Id = c.TurWertId
            LEFT JOIN ProductAttributeValue vAus ON vAus.Id = c.AusnahmeTurWertId
            LEFT JOIN ProductAttributeValue vGrp ON vGrp.Id = c.KundenGruppeWertId
            WHERE  (@Suche              IS NULL
                    OR c.Kundenname    LIKE '%' + @Suche + '%'
                    OR c.Kundennummer  LIKE '%' + @Suche + '%'
                    OR c.Telefonnummer LIKE '%' + @Suche + '%')
            AND    (@NurAktiv           IS NULL OR c.Aktiv            = @NurAktiv)
            AND    (@TourWertId         IS NULL OR c.TurWertId        = @TourWertId)
            AND    (@KundenGruppeWertId IS NULL OR c.KundenGruppeWertId = @KundenGruppeWertId)
            ORDER  BY c.Kundenname
            """;

        using var conn = _dapper.CreateConnection();
        return (await conn.QueryAsync<CustomerListDto>(sql, new
        {
            Suche              = filter.Suche,
            NurAktiv           = filter.NurAktiv,
            TourWertId         = filter.TourWertId,
            KundenGruppeWertId = filter.KundenGruppeWertId
        })).AsList();
    }

    public async Task<Customer?> GetByIdAsync(int id)
        => await _db.Customer.FirstOrDefaultAsync(c => c.Id == id);

    public async Task SaveAsync(Customer customer)
    {
        await ValidateAsync(customer);

        // AusnahmeTur darf nicht gleich StandardTur sein
        if (customer.TurWertId.HasValue
         && customer.AusnahmeTurWertId.HasValue
         && customer.TurWertId == customer.AusnahmeTurWertId)
            throw new ValidationException(
                "Ausnahme-Tour darf nicht identisch mit der Standard-Tour sein.");

        if (customer.Id == 0) _db.Customer.Add(customer);
        else                  _db.Customer.Update(customer);
        await _db.SaveChangesAsync();
    }

    public async Task SetActiveAsync(int id, bool aktiv)
    {
        var customer = await _db.Customer.FindAsync(id)
            ?? throw new InvalidOperationException("Kunde nicht gefunden.");
        customer.Aktiv = aktiv;
        await _db.SaveChangesAsync();
    }

    private async Task ValidateAsync(Customer customer)
    {
        if (string.IsNullOrWhiteSpace(customer.Kundennummer))
            throw new ValidationException("Kundennummer ist ein Pflichtfeld.");
        if (string.IsNullOrWhiteSpace(customer.Kundenname))
            throw new ValidationException("Kundenname ist ein Pflichtfeld.");

        bool duplicate = await _db.Customer
            .AnyAsync(c => c.Kundennummer == customer.Kundennummer && c.Id != customer.Id);
        if (duplicate)
            throw new ValidationException(
                $"Kundennummer '{customer.Kundennummer}' ist bereits vergeben.");
    }
}