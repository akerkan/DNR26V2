namespace DNR26V2.Domain.DTOs;

public sealed class CustomerListDto
{
    public int      Id              { get; set; }
    public string   Kundennummer    { get; set; } = string.Empty;
    public string   Kundenname      { get; set; } = string.Empty;
    public string?  Name2           { get; set; }
    public string?  Inhaber         { get; set; }
    public string?  Telefonnummer   { get; set; }
    public string?  Handynummer     { get; set; }
    public string?  EMail           { get; set; }
    public string?  PLZ             { get; set; }
    public string?  Ort             { get; set; }
    public string?  Adresse         { get; set; }
    public string?  Tur             { get; set; }      // AttributeValue.Bezeichnung
    public string?  AusnahmeTur     { get; set; }
    public string?  KundenGruppe    { get; set; }      // war: Kundenfilter
    public int      Routenfolge     { get; set; }
    public decimal  Limit           { get; set; }
    public bool     LiefertMo       { get; set; }
    public bool     LiefertDi       { get; set; }
    public bool     LiefertMi       { get; set; }
    public bool     LiefertDo       { get; set; }
    public bool     LiefertFr       { get; set; }
    public bool     LiefertSa       { get; set; }
    public bool     LiefertSo       { get; set; }
    public bool     PreisAusblenden { get; set; }
    public bool     Offen           { get; set; }
    public bool     Aktiv           { get; set; }
}