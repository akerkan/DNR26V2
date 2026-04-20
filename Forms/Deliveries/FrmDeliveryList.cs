using System.ComponentModel;
using DNR26V2.Domain.DTOs;
using DNR26V2.Domain.Enums;
using DNR26V2.Domain.Exceptions;
using DNR26V2.Forms.Base;
using DNR26V2.Helpers;
using DNR26V2.Services.Deliveries;

namespace DNR26V2.Forms.Deliveries;

public partial class FrmDeliveryList : BaseListForm
{
    private static readonly Dictionary<string, string> _columnHeaders = new()
    {
        ["Id"]               = "Id",
        ["KundeId"]          = "KundeId",
        ["LieferscheinNr"]   = "Lieferschein-Nr.",
        ["Lieferdatum"]      = "Lieferdatum",
        ["Kundenname"]       = "Kunde",
        ["Tour"]             = "Tour",
        ["AuftragNr"]        = "Auftrag-Nr.",
        ["Status"]           = "Status",
        ["AnzahlPositionen"] = "Pos.",
        ["Gesamtbetrag"]     = "Gesamtbetrag",
    };

    private readonly IDeliveryService  _deliveryService;
    private readonly SemaphoreSlim     _lock           = new(1, 1);
    private readonly HashSet<int>      _checkedIds     = new();
    private bool                       _isLoading;
    private bool                       _suppressChecked;

    // ── Konstruktoren ─────────────────────────────────────────────────────────

    public FrmDeliveryList(IDeliveryService deliveryService)
    {
        _deliveryService = deliveryService;
        InitializeComponent();
        WireUpEvents();
    }

    public FrmDeliveryList()
    {
        _deliveryService = null!;
        InitializeComponent();
        WireUpEvents();
    }

    private static bool IsDesignMode() =>
        LicenseManager.UsageMode == LicenseUsageMode.Designtime;

    // ── Events verdrahten ─────────────────────────────────────────────────────

    private void WireUpEvents()
    {
        Load += FrmDeliveryList_Load;

        btnSuchen.Click  += async (_, _) => await LoadListAsync();
        txtKunde.KeyDown += async (s, e) => { if (e.KeyCode == Keys.Enter) await LoadListAsync(); };

        dgwLieferscheine.CellFormatting               += DgwLieferscheine_CellFormatting;
        dgwLieferscheine.SelectionChanged             += DgwLieferscheine_SelectionChanged;
        dgwLieferscheine.CurrentCellDirtyStateChanged += DgwLieferscheine_DirtyStateChanged;
        dgwLieferscheine.CellValueChanged             += DgwLieferscheine_CellValueChanged;
        dgwLieferscheine.KeyDown                      += DgwLieferscheine_KeyDown;

        btnAbschliessen.Click  += BtnAbschliessen_Click;
        btnStornieren.Click    += BtnStornieren_Click;
        btnAlleMarkieren.Click += BtnAlleMarkieren_Click;

        EnableColumnChooser(dgwLieferscheine);
    }

    // ── Load ──────────────────────────────────────────────────────────────────

    private async void FrmDeliveryList_Load(object? sender, EventArgs e)
    {
        if (IsDesignMode() || _deliveryService is null) return;

        WindowState = FormWindowState.Maximized;

        _isLoading = true;
        dtpVon.Value = DateTime.Today.AddDays(-7);
        dtpBis.Value = DateTime.Today;
        _isLoading = false;
        cmbStatus.SelectedIndex = 1;   // Standard: Offen — bereit zum Abschliessen

        await LoadListAsync();
    }

    // ── Liste laden ───────────────────────────────────────────────────────────

