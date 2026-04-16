using DNR26V2.Domain.DTOs;
using DNR26V2.Domain.Entities.MasterData;

namespace DNR26V2.Services.MasterData;

public interface IProductService
{
    /// <summary>Schnelle Listenabfrage via Dapper.</summary>
    Task<IReadOnlyList<ProductListDto>> GetListAsync(ProductListFilter filter);

    /// <summary>Einzelprodukt via EF Core.</summary>
    Task<Product?> GetByIdAsync(int id);

    /// <summary>Neu anlegen oder aktualisieren (inkl. Validierung).</summary>
    Task SaveAsync(Product product);

    /// <summary>Soft-Delete: setzt Aktiv = false/true.</summary>
    Task SetActiveAsync(int id, bool aktiv);
}