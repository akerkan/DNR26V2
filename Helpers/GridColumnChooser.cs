using System.Text.Json;
using DNR26V2.Services.System;

namespace DNR26V2.Helpers;

/// <summary>
/// DataGridView sütun seçici — sağ tık menüsü ile TÜM kolonları göster/gizle,
/// sürükle-bırak ile sıralama, genişlik hatırlama, "Standard zurücksetzen".
/// Ayarlar veritabanına kullanıcı bazlı yazılır.
/// </summary>
public static class GridColumnChooser
{
    private static IGridSettingsService? _service;
    private static readonly Dictionary<string, List<ColumnSetting>> _defaults = new();
    private static readonly HashSet<string> _attached = new();

    /// <summary>Program.cs'te bir kez çağrılır.</summary>
    public static void SetService(IGridSettingsService service) => _service = service;

    // ── Attach ────────────────────────────────────────────────────────────────

    public static void Attach(DataGridView grid, string settingsKey)
    {
        var instanceKey = $"{settingsKey}_{grid.GetHashCode()}";
        if (!_attached.Add(instanceKey)) return;

        grid.AllowUserToOrderColumns = true;

        grid.ColumnHeaderMouseClick += (sender, e) =>
        {
            if (e.Button == MouseButtons.Right)
                ShowColumnMenu(grid, settingsKey);
        };

        // async void — fire-and-forget (CS0029 hatası düzeltildi)
        grid.ColumnDisplayIndexChanged += async (sender, e) => await SaveAsync(grid, settingsKey);
        grid.ColumnWidthChanged        += async (sender, e) => await SaveAsync(grid, settingsKey);
    }

    // ── ApplyUserSettings (StyleGrid() sonunda çağrılır) ──────────────────────

    public static void ApplyUserSettings(DataGridView grid, string settingsKey)
    {
        if (!_defaults.ContainsKey(settingsKey))
            CaptureDefaults(grid, settingsKey);

        if (_service is null) return;

        var json = _service.Load(settingsKey);
        if (string.IsNullOrEmpty(json)) return;

        try
        {
            var saved = JsonSerializer.Deserialize<List<ColumnSetting>>(json);
            if (saved is null || saved.Count == 0) return;

            grid.SuspendLayout();
            try
            {
                foreach (var s in saved)
                {
                    if (!grid.Columns.Contains(s.Name)) continue;
                    var col = grid.Columns[s.Name]!;
                    col.Visible = s.Visible;
                    if (s.Fill)
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    else
                    {
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        if (s.Width > 0) col.Width = s.Width;
                    }
                }

                var ordered = saved
                    .Where(s => grid.Columns.Contains(s.Name))
                    .OrderBy(s => s.DisplayIndex)
                    .ToList();

                for (int i = 0; i < ordered.Count; i++)
                {
                    try { grid.Columns[ordered[i].Name]!.DisplayIndex = i; }
                    catch { }
                }
            }
            finally { grid.ResumeLayout(); }
        }
        catch { }
    }

    // ── Menü ──────────────────────────────────────────────────────────────────

    private static void ShowColumnMenu(DataGridView grid, string settingsKey)
    {
        var menu = new ContextMenuStrip();

        var allCols = grid.Columns.Cast<DataGridViewColumn>()
            .OrderBy(c => c.Visible ? c.DisplayIndex : 9999)
            .ThenBy(c => c.HeaderText)
            .ToList();

        foreach (var col in allCols)
        {
            var header = !string.IsNullOrEmpty(col.HeaderText) ? col.HeaderText : col.Name;
            var item = new ToolStripMenuItem(header)
            {
                Checked      = col.Visible,
                CheckOnClick = true,
                Tag          = col.Name
            };
            item.CheckedChanged += async (sender, e) =>
            {
                if (sender is not ToolStripMenuItem mi || mi.Tag is not string name) return;
                if (!grid.Columns.Contains(name)) return;
                grid.Columns[name]!.Visible = mi.Checked;
                await SaveAsync(grid, settingsKey);
            };
            menu.Items.Add(item);
        }

        menu.Items.Add(new ToolStripSeparator());

        var resetItem = new ToolStripMenuItem("Standard zurücksetzen")
        {
            Font = new Font(SystemFonts.MenuFont!, FontStyle.Bold)
        };
        resetItem.Click += async (sender, e) => await ResetToDefaultsAsync(grid, settingsKey);
        menu.Items.Add(resetItem);

        menu.Show(grid, grid.PointToClient(Cursor.Position));
    }

    // ── Defaults ──────────────────────────────────────────────────────────────

    private static void CaptureDefaults(DataGridView grid, string key)
    {
        _defaults[key] = grid.Columns.Cast<DataGridViewColumn>()
            .Select(c => new ColumnSetting
            {
                Name         = c.Name,
                HeaderText   = c.HeaderText,
                Visible      = c.Visible,
                Width        = c.Width,
                DisplayIndex = c.DisplayIndex,
                Fill         = c.AutoSizeMode == DataGridViewAutoSizeColumnMode.Fill
            }).ToList();
    }

    private static async Task ResetToDefaultsAsync(DataGridView grid, string key)
    {
        if (!_defaults.TryGetValue(key, out var defaults)) return;

        grid.SuspendLayout();
        try
        {
            foreach (var def in defaults)
            {
                if (!grid.Columns.Contains(def.Name)) continue;
                var col = grid.Columns[def.Name]!
;
                col.Visible = def.Visible;
                if (def.Fill)
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                else
                {
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    col.Width = def.Width;
                }
            }
            var ordered = defaults.OrderBy(d => d.DisplayIndex).ToList();
            for (int i = 0; i < ordered.Count; i++)
            {
                if (!grid.Columns.Contains(ordered[i].Name)) continue;
                try { grid.Columns[ordered[i].Name]!.DisplayIndex = i; }
                catch { }
            }
        }
        finally { grid.ResumeLayout(); }

        if (_service is not null)
            await _service.DeleteAsync(key);
        _defaults.Remove(key);
    }

    // ── DB kayıt ──────────────────────────────────────────────────────────────

    private static async Task SaveAsync(DataGridView grid, string key)
    {
        if (_service is null) return;
        var cols = grid.Columns.Cast<DataGridViewColumn>()
            .Select(c => new ColumnSetting
            {
                Name         = c.Name,
                HeaderText   = c.HeaderText,
                Visible      = c.Visible,
                Width        = c.Width,
                DisplayIndex = c.DisplayIndex,
                Fill         = c.AutoSizeMode == DataGridViewAutoSizeColumnMode.Fill
            }).ToList();

        await _service.SaveAsync(key, JsonSerializer.Serialize(cols));
    }

    // ── DTO ───────────────────────────────────────────────────────────────────

    private sealed class ColumnSetting
    {
        public string Name         { get; set; } = string.Empty;
        public string HeaderText   { get; set; } = string.Empty;
        public bool   Visible      { get; set; }
        public int    Width        { get; set; }
        public int    DisplayIndex { get; set; }
        public bool   Fill         { get; set; }
    }
}