    private async Task LoadListAsync()
    {
        if (_deliveryService is null || _isLoading) return;

        DateTime?       von    = dtpVon.Value.Date;
        DateTime?       bis    = dtpBis.Value.Date;
        string?         kunde  = string.IsNullOrWhiteSpace(txtKunde.Text) ? null : txtKunde.Text.Trim();
        DeliveryStatus? status = cmbStatus.SelectedIndex switch
        {
            1 => DeliveryStatus.Offen,
            2 => DeliveryStatus.Abgeschlossen,
            3 => DeliveryStatus.Fakturiert,
            4 => DeliveryStatus.Storniert,
            _ => null
        };

        IReadOnlyList<LieferscheinListDto> liste;

        await _lock.WaitAsync();
        try
        {
            Cursor = Cursors.WaitCursor;
            liste  = await _deliveryService.GetLieferscheinListeAsync(von, bis, kunde, status);
        }
        catch (Exception ex) { ShowError($"Ladefehler:\n{ex.Message}"); return; }
        finally { Cursor = Cursors.Default; _lock.Release(); }

        _suppressChecked = true;
        try
        {
            dgwLieferscheine.DataSource = null;
            dgwLieferscheine.DataSource = liste.ToList();
            StyleGrid();
            ApplyCheckmarks();
        }
        finally { _suppressChecked = false; }

        UpdateButtonStates();
    }

    // ── Grid stylen ───────────────────────────────────────────────────────────

    private void StyleGrid()
    {
        if (dgwLieferscheine.Columns.Count == 0) return;

        ApplyColumnHeaders(dgwLieferscheine, _columnHeaders);
        ConfigureGrid(dgwLieferscheine);

        if (dgwLieferscheine.Columns.Contains("colLsChecked"))
        {
            dgwLieferscheine.Columns["colLsChecked"].Visible    = true;
            dgwLieferscheine.Columns["colLsChecked"].ReadOnly   = false;
            dgwLieferscheine.Columns["colLsChecked"].Width      = 30;
            dgwLieferscheine.Columns["colLsChecked"].HeaderText = "";
        }

        foreach (DataGridViewColumn col in dgwLieferscheine.Columns)
        {
            if (col.Name == "colLsChecked") continue;
            col.Visible = false;
        }

        ShowCol("LieferscheinNr",   "Lieferschein-Nr.", 125);
        ShowCol("Lieferdatum",      "Lieferdatum",       95, format: "dd.MM.yyyy");
        ShowCol("Kundenname",       "Kunde",              0, fill: true);
        ShowCol("Tour",             "Tour",              80);
        ShowCol("AuftragNr",        "Auftrag-Nr.",      110);
        ShowCol("Status",           "Status",            95);
        ShowCol("AnzahlPositionen", "Pos.",              50, right: true);
        ShowCol("Gesamtbetrag",     "Gesamt",            95, format: "N2", right: true);

        dgwLieferscheine.ReadOnly = false;
        if (dgwLieferscheine.Columns.Contains("colLsChecked"))
            dgwLieferscheine.Columns["colLsChecked"].ReadOnly = false;

        foreach (DataGridViewColumn col in dgwLieferscheine.Columns)
        {
            if (col.Name != "colLsChecked") col.ReadOnly = true;
        }

        ApplyColumnChooserSettings(dgwLieferscheine);
    }

    private void ShowCol(string name, string header, int width,
        bool fill = false, string? format = null, bool right = false)
    {
        if (!dgwLieferscheine.Columns.Contains(name)) return;
        var col = dgwLieferscheine.Columns[name];
        col.Visible    = true;
        col.HeaderText = header;
        if (fill) col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        else { col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; col.Width = width; }
        if (format is not null) col.DefaultCellStyle.Format = format;
        if (right) col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
    }

    // ── Checkmarks anwenden ───────────────────────────────────────────────────

    private void ApplyCheckmarks()
    {
        foreach (DataGridViewRow row in dgwLieferscheine.Rows)
        {
            if (row.DataBoundItem is not LieferscheinListDto dto) continue;
            if (!dgwLieferscheine.Columns.Contains("colLsChecked")) continue;
            row.Cells["colLsChecked"].Value = _checkedIds.Contains(dto.Id);
        }
    }

    // ── Checkbox: commit on single click ──────────────────────────────────────

    private void DgwLieferscheine_DirtyStateChanged(object? s, EventArgs e)
    {
        if (dgwLieferscheine.IsCurrentCellDirty &&
            dgwLieferscheine.CurrentCell?.OwningColumn?.Name == "colLsChecked")
            dgwLieferscheine.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }

