namespace DNR26V2.Forms.Base;

/// <summary>
/// Basis für Detail-/Karten-Formulare (Einzelbeleg, Einzelkunde etc.).
/// Öffnet als modaler Dialog oder kontrolliertes MDI-Child.
/// </summary>
public class BaseCardForm : BaseForm
{
    protected BaseCardForm()
    {
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
    }

    /// <summary>
    /// Setzt alle Eingabefelder in einem Container auf ReadOnly.
    /// Nützlich für finalisierte/stornierte Belege.
    /// </summary>
    protected static void SetReadOnly(Control container, bool readOnly)
    {
        foreach (Control control in container.Controls)
        {
            switch (control)
            {
                case TextBox tb:
                    tb.ReadOnly = readOnly;
                    break;
                case ComboBox cb:
                    cb.Enabled = !readOnly;
                    break;
                case NumericUpDown nud:
                    nud.ReadOnly = readOnly;
                    break;
                case DateTimePicker dtp:
                    dtp.Enabled = !readOnly;
                    break;
                case CheckBox chk:
                    chk.Enabled = !readOnly;
                    break;
            }

            if (control.HasChildren)
                SetReadOnly(control, readOnly);
        }
    }
}