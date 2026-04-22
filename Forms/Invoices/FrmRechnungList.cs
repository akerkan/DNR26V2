using System.ComponentModel;
using DNR26V2.Domain.DTOs;
using DNR26V2.Domain.Enums;
using DNR26V2.Forms.Base;
using DNR26V2.Services.Invoices;

namespace DNR26V2.Forms.Invoices;

public partial class FrmRechnungList : BaseListForm
{
    private readonly IInvoiceService _invoiceService;
    private readonly SemaphoreSlim   _lock = new(1, 1);
    private bool                     _isLoading;

    public FrmRechnungList()
    {
        _invoiceService = null!;
        InitializeComponent();
        WireUpEvents();
    }

    public FrmRechnungList(IInvoiceService invoiceService)
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
        Load += FrmRechnungList_Load;

        btnSuchen.Click                  += async (_, _) => await LoadRechnungenAsync();
        txtKundeFilter.KeyDown           += async (_, e) => { if (e.KeyCode == Keys.Enter) await LoadRechnungenAsync(); };
        cmbStatus.SelectedIndexChanged   += async (_, _) => { if (!_isLoading) await LoadRechnungenAsync(); };
        dgwRechnungen.SelectionChanged   += DgwRechnungen_SelectionChanged;
        btnStornieren.Click              += BtnStornieren_Click;
    }

    // ── Load ─────────────────────────────────────────────────────────────────

    private async void FrmRechnungList_Load(object? sender, EventArgs e)
    {
        if (IsDesignMode() || _invoiceService is null) return;

        WindowState = FormWindowState.Maximized;

        _isLoading = true;
        dtpVon.Value = new DateTime(DateTime.Today.Year, 1, 1);
        dtpBis.Value = DateTime.Today;
        FillStatusCombo();
        _isLoading = false;

        panelDetail.Visible = true;
        await LoadRechnungenAsync();
    }

    private void FillStatusCombo()
    {
        cmbStatus.Items.Clear();
        cmbStatus.Items.Add(new StatusItem(null,                  "Alle"));
        cmbStatus.Items.Add(new StatusItem(InvoiceStatus.Gebucht,   "Gebucht"));
        cmbStatus.Items.Add(new StatusItem(InvoiceStatus.Storniert, "Storniert"));
        cmbStatus.DisplayMember   = "Text";
        cmbStatus.SelectedIndex   = 0;
    }

    private record StatusItem(InvoiceStatus? Value, string Text);

    // ── Laden ─────────────────────────────────────────────────────────────────

    private async Task LoadRechnungenAsync()
    {
        if (_invoiceService is null) return;

        InvoiceStatus? status = null;
        if (cmbStatus.SelectedItem is StatusItem si) status = si.Value;

        IReadOnlyList<RechnungListDto> liste;

        await _lock.WaitAsync();
        try
        {
            Cursor = Cursors.WaitCursor;
            liste  = await _invoiceService.GetRechnungListeAsync(
                dtpVon.Value.Date,
                dtpBis.Value.Date,
                string.IsNullOrWhiteSpace(txtKundeFilter.Text) ? null : txtKundeFilter.Text.Trim(),
                status);
        }
        catch (Exception ex) { ShowError($"Ladefehler:\n{ex.Message}"); return; }
        finally { Cursor = Cursors.Default; _lock.Release(); }

        dgwRechnungen.DataSource = null;
        dgwRechnungen.DataSource = liste.ToList();
        StyleGridRechnungen();

        dgwZeilen.DataSource = null;
        ClearDetail();
    }

    private void StyleGridRechnungen()
    {
        if (dgwRechnungen.Columns.Count == 0) return;
        ConfigureGrid(dgwRechnungen);
        foreach (DataGridViewColumn col in dgwRechnungen.Columns) col.Visible = false;

        ShowRCol("Rechnungsnummer",   "Rechnungs-Nr.",    130);
        ShowRCol("Rechnungsdatum",    "Datum",             90, format: "dd.MM.yyyy");
        ShowRCol("Von",               "Zeitr. Von",        90, format: "dd.MM.yyyy");
        ShowRCol("Bis",               "Zeitr. Bis",        90, format: "dd.MM.yyyy");
        ShowRCol("Kundenname",        "Kunde",               0, fill: true);
        ShowRCol("AnzahlPositionen",  "Pos.",               50, right: true);
        ShowRCol("Gesamtnetto",       "Netto €",           100, format: "N2", right: true);
        ShowRCol("Gesamtbrutto",      "Brutto €",          100, format: "N2", right: true);
        ShowRCol("IstSammelrechnung", "Sammel",             55);
    }

    private void ShowRCol(string name, string header, int width,
        bool fill = false, string? format = null, bool right = false)
    {
        if (!dgwRechnungen.Columns.Contains(name)) return;
        var col        = dgwRechnungen.Columns[name];
        col.Visible    = true;
        col.HeaderText = header;
        if (fill) col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        else { col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; col.Width = width; }
        if (format is not null) col.DefaultCellStyle.Format = format;
        if (right) col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
    }

    // ── Selection ─────────────────────────────────────────────────────────────

    private async void DgwRechnungen_SelectionChanged(object? s, EventArgs e)
    {
        if (IsDesignMode()) return;

        if (dgwRechnungen.CurrentRow?.DataBoundItem is not RechnungListDto dto)
        {
            ClearDetail();
            return;
        }

        UpdateDetailPanel(dto);
        await LoadZeilenAsync(dto.Id);
    }

    private void UpdateDetailPanel(RechnungListDto dto)
    {
        lblRechnungsnrWert.Text  = dto.Rechnungsnummer;
        lblNettoWert.Text        = $"{dto.Gesamtnetto:N2} €";
        lblBruttoWert.Text       = $"{dto.Gesamtbrutto:N2} €";
        lblStatusWert.Text       = dto.Status.ToString();
        lblStatusWert.ForeColor  = dto.Status == InvoiceStatus.Storniert
            ? Color.Firebrick : Color.DarkGreen;

        bool canStorno           = dto.Status == InvoiceStatus.Gebucht;
        btnStornieren.Enabled    = canStorno;
        btnStornieren.BackColor  = canStorno
            ? Color.FromArgb(180, 30, 30)
            : Color.FromArgb(160, 160, 160);
    }

    private void ClearDetail()
    {
        lblRechnungsnrWert.Text = "–";
        lblNettoWert.Text       = "–";
        lblBruttoWert.Text      = "–";
        lblStatusWert.Text      = "–";
        lblStatusWert.ForeColor = SystemColors.ControlText;
        btnStornieren.Enabled   = false;
        btnStornieren.BackColor = Color.FromArgb(160, 160, 160);
    }

    private async Task LoadZeilenAsync(int rechnungId)
    {
        if (_invoiceService is null) return;

        IReadOnlyList<RechnungZeileDetailDto> zeilen;

        await _lock.WaitAsync();
        try
        {
            Cursor = Cursors.WaitCursor;
            zeilen = await _invoiceService.GetZeilenByRechnungIdAsync(rechnungId);
        }
        catch { zeilen = []; }
        finally { Cursor = Cursors.Default; _lock.Release(); }

        dgwZeilen.DataSource = null;
        dgwZeilen.DataSource = zeilen.ToList();
        StyleGridZeilen();
    }

    private void StyleGridZeilen()
    {
        if (dgwZeilen.Columns.Count == 0) return;
        ConfigureGrid(dgwZeilen);
        foreach (DataGridViewColumn col in dgwZeilen.Columns) col.Visible = false;

        ShowZCol("Artikelnummer",      "Artikelnr.",         90);
        ShowZCol("Bezeichnung",        "Bezeichnung",          0, fill: true);
        ShowZCol("Lieferscheinnummer", "Lieferschein",       120);
        ShowZCol("Lieferdatum",        "Lieferdatum",         90, format: "dd.MM.yyyy");
        ShowZCol("Menge",              "Menge",               70, format: "N3", right: true);
        ShowZCol("Preis",              "Preis €",             80, format: "N2", right: true);
        ShowZCol("MwstProzent",        "MwSt %",              60, format: "N2", right: true);
        ShowZCol("LineAmount",         "Gesamt €",            90, format: "N2", right: true);
    }

    private void ShowZCol(string name, string header, int width,
        bool fill = false, string? format = null, bool right = false)
    {
        if (!dgwZeilen.Columns.Contains(name)) return;
        var col        = dgwZeilen.Columns[name];
        col.Visible    = true;
        col.HeaderText = header;
        if (fill) col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        else { col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; col.Width = width; }
        if (format is not null) col.DefaultCellStyle.Format = format;
        if (right) col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
    }

    // ── Stornieren ────────────────────────────────────────────────────────────

    private async void BtnStornieren_Click(object? s, EventArgs e)
    {
        if (dgwRechnungen.CurrentRow?.DataBoundItem is not RechnungListDto dto) return;
        if (dto.Status != InvoiceStatus.Gebucht) return;

        if (!Confirm(
            $"Rechnung '{dto.Rechnungsnummer}' stornieren?\n\n" +
            $"Kunde: {dto.Kundenname}\n" +
            "Die Lieferscheine werden wieder auf 'Aktiv' zurückgesetzt."))
            return;

        await _lock.WaitAsync();
        try
        {
            Cursor = Cursors.WaitCursor;
            await _invoiceService.StornierenAsync(dto.Id);
        }
        catch (Exception ex) { ShowError($"Stornierung fehlgeschlagen:\n{ex.Message}"); return; }
        finally { Cursor = Cursors.Default; _lock.Release(); }

        ShowSuccess($"Rechnung '{dto.Rechnungsnummer}' wurde storniert.");
        await LoadRechnungenAsync();
    }

    // ── Keyboard ─────────────────────────────────────────────────────────────

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.F5) { _ = LoadRechnungenAsync(); return true; }
        return base.ProcessCmdKey(ref msg, keyData);
    }
}