namespace DNR26V2.Forms.Base;

/// <summary>
/// Basis für alle Formulare. Stellt gemeinsame Hilfsfunktionen bereit.
/// </summary>
public class BaseForm : Form
{
    protected BaseForm()
    {
        StartPosition = FormStartPosition.CenterParent;
        KeyPreview = true;
    }

    /// <summary>
    /// Aktiviert DoubleBuffering für ein Control (flackerfreie DataGridViews).
    /// </summary>
    protected static void SetDoubleBuffered(Control control)
    {
        var prop = control.GetType().GetProperty(
            "DoubleBuffered",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        prop?.SetValue(control, true);
    }

    /// <summary>
    /// Zeigt eine Fehlermeldung.
    /// </summary>
    protected static void ShowError(string message, string title = "Fehler")
        => MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);

    /// <summary>
    /// Zeigt eine Erfolgsmeldung.
    /// </summary>
    protected static void ShowSuccess(string message, string title = "Erfolg")
        => MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);

    /// <summary>
    /// Zeigt eine Ja/Nein-Bestätigung.
    /// </summary>
    protected static bool Confirm(string message, string title = "Bestätigung")
        => MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
           == DialogResult.Yes;
}