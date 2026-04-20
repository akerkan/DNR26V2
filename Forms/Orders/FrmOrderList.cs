using System.ComponentModel;
using DNR26V2.Domain.DTOs;
using DNR26V2.Domain.Enums;
using DNR26V2.Domain.Exceptions;
using DNR26V2.Forms.Base;
using DNR26V2.Helpers;
using DNR26V2.Services.Orders;

namespace DNR26V2.Forms.Orders;

public partial class FrmOrderList : BaseListForm
{
    private static readonly Dictionary<string, string> _columnHeaders = new()
    {
        ["Id"]               = "Id",
        ["KundeId"]          = "KundeId",
        ["AuftragNr"]        = "Auftrag-Nr.",
        ["Lieferdatum"]      = "Lieferdatum",
        ["Kundenname"]       = "Kunde",
        ["Tour"]             = "Tour",
        ["Status"]           = "Status",
        ["LieferscheinNr"]   = "Lieferschein-Nr.",
        ["AnzahlPositionen"] = "Pos.",
        ["Gesamtbetrag"]     = "Gesamtbetrag",
    };

    private readonly IOrderService     _orderService;
    private readonly SemaphoreSlim     _lock      = new(1, 1);
    private readonly HashSet<int>      _checkedIds = new();
    private bool                       _isLoading;
    private bool                       _suppressChecked;

    // ── Konstruktoren ─────────────────────────────────────────────────────────

    public FrmOrderList(IOrderService orderService)
    {
        _orderService = orderService;
        InitializeComponent();
        WireUpEvents();
    }

    public FrmOrderList()
    {
        _orderService = null!;
        InitializeComponent();
        WireUpEvents();
    }

    private static bool IsDesignMode() =>
        LicenseManager.UsageMode == LicenseUsageMode.Designtime;

    // ── Events verdrahten ─────────────────────────────────────────────────────

    private void WireUpEvents()
    {
        Load += FrmOrderList_Load;

        btnSuchen.Click  += async (_, _) => await LoadListAsync();
        txtKunde.KeyDown += async (s, e) => { if (e.KeyCode == Keys.Enter) await LoadListAsync(); };

        dgwAuftraege.CellFormatting               += DgwAuftraege_CellFormatting;
        dgwAuftraege.SelectionChanged             += DgwAuftraege_SelectionChanged;
        dgwAuftraege.CellDoubleClick              += DgwAuftraege_CellDoubleClick;
        dgwAuftraege.CurrentCellDirtyStateChanged += DgwAuftraege_DirtyStateChanged;
        dgwAuftraege.CellValueChanged             += DgwAuftraege_CellValueChanged;
        dgwAuftraege.KeyDown                      += DgwAuftraege_KeyDown;

        btnFreigeben.Click    += BtnFreigeben_Click;
        btnStornieren.Click   += BtnOeffnen_Click;
        btnLoeschen.Click     += BtnLoeschen_Click;
        btnBuchen.Click       += BtnBuchen_Click;
        btnAlleMarkieren.Click += BtnAlleMarkieren_Click;

        EnableColumnChooser(dgwAuftraege);
    }

    // ── Load ──────────────────────────────────────────────────────────────────

    private async void FrmOrderList_Load(object? sender, EventArgs e)
    {
        if (IsDesignMode() || _orderService is null) return;

        WindowState = FormWindowState.Maximized;

        _isLoading = true;
        dtpVon.Value = DateTime.Today.AddDays(-7);
        dtpBis.Value = DateTime.Today;
        _isLoading = false;
        cmbStatus.SelectedIndex = 2;   // Standard: Freigegeben — bereit zum Buchen

        await LoadListAsync();
    }

    // ── Liste laden ───────────────────────────────────────────────────────────

