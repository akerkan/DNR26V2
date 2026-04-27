using System.ComponentModel;
using DNR26V2.Domain.DTOs;
using DNR26V2.Domain.Entities.Invoices;
using DNR26V2.Domain.Enums;
using DNR26V2.Forms.Base;
using DNR26V2.Services.Invoices;
using DNR26V2.Services.Reports;

namespace DNR26V2.Forms.Invoices;

public partial class FrmRechnungList : BaseListForm
{
    private readonly IInvoiceService _invoiceService;
    private readonly IReportRenderService _reportService;
    private readonly SemaphoreSlim   _lock = new(1, 1);
    private readonly HashSet<int>    _selectedInvoiceIds = [];
    private bool                     _isLoading;

    public FrmRechnungList()
    {
        _invoiceService = null!;
        _reportService = null!;
        InitializeComponent();
        WireUpEvents();
    }

    public FrmRechnungList(IInvoiceService invoiceService, IReportRenderService reportService)
    {
        _invoiceService = invoiceService;
        _reportService = reportService;
        InitializeComponent();
        WireUpEvents();
    }

    private static bool IsDesignMode() =>
        System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime;

    // ── Events ───────────────────────────────────────────────────────────────

    private void WireUpEvents()
    {
        Load += FrmRechnungList_Load;

        btnSuchen.Click += async (_, _) => await LoadRechnungenAsync();
        txtKundeFilter.KeyDown += async (_, e) => { if (e.KeyCode == Keys.Enter) await LoadRechnungenAsync(); };
        cmbStatus.SelectedIndexChanged += async (_, _) => { if (!_isLoading) await LoadRechnungenAsync(); };
        dgwRechnungen.SelectionChanged += DgwRechnungen_SelectionChanged;
        dgwRechnungen.CellContentClick += DgwRechnungen_CellContentClick;
        btnGutschrift.Click += BtnGutschrift_Click;
        btnDrucken.Click += async (_, _) => await BtnDrucken_ClickAsync();
        btnAlleAuswaehlen.Click += BtnAlleAuswaehlen_Click;
        btnBulkDruck.Click += async (_, _) => await BtnBulkDruck_ClickAsync();
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

        btnStornieren.Visible  = false;   // deprecated — use Gutschrift instead
        panelDetail.Visible    = true;
        EnsureSelectionColumn();
        UpdateSelectionUi();
        await LoadRechnungenAsync();
    }

    private void FillStatusCombo()
    {
        cmbStatus.Items.Clear();
        cmbStatus.Items.Add(new StatusItem(null, "Alle"));
        cmbStatus.Items.Add(new StatusItem(InvoiceStatus.Gebucht, "Gebucht"));
        cmbStatus.Items.Add(new StatusItem(InvoiceStatus.Gutgeschrieben, "Gutgeschrieben"));
        cmbStatus.DisplayMember = "Text";
        cmbStatus.SelectedIndex = 0;
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
            liste = await _invoiceService.GetRechnungListeAsync(
                dtpVon.Value.Date,
                dtpBis.Value.Date,
                string.IsNullOrWhiteSpace(txtKundeFilter.Text) ? null : txtKundeFilter.Text.Trim(),
                status);
        }
        catch (Exception ex) { ShowError($"Ladefehler:\n{ex.Message}"); return; }
        finally { Cursor = Cursors.Default; _lock.Release(); }

        dgwRechnungen.DataSource = null;
        dgwRechnungen.DataSource = liste.ToList();
        EnsureSelectionColumn();
        ReapplySelection();
        StyleGridRechnungen();