    private void DgwLieferscheine_CellValueChanged(object? s, DataGridViewCellEventArgs e)
    {
        if (_suppressChecked || e.RowIndex < 0) return;
        if (dgwLieferscheine.Columns[e.ColumnIndex]?.Name != "colLsChecked") return;
        if (dgwLieferscheine.Rows[e.RowIndex].DataBoundItem is not LieferscheinListDto dto) return;

        if (dto.Status != DeliveryStatus.Offen)
        {
            _suppressChecked = true;
            dgwLieferscheine.Rows[e.RowIndex].Cells["colLsChecked"].Value = false;
            _suppressChecked = false;
            MessageBox.Show("Nur offene Lieferscheine können markiert werden.",
                "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        bool isChecked = dgwLieferscheine.Rows[e.RowIndex].Cells["colLsChecked"].Value is true;
        if (isChecked) _checkedIds.Add(dto.Id);
        else           _checkedIds.Remove(dto.Id);

        UpdateButtonStates();
    }

    // ── Leertaste = Checkbox umschalten ───────────────────────────────────────

    private void DgwLieferscheine_KeyDown(object? s, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Space) return;
        if (dgwLieferscheine.CurrentRow?.DataBoundItem is not LieferscheinListDto dto) return;

        e.Handled          = true;
        e.SuppressKeyPress = true;

        if (dto.Status != DeliveryStatus.Offen)
        {
            MessageBox.Show("Nur offene Lieferscheine können markiert werden.",
                "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        bool current = _checkedIds.Contains(dto.Id);
        if (current) _checkedIds.Remove(dto.Id);
        else         _checkedIds.Add(dto.Id);

        if (dgwLieferscheine.Columns.Contains("colLsChecked"))
            dgwLieferscheine.CurrentRow.Cells["colLsChecked"].Value = !current;

        UpdateButtonStates();
    }

    // ── Alle markieren (toggle) ───────────────────────────────────────────────

    private void BtnAlleMarkieren_Click(object? s, EventArgs e)
    {
        // Only Offen rows can be batch-processed
        const DeliveryStatus filterStatus = DeliveryStatus.Offen;

        int total = 0, checked_ = 0;
        foreach (DataGridViewRow row in dgwLieferscheine.Rows)
        {
            if (row.DataBoundItem is not LieferscheinListDto dto) continue;
            if (dto.Status != filterStatus) continue;
            total++;
            if (_checkedIds.Contains(dto.Id)) checked_++;
        }

        if (total == 0) return;

        foreach (DataGridViewRow row in dgwLieferscheine.Rows)
        {
            if (row.DataBoundItem is not LieferscheinListDto dto) continue;
            if (dto.Status != filterStatus) continue;

            if (checked_ < total) _checkedIds.Add(dto.Id);
            else                  _checkedIds.Remove(dto.Id);
        }

        _suppressChecked = true;
        try { ApplyCheckmarks(); }
        finally { _suppressChecked = false; }

        dgwLieferscheine.Refresh();
        UpdateButtonStates();
    }

    // ── Grid-Farben + Status-Text ──────────────────────────────────────────────

    private void DgwLieferscheine_CellFormatting(object? s, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0) return;
        if (dgwLieferscheine.Rows[e.RowIndex].DataBoundItem is not LieferscheinListDto dto) return;

        dgwLieferscheine.Rows[e.RowIndex].DefaultCellStyle.BackColor =
            StatusColorHelper.GetDeliveryStatusBackColor(dto.Status);

        if (dgwLieferscheine.Columns[e.ColumnIndex].Name == "Status")
        {
            e.Value = dto.Status switch
            {
                DeliveryStatus.Offen         => "Offen",
                DeliveryStatus.Abgeschlossen => "Abgeschlossen",
                DeliveryStatus.Fakturiert    => "Fakturiert",
                DeliveryStatus.Storniert     => "Storniert",
                _                            => dto.Status.ToString()
            };
            e.FormattingApplied = true;
        }
    }

    // ── Button-Zustände ───────────────────────────────────────────────────────

    private void DgwLieferscheine_SelectionChanged(object? s, EventArgs e)
        => UpdateButtonStates();

    private void UpdateButtonStates()
    {
        var dto = SelectedDto();

        bool hasOffen = dgwLieferscheine.Rows
            .Cast<DataGridViewRow>()
            .Any(r => r.DataBoundItem is LieferscheinListDto a && a.Status == DeliveryStatus.Offen);

        bool hasCheckedOffen = _checkedIds.Count > 0;

        // Abschliessen: selected Offen OR at least one checked
        ApplyBtnState(btnAbschliessen,
            dto?.Status == DeliveryStatus.Offen || hasCheckedOffen);

        // Stornieren: Offen or Abgeschlossen (single selection)
        ApplyBtnState(btnStornieren,
            dto?.Status == DeliveryStatus.Offen ||
            dto?.Status == DeliveryStatus.Abgeschlossen);

        ApplyBtnState(btnAlleMarkieren, hasOffen);
    }

    private static void ApplyBtnState(Button btn, bool enabled)
    {
        btn.Enabled   = enabled;
        btn.ForeColor = SystemColors.ControlText;
    }

    // ── Abschliessen (Batch) ──────────────────────────────────────────────────

    private async void BtnAbschliessen_Click(object? s, EventArgs e)
    {
        var targets = new List<int>();

        foreach (DataGridViewRow row in dgwLieferscheine.Rows)
        {
            if (row.DataBoundItem is not LieferscheinListDto dto) continue;
            if (dto.Status == DeliveryStatus.Offen && _checkedIds.Contains(dto.Id))
                targets.Add(dto.Id);
        }

        // Fallback: single selected row
        if (targets.Count == 0)
        {
            var sel = SelectedDto();
            if (sel is null || sel.Status != DeliveryStatus.Offen) return;
            targets.Add(sel.Id);
        }

        if (!Confirm($"{targets.Count} Lieferschein(e) abschliessen?")) return;

        int success = 0;
        var errors = new List<string>();

        await _lock.WaitAsync();
        try
        {
            Cursor = Cursors.WaitCursor;
            foreach (var id in targets)
            {
                try
                {
                    await _deliveryService.AbschliessenAsync(id);
                    success++;
                    _checkedIds.Remove(id);
                }
                catch (ValidationException ex) { errors.Add(ex.Message); }
                catch (Exception ex)           { errors.Add($"Id {id}: {ex.Message}"); }
            }
        }
        finally { Cursor = Cursors.Default; _lock.Release(); }

        if (errors.Count > 0)
            ShowError($"{success} Lieferschein(e) abgeschlossen.\n\nFehler:\n{string.Join("\n", errors)}");

        await LoadListAsync();
    }

    // ── Stornieren (Einzel) ───────────────────────────────────────────────────

    private async void BtnStornieren_Click(object? s, EventArgs e)
    {
        var dto = SelectedDto();
        if (dto is null) return;
        if (dto.Status == DeliveryStatus.Fakturiert)
        {
            ShowError("Fakturierte Lieferscheine können nicht storniert werden.");
            return;
        }
        if (!Confirm($"Lieferschein '{dto.LieferscheinNr}' stornieren?\n\nDer verknüpfte Auftrag wird wieder auf 'Freigegeben' gesetzt."))
            return;

        try
        {
            await _lock.WaitAsync();
            try { await _deliveryService.StornierenAsync(dto.Id); }
            finally { _lock.Release(); }

            await LoadListAsync();
        }
        catch (ValidationException ex) { ShowError(ex.Message); }
        catch (Exception ex)           { ShowError($"Fehler:\n{ex.Message}"); }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private LieferscheinListDto? SelectedDto() =>
        dgwLieferscheine.CurrentRow?.DataBoundItem as LieferscheinListDto;

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.F5) { _ = LoadListAsync(); return true; }
        return base.ProcessCmdKey(ref msg, keyData);
    }
}