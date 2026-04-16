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
}