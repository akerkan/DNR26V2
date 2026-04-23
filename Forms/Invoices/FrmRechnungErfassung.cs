using System.ComponentModel;
using DNR26V2.Domain.DTOs;
using DNR26V2.Domain.Enums;
using DNR26V2.Forms.Base;
using DNR26V2.Services.Invoices;

namespace DNR26V2.Forms.Invoices;

public partial class FrmRechnungErfassung : BaseListForm
{
    private readonly IInvoiceService _invoiceService;
    private readonly SemaphoreSlim   _lock            = new(1, 1);
    private readonly HashSet<int>    _checkedLsIds    = [];
    private bool                     _suppressChecked;
    private bool                     _isLoading;

    private KundeOffeneLsDto? _selectedKunde;

    // ── Konstruktoren ─────────────────────────────────────────────────────────

    public FrmRechnungErfassung()
    {
        _invoiceService = null!;
        InitializeComponent();
        WireUpEvents();
    }

    public FrmRechnungErfassung(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
        InitializeComponent();
        WireUpEvents();
    }
    

    private static bool IsDesignMode() =>
        LicenseManager.UsageMode == LicenseUsageMode.Designtime;

    // ── Events verdrahten ─────────────────────────────────────────────────────

    private void WireUpEvents()
    {
        Load += FrmRechnungErfassung_Load;

        btnSuchen.Click += async (_, _) => await LoadKundenAsync();
        dtpVon.ValueChanged += (_, _) => { if (!_isLoading) _ = LoadKundenAsync(); };
        dtpBis.ValueChanged += (_, _) => { if (!_isLoading) _ = LoadKundenAsync(); };

        dgwKunden.SelectionChanged += DgwKunden_SelectionChanged;

        dgwLieferscheine.CellFormatting               += DgwLieferscheine_CellFormatting;
        dgwLieferscheine.CurrentCellDirtyStateChanged += DgwLieferscheine_DirtyStateChanged;
        dgwLieferscheine.CellValueChanged             += DgwLieferscheine_CellValueChanged;
        dgwLieferscheine.KeyDown                      += DgwLieferscheine_KeyDown;
        dgwLieferscheine.SelectionChanged             += DgwLieferscheine_SelectionChanged;

        btnBuchen.Click += BtnBuchen_Click;
    }

    // ── Load ──────────────────────────────────────────────────────────────────

    private async void FrmRechnungErfassung_Load(object? sender, EventArgs e)
    {
        if (IsDesignMode() || _invoiceService is null) return;

        WindowState = FormWindowState.Maximized;

        _isLoading = true;
        dtpVon.Value              = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        dtpBis.Value              = DateTime.Today;
        dtpRechnungsdatum.Value   = DateTime.Today;
        _isLoading = false;

        panelDetail.Visible = true;
        UpdateButtonStates();
        await LoadKundenAsync();
    }

    // ── Kunden laden ──────────────────────────────────────────────────────────

    private async Task LoadKundenAsync()
    {
        if (_invoiceService is null) return;

        IReadOnlyList<KundeOffeneLsDto> liste;

        await _lock.WaitAsync();
        try
        {
            Cursor = Cursors.WaitCursor;
            liste  = await _invoiceService.GetKundenMitOffenenLsAsync(
                dtpVon.Value.Date, dtpBis.Value.Date);
        }
        catch (Exception ex) { ShowError($"Ladefehler:\n{ex.Message}"); return; }
        finally { Cursor = Cursors.Default; _lock.Release(); }

        dgwKunden.DataSource = null;
        dgwKunden.DataSource = liste.ToList();
        StyleGridKunden();

        // If only a single customer is returned, select it so the Lieferscheine list
        // and booking controls become active without requiring an extra click.
        if (dgwKunden.Rows.Count == 1)
        {
            dgwKunden.ClearSelection();
            var row = dgwKunden.Rows[0];
            row.Selected = true;

            int firstVisibleCol = -1;
            for (int i = 0; i < dgwKunden.Columns.Count; i++)
            {
                if (dgwKunden.Columns[i].Visible)
                {
                    firstVisibleCol = i;
                    break;
                }
            }
            if (firstVisibleCol >= 0)
            {
                try { dgwKunden.CurrentCell = row.Cells[firstVisibleCol]; }
                catch { /* ignore if cannot set current cell */ }
            }
        }

        // If we auto-selected the single customer above, load its Lieferscheine immediately
        if (dgwKunden.Rows.Count == 1)
        {
            var dto = dgwKunden.CurrentRow?.DataBoundItem as KundeOffeneLsDto;
            if (dto is not null)
            {
                _selectedKunde = dto;
                _checkedLsIds.Clear();
                // Load Lieferscheine for the selected customer so buttons/preview update
                await LoadLieferscheineAsync(dto.KundeId);
                UpdateTotals();
                UpdateButtonStates();
                return;
            }
        }

        // Default: clear selection state
        _selectedKunde = null;
        _checkedLsIds.Clear();
        dgwLieferscheine.DataSource = null;
        dgwZeilen.DataSource        = null;
        UpdateTotals();
        UpdateButtonStates();
    }

