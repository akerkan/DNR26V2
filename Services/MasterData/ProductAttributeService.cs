using DNR26V2.Data.Context;
using DNR26V2.Domain.Entities.MasterData;
using DNR26V2.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DNR26V2.Services.MasterData;

public class ProductAttributeService : IProductAttributeService
{
    private readonly AppDbContext _db;
    public ProductAttributeService(AppDbContext db) => _db = db;

    // ── Attribute-Definitionen ─────────────────────────────────────────────

    public async Task<IReadOnlyList<ProductAttribute>> GetAllAttributesAsync(bool nurAktiv = false)
        => await _db.ProductAttribute
                    .Where(a => !nurAktiv || a.Aktiv)
                    .OrderBy(a => a.Bezeichnung)
                    .Include(a => a.Werte.Where(v => v.Aktiv).OrderBy(v => v.Sortierung))
                    .ToListAsync();

    public async Task<ProductAttribute?> GetAttributeByIdAsync(int id)
        => await _db.ProductAttribute
                    .Include(a => a.Werte.OrderBy(v => v.Sortierung))
                    .FirstOrDefaultAsync(a => a.Id == id);

    public async Task SaveAttributeAsync(ProductAttribute attribute)
    {
        if (string.IsNullOrWhiteSpace(attribute.Bezeichnung))
            throw new ValidationException("Bezeichnung ist ein Pflichtfeld.");

        bool duplicate = await _db.ProductAttribute
            .AnyAsync(a => a.Bezeichnung == attribute.Bezeichnung && a.Id != attribute.Id);
        if (duplicate)
            throw new ValidationException($"Attribut '{attribute.Bezeichnung}' existiert bereits.");

        if (attribute.Id == 0) _db.ProductAttribute.Add(attribute);
        else                   _db.ProductAttribute.Update(attribute);

        await _db.SaveChangesAsync();
    }

    public async Task SetAttributeActiveAsync(int id, bool aktiv)
    {
        var attr = await _db.ProductAttribute.FindAsync(id)
            ?? throw new InvalidOperationException("Attribut nicht gefunden.");
        attr.Aktiv = aktiv;
        await _db.SaveChangesAsync();
    }

    // ── Attribute-Werte ────────────────────────────────────────────────────

    public async Task<IReadOnlyList<ProductAttributeValue>> GetValuesByAttributeAsync(int attributId)
        => await _db.ProductAttributeValue
                    .Where(v => v.AttributId == attributId)
                    .OrderBy(v => v.Sortierung)
                    .ThenBy(v => v.Bezeichnung)
                    .ToListAsync();

    public async Task SaveValueAsync(ProductAttributeValue value)
    {
        if (string.IsNullOrWhiteSpace(value.Bezeichnung))
            throw new ValidationException("Wert-Bezeichnung ist ein Pflichtfeld.");

        bool duplicate = await _db.ProductAttributeValue
            .AnyAsync(v => v.AttributId   == value.AttributId
                        && v.Bezeichnung  == value.Bezeichnung
                        && v.Id           != value.Id);
        if (duplicate)
            throw new ValidationException($"Wert '{value.Bezeichnung}' existiert bereits.");

        if (value.Id == 0) _db.ProductAttributeValue.Add(value);
        else               _db.ProductAttributeValue.Update(value);

        await _db.SaveChangesAsync();
    }

    public async Task SetValueActiveAsync(int id, bool aktiv)
    {
        var val = await _db.ProductAttributeValue.FindAsync(id)
            ?? throw new InvalidOperationException("Attributwert nicht gefunden.");
        val.Aktiv = aktiv;
        await _db.SaveChangesAsync();
    }

    public async Task DeleteValueAsync(int id)
    {
        bool referenced = await _db.ProductAttributeMapping
            .AnyAsync(m => m.AttributWertId == id);
        if (referenced)
            throw new ValidationException(
                "Dieser Wert ist in Produkt-Zuweisungen in Verwendung und kann nicht gelöscht werden.");

        var val = await _db.ProductAttributeValue.FindAsync(id)
            ?? throw new InvalidOperationException("Attributwert nicht gefunden.");
        _db.ProductAttributeValue.Remove(val);
        await _db.SaveChangesAsync();
    }

    // ── Produkt-Zuweisungen ────────────────────────────────────────────────

    public async Task<IReadOnlyList<ProductAttributeMapping>> GetMappingsByProductAsync(int produktId)
        => await _db.ProductAttributeMapping
                    .Where(m => m.ArtikelId == produktId)
                    .Include(m => m.Attribut)
                    .Include(m => m.AttributWert)
                    .OrderBy(m => m.Attribut.Bezeichnung)
                    .ToListAsync();

    public async Task SaveMappingsAsync(int produktId, IEnumerable<ProductAttributeMapping> mappings)
    {
        var existing = await _db.ProductAttributeMapping
            .Where(m => m.ArtikelId == produktId)
            .ToListAsync();
        _db.ProductAttributeMapping.RemoveRange(existing);

        foreach (var m in mappings)
        {
            m.ArtikelId = produktId;
            m.Id        = 0;
            _db.ProductAttributeMapping.Add(m);
        }
        await _db.SaveChangesAsync();
    }

    public async Task DeleteMappingAsync(int mappingId)
    {
        var mapping = await _db.ProductAttributeMapping.FindAsync(mappingId)
            ?? throw new InvalidOperationException("Zuweisung nicht gefunden.");
        _db.ProductAttributeMapping.Remove(mapping);
        await _db.SaveChangesAsync();
    }
}