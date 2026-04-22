using System.ComponentModel;
using DNR26V2.Domain.DTOs;
using DNR26V2.Forms.Base;
using DNR26V2.Services.Invoices;

namespace DNR26V2.Forms.Invoices;

public partial class FrmSammelRechnung : BaseListForm
{
    private readonly IInvoiceService _invoiceService;
    private readonly SemaphoreSlim   _lock           = new(1, 1);
    private readonly HashSet<int>    _checkedKunden  = [];
    private bool                     _suppressChecked;
    private bool                     _isLoading;

    public FrmSammelRechnung()
    {
        _invoiceService = null!;
        InitializeComponent();
        WireUpEvents();
    }

    public FrmSammelRechnung(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
        InitializeComponent();
        WireUpEvents();
    }

    private static bool IsDesignMode() =>
        LicenseManager.UsageMode == LicenseUsageMode.Designtime;

    // ── Events ───────────────────────────────────────────────────────────────

    private void WireUpEvents()
    {
        Load += FrmSammelRechnung_Load;

        btnSuchen.Click           += async (_, _) => await LoadKundenAsync();
        btnAlleAuswaehlen.Click   += (_, _) => SetAlleChecked(true);
        btnAlleAbwaehlen.Click    += (_, _) => SetAlleChecked(false);
        btnSammelBuchen.Click     += BtnSammelBuchen_Click;

        dgwKunden.CurrentCellDirtyStateChanged += DgwKunden_DirtyStateChanged;
        dgwKunden.CellValueChanged             += DgwKunden_CellValueChanged;
        dgwKunden.CellFormatting               += DgwKunden_CellFormatting;
        dgwKunden.KeyDown                      += DgwKunden_KeyDown;
    }

    // ── Load ─────────────────────────────────────────────────────────────────

    private async void FrmSammelRechnung_Load(object? sender, EventArgs e)
    {
        if (IsDesignMode() || _invoiceService is null) return;

        WindowState = FormWindowState.Maximized;

        _isLoading = true;
        dtpVon.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        dtpBis.Value = DateTime.Today;
        _isLoading   = false;

        panelDetail.Visible = true;
        UpdateSummary();
        await LoadKundenAsync();
    }

    // ── Kunden laden ─────────────────────────────────────────────────────────

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

        _checkedKunden.Clear();

        _suppressChecked = true;
        try
        {
            dgwKunden.DataSource = null;
            dgwKunden.DataSource = liste.ToList();
            StyleGrid();
            // Alle vorauswählen
            foreach (var k in liste) _checkedKunden.Add(k.KundeId);
            ApplyCheckmarks();
        }
        finally { _suppressChecked = false; }

