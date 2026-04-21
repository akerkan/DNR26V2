namespace DNR26V2.Domain.Entities.System;

/// <summary>
/// Zentrale Anwendungseinstellungen – immer genau 1 Zeile (Id = 1).
/// </summary>
public class AppSetup : AuditableEntity
{
    /// <summary>Singleton-PK – darf nur den Wert 1 haben (CHECK-Constraint in DB).</summary>
    public int     Id                   { get; set; } = 1;

    // Firmendaten
    public string  Firmenname           { get; set; } = string.Empty;
    public string? Firmenadresse        { get; set; }
    public string? FirmenPLZ            { get; set; }
    public string? FirmenOrt            { get; set; }
    public string  FirmenLand           { get; set; } = "Deutschland";
    public string? FirmenTelefon        { get; set; }
    public string? FirmenEmail          { get; set; }
    public string? FirmenSteuernummer   { get; set; }
    public string? FirmenUStIdNr        { get; set; }

    // Steuer + Standort
    public decimal StandardMwstProzent  { get; set; } = 7.00m;
    public string? StandardStandortCode { get; set; }

    // Drucker
    public string? DruckerWeissesPapier { get; set; }
    public string? DruckerMitLogo       { get; set; }

    // Nummernserien-Präfixe
    public string RechnungPraefix      { get; set; } = "RE";
    public string LieferscheinPraefix  { get; set; } = "LS";
    public string GutschriftPraefix    { get; set; } = "GS";
    public string ZahlungPraefix       { get; set; } = "ZA";

    // UI
    public int SeitenGroesse           { get; set; } = 20;

    // ── Order archiving (Module 4) ────────────────────────────────────────────
    /// <summary>
    /// If true: after booking, order Status = Archiviert (disappears from active list).
    /// If false: order stays visible as Bestaetigt (BC-style "keep history").
    /// </summary>
    public bool AuftraegeArchivieren { get; set; } = false;

    // New: enforce that customer must have a Tour before saving/freigeben
    public bool TurKontrolle { get; set; } = false;

    // Neue konfigurierbare Farben (hex), UI-Bereich
    public string ColorOrderOffen { get; set; } = "#FFFFC8";        // Gelb (Default: 255,255,200)
    public string ColorOrderFreigegeben { get; set; } = "#C8E6FF";  // Hellblau (Default: 200,230,255)
    public string ColorOrderGebucht { get; set; } = "#C8FFC8";      // Grün (Default: 200,255,200)
    public string ColorOrderStorniert { get; set; } = "#F0F0F0";    // Grau (Default: 240,240,240)
    // Optional: Fore-Color für Status-Label (falls benötigt)
    public string? ColorOrderLabelOffen { get; set; } = null;
    public string? ColorOrderLabelFreigegeben { get; set; } = null;
    public string? ColorOrderLabelGebucht { get; set; } = null;
    public string? ColorOrderLabelStorniert { get; set; } = null;

    // ── Zahlungskonditionen (für Rechnungen) ──────────────────────────────────
    public string? BankName           { get; set; }
    public string? IBAN               { get; set; }
    public string? BIC                { get; set; }
    public string? Kontoinhaber       { get; set; }
    public int     ZahlungszielTage   { get; set; } = 14;
    public decimal SkontoProzent      { get; set; } = 0.00m;
    public int     SkontoTage         { get; set; } = 7;
}