    private void StyleGridKunden()
    {
        if (dgwKunden.Columns.Count == 0) return;
        ConfigureGrid(dgwKunden);
        foreach (DataGridViewColumn col in dgwKunden.Columns) col.Visible = false;

        ShowKundeCol("Kundennummer",      "Kunden-Nr.",   90);
        ShowKundeCol("Kundenname",        "Name",          0, fill: true);
        ShowKundeCol("AnzahlOffeneLs",    "Anz. LS",      60, right: true);
        ShowKundeCol("GesamtbetragOffen", "Offen €",      90, format: "N2", right: true);
    }

    private void ShowKundeCol(string name, string header, int width,
        bool fill = false, string? format = null, bool right = false)
    {
        if (!dgwKunden.Columns.Contains(name)) return;
        var col        = dgwKunden.Columns[name];
        col.Visible    = true;
        col.HeaderText = header;
        if (fill) col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        else { col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; col.Width = width; }
        if (format is not null) col.DefaultCellStyle.Format = format;
        if (right) col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
    }

    // ── Lieferscheine laden ───────────────────────────────────────────────────

    private async void DgwKunden_SelectionChanged(object? s, EventArgs e)
    {
        if (IsDesignMode()) return;
        var dto = dgwKunden.CurrentRow?.DataBoundItem as KundeOffeneLsDto;
        if (dto is null) return;

        _selectedKunde = dto;
        _checkedLsIds.Clear();
        await LoadLieferscheineAsync(dto.KundeId);
    }

    private async Task LoadLieferscheineAsync(int kundeId)
    {
        if (_invoiceService is null) return;

        IReadOnlyList<LieferscheinFuerRechnungDto> liste;

        await _lock.WaitAsync();
        try
        {
            Cursor = Cursors.WaitCursor;
            liste  = await _invoiceService.GetOffeneLieferscheineAsync(
                kundeId, dtpVon.Value.Date, dtpBis.Value.Date);
        }
        catch (Exception ex) { ShowError($"Ladefehler:\n{ex.Message}"); return; }
        finally { Cursor = Cursors.Default; _lock.Release(); }

        // Alle LS standardmäßig angehakt
        _checkedLsIds.Clear();
        foreach (var ls in liste)
            _checkedLsIds.Add(ls.LieferscheinId);

        _suppressChecked = true;
        try
        {
            dgwLieferscheine.DataSource = null;
            dgwLieferscheine.DataSource = liste.ToList();
            StyleGridLieferscheine();
            ApplyCheckmarks();
        }
        finally { _suppressChecked = false; }

        dgwZeilen.DataSource = null;
        UpdateTotals();
        UpdateButtonStates();
    }

    private void StyleGridLieferscheine()
    {
        if (dgwLieferscheine.Columns.Count == 0) return;
        ConfigureGrid(dgwLieferscheine);

        if (dgwLieferscheine.Columns.Contains("colLsChecked"))
        {
            dgwLieferscheine.Columns["colLsChecked"].Visible   = true;
            dgwLieferscheine.Columns["colLsChecked"].ReadOnly  = false;
            dgwLieferscheine.Columns["colLsChecked"].Width     = 30;
            dgwLieferscheine.Columns["colLsChecked"].HeaderText = "";
        }

        foreach (DataGridViewColumn col in dgwLieferscheine.Columns)
        {
            if (col.Name == "colLsChecked") continue;
            col.Visible  = false;
            col.ReadOnly = true;
        }

        ShowLsCol("Lieferscheinnummer", "Lieferschein-Nr.", 130);
        ShowLsCol("Lieferdatum",        "Lieferdatum",       90, format: "dd.MM.yyyy");
        ShowLsCol("AnzahlPositionen",   "Pos.",              50, right: true);
        ShowLsCol("Gesamtbetrag",       "Betrag €",          90, format: "N2", right: true);

        dgwLieferscheine.ReadOnly = false;
        if (dgwLieferscheine.Columns.Contains("colLsChecked"))
            dgwLieferscheine.Columns["colLsChecked"].ReadOnly = false;
    }

