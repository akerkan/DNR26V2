namespace DNR26V2.Domain.Entities.MasterData;

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
    public bool    AbweichendeLieferadresse { get; set; }
    public string? ALName2    { get; set; }
    public string? ALInhaber  { get; set; }
    public string? ALAdresse  { get; set; }
    public string? ALAdresse2 { get; set; }
    public string? ALPLZ      { get; set; }
    public string? ALOrt      { get; set; }
    public string? ALLand     { get; set; }

    // Tour / Route — jetzt über AttributeValue (EntityType = Tour)
    public int  Routenfolge     { get; set; } = 0;
    public int? TurWertId       { get; set; }   // FK → ProductAttributeValue (Tour)
    public int? AusnahmeTurWertId { get; set; } // FK → ProductAttributeValue (Tour)

    // Kundengruppe — über AttributeValue (EntityType = KundenGruppe)
    public int? KundenGruppeWertId { get; set; } // FK → ProductAttributeValue (KundenGruppe)

    // Finanzen
    public decimal Limit           { get; set; } = 0;
    public bool    PreisAusblenden { get; set; } = false;

    // Liefertage (MO–SO)
    public bool LiefertMo { get; set; }
    public bool LiefertDi { get; set; }
    public bool LiefertMi { get; set; }
    public bool LiefertDo { get; set; }
    public bool LiefertFr { get; set; }
    public bool LiefertSa { get; set; }
    public bool LiefertSo { get; set; }

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
    public ProductAttributeValue? TurWert          { get; set; }
    public ProductAttributeValue? AusnahmeTurWert   { get; set; }
    public ProductAttributeValue? KundenGruppeWert  { get; set; }
}