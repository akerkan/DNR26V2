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

    private readonly IDeliveryService _deliveryService;
    private readonly SemaphoreSlim    _lock            = new(1, 1);
    private readonly HashSet<int>     _checkedIds      = new();
    private bool                      _isLoading;
    private bool                      _suppressChecked;

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

        btnStornieren.Click    += BtnStornieren_Click;
        btnAlleMarkieren.Click += BtnAlleMarkieren_Click;

        dgwLsPositionen.CellFormatting += DgwLsPositionen_CellFormatting;

        ctxZeileStornieren.Click          += CtxZeileStornieren_Click;
        contextMenuLsPositionen.Opening   += ContextMenuLsPositionen_Opening;

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
        cmbStatus.SelectedIndex = 1;   // Standard: Offen

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
            1 => DeliveryStatus.Offen,           // Aktiv
            2 => DeliveryStatus.TeilStorniert,   // Teil-Storniert
            3 => DeliveryStatus.Storniert,
            4 => DeliveryStatus.Fakturiert,
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
        ClearDetail();
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
                DeliveryStatus.Offen => "Aktiv",
                DeliveryStatus.TeilStorniert => "Teil-Storniert",
                DeliveryStatus.Fakturiert => "Fakturiert",
                DeliveryStatus.Storniert => "Storniert",
                _                            => dto.Status.ToString()
            };
            e.FormattingApplied = true;
        }
    }

    // ── Button-Zustände ───────────────────────────────────────────────────────

    private void DgwLieferscheine_SelectionChanged(object? s, EventArgs e)
    {
        UpdateButtonStates();
        _ = LoadDetailAsync(SelectedDto());
    }

    private void UpdateButtonStates()
    {
        var dto = SelectedDto();

        bool hasOffen = dgwLieferscheine.Rows
            .Cast<DataGridViewRow>()
            .Any(r => r.DataBoundItem is LieferscheinListDto a && a.Status == DeliveryStatus.Offen);

        ApplyBtnState(btnStornieren,
            dto?.Status == DeliveryStatus.Offen ||
            dto?.Status == DeliveryStatus.TeilStorniert);

        ApplyBtnState(btnAlleMarkieren, hasOffen);
    }

    private static void ApplyBtnState(Button btn, bool enabled)
    {
        btn.Enabled   = enabled;
        btn.ForeColor = SystemColors.ControlText;
    }

    // ── Header Stornieren ─────────────────────────────────────────────────────

    private async void BtnStornieren_Click(object? s, EventArgs e)
    {
        var dto = SelectedDto();
        if (dto is null) return;
        if (dto.Status == DeliveryStatus.Fakturiert)
        {
            ShowError("Fakturierte Lieferscheine können nicht storniert werden.");
            return;
        }
        if (!Confirm($"Lieferschein '{dto.LieferscheinNr}' komplett stornieren?\n\nAlle Zeilen werden ungültig."))
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

    // ── Zeile Stornieren (Rechtsklick-Kontextmenü) ────────────────────────────

    private void ContextMenuLsPositionen_Opening(object? s, System.ComponentModel.CancelEventArgs e)
    {
        var dto      = SelectedDto();
        var zeileDuo = dgwLsPositionen.CurrentRow?.DataBoundItem as OrderLineDto;

        // Only show storno option for non-storno lines on open Lieferscheine
        bool canStorno = dto?.Status == DeliveryStatus.Offen
                      && zeileDuo is not null
                      && zeileDuo.Menge > 0;   // storno lines (negative) cannot be storniert again

        ctxZeileStornieren.Enabled = canStorno;

        if (!canStorno && zeileDuo is null)
            e.Cancel = true;   // no row selected — suppress menu entirely
    }

    private async void CtxZeileStornieren_Click(object? s, EventArgs e)
    {
        var lsDatum = SelectedDto();
        if (lsDatum is null) return;

        if (dgwLsPositionen.CurrentRow?.DataBoundItem is not OrderLineDto zeile) return;
        if (zeile.Menge <= 0)
        {
            ShowError("Storno-Zeilen können nicht erneut storniert werden.");
            return;
        }

        if (!Confirm($"Zeile '{zeile.Produktname}' (Menge: {zeile.Menge:N3}) stornieren?\n\nEine Storno-Zeile mit negativer Menge wird erstellt."))
            return;

        try
        {
            await _lock.WaitAsync();
            try { await _deliveryService.StornierenZeileAsync(lsDatum.Id, zeile.OrderLineId); }
            finally { _lock.Release(); }

            // Reload detail grid only — no full list reload needed
            await LoadDetailAsync(lsDatum);
        }
        catch (ValidationException ex) { ShowError(ex.Message); }
        catch (Exception ex)           { ShowError($"Fehler:\n{ex.Message}"); }
    }

    // ── Detail-Panel laden (Read-Only) ────────────────────────────────────────

    private async Task LoadDetailAsync(LieferscheinListDto? dto)
    {
        if (dto is null)
        {
            ClearDetail();
            return;
        }

        lblDetailLsNrWert.Text      = dto.LieferscheinNr;
        lblDetailAuftragNrWert.Text = dto.AuftragNr ?? "-";
        lblDetailKundeWert.Text     = dto.Kundenname;
        lblDetailDatumWert.Text     = dto.Lieferdatum.ToString("dd.MM.yyyy");
        lblDetailGesamtWert.Text    = dto.Gesamtbetrag.ToString("N2") + " €";

        lblDetailStatusWert.Text = dto.Status switch
        {
            DeliveryStatus.Offen         => "Offen",
            DeliveryStatus.TeilStorniert => "Teil-Storniert",
            DeliveryStatus.Fakturiert    => "Fakturiert",
            DeliveryStatus.Storniert     => "Storniert",
            _                            => dto.Status.ToString()
        };
        lblDetailStatusWert.ForeColor = StatusColorHelper.GetDeliveryStatusLabelColor(dto.Status);

        if (_deliveryService is null) return;

        try
        {
            var positionen = await _deliveryService.GetPositionenByLieferscheinIdAsync(dto.Id);
            dgwLsPositionen.DataSource = positionen.ToList();
            StyleGridLsPositionen();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"LoadDetailAsync error LsId={dto.Id}: {ex.Message}");
            dgwLsPositionen.DataSource = null;
        }
    }

    private void ClearDetail()
    {
        lblDetailLsNrWert.Text        = "-";
        lblDetailAuftragNrWert.Text   = "-";
        lblDetailKundeWert.Text       = "-";
        lblDetailDatumWert.Text       = "-";
        lblDetailStatusWert.Text      = "-";
        lblDetailStatusWert.ForeColor = SystemColors.ControlText;
        lblDetailGesamtWert.Text      = "-";
        dgwLsPositionen.DataSource    = null;
    }

    // ── Positions-Grid stylen (Read-Only) ─────────────────────────────────────

    private void StyleGridLsPositionen()
    {
        if (dgwLsPositionen.Columns.Count == 0) return;

        dgwLsPositionen.SuspendLayout();
        try
        {
            ConfigureGrid(dgwLsPositionen);

            foreach (DataGridViewColumn col in dgwLsPositionen.Columns)
                col.Visible = false;

            ShowLsDetailCol("Artikelnummer", "Art.-Nr.",     80);
            ShowLsDetailCol("Produktname",   "Bezeichnung",   0, fill: true);
            ShowLsDetailCol("Menge",         "Menge",         70, format: "N3", right: true);
            ShowLsDetailCol("Gewicht",       "Gewicht",       70, format: "N3", right: true);
            ShowLsDetailCol("Preis",         "Preis",         80, format: "N2", right: true);
            ShowLsDetailCol("Notiz",         "Notiz",        150);
        }
        finally
        {
            dgwLsPositionen.ResumeLayout();
        }
    }

    private void ShowLsDetailCol(string name, string header, int width,
        bool fill = false, string? format = null, bool right = false)
    {
        if (!dgwLsPositionen.Columns.Contains(name)) return;
        var col = dgwLsPositionen.Columns[name];
        col.Visible    = true;
        col.HeaderText = header;
        col.ReadOnly   = true;
        if (fill) col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        else { col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; col.Width = width; }
        if (format is not null) col.DefaultCellStyle.Format = format;
        if (right) col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
    }

    // ── Storno-Zeilen rot hinterlegen ─────────────────────────────────────────

    private void DgwLsPositionen_CellFormatting(object? s, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0) return;
        if (dgwLsPositionen.Rows[e.RowIndex].DataBoundItem is not OrderLineDto dto) return;

        if (dto.Menge < 0)
        {
            dgwLsPositionen.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 220, 220);
            dgwLsPositionen.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.DarkRed;
        }
    }

    // ── Navigation von FrmOrderList (Doppelklick Gebucht) ────────────────────

    public async void NavigateToLieferschein(string lieferscheinNr)
    {
        BringToFront();
        cmbStatus.SelectedIndex = 0;
        await LoadListAsync();

        foreach (DataGridViewRow row in dgwLieferscheine.Rows)
        {
            if (row.DataBoundItem is not LieferscheinListDto dto) continue;
            if (dto.LieferscheinNr != lieferscheinNr) continue;

            dgwLieferscheine.ClearSelection();
            row.Selected = true;
            dgwLieferscheine.FirstDisplayedScrollingRowIndex = row.Index;
            break;
        }
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