    private void ShowLsCol(string name, string header, int width,
        bool fill = false, string? format = null, bool right = false)
    {
        if (!dgwLieferscheine.Columns.Contains(name)) return;
        var col        = dgwLieferscheine.Columns[name];
        col.Visible    = true;
        col.HeaderText = header;
        if (fill) col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        else { col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; col.Width = width; }
        if (format is not null) col.DefaultCellStyle.Format = format;
        if (right) col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
    }

    // ── Checkmarks ───────────────────────────────────────────────────────────

    private void ApplyCheckmarks()
    {
        foreach (DataGridViewRow row in dgwLieferscheine.Rows)
        {
            if (row.DataBoundItem is not LieferscheinFuerRechnungDto dto) continue;
            if (!dgwLieferscheine.Columns.Contains("colLsChecked")) continue;
            row.Cells["colLsChecked"].Value = _checkedLsIds.Contains(dto.LieferscheinId);
        }
    }

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
        if (dgwLieferscheine.Rows[e.RowIndex].DataBoundItem is not LieferscheinFuerRechnungDto dto) return;

        bool isChecked = dgwLieferscheine.Rows[e.RowIndex].Cells["colLsChecked"].Value is true;
        if (isChecked) _checkedLsIds.Add(dto.LieferscheinId);
        else           _checkedLsIds.Remove(dto.LieferscheinId);