        UpdateSummary();
    }

    private void StyleGrid()
    {
        if (dgwKunden.Columns.Count == 0) return;
        ConfigureGrid(dgwKunden);

        foreach (DataGridViewColumn col in dgwKunden.Columns)
        {
            if (col.Name == "colKundeChecked") continue;
            col.Visible  = false;
            col.ReadOnly = true;
        }

        if (dgwKunden.Columns.Contains("colKundeChecked"))
        {
            dgwKunden.Columns["colKundeChecked"].Visible    = true;
            dgwKunden.Columns["colKundeChecked"].ReadOnly   = false;
            dgwKunden.Columns["colKundeChecked"].Width      = 30;
            dgwKunden.Columns["colKundeChecked"].HeaderText = "";
        }

        ShowCol("Kundennummer",      "Kunden-Nr.",         90);
        ShowCol("Kundenname",        "Name",                 0, fill: true);
        ShowCol("AnzahlOffeneLs",    "Anz. LS",            60, right: true);
        ShowCol("GesamtbetragOffen", "Gesamtbetrag offen", 130, format: "N2", right: true);
    }

    private void ShowCol(string name, string header, int width,
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

    // ── Checkmarks ───────────────────────────────────────────────────────────

    private void ApplyCheckmarks()
    {
        foreach (DataGridViewRow row in dgwKunden.Rows)
        {
            if (row.DataBoundItem is not KundeOffeneLsDto dto) continue;
            if (!dgwKunden.Columns.Contains("colKundeChecked")) continue;
            row.Cells["colKundeChecked"].Value = _checkedKunden.Contains(dto.KundeId);
        }
    }

    private void SetAlleChecked(bool check)
    {
        _suppressChecked = true;
        try
        {
            _checkedKunden.Clear();
            foreach (DataGridViewRow row in dgwKunden.Rows)
            {
                if (row.DataBoundItem is not KundeOffeneLsDto dto) continue;
                if (check) _checkedKunden.Add(dto.KundeId);
                if (dgwKunden.Columns.Contains("colKundeChecked"))
                    row.Cells["colKundeChecked"].Value = check;
            }
        }
        finally { _suppressChecked = false; }

        UpdateSummary();
    }

    private void DgwKunden_DirtyStateChanged(object? s, EventArgs e)
    {
        if (dgwKunden.IsCurrentCellDirty &&
            dgwKunden.CurrentCell?.OwningColumn?.Name == "colKundeChecked")
            dgwKunden.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }

    private void DgwKunden_CellValueChanged(object? s, DataGridViewCellEventArgs e)
    {
        if (_suppressChecked || e.RowIndex < 0) return;
        if (dgwKunden.Columns[e.ColumnIndex]?.Name != "colKundeChecked") return;
        if (dgwKunden.Rows[e.RowIndex].DataBoundItem is not KundeOffeneLsDto dto) return;

        bool isChecked = dgwKunden.Rows[e.RowIndex].Cells["colKundeChecked"].Value is true;
        if (isChecked) _checkedKunden.Add(dto.KundeId);
        else           _checkedKunden.Remove(dto.KundeId);

        UpdateSummary();
    }

    private void DgwKunden_KeyDown(object? s, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Space) return;
        if (dgwKunden.CurrentRow?.DataBoundItem is not KundeOffeneLsDto dto) return;

        e.Handled = e.SuppressKeyPress = true;
        bool current = _checkedKunden.Contains(dto.KundeId);
        if (current) _checkedKunden.Remove(dto.KundeId);
        else         _checkedKunden.Add(dto.KundeId);

        if (dgwKunden.Columns.Contains("colKundeChecked"))
            dgwKunden.CurrentRow.Cells["colKundeChecked"].Value = !current;

        UpdateSummary();
    }

    private void DgwKunden_CellFormatting(object? s, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0) return;
        if (dgwKunden.Rows[e.RowIndex].DataBoundItem is not KundeOffeneLsDto dto) return;
        dgwKunden.Rows[e.RowIndex].DefaultCellStyle.BackColor =
            _checkedKunden.Contains(dto.KundeId)
                ? Color.FromArgb(220, 240, 220)
                : SystemColors.Window;
    }

    // ── Summary ───────────────────────────────────────────────────────────────

    private void UpdateSummary()
    {
        decimal gesamt  = 0m;
        int     anzahl  = 0;

        foreach (DataGridViewRow row in dgwKunden.Rows)
        {
            if (row.DataBoundItem is not KundeOffeneLsDto dto) continue;
            if (!_checkedKunden.Contains(dto.KundeId)) continue;
            gesamt += dto.GesamtbetragOffen;
            anzahl++;
        }

        lblAusgewaehltWert.Text  = $"{anzahl} Kunden";
        lblGesamtWert.Text       = $"{gesamt:N2} €";

        bool canBuchen          = anzahl > 0;
        btnSammelBuchen.Enabled   = canBuchen;
        btnSammelBuchen.BackColor = canBuchen
            ? Color.FromArgb(0, 122, 204)
            : Color.FromArgb(160, 160, 160);
    }

    // ── Buchen ───────────────────────────────────────────────────────────────

    private async void BtnSammelBuchen_Click(object? s, EventArgs e)
    {
        if (_checkedKunden.Count == 0) return;

        if (!Confirm(
            $"Sammelrechnung für {_checkedKunden.Count} Kunden buchen?\n\n" +
            $"Zeitraum: {dtpVon.Value:dd.MM.yyyy} – {dtpBis.Value:dd.MM.yyyy}\n\n" +
            "Für jeden Kunden wird automatisch eine Rechnung erstellt."))
            return;

        Domain.DTOs.SammelrechnungResultDto result;

        await _lock.WaitAsync();
        try
        {
            Cursor                  = Cursors.WaitCursor;
            btnSammelBuchen.Enabled = false;

            result = await _invoiceService.SammelBuchenAsync(
                _checkedKunden.ToList(),
                dtpVon.Value.Date,
                dtpBis.Value.Date);
        }
        catch (Exception ex) { ShowError($"Fehler:\n{ex.Message}"); return; }
        finally { Cursor = Cursors.Default; _lock.Release(); }

        var msg = $"{result.Erstellt} Rechnung(en) erfolgreich gebucht.";
        if (result.Fehler.Count > 0)
            msg += $"\n\nFehler ({result.Fehler.Count}):\n" + string.Join("\n", result.Fehler);

        if (result.Fehler.Count > 0) ShowError(msg);
        else                          ShowSuccess(msg);

        await LoadKundenAsync();
    }

    // ── Keyboard ─────────────────────────────────────────────────────────────

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.F5) { _ = LoadKundenAsync(); return true; }
        return base.ProcessCmdKey(ref msg, keyData);
    }
}