    private async Task LoadListAsync()
    {
        if (_orderService is null || _isLoading) return;

        DateTime?    von    = dtpVon.Value.Date;
        DateTime?    bis    = dtpBis.Value.Date;
        string?      kunde  = string.IsNullOrWhiteSpace(txtKunde.Text) ? null : txtKunde.Text.Trim();
        OrderStatus? status = cmbStatus.SelectedIndex switch
        {
            1 => OrderStatus.Offen,
            2 => OrderStatus.Freigegeben,
            3 => OrderStatus.Gebucht,
            4 => OrderStatus.Storniert,
            _ => null
        };

        IReadOnlyList<AuftragListDto> liste;

        await _lock.WaitAsync();
        try
        {
            Cursor = Cursors.WaitCursor;
            liste  = await _orderService.GetAuftragListeAsync(von, bis, kunde, status);
        }
        catch (Exception ex) { ShowError($"Ladefehler:\n{ex.Message}"); return; }
        finally { Cursor = Cursors.Default; _lock.Release(); }

        _suppressChecked = true;
        try
        {
            dgwAuftraege.DataSource = null;
            dgwAuftraege.DataSource = liste.ToList();
            StyleGrid();
            ApplyCheckmarks();
        }
        finally { _suppressChecked = false; }

        UpdateButtonStates();
    }

    // ── Grid stylen ───────────────────────────────────────────────────────────

    private void StyleGrid()
    {
        if (dgwAuftraege.Columns.Count == 0) return;

        ApplyColumnHeaders(dgwAuftraege, _columnHeaders);
        ConfigureGrid(dgwAuftraege);

        // Keep checkbox column visible and not read-only
        if (dgwAuftraege.Columns.Contains("colAuftragChecked"))
        {
            dgwAuftraege.Columns["colAuftragChecked"].Visible   = true;
            dgwAuftraege.Columns["colAuftragChecked"].ReadOnly  = false;
            dgwAuftraege.Columns["colAuftragChecked"].Width     = 30;
            dgwAuftraege.Columns["colAuftragChecked"].HeaderText = "";
        }

        foreach (DataGridViewColumn col in dgwAuftraege.Columns)
        {
            if (col.Name == "colAuftragChecked") continue;
            col.Visible = false;
        }

        ShowCol("AuftragNr",        "Auftrag-Nr.",      110);
        ShowCol("Lieferdatum",      "Lieferdatum",       95, format: "dd.MM.yyyy");
        ShowCol("Kundenname",       "Kunde",              0, fill: true);
        ShowCol("Tour",             "Tour",              80);
        ShowCol("Status",           "Status",            95);
        ShowCol("LieferscheinNr",   "Lieferschein-Nr.", 125);
        ShowCol("AnzahlPositionen", "Pos.",              50, right: true);
        ShowCol("Gesamtbetrag",     "Gesamt",            95, format: "N2", right: true);

        // Checkbox column must stay non-readonly after ConfigureGrid sets ReadOnly=true
        dgwAuftraege.ReadOnly = false;
        if (dgwAuftraege.Columns.Contains("colAuftragChecked"))
            dgwAuftraege.Columns["colAuftragChecked"].ReadOnly = false;

        foreach (DataGridViewColumn col in dgwAuftraege.Columns)
        {
            if (col.Name != "colAuftragChecked") col.ReadOnly = true;
        }

        ApplyColumnChooserSettings(dgwAuftraege);
    }

    private void ShowCol(string name, string header, int width,
        bool fill = false, string? format = null, bool right = false)
    {
        if (!dgwAuftraege.Columns.Contains(name)) return;
        var col = dgwAuftraege.Columns[name];
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
        foreach (DataGridViewRow row in dgwAuftraege.Rows)
        {
            if (row.DataBoundItem is not AuftragListDto dto) continue;
            if (!dgwAuftraege.Columns.Contains("colAuftragChecked")) continue;
            row.Cells["colAuftragChecked"].Value = _checkedIds.Contains(dto.Id);
        }
    }

    // ── Checkbox: commit on single click ──────────────────────────────────────

    private void DgwAuftraege_DirtyStateChanged(object? s, EventArgs e)
    {
        if (dgwAuftraege.IsCurrentCellDirty &&
            dgwAuftraege.CurrentCell?.OwningColumn?.Name == "colAuftragChecked")
            dgwAuftraege.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }

