using DNR26V2.Domain.DTOs;
using DNR26V2.Domain.Entities.Invoices;
using DNR26V2.Domain.Enums;

namespace DNR26V2.Services.Invoices;

public interface IInvoiceService
{
    // ── FrmRechnungErfassung ──────────────────────────────────────────────────

    /// <summary>Kunden mit mindestens einem offenen (nicht fakturierten) LS im Zeitraum.</summary>
    Task<IReadOnlyList<KundeOffeneLsDto>> GetKundenMitOffenenLsAsync(DateTime von, DateTime bis);

    /// <summary>Offene Lieferscheine eines Kunden im Zeitraum → werden zu Rechnungszeilen.</summary>
    Task<IReadOnlyList<LieferscheinFuerRechnungDto>> GetOffeneLieferscheineAsync(
        int kundeId, DateTime von, DateTime bis);

    /// <summary>Einzelrechnung buchen: LS → InvoiceLines, LS-Status → Fakturiert.</summary>
    Task<InvoiceHeader> BuchenAsync(
        int kundeId, DateTime von, DateTime bis,
        IEnumerable<int> lieferscheinIds,
        string? notiz);

    // ── FrmSammelRechnung ─────────────────────────────────────────────────────

    /// <summary>Für jeden Kunden in der Liste eine Rechnung buchen (kein Notiz-Dialog).</summary>
    Task<SammelrechnungResultDto> SammelBuchenAsync(
        IEnumerable<int> kundeIds, DateTime von, DateTime bis);

    // ── FrmRechnungList ───────────────────────────────────────────────────────

    Task<IReadOnlyList<RechnungListDto>> GetRechnungListeAsync(
        DateTime? von, DateTime? bis, string? kunde, InvoiceStatus? status);

    Task<IReadOnlyList<RechnungZeileDetailDto>> GetZeilenByRechnungIdAsync(int rechnungId);

    /// <summary>Rechnung stornieren → LS-Status zurück auf Offen (nur Gebucht möglich).</summary>
    Task StornierenAsync(int rechnungId);

    // Bestehende Methoden beibehalten — nur ergänzen:

    /// <summary>Lieferschein-Zeilen Vorschau für das Preview-Grid.</summary>
    Task<IReadOnlyList<LieferscheinZeileVorschauDto>> GetZeilenVorschauAsync(int lieferscheinId);
}