        // If only a single invoice row is returned, select it and load detail immediately
        if (dgwRechnungen.Rows.Count == 1)
        {
            dgwRechnungen.ClearSelection();
            var row = dgwRechnungen.Rows[0];
            row.Selected = true;

            int firstVisibleCol = -1;
            for (int i = 0; i < dgwRechnungen.Columns.Count; i++)
            {
                if (dgwRechnungen.Columns[i].Visible)
                {
                    firstVisibleCol = i;
                    break;
                }
            }
            if (firstVisibleCol >= 0)
            {
                try { dgwRechnungen.CurrentCell = row.Cells[firstVisibleCol]; }
                catch { /* ignore */ }
            }

            if (dgwRechnungen.CurrentRow?.DataBoundItem is RechnungListDto dto)
            {
                UpdateDetailPanel(dto);
                await LoadZeilenAsync(dto.Id);
            }
        }
        else
        {
            dgwZeilen.DataSource = null;
            ClearDetail();
        }
    }

    private void StyleGridRechnungen()
    {
        if (dgwRechnungen.Columns.Count == 0) return;
        ConfigureGrid(dgwRechnungen);

        foreach (DataGridViewColumn col in dgwRechnungen.Columns) col.Visible = false;
        if (dgwRechnungen.Columns.Contains("colSelected"))
        {
            var selCol = dgwRechnungen.Columns["colSelected"];
            selCol.Visible = true;
            selCol.DisplayIndex = 0;
            selCol.HeaderText = "✓";
            selCol.Width = 36;
            selCol.ReadOnly = false;
        }

        ShowRCol("Rechnungsnummer",   "Rechnungs-Nr.",    130);
        ShowRCol("Rechnungsdatum",    "Datum",             90, format: "dd.MM.yyyy");
        ShowRCol("Von",               "Zeitr. Von",        90, format: "dd.MM.yyyy");
        ShowRCol("Bis",               "Zeitr. Bis",        90, format: "dd.MM.yyyy");
        ShowRCol("Kundenname",        "Kunde",               0, fill: true);
        ShowRCol("AnzahlPositionen",  "Pos.",               50, right: true);
        ShowRCol("Gesamtnetto",       "Netto €",           100, format: "N2", right: true);
        ShowRCol("Gesamtbrutto",      "Brutto €",          100, format: "N2", right: true);
        ShowRCol("IstSammelrechnung", "Sammel",             55);

        dgwRechnungen.ReadOnly = false;
        foreach (DataGridViewColumn col in dgwRechnungen.Columns)
            if (col.Name != "colSelected") col.ReadOnly = true;
    }

    private void EnsureSelectionColumn()
    {
        if (dgwRechnungen.Columns.Contains("colSelected")) return;

        var col = new DataGridViewCheckBoxColumn
        {
            Name = "colSelected",
            HeaderText = "✓",
            Width = 36,
            ReadOnly = false,
            Frozen = true
        };
        dgwRechnungen.Columns.Insert(0, col);
    }

    private void ReapplySelection()
    {
        foreach (DataGridViewRow row in dgwRechnungen.Rows)
        {
            if (row.DataBoundItem is not RechnungListDto dto) continue;
            row.Cells["colSelected"].Value = _selectedInvoiceIds.Contains(dto.Id);
        }
        UpdateSelectionUi();
    }

    private void ShowRCol(string name, string header, int width,
        bool fill = false, string? format = null, bool right = false)
    {
        if (!dgwRechnungen.Columns.Contains(name)) return;
        var col = dgwRechnungen.Columns[name];
        col.Visible = true;
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
        lblRechnungsnrWert.Text = dto.Rechnungsnummer;
        lblNettoWert.Text = $"{dto.Gesamtnetto:N2} \u20ac";
        lblBruttoWert.Text = $"{dto.Gesamtbrutto:N2} \u20ac";
        lblStatusWert.Text = dto.BelegArt == InvoiceDocumentType.Gutschrift
            ? $"Gutschrift ({dto.Status})"
            : dto.Status.ToString();
        lblStatusWert.ForeColor = dto.Status switch
        {
            InvoiceStatus.Storniert => Color.Firebrick,
            InvoiceStatus.Gutgeschrieben => Color.DarkOrange,
            InvoiceStatus.Gebucht when dto.BelegArt == InvoiceDocumentType.Gutschrift => Color.SteelBlue,
            _ => Color.DarkGreen,
        };

        bool canAction = dto.Status == InvoiceStatus.Gebucht
                             && dto.BelegArt == InvoiceDocumentType.Rechnung;

        btnGutschrift.Enabled   = canAction;
        btnGutschrift.BackColor = canAction
            ? Color.FromArgb(30, 100, 160)
            : Color.FromArgb(160, 160, 160);
        btnDrucken.Enabled = dto.Id > 0;
    }

    private void ClearDetail()
    {
        lblRechnungsnrWert.Text = "\u2013";
        lblNettoWert.Text = "\u2013";
        lblBruttoWert.Text = "\u2013";
        lblStatusWert.Text = "\u2013";
        lblStatusWert.ForeColor = SystemColors.ControlText;
        btnGutschrift.Enabled   = false;
        btnGutschrift.BackColor = Color.FromArgb(160, 160, 160);
        btnDrucken.Enabled = false;
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

        ShowZCol("Artikelnummer", "Artikelnr.", 90);
        ShowZCol("Bezeichnung", "Bezeichnung", 0, fill: true);
        ShowZCol("Lieferscheinnummer", "Lieferschein", 120);
        ShowZCol("Lieferdatum", "Lieferdatum", 90, format: "dd.MM.yyyy");
        ShowZCol("Menge", "Menge", 70, format: "N3", right: true);
        ShowZCol("Preis", "Preis €", 80, format: "N2", right: true);
        ShowZCol("MwstProzent", "MwSt %", 60, format: "N2", right: true);
        ShowZCol("LineAmount", "Gesamt €", 90, format: "N2", right: true);
    }

    private void ShowZCol(string name, string header, int width,
        bool fill = false, string? format = null, bool right = false)
    {
        if (!dgwZeilen.Columns.Contains(name)) return;
        var col = dgwZeilen.Columns[name];
        col.Visible = true;
        col.HeaderText = header;
        if (fill) col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        else { col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; col.Width = width; }
        if (format is not null) col.DefaultCellStyle.Format = format;
        if (right) col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
    }

    // ── Gutschrift ────────────────────────────────────────────────────────────

    private async void BtnGutschrift_Click(object? s, EventArgs e)
    {
        if (dgwRechnungen.CurrentRow?.DataBoundItem is not RechnungListDto dto) return;
        if (dto.Status != InvoiceStatus.Gebucht || dto.BelegArt != InvoiceDocumentType.Rechnung) return;

        if (!Confirm(
            $"Gutschrift f\u00fcr Rechnung '{dto.Rechnungsnummer}' erstellen?\n\n" +
            $"Kunde: {dto.Kundenname}\n\n" +
            "Die fakturierten Mengen werden zur\u00fcckgerollt. " +
            "Die Originalrechnung bleibt im System (Status: Gutgeschrieben)."))
            return;

        InvoiceHeader gutschrift;
        await _lock.WaitAsync();
        try
        {
            Cursor = Cursors.WaitCursor;
            gutschrift = await _invoiceService.CreateGutschriftAsync(dto.Id);
        }
        catch (Exception ex) { ShowError($"Gutschrift fehlgeschlagen:\n{ex.Message}"); return; }
        finally { Cursor = Cursors.Default; _lock.Release(); }

        ShowSuccess($"Gutschrift '{gutschrift.Rechnungsnummer}' wurde erstellt.");
        await LoadRechnungenAsync();
    }

    // ── Keyboard ─────────────────────────────────────────────────────────────

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.F5) { _ = LoadRechnungenAsync(); return true; }
        return base.ProcessCmdKey(ref msg, keyData);
    }

    private async Task BtnDrucken_ClickAsync()
    {
        if (dgwRechnungen.CurrentRow?.DataBoundItem is not RechnungListDto dto) return;

        try
        {
            Cursor = Cursors.WaitCursor;
            await _reportService.PreviewInvoiceAsync(dto.Id);
        }
        catch (Exception ex) { ShowError($"Vorschaufehler:\n{ex.Message}"); }
        finally { Cursor = Cursors.Default; }
    }

    private void DgwRechnungen_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
        if (dgwRechnungen.Columns[e.ColumnIndex].Name != "colSelected") return;
        if (dgwRechnungen.Rows[e.RowIndex].DataBoundItem is not RechnungListDto dto) return;

        var current = rowBool(dgwRechnungen.Rows[e.RowIndex].Cells["colSelected"].Value);
        var next = !current;
        dgwRechnungen.Rows[e.RowIndex].Cells["colSelected"].Value = next;

        if (next) _selectedInvoiceIds.Add(dto.Id);
        else _selectedInvoiceIds.Remove(dto.Id);

        UpdateSelectionUi();

        static bool rowBool(object? value) => value is bool b && b;
    }

    private void BtnAlleAuswaehlen_Click(object? sender, EventArgs e)
    {
        // Wenn mindestens eine sichtbare Zeile nicht ausgewählt ist → alles auswählen,
        // andernfalls alle Auswahl entfernen (Toggle-Verhalten).
        bool anyUnselected = false;
        foreach (DataGridViewRow row in dgwRechnungen.Rows)
        {
            if (row.DataBoundItem is not RechnungListDto) continue;
            if (!IsRowChecked(row)) { anyUnselected = true; break; }
        }

        bool select = anyUnselected;

        foreach (DataGridViewRow row in dgwRechnungen.Rows)
        {
            if (row.DataBoundItem is not RechnungListDto dto) continue;

            row.Cells["colSelected"].Value = select;

            if (select) _selectedInvoiceIds.Add(dto.Id);
            else _selectedInvoiceIds.Remove(dto.Id);
        }

        UpdateSelectionUi();

        static bool IsRowChecked(DataGridViewRow r)
            => r.Cells["colSelected"].Value is bool b && b;
    }

    private async Task BtnBulkDruck_ClickAsync()
    {
        var ids = _selectedInvoiceIds.ToList();
        if (ids.Count == 0) return;

        try
        {
            Cursor = Cursors.WaitCursor;
            await _reportService.PreviewInvoicesAsync(ids);
        }
        catch (Exception ex) { ShowError($"Mehrfachvorschau fehlgeschlagen:\n{ex.Message}"); }
        finally { Cursor = Cursors.Default; }
    }

    private void UpdateSelectionUi()
    {
        lblSelection.Text = $"{_selectedInvoiceIds.Count} Rechnung(en) ausgewählt";
        btnBulkDruck.Enabled = _selectedInvoiceIds.Count > 0;
    }
}