    private void DgwAuftraege_CellValueChanged(object? s, DataGridViewCellEventArgs e)
    {
        if (_suppressChecked || e.RowIndex < 0) return;
        if (dgwAuftraege.Columns[e.ColumnIndex]?.Name != "colAuftragChecked") return;
        if (dgwAuftraege.Rows[e.RowIndex].DataBoundItem is not AuftragListDto dto) return;

        // Only Freigegeben orders can be checked (for Buchen)
        if (dto.Status != OrderStatus.Freigegeben)
        {
            _suppressChecked = true;
            dgwAuftraege.Rows[e.RowIndex].Cells["colAuftragChecked"].Value = false;
            _suppressChecked = false;
            MessageBox.Show("Nur freigegebene Aufträge können zum Buchen markiert werden.",
                "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        bool isChecked = dgwAuftraege.Rows[e.RowIndex].Cells["colAuftragChecked"].Value is true;
        if (isChecked) _checkedIds.Add(dto.Id);
        else           _checkedIds.Remove(dto.Id);

        UpdateButtonStates();
    }

    // ── Leertaste = Checkbox umschalten ───────────────────────────────────────

    private void DgwAuftraege_KeyDown(object? s, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Space) return;
        if (dgwAuftraege.CurrentRow?.DataBoundItem is not AuftragListDto dto) return;

        e.Handled         = true;
        e.SuppressKeyPress = true;

        if (dto.Status != OrderStatus.Freigegeben)
        {
            MessageBox.Show("Nur freigegebene Aufträge können markiert werden.",
                "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        bool current = _checkedIds.Contains(dto.Id);
        if (current) _checkedIds.Remove(dto.Id);
        else         _checkedIds.Add(dto.Id);

        if (dgwAuftraege.Columns.Contains("colAuftragChecked"))
            dgwAuftraege.CurrentRow.Cells["colAuftragChecked"].Value = !current;

        UpdateButtonStates();
    }

    // ── Alle markieren ────────────────────────────────────────────────────────

    private void BtnAlleMarkieren_Click(object? s, EventArgs e)
    {
        // Determine current status filter: 1 = Offen, 2 = Freigegeben
        OrderStatus? filterStatus = cmbStatus.SelectedIndex switch
        {
            1 => OrderStatus.Offen,
            2 => OrderStatus.Freigegeben,
            _ => null
        };

        if (filterStatus is null) return;

        // Count visible rows with matching status and how many of them are currently checked
        int totalMatching = 0;
        int checkedMatching = 0;

        foreach (DataGridViewRow row in dgwAuftraege.Rows)
        {
            if (row.DataBoundItem is not AuftragListDto dto) continue;
            if (dto.Status != filterStatus) continue;

            totalMatching++;
            if (_checkedIds.Contains(dto.Id)) checkedMatching++;
        }

        if (totalMatching == 0) return;

        // Toggle: if not all selected => select all; if all selected => deselect all
        if (checkedMatching < totalMatching)
        {
            foreach (DataGridViewRow row in dgwAuftraege.Rows)
            {
                if (row.DataBoundItem is not AuftragListDto dto) continue;
                if (dto.Status == filterStatus)
                    _checkedIds.Add(dto.Id);
            }
        }
        else
        {
            foreach (DataGridViewRow row in dgwAuftraege.Rows)
            {
                if (row.DataBoundItem is not AuftragListDto dto) continue;
                if (dto.Status == filterStatus)
                    _checkedIds.Remove(dto.Id);
            }
        }

        _suppressChecked = true;
        try { ApplyCheckmarks(); }
        finally { _suppressChecked = false; }

        dgwAuftraege.Refresh();
        UpdateButtonStates();
    }

    // ── Grid-Farben + Status-Text ──────────────────────────────────────────────

    private void DgwAuftraege_CellFormatting(object? s, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0) return;
        if (dgwAuftraege.Rows[e.RowIndex].DataBoundItem is not AuftragListDto dto) return;

        dgwAuftraege.Rows[e.RowIndex].DefaultCellStyle.BackColor =
            StatusColorHelper.GetOrderStatusBackColor(dto.Status);

        if (dgwAuftraege.Columns[e.ColumnIndex].Name == "Status")
        {
            e.Value = dto.Status switch
            {
                OrderStatus.Offen       => "Offen",
                OrderStatus.Freigegeben => "Freigegeben",
                OrderStatus.Gebucht     => "Gebucht",
                OrderStatus.Storniert   => "Storniert",
                _                       => dto.Status.ToString()
            };
            e.FormattingApplied = true;
        }
    }

    // ── Button-Zustände ───────────────────────────────────────────────────────

    private void DgwAuftraege_SelectionChanged(object? s, EventArgs e)
        => UpdateButtonStates();

    private void UpdateButtonStates()
    {
        var dto = SelectedDto();

        // Determine if there are visible rows matching Offen/Freigegeben
        bool hasFreigegeben = dgwAuftraege.Rows
            .Cast<DataGridViewRow>()
            .Any(r => r.DataBoundItem is AuftragListDto a && a.Status == OrderStatus.Freigegeben);

        bool hasOffen = dgwAuftraege.Rows
            .Cast<DataGridViewRow>()
            .Any(r => r.DataBoundItem is AuftragListDto a && a.Status == OrderStatus.Offen);

        // Any checked Offen entries?
        bool hasCheckedOffen = dgwAuftraege.Rows
            .Cast<DataGridViewRow>()
            .Any(r => r.DataBoundItem is AuftragListDto a && _checkedIds.Contains(a.Id) && a.Status == OrderStatus.Offen);

        // Button states: Freigeben enabled when selected is Offen OR at least one checked Offen exists
        ApplyBtnState(btnFreigeben, dto?.Status == OrderStatus.Offen || hasCheckedOffen);
        ApplyBtnState(btnStornieren, dto?.Status == OrderStatus.Freigegeben);
        ApplyBtnState(btnLoeschen, dto?.Status == OrderStatus.Offen);
        ApplyBtnState(btnBuchen, _checkedIds.Count > 0);

        // Enable "Alle markieren" when the grid contains at least one Offen or Freigegeben row,
        // depending on which status the user filtered to — simplify: enable if either present,
        // but earlier logic will toggle only the currently filtered status.
        ApplyBtnState(btnAlleMarkieren, hasFreigegeben || hasOffen);
    }

    /// <summary>
    /// Setzt Enabled und setzt ForeColor hart zurück — verhindert
    /// den "grau aber klickbar" Effekt durch veraltete ForeColor-Overrides.
    /// </summary>
    private static void ApplyBtnState(Button btn, bool enabled)
    {
        btn.Enabled = enabled;
        btn.ForeColor = SystemColors.ControlText;   // WinForms grayt bei Enabled=false selbst
    }

    // ── Doppelklick → FrmOrderEntry navigieren ────────────────────────────────

    private void DgwAuftraege_CellDoubleClick(object? s, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        if (dgwAuftraege.Columns[e.ColumnIndex]?.Name == "colAuftragChecked") return;
        if (dgwAuftraege.Rows[e.RowIndex].DataBoundItem is not AuftragListDto dto) return;

        if (MdiParent is FrmMain main)
            main.OpenAuftragserfassung(dto.KundeId, dto.Lieferdatum);
    }

    // ── Buchen (Lieferschein erstellen für markierte Freigegeben-Aufträge) ────

    private async void BtnBuchen_Click(object? s, EventArgs e)
    {
        if (_checkedIds.Count == 0) return;

        var targets = _checkedIds.ToList();

        if (!Confirm($"{targets.Count} Auftrag/Aufträge buchen und Lieferscheine erstellen?")) return;

        int success = 0;
        var errors  = new List<string>();

        await _lock.WaitAsync();
        try
        {
            Cursor = Cursors.WaitCursor;
            foreach (var id in targets)
            {
                try
                {
                    await _orderService.BuchenAsync(id);
                    success++;
                }
                catch (ValidationException ex) { errors.Add(ex.Message); }
                catch (Exception ex)           { errors.Add($"Id {id}: {ex.Message}"); }
            }
        }
        finally { Cursor = Cursors.Default; _lock.Release(); }

        _checkedIds.Clear();

        if (errors.Count > 0)
            ShowError($"{success} Auftrag/Aufträge gebucht.\n\nFehler:\n{string.Join("\n", errors)}");

        await LoadListAsync();
    }

    // ── Freigeben ─────────────────────────────────────────────────────────────

    private async void BtnFreigeben_Click(object? s, EventArgs e)
    {
        // Collect targets: checked Offen orders first
        var targets = new List<int>();

        foreach (DataGridViewRow row in dgwAuftraege.Rows)
        {
            if (row.DataBoundItem is not AuftragListDto dto) continue;
            if (dto.Status == OrderStatus.Offen && _checkedIds.Contains(dto.Id))
                targets.Add(dto.Id);
        }

        // If no checked Offen orders, fall back to single selected Offen row
        if (targets.Count == 0)
        {
            var sel = SelectedDto();
            if (sel is null || sel.Status != OrderStatus.Offen) return;
            targets.Add(sel.Id);
        }

        if (targets.Count == 0) return;

        if (!Confirm($"{targets.Count} Auftrag/Aufträge freigeben?")) return;

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
                    await _orderService.FreigebenAsync(id);
                    success++;
                    _checkedIds.Remove(id);
                }
                catch (ValidationException ex) { errors.Add(ex.Message); }
                catch (Exception ex) { errors.Add($"Id {id}: {ex.Message}"); }
            }
        }
        finally
        {
            Cursor = Cursors.Default;
            _lock.Release();
        }

