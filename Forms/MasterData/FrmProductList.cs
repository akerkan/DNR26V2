using System.ComponentModel;
using System.Threading;
using DNR26V2.Domain.DTOs;
using DNR26V2.Domain.Entities.MasterData;
using DNR26V2.Domain.Exceptions;
using DNR26V2.Forms.Base;
using DNR26V2.Helpers;
using DNR26V2.Services.MasterData;

namespace DNR26V2.Forms.MasterData;

public partial class FrmProductList : BaseListForm
{
    private readonly IProductService _productService;

    private Product? _currentProduct;
    private bool     _isDirty;

    private readonly SemaphoreSlim _serviceLock = new(1, 1);
    private bool _suppressSelectionChanged;

    private readonly System.Windows.Forms.Timer _searchTimer = new() { Interval = 350 };

    // Parameterloser Konstruktor — wird vom WinForms-Designer benötigt
    public FrmProductList()
    {
        _productService = null!;
        InitializeComponent();
        WireUpEvents();
    }

    public FrmProductList(IProductService productService)
    {
        _productService = productService;
        InitializeComponent();
        WireUpEvents();
    }

    private static bool IsDesignMode() =>
        LicenseManager.UsageMode == LicenseUsageMode.Designtime;

    // ── Events verdrahten ─────────────────────────────────────────────────────

    private void WireUpEvents()
    {
        Load         += FrmProductList_Load;
        FormClosing  += FrmProductList_FormClosing;

        txtSuche.TextChanged       += TxtSuche_TextChanged;
        txtSuche.KeyDown           += TxtSuche_KeyDown;
        chkNurAktiv.CheckedChanged += Filter_Changed;

        dgwArtikel.SelectionChanged += DgwArtikel_SelectionChanged;
        dgwArtikel.CellFormatting   += DgwArtikel_CellFormatting;

        btnNeu.Click          += BtnNeu_Click;
        btnSpeichern.Click    += BtnSpeichern_Click;
        btnDeaktivieren.Click += BtnDeaktivieren_Click;
        btnPrintfarbe.Click   += BtnPrintfarbe_Click;

        _searchTimer.Tick += async (_, _) =>
        {
            _searchTimer.Stop();
            await LoadListAsync();
        };

        foreach (var tb in new Control[] {
            txtArtikelnummer, txtBezeichnung, txtBezeichnung2,
            txtEinheit, txtBarcode, txtNotizen,
            txtFeld1, txtFeld2, txtFeld3, txtFeld4 })
            ((TextBox)tb).TextChanged += Detail_Changed;

        nudVKPreis.ValueChanged     += (_, _) => _isDirty = true;
        nudEKPreis.ValueChanged     += (_, _) => _isDirty = true;
        nudMwstProzent.ValueChanged += (_, _) => _isDirty = true;
        chkAktiv.CheckedChanged     += (_, _) => _isDirty = true;
        
        EnableColumnChooser(dgwArtikel);
    }

    // ── Laden ─────────────────────────────────────────────────────────────────

    private async void FrmProductList_Load(object? sender, EventArgs e)
    {
        if (IsDesignMode() || _productService is null) return;

        WindowState = FormWindowState.Maximized;
        await LoadListAsync();

        if (dgwArtikel.Rows.Count == 0)
            NewProduct();
    }

    private async Task LoadListAsync()
    {
        int idToLoad = 0;

        await _serviceLock.WaitAsync();
        try
        {
            Cursor = Cursors.WaitCursor;
            _suppressSelectionChanged = true;

            var filter = new ProductListFilter
            {
                Suche    = txtSuche.Text.NullIfEmpty(),
                NurAktiv = chkNurAktiv.Checked ? true : null
            };

            var prevId = SelectedId();
            var liste  = await _productService.GetListAsync(filter);

            dgwArtikel.DataSource = null;
            dgwArtikel.DataSource = liste.ToList();
            StyleGrid();

            if (prevId > 0)
                SelectById(prevId);
            else if (dgwArtikel.Rows.Count > 0)
            {
                var firstRow = dgwArtikel.Rows[0];
                firstRow.Selected = true;
                var cell = GetFirstVisibleCell(firstRow);
                if (cell is not null) dgwArtikel.CurrentCell = cell;
            }

            idToLoad = SelectedId();
        }
        catch (Exception ex) { ShowError($"Ladefehler:\n{ex.Message}"); }
        finally
        {
            _suppressSelectionChanged = false;
            Cursor = Cursors.Default;
            _serviceLock.Release();
        }

        if (idToLoad > 0)
            await LoadDetailAsync(idToLoad);
    }

