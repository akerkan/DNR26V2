using Dapper;
using DNR26V2.Data.Context;
using DNR26V2.Domain.DTOs;
using DNR26V2.Domain.Entities.MasterData;
using DNR26V2.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DNR26V2.Services.MasterData;

public class ProductService : IProductService
{
    private readonly AppDbContext  _db;
    private readonly DapperContext _dapper;

    public ProductService(AppDbContext db, DapperContext dapper)
    {
        _db     = db;
        _dapper = dapper;
    }

    // ── Liste via Dapper ──────────────────────────────────────────────────────

    public async Task<IReadOnlyList<ProductListDto>> GetListAsync(ProductListFilter filter)
    {
        const string sql = """
            SELECT p.Id,
                   p.Artikelnummer,
                   p.Bezeichnung,
                   p.Einheit,
                   p.VKPreis,
                   p.MwstProzent,
                   p.Feld1,
                   p.Feld2,
                   p.Feld3,
                   p.Feld4,
                   p.Printfarbe,
                   p.Aktiv
            FROM   Product p
            WHERE  (@Suche    IS NULL
                    OR p.Bezeichnung   LIKE '%' + @Suche + '%'
                    OR p.Artikelnummer LIKE '%' + @Suche + '%'
                    OR p.Barcode       LIKE '%' + @Suche + '%')
            AND    (@NurAktiv IS NULL OR p.Aktiv = @NurAktiv)
            ORDER  BY p.Bezeichnung
            """;

        using var conn = _dapper.CreateConnection();
        var result = await conn.QueryAsync<ProductListDto>(sql, new
        {
            Suche    = filter.Suche,
            NurAktiv = filter.NurAktiv
        });
        return result.AsList();
    }

    // ── Einzelprodukt via EF Core ─────────────────────────────────────────────

    public async Task<Product?> GetByIdAsync(int id)
        => await _db.Product.FirstOrDefaultAsync(p => p.Id == id);

    // ── Speichern ─────────────────────────────────────────────────────────────

    public async Task SaveAsync(Product product)
    {
        await ValidateAsync(product);

        if (product.Id == 0) _db.Product.Add(product);
        else                 _db.Product.Update(product);

        await _db.SaveChangesAsync();
    }

    // ── Soft-Delete ───────────────────────────────────────────────────────────

    public async Task SetActiveAsync(int id, bool aktiv)
    {
        var product = await _db.Product.FindAsync(id)
            ?? throw new InvalidOperationException("Artikel nicht gefunden.");
        product.Aktiv = aktiv;
        await _db.SaveChangesAsync();
    }

    // ── Validierung ───────────────────────────────────────────────────────────

    private async Task ValidateAsync(Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Artikelnummer))
            throw new ValidationException("Artikelnummer ist ein Pflichtfeld.");

        if (string.IsNullOrWhiteSpace(product.Bezeichnung))
            throw new ValidationException("Bezeichnung ist ein Pflichtfeld.");

        bool duplicate = await _db.Product
            .AnyAsync(p => p.Artikelnummer == product.Artikelnummer
                        && p.Id != product.Id);
        if (duplicate)
            throw new ValidationException(
                $"Artikelnummer '{product.Artikelnummer}' ist bereits vergeben.");
    }
}