        if (errors.Count > 0)
            ShowError($"{success} Auftrag/Aufträge freigegeben.\n\nFehler:\n{string.Join("\n", errors)}");

        await LoadListAsync();
    }

    // ── Öffnen (Freigegeben → Offen) ─────────────────────────────────────────

    private async void BtnOeffnen_Click(object? s, EventArgs e)
    {
        // Collect checked Freigegeben-Aufträge
        var targets = new List<int>();
        foreach (DataGridViewRow row in dgwAuftraege.Rows)
        {
            if (row.DataBoundItem is not AuftragListDto dto) continue;
            if (dto.Status == OrderStatus.Freigegeben && _checkedIds.Contains(dto.Id))
                targets.Add(dto.Id);
        }

        // Fallback: wenn nichts markiert ist → die aktuell ausgewählte Zeile verwenden
        if (targets.Count == 0)
        {
            var sel = SelectedDto();
            if (sel is null || sel.Status != OrderStatus.Freigegeben) return;
            targets.Add(sel.Id);
        }

        if (!Confirm($"{targets.Count} Auftrag/Aufträge wieder auf Offen setzen?")) return;

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
                    await _orderService.OeffnenAsync(id);
                    success++;
                    _checkedIds.Remove(id);
                }
                catch (ValidationException ex) { errors.Add(ex.Message); }
                catch (Exception ex) { errors.Add($"Id {id}: {ex.Message}"); }
            }
        }
        finally
        {
            Cursor = Cursors.Default;
            _lock.Release();
        }

        if (errors.Count > 0)
            ShowError($"{success} Auftrag/Aufträge geöffnet.\n\nFehler:\n{string.Join("\n", errors)}");

        await LoadListAsync();
    }

    // ── Stornieren ────────────────────────────────────────────────────────────

    //private async void BtnStornieren_Click(object? s, EventArgs e)
    //{
    //    var dto = SelectedDto();
    //    if (dto is null) return;
    //    if (!Confirm($"Auftrag '{dto.AuftragNr}' stornieren?")) return;

    //    try
    //    {
    //        await _lock.WaitAsync();
    //        try { await _orderService.StornierenAsync(dto.Id); }
    //        finally { _lock.Release(); }

    //        await LoadListAsync();
    //    }
    //    catch (ValidationException ex) { ShowError(ex.Message); }
    //    catch (Exception ex)           { ShowError($"Fehler:\n{ex.Message}"); }
    //}

    // ── Löschen ───────────────────────────────────────────────────────────────

    private async void BtnLoeschen_Click(object? s, EventArgs e)
    {
        var dto = SelectedDto();
        if (dto is null || dto.Status != OrderStatus.Offen) return;
        if (!Confirm($"Auftrag '{dto.AuftragNr}' löschen?")) return;

        try
        {
            await _lock.WaitAsync();
            try { await _orderService.LoeschenAsync(dto.Id); }
            finally { _lock.Release(); }

            await LoadListAsync();
        }
        catch (ValidationException ex) { ShowError(ex.Message); }
        catch (Exception ex)           { ShowError($"Fehler:\n{ex.Message}"); }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private AuftragListDto? SelectedDto() =>
        dgwAuftraege.CurrentRow?.DataBoundItem as AuftragListDto;

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.F5) { _ = LoadListAsync(); return true; }
        return base.ProcessCmdKey(ref msg, keyData);
    }
}