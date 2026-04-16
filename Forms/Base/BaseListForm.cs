namespace DNR26V2.Forms.Base;

/// <summary>
/// Basis für Listen-Formulare (Kunden, Produkte, Rechnungen etc.).
/// Öffnet als MDI-Child maximiert. Singleton-Steuerung via GetOrCreateInstance().
/// </summary>
public class BaseListForm : BaseForm
{
    protected BaseListForm()
    {
        WindowState = FormWindowState.Maximized;
    }

    /// <summary>
    /// Singleton-Pattern für MDI-Kindfenster.
    /// Verhindert doppeltes Öffnen desselben Formulartyps.
    /// </summary>
    public static T GetOrCreateInstance<T>(
        ref T? instance,
        Form mdiParent,
        Func<T> factory) where T : BaseListForm
    {
        if (instance == null || instance.IsDisposed)
        {
            instance = factory();
            instance.MdiParent = mdiParent;

            // Workaround: Capture a local variable for the event handler
            T? localInstance = instance;
            instance.FormClosed += (_, _) => localInstance = null;
        }

        instance.Show();
        instance.BringToFront();
        instance.WindowState = FormWindowState.Maximized;
        return instance;
    }

    /// <summary>
    /// Standardmäßige DataGridView-Konfiguration für Listenformulare.
    /// </summary>
    protected static void ConfigureGrid(DataGridView grid)
    {
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.ReadOnly = true;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.MultiSelect = false;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.RowHeadersVisible = false;
        grid.BackgroundColor = SystemColors.Window;
        grid.BorderStyle = BorderStyle.Fixed3D;
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 250);
        SetDoubleBuffered(grid);
    }
}