    private void StyleGrid()
    {
        if (dgwArtikel.Columns.Count == 0) return;
        foreach (DataGridViewColumn col in dgwArtikel.Columns) col.Visible = false;
        ShowCol("Artikelnummer", "Art.-Nr.", 100);
        ShowCol("Bezeichnung", "Bezeichnung", 0, fill: true);
        ShowCol("Feld1", "Feld 1", 120);
        ShowCol("Feld2", "Feld 2", 120);
        ShowCol("Feld3", "Feld 3", 120);
        ShowCol("Feld4", "Feld 4", 120);
        ShowCol("Printfarbe", "Printfarbe", 90);
        ShowCol("Aktiv", "Aktiv", 50);

        ApplyColumnChooserSettings(dgwArtikel);  // ← NEU
    }

    private void ShowCol(string name, string header, int width,
        bool fill = false, string? format = null, bool right = false)
    {
        if (!dgwArtikel.Columns.Contains(name)) return;
        var col = dgwArtikel.Columns[name];
        col.Visible    = true;
        col.HeaderText = header;
        if (fill) col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        else { col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; col.Width = width; }
        if (format is not null) col.DefaultCellStyle.Format = format;
        if (right) col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
    }

    // ── Printfarbe: Grid-Zelle einfärben ──────────────────────────────────────

    private void DgwArtikel_CellFormatting(object? s, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0) return;
        if (dgwArtikel.Columns[e.ColumnIndex].Name != "Printfarbe") return;

        var colorStr = e.Value as string;
        if (string.IsNullOrWhiteSpace(colorStr)) return;

