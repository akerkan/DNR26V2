using Dapper;
using DNR26V2.Data.Context;
using DNR26V2.Domain.DTOs;
using DNR26V2.Domain.Entities.MasterData;
using DNR26V2.Domain.Enums;
using DNR26V2.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DNR26V2.Services.MasterData;

public class CustomerProductService : ICustomerProductService
{
    private readonly AppDbContext  _db;
    private readonly DapperContext _dapper;

    public CustomerProductService(AppDbContext db, DapperContext dapper)
    {
        _db     = db;
        _dapper = dapper;
    }

    public async Task<IReadOnlyList<CustomerProductDto>> GetByCustomerAsync(int kundeId)
    {
        const string sql = """
            SELECT cp.Id,
                   cp.KundeId,
                   cp.ArtikelId,
                   p.Artikelnummer,
                   p.Bezeichnung AS Produktname,
                   cp.Preis,
                   cp.Menge,
                   cp.Gewicht
            FROM   CustomerProduct cp
            INNER JOIN Product p ON p.Id = cp.ArtikelId
            WHERE  cp.KundeId = @KundeId
            AND    cp.Aktiv   = 1
            ORDER  BY p.Bezeichnung
            """;
        using var conn = _dapper.CreateConnection();
        return (await conn.QueryAsync<CustomerProductDto>(sql, new { KundeId = kundeId })).AsList();
    }

    public async Task<IReadOnlyList<CustomerProductAttributeDto>> GetAttributesAsync(int customerProductId)
    {
        const string sql = """
            SELECT m.Id,
                   m.CustomerProductId,
                   m.AttributId,
                   a.Bezeichnung  AS AttributName,
                   CAST(a.Feldtyp AS int) AS Feldtyp,
                   m.AttributWertId,
                   v.Bezeichnung  AS WertBezeichnung,
                   m.FreierText
            FROM   CustomerProductAttributeMapping m
            INNER JOIN ProductAttribute      a ON a.Id = m.AttributId
            LEFT  JOIN ProductAttributeValue v ON v.Id = m.AttributWertId
            WHERE  m.CustomerProductId = @CustomerProductId
            """;
        using var conn = _dapper.CreateConnection();
        return (await conn.QueryAsync<CustomerProductAttributeDto>(sql,
            new { CustomerProductId = customerProductId })).AsList();
    }

    public async Task<IReadOnlyList<ProductAttribute>> GetApplicableAttributesAsync()
        => await _db.ProductAttribute
                    .Where(a => a.Aktiv
                             && (a.EntityType == AttributeEntityType.Product
                              || a.EntityType == AttributeEntityType.CustomerProduct
                              || a.EntityType == AttributeEntityType.Shared))
                    .Include(a => a.Werte.Where(v => v.Aktiv))
                    .OrderBy(a => a.Bezeichnung)
                    .ToListAsync();

    public async Task<IReadOnlyList<ProductAttributeValue>> GetAttributeValuesAsync(int attributId)
        => await _db.ProductAttributeValue
                    .Where(v => v.AttributId == attributId && v.Aktiv)
                    .OrderBy(v => v.Sortierung).ThenBy(v => v.Bezeichnung)
                    .ToListAsync();

    public async Task<CustomerProduct?> GetByIdAsync(int id)
        => await _db.CustomerProduct.FirstOrDefaultAsync(cp => cp.Id == id);

    public async Task<CustomerProduct> AddProductAsync(int kundeId, int artikelId, decimal initialPreis = 0)
    {
        // Sadece aktif kayıtları kontrol et — böylece "Entfernen" sonrası tekrar eklenebilir
        bool exists = await _db.CustomerProduct
            .AnyAsync(cp => cp.KundeId == kundeId && cp.ArtikelId == artikelId && cp.Aktiv);
        if (exists)
            throw new ValidationException("Dieses Produkt ist bereits diesem Kunden zugeordnet.");

        // Deaktiv edilenleri etkinleştir
        var deactivated = await _db.CustomerProduct
            .FirstOrDefaultAsync(cp => cp.KundeId == kundeId && cp.ArtikelId == artikelId && !cp.Aktiv);

        if (deactivated is not null)
        {
            deactivated.Aktiv  = true;
            deactivated.Preis  = initialPreis;
            deactivated.Menge  = 0;
            deactivated.Gewicht = 0;
            await _db.SaveChangesAsync();
            return deactivated;
        }

        var entity = new CustomerProduct
        {
            KundeId   = kundeId,
            ArtikelId = artikelId,
            Preis     = initialPreis,
            Menge     = 0,
            Gewicht   = 0,
            Aktiv     = true
        };
        _db.CustomerProduct.Add(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task SaveAsync(CustomerProduct entity)
    {
        if (entity.KundeId   == 0) throw new ValidationException("Kunde fehlt.");
        if (entity.ArtikelId == 0) throw new ValidationException("Artikel fehlt.");
        if (entity.Id == 0) _db.CustomerProduct.Add(entity);
        else                _db.CustomerProduct.Update(entity);
        await _db.SaveChangesAsync();
    }

    public async Task SaveAttributeAsync(CustomerProductAttributeMapping mapping)
    {
        var existing = await _db.CustomerProductAttributeMapping
            .FirstOrDefaultAsync(m => m.CustomerProductId == mapping.CustomerProductId
                                   && m.AttributId        == mapping.AttributId);
        if (existing is null)
            _db.CustomerProductAttributeMapping.Add(mapping);
        else
        {
            existing.AttributWertId = mapping.AttributWertId;
            existing.FreierText     = mapping.FreierText;
            _db.CustomerProductAttributeMapping.Update(existing);
        }
        await _db.SaveChangesAsync();
    }

    public async Task RemoveAsync(int id)
    {
        var entity = await _db.CustomerProduct.FindAsync(id)
            ?? throw new InvalidOperationException("Kundenprodukt nicht gefunden.");
        entity.Aktiv = false;
        await _db.SaveChangesAsync();
    }
}