        UpdateTotals();
        UpdateButtonStates();
    }

    private void DgwLieferscheine_KeyDown(object? s, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Space) return;
        if (dgwLieferscheine.CurrentRow?.DataBoundItem is not LieferscheinFuerRechnungDto dto) return;

        e.Handled          = true;
        e.SuppressKeyPress = true;

        bool current = _checkedLsIds.Contains(dto.LieferscheinId);
        if (current) _checkedLsIds.Remove(dto.LieferscheinId);
        else         _checkedLsIds.Add(dto.LieferscheinId);

        if (dgwLieferscheine.Columns.Contains("colLsChecked"))
            dgwLieferscheine.CurrentRow.Cells["colLsChecked"].Value = !current;

        UpdateTotals();
        UpdateButtonStates();
    }

    private void DgwLieferscheine_CellFormatting(object? s, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0) return;
        if (dgwLieferscheine.Rows[e.RowIndex].DataBoundItem is not LieferscheinFuerRechnungDto dto) return;

        bool isChecked = _checkedLsIds.Contains(dto.LieferscheinId);
        dgwLieferscheine.Rows[e.RowIndex].DefaultCellStyle.BackColor =
            isChecked ? Color.FromArgb(220, 240, 220) : SystemColors.Window;
    }

    // ── Zeilen-Vorschau ──────────────────────────────────────────────────────

    private async void DgwLieferscheine_SelectionChanged(object? s, EventArgs e)
    {
        if (IsDesignMode() || _invoiceService is null) return;

        if (dgwLieferscheine.CurrentRow?.DataBoundItem is not LieferscheinFuerRechnungDto dto)
        {
            dgwZeilen.DataSource = null;
            return;
        }

        IReadOnlyList<LieferscheinZeileVorschauDto> zeilen;

        try
        {
            zeilen = await _invoiceService.GetZeilenVorschauAsync(dto.LieferscheinId);
        }
        catch (Exception ex)
        {
            ShowError($"Vorschau Ladefehler:\n{ex.Message}");
            zeilen = [];
        }

        dgwZeilen.DataSource = null;
        dgwZeilen.DataSource = zeilen.ToList();
        StyleGridZeilen();
    }

    // StyleGridZeilen() — Gewicht nach Bezeichnung einfügen:

    private void StyleGridZeilen()
    {
        if (dgwZeilen.Columns.Count == 0) return;
        ConfigureGrid(dgwZeilen);
        foreach (DataGridViewColumn col in dgwZeilen.Columns) col.Visible = false;

        ShowZeileCol("Artikelnummer",    "Artikelnr.",       90);
        ShowZeileCol("Bezeichnung",      "Bezeichnung",       0, fill: true);
        ShowZeileCol("Menge",            "Menge",            70, format: "N3", right: true);
        ShowZeileCol("Gewicht",          "Gewicht kg",       80, format: "N3", right: true);
        ShowZeileCol("FakturierteMenge", "Fakt. Menge",      80, format: "N3", right: true);
        ShowZeileCol("Preis",            "Preis €",          80, format: "N2", right: true);
        ShowZeileCol("LineAmount",       "Gesamt €",         90, format: "N2", right: true);
    }

    private void ShowZeileCol(string name, string header, int width,
        bool fill = false, string? format = null, bool right = false)
    {
        if (!dgwZeilen.Columns.Contains(name)) return;
        var col        = dgwZeilen.Columns[name];
        col.Visible    = true;
        col.HeaderText = header;
        col.ReadOnly   = true;
        if (fill) col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        else { col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; col.Width = width; }
        if (format is not null) col.DefaultCellStyle.Format = format;
        if (right) col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
    }

    // ── Totals berechnen ─────────────────────────────────────────────────────

    private void UpdateTotals()
    {
        decimal netto  = 0m;
        decimal brutto = 0m;

        foreach (DataGridViewRow row in dgwLieferscheine.Rows)
        {
            if (row.DataBoundItem is not LieferscheinFuerRechnungDto dto) continue;
            if (!_checkedLsIds.Contains(dto.LieferscheinId)) continue;
            netto  += dto.Gesamtbetrag;
            brutto += dto.Gesamtbrutto;   // pre-calculated via InvoiceCalculator.CalcHeader
        }

        lblNettoWert.Text  = $"{netto:N2} €";
        lblBruttoWert.Text = $"{brutto:N2} €";
    }

    private void UpdateButtonStates()
    {
        bool canBuchen = _selectedKunde is not null && _checkedLsIds.Count > 0;
        btnBuchen.Enabled   = canBuchen;
        btnBuchen.BackColor = canBuchen ? Color.FromArgb(0, 122, 204) : Color.FromArgb(160, 160, 160);
    }

    // ── Buchen ───────────────────────────────────────────────────────────────

    private async void BtnBuchen_Click(object? s, EventArgs e)
    {
        if (_selectedKunde is null || _checkedLsIds.Count == 0) return;

        var notiz = string.IsNullOrWhiteSpace(txtNotiz.Text) ? null : txtNotiz.Text.Trim();
        var kunde = _selectedKunde;

        if (!Confirm(
            $"Rechnung für '{kunde.Kundenname}' über {_checkedLsIds.Count} Lieferschein(e) buchen?\n\n" +
            $"Zeitraum: {dtpVon.Value:dd.MM.yyyy} – {dtpBis.Value:dd.MM.yyyy}\n" +
            $"Brutto: {lblBruttoWert.Text}"))
            return;

        try
        {
            Cursor         = Cursors.WaitCursor;
            btnBuchen.Enabled = false;

            await _lock.WaitAsync();
            try
            {
                await _invoiceService.BuchenAsync(
                    kunde.KundeId,
                    dtpVon.Value.Date,
                    dtpBis.Value.Date,
                    _checkedLsIds,
                    notiz);
            }
            finally { _lock.Release(); }

            ShowSuccess($"Rechnung für '{kunde.Kundenname}' erfolgreich gebucht.");
            txtNotiz.Clear();
            await LoadKundenAsync();
        }
        catch (Exception ex) { ShowError($"Fehler beim Buchen:\n{ex.Message}"); }
        finally
        {
            Cursor = Cursors.Default;
            UpdateButtonStates();
        }
    }

    // ── Keyboard-Shortcuts ────────────────────────────────────────────────────

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.F5)               { _ = LoadKundenAsync(); return true; }
        if (keyData == (Keys.Control | Keys.B)) { BtnBuchen_Click(null, EventArgs.Empty); return true; }
        return base.ProcessCmdKey(ref msg, keyData);
    }
}