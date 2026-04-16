namespace DNR26V2.Helpers;

/// <summary>
/// String-Hilfsmethoden für die gesamte WinForms-Schicht.
/// </summary>
internal static class StringExtensions
{
    /// <summary>Gibt null zurück wenn der String leer/whitespace ist, sonst getrimmt.</summary>
    public static string? NullIfEmpty(this string? s)
        => string.IsNullOrWhiteSpace(s) ? null : s.Trim();
}