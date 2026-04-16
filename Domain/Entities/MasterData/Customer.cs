namespace DNR26V2.Domain.Entities.MasterData;

/// <summary>
/// Kundenstammdaten. Kernentität des Systems.
/// Soft-Delete via Aktiv=false — physisches Löschen ist verboten.
/// </summary>
public class Customer : AuditableEntity
{
    public int    Id           { get; set; }
    public string Kundennummer { get; set; } = string.Empty;
    public string Kundenname   { get; set; } = string.Empty;
    public string? Name2       { get; set; }
    public string? Inhaber     { get; set; }

    // Hauptadresse
    public string? Adresse  { get; set; }
    public string? Adresse2 { get; set; }
    public string? PLZ      { get; set; }
    public string? Ort      { get; set; }
    public string  Land     { get; set; } = "Deutschland";

    // Kontakt
    public string? Telefonnummer { get; set; }
    public string? Handynummer   { get; set; }
    public string? EMail         { get; set; }

    // Alternative Lieferadresse
    public bool    AbweichendeLieferadresse { get; set; } = false;
    public string? ALName2    { get; set; }
    public string? ALInhaber  { get; set; }
    public string? ALAdresse  { get; set; }
    public string? ALAdresse2 { get; set; }
    public string? ALPLZ      { get; set; }
    public string? ALOrt      { get; set; }
    public string? ALLand     { get; set; }

    // Tour / Route
    /// <summary>
    /// Routenfolge: Integer-Wert für die Nähe zum Fahrer — Sortierung der Fahrerliste beim Druck.
    /// Wird manuell eingetragen.
    /// </summary>
    public int  Routenfolge  { get; set; } = 0;

    /// <summary>
    /// StandardTur: Normalerweise zugewiesene Tour für tägliche Lieferungen (FK → Route).
    /// </summary>
    public int? TurId        { get; set; }

    /// <summary>
    /// AusnahmeTur: Ausnahme-Tour (z.B. Fahrer krank). Darf nicht gleich TurId sein.
    /// Wird in der Lieferungen-Form per Funktion zugewiesen oder zurückgesetzt.
    /// </summary>
    public int? AusnahmeTurId { get; set; }

    // Filter
    public int? KundenfilterId { get; set; }

    // Finanzen
    public decimal Limit          { get; set; } = 0;
    public bool    PreisAusblenden { get; set; } = false;

    // Liefertage (MO–SO)
    public bool LiefertMo { get; set; } = false;
    public bool LiefertDi { get; set; } = false;
    public bool LiefertMi { get; set; } = false;
    public bool LiefertDo { get; set; } = false;
    public bool LiefertFr { get; set; } = false;
    public bool LiefertSa { get; set; } = false;
    public bool LiefertSo { get; set; } = false;

    // Geräte / Ausstattung
    public string? Geraete1 { get; set; }
    public string? Geraete2 { get; set; }
    public string? Geraete3 { get; set; }
    public string? Geraete4 { get; set; }
    public string? Geraete5 { get; set; }

    // Status
    public bool    Aktiv   { get; set; } = true;
    public bool    Offen   { get; set; } = true;
    public string? Notizen { get; set; }

    // Navigation
    public Route?          Tur                    { get; set; }
    public Route?          AusnahmeTur            { get; set; }
    public CustomerFilter? KundenfilterNavigation { get; set; }
}