        try
        {
            var color = ColorTranslator.FromHtml(colorStr);
            e.CellStyle.BackColor = color;
            // Kontrastfarbe für Text wählen
            e.CellStyle.ForeColor = (color.R * 0.299 + color.G * 0.587 + color.B * 0.114) > 128
                ? Color.Black : Color.White;
        }
        catch { /* ungültige Farbangabe → keine Einfärbung */ }
    }

    private async Task LoadDetailAsync(int id)
    {
        await _serviceLock.WaitAsync();
        try
        {
            _currentProduct = await _productService.GetByIdAsync(id);
            if (_currentProduct is null) return;
            BindToForm(_currentProduct);
            SetDetailVisible(true);
            _isDirty = false;
        }
        catch (Exception ex) { ShowError($"Ladefehler:\n{ex.Message}"); }
        finally { _serviceLock.Release(); }
    }

    private void BindToForm(Product p)
    {
        txtArtikelnummer.Text     = p.Artikelnummer;
        txtBezeichnung.Text       = p.Bezeichnung;
        txtBezeichnung2.Text      = p.Bezeichnung2 ?? "";
        txtEinheit.Text           = p.Einheit;
        nudVKPreis.Value          = p.VKPreis;
        nudEKPreis.Value          = p.EKPreis;
        nudMwstProzent.Value      = p.MwstProzent;
        txtFeld1.Text             = p.Feld1 ?? "";
        txtFeld2.Text             = p.Feld2 ?? "";
        txtFeld3.Text             = p.Feld3 ?? "";
        txtFeld4.Text             = p.Feld4 ?? "";
        ApplyPrintfarbe(p.Printfarbe);
        txtBarcode.Text           = p.Barcode ?? "";
        txtNotizen.Text           = p.Notizen ?? "";
        chkAktiv.Checked          = p.Aktiv;
        txtArtikelnummer.ReadOnly = p.Id > 0;
        btnDeaktivieren.Text      = p.Aktiv ? "Deaktivieren" : "Aktivieren";
        _isDirty = false;
    }

    private void BindFromForm(Product p)
    {
        p.Artikelnummer = txtArtikelnummer.Text.Trim();
        p.Bezeichnung   = txtBezeichnung.Text.Trim();
        p.Bezeichnung2  = txtBezeichnung2.Text.NullIfEmpty();
        p.Einheit       = string.IsNullOrWhiteSpace(txtEinheit.Text) ? "STK" : txtEinheit.Text.Trim();
        p.VKPreis       = nudVKPreis.Value;
        p.EKPreis       = nudEKPreis.Value;
        p.MwstProzent   = nudMwstProzent.Value;
        p.Feld1         = txtFeld1.Text.NullIfEmpty();
        p.Feld2         = txtFeld2.Text.NullIfEmpty();
        p.Feld3         = txtFeld3.Text.NullIfEmpty();
        p.Feld4         = txtFeld4.Text.NullIfEmpty();
        p.Printfarbe    = pnlPrintfarbe.BackColor == SystemColors.Control
                          ? null
                          : ColorTranslator.ToHtml(pnlPrintfarbe.BackColor);
        p.Barcode       = txtBarcode.Text.NullIfEmpty();
        p.Notizen       = txtNotizen.Text.NullIfEmpty();
        p.Aktiv         = chkAktiv.Checked;
    }

    // ── Printfarbe-Hilfe ──────────────────────────────────────────────────────

    private void ApplyPrintfarbe(string? colorStr)
    {
        if (string.IsNullOrWhiteSpace(colorStr))
        {
            pnlPrintfarbe.BackColor = SystemColors.Control;
            return;
        }
        try   { pnlPrintfarbe.BackColor = ColorTranslator.FromHtml(colorStr); }
        catch { pnlPrintfarbe.BackColor = SystemColors.Control; }
    }

    private void BtnPrintfarbe_Click(object? s, EventArgs e)
    {
        using var dlg = new ColorDialog
        {
            Color            = pnlPrintfarbe.BackColor == SystemColors.Control
                               ? Color.White
                               : pnlPrintfarbe.BackColor,
            FullOpen         = true,
            AllowFullOpen    = true
        };
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            pnlPrintfarbe.BackColor = dlg.Color;
            _isDirty = true;
        }
    }

    private async Task SaveAsync()
    {
        if (_currentProduct is null) return;
        BindFromForm(_currentProduct);
        try
        {
            await _serviceLock.WaitAsync();
            try { await _productService.SaveAsync(_currentProduct); _isDirty = false; }
            finally { _serviceLock.Release(); }

            await LoadListAsync();
            SelectById(_currentProduct.Id);
            ShowSuccess("Artikel gespeichert.");
        }
        catch (ValidationException ex) { ShowError(ex.Message); }
        catch (Exception ex) { ShowError($"Fehler:\n{ex.Message}"); }
    }

    private void NewProduct()
    {
        if (_isDirty && !Confirm("Ungespeicherte Änderungen verwerfen?")) return;
        _currentProduct = new Product();
        BindToForm(_currentProduct);
        txtArtikelnummer.ReadOnly = false;
        SetDetailVisible(true);
        txtArtikelnummer.Focus();
    }

    private async Task ToggleActiveAsync()
    {
        if (_currentProduct is null || _currentProduct.Id == 0) return;
        bool newState = !_currentProduct.Aktiv;
        if (!Confirm($"Artikel '{_currentProduct.Bezeichnung}' {(newState ? "aktivieren" : "deaktivieren")}?")) return;
        try
        {
            await _serviceLock.WaitAsync();
            try { await _productService.SetActiveAsync(_currentProduct.Id, newState); _currentProduct.Aktiv = newState; }
            finally { _serviceLock.Release(); }

            chkAktiv.Checked     = newState;
            btnDeaktivieren.Text = newState ? "Deaktivieren" : "Aktivieren";
            await LoadListAsync();
        }
        catch (Exception ex) { ShowError($"Fehler:\n{ex.Message}"); }
    }

    // ── Hilfsmethoden ─────────────────────────────────────────────────────────

    private void SetDetailVisible(bool v) => panelDetail.Visible = v;

    private int SelectedId() =>
        dgwArtikel.CurrentRow?.DataBoundItem is ProductListDto d ? d.Id : 0;

    private static DataGridViewCell? GetFirstVisibleCell(DataGridViewRow row)
    {
        foreach (DataGridViewCell cell in row.Cells)
            if (cell.OwningColumn?.Visible == true) return cell;
        return null;
    }

    private void SelectById(int id)
    {
        foreach (DataGridViewRow row in dgwArtikel.Rows)
        {
            if (row.DataBoundItem is not ProductListDto d || d.Id != id) continue;
            row.Selected = true;
            var cell = GetFirstVisibleCell(row);
            if (cell is not null) dgwArtikel.CurrentCell = cell;
            break;
        }
    }

    // ── Events ────────────────────────────────────────────────────────────────

    private async void DgwArtikel_SelectionChanged(object? s, EventArgs e)
    {
        if (IsDesignMode() || _suppressSelectionChanged) return;
        var id = SelectedId();
        if (id > 0) await LoadDetailAsync(id);
    }

    private void TxtSuche_TextChanged(object? s, EventArgs e)
    {
        _searchTimer.Stop();
        _searchTimer.Start();
    }

    private async void TxtSuche_KeyDown(object? s, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;
        _searchTimer.Stop();
        await LoadListAsync();
        e.Handled = true;
    }

    private async void Filter_Changed(object? s, EventArgs e)
    {
        if (IsDesignMode()) return;
        await LoadListAsync();
    }

    private void BtnNeu_Click(object? s, EventArgs e)               => NewProduct();
    private async void BtnSpeichern_Click(object? s, EventArgs e)   => await SaveAsync();
    private async void BtnDeaktivieren_Click(object? s, EventArgs e)=> await ToggleActiveAsync();
    private void Detail_Changed(object? s, EventArgs e)              => _isDirty = true;

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.F2)               { NewProduct();    return true; }
        if (keyData == (Keys.Control | Keys.S)) { _ = SaveAsync(); return true; }
        return base.ProcessCmdKey(ref msg, keyData);
    }

    private void FrmProductList_FormClosing(object? s, FormClosingEventArgs e)
    {
        if (_isDirty && !Confirm("Ungespeicherte Änderungen verwerfen und schließen?"))
            e.Cancel = true;
    }
}