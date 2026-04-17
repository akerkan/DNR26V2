namespace DNR26V2.Domain.Entities.System;

/// <summary>Benutzer-spezifische Spalteneinstellungen für DataGridViews.</summary>
public class UserGridSetting
{
    public int      Id            { get; set; }
    public string   Benutzername  { get; set; } = string.Empty;
    public string   GridKey       { get; set; } = string.Empty;
    public string   Einstellungen { get; set; } = string.Empty; // JSON
    public DateTime GeaendertAm   { get; set; }
}