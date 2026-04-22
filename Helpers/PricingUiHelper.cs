using DNR26V2.Domain.Enums;

namespace DNR26V2.Helpers;

/// <summary>
/// Centralized UI pricing behavior.
/// All Gewicht enable/disable logic must go through this class — never inline in forms.
/// </summary>
public static class PricingUiHelper
{
    /// <summary>
    /// Returns false when Gewicht editing must be blocked (MengeXPreis — weight is irrelevant).
    /// </summary>
    public static bool CanEditGewicht(PreisFormel formel)
        => formel != PreisFormel.MengeXPreis;

    /// <summary>
    /// Applies Gewicht cell appearance and editability for a single grid row.
    /// - MengeXPreis:         ReadOnly=true, gray background, value reset to 0
    /// - MengeXGewichtXPreis: editable, white background
    /// </summary>
    public static void ApplyGewichtRule(DataGridViewRow row, PreisFormel formel,
        string columnName = "Gewicht")
    {
        if (row.DataGridView is null) return;
        if (!row.DataGridView.Columns.Contains(columnName)) return;

        var cell = row.Cells[columnName];

        if (formel == PreisFormel.MengeXPreis)
        {
            cell.ReadOnly        = true;
            cell.Style.BackColor = Color.LightGray;
            cell.Style.ForeColor = Color.Gray;
            cell.Value           = 0m;
        }
        else
        {
            cell.ReadOnly        = false;
            cell.Style.BackColor = Color.White;
            cell.Style.ForeColor = SystemColors.WindowText;
        }
    }
}
