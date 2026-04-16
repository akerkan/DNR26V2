namespace DNR26V2.Domain.Entities.System;

/// <summary>
/// Standort / Lager. V1: genau 1 Zeile (Hauptstandort).
/// Erweiterungspfad für spätere Multi-Location / Inventory-Logik.
/// </summary>
public class Location : AuditableEntity
{
    public int     Id           { get; set; }
    public string  Standortcode { get; set; } = string.Empty;
    public string  Bezeichnung  { get; set; } = string.Empty;
    public string? Adresse      { get; set; }
    public string? PLZ          { get; set; }
    public string? Ort          { get; set; }
    public string  Land         { get; set; } = "Deutschland";
    public bool    IstStandard  { get; set; } = false;
    public bool    Aktiv        { get; set; } = true;
}