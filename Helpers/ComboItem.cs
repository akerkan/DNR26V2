namespace DNR26V2.Helpers;

/// <summary>
/// Einfaches Datentransferobjekt für ComboBox-Einträge mit ID und Anzeigetext.
/// </summary>
public sealed class ComboItem(int id, string display)
{
    public int Id { get; } = id;
    public string Display { get; } = display;

    public override string ToString() => Display;
}