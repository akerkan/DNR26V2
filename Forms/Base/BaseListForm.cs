namespace DNR26V2.Forms.Base;

public class BaseListForm : BaseForm
{
    protected BaseListForm() { }

    public static T GetOrCreateInstance<T>(
        ref T? instance, Form mdiParent, Func<T> factory) where T : BaseListForm
    {
        if (instance == null || instance.IsDisposed)
        {
            instance = factory();
            instance.MdiParent = mdiParent;
            T? local = instance;
            instance.FormClosed += (_, _) => local = null;
        }
        instance.Show();
        instance.BringToFront();
        return instance;
    }

    protected static void ConfigureGrid(DataGridView grid)
    {
        grid.AllowUserToAddRows    = false;
        grid.AllowUserToDeleteRows = false;
        grid.ReadOnly              = true;
        grid.SelectionMode         = DataGridViewSelectionMode.FullRowSelect;
        grid.MultiSelect           = false;
        grid.RowHeadersVisible     = false;
        grid.BackgroundColor       = SystemColors.Window;
        grid.BorderStyle           = BorderStyle.Fixed3D;
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 250);
        SetDoubleBuffered(grid);
    }

    /// <summary>
    /// Tüm kolonlara (gizli olanlar dahil) Almanca başlık uygular.
    /// StyleGrid() içinde, hide-all adımından ÖNCE çağrılmalı.
    /// </summary>
    protected static void ApplyColumnHeaders(DataGridView grid, IReadOnlyDictionary<string, string> headers)
    {
        foreach (DataGridViewColumn col in grid.Columns)
            if (headers.TryGetValue(col.Name, out var header))
                col.HeaderText = header;
    }

    /// <summary>Einmalig in WireUpEvents aufrufen.</summary>
    protected void EnableColumnChooser(DataGridView grid, string? key = null)
    {
        key ??= $"{GetType().Name}_{grid.Name}";
        Helpers.GridColumnChooser.Attach(grid, key);
    }

    /// <summary>Am Ende von StyleGrid() aufrufen.</summary>
    protected void ApplyColumnChooserSettings(DataGridView grid, string? key = null)
    {
        key ??= $"{GetType().Name}_{grid.Name}";
        Helpers.GridColumnChooser.ApplyUserSettings(grid, key);
    }
}