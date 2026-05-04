using DNR26V2.Domain.DTOs.Etikett;
using DNR26V2.Domain.Entities.Etikett;

namespace DNR26V2.Services.Etikett;

public interface IEtiketService
{
    Task<IReadOnlyList<EtiketKundeDto>>    GetKundenAsync();
    Task<IReadOnlyList<EtiketProduktDto>>  GetProdukteByKundeAsync(int kundeId);
    Task<IReadOnlyList<EtiketLayoutField>> GetLayoutAsync(string layoutName = "Default");
    Task SaveLayoutAsync(IEnumerable<EtiketLayoutField> fields);
    Task ResetLayoutAsync(string layoutName = "Default");

    // ── Paper size ────────────────────────────────────────────────────────────
    Task<EtiketPaperConfig> GetPaperConfigAsync(string layoutName = "Default");
    Task SavePaperConfigAsync(EtiketPaperConfig config);
}
