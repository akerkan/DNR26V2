using Dapper;
using DNR26V2.Data.Context;
using DNR26V2.Domain.DTOs.Etikett;
using DNR26V2.Domain.Entities.Etikett;
using DNR26V2.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DNR26V2.Services.Etikett;

public class EtiketService : IEtiketService
{
    private readonly AppDbContext  _db;
    private readonly DapperContext _dapper;

    public EtiketService(AppDbContext db, DapperContext dapper)
    {
        _db     = db;
        _dapper = dapper;
    }

    public async Task<IReadOnlyList<EtiketKundeDto>> GetKundenAsync()
    {
        const string sql = """
            SELECT c.Id,
                   c.Kundenname,
                   c.Kundennummer,
                   pav.Bezeichnung AS Tur
            FROM   Customer c
            LEFT JOIN ProductAttributeValue pav ON pav.Id = c.TurWertId
            ORDER  BY c.Kundenname
            """;
        using var conn = _dapper.CreateConnection();
        return (await conn.QueryAsync<EtiketKundeDto>(sql)).AsList();
    }

    public async Task<IReadOnlyList<EtiketProduktDto>> GetProdukteByKundeAsync(int kundeId)
    {
        const string sql = """
            SELECT cp.KundeId,
                   c.Kundenname,
                   cp.ArtikelId,
                   p.Artikelnummer,
                   p.Bezeichnung  AS Produktname,
                   cp.Menge,
                   cp.Gewicht,
                   p.Printfarbe,
                   p.Feld1,
                   p.Feld2,
                   p.Feld3,
                   p.Barcode
            FROM   CustomerProduct cp
            INNER JOIN Customer c  ON c.Id  = cp.KundeId
            INNER JOIN Product  p  ON p.Id  = cp.ArtikelId
            WHERE  cp.KundeId = @KundeId
            AND    cp.Aktiv   = 1
            ORDER  BY p.Bezeichnung
            """;
        using var conn = _dapper.CreateConnection();
        return (await conn.QueryAsync<EtiketProduktDto>(sql, new { KundeId = kundeId })).AsList();
    }

    public async Task<IReadOnlyList<EtiketLayoutField>> GetLayoutAsync(string layoutName = "Default")
    {
        return await _db.EtiketLayoutFields
            .Where(f => f.LayoutName == layoutName)
            .OrderBy(f => f.Feld)
            .ToListAsync();
    }

    public async Task SaveLayoutAsync(IEnumerable<EtiketLayoutField> fields)
    {
        foreach (var field in fields)
        {
            var existing = await _db.EtiketLayoutFields.FindAsync(field.Id);
            if (existing is null)
                _db.EtiketLayoutFields.Add(field);
            else
            {
                existing.X           = field.X;
                existing.Y           = field.Y;
                existing.Width       = field.Width;
                existing.Height      = field.Height;
                existing.FontName    = field.FontName;
                existing.FontSize    = field.FontSize;
                existing.Bold        = field.Bold;
                existing.Italic      = field.Italic;
                existing.ForeColorHex= field.ForeColorHex;
                existing.BackColorHex= field.BackColorHex;
                existing.TextAlignH  = field.TextAlignH;
                existing.Visible     = field.Visible;
            }
        }
        await _db.SaveChangesAsync();
    }

    public async Task ResetLayoutAsync(string layoutName = "Default")
    {
        var existing = _db.EtiketLayoutFields.Where(f => f.LayoutName == layoutName);
        _db.EtiketLayoutFields.RemoveRange(existing);
        await _db.SaveChangesAsync();

        _db.EtiketLayoutFields.AddRange(EtiketDefaultLayout.Build(layoutName));
        await _db.SaveChangesAsync();
    }
}
