using System.ComponentModel;
using System.Threading;
using DNR26V2.Domain.Entities.MasterData;
using DNR26V2.Domain.Enums;
using DNR26V2.Domain.Exceptions;
using DNR26V2.Forms.Base;
using DNR26V2.Services.MasterData;

namespace DNR26V2.Forms.MasterData;

public partial class FrmProductAttributeList : BaseListForm
{
    private readonly IProductAttributeService _attributeService;

    private ProductAttribute? _currentAttribute;
    private BindingList<ProductAttributeValue> _werte = new();
    private readonly List<int> _deletedWertIds = new();
    private bool _isDirty;

    private AttributeEntityType? _pendingNewEntityType;

    private readonly SemaphoreSlim _serviceLock = new(1, 1);
    private bool _suppressSelectionChanged;
    private readonly System.Windows.Forms.Timer _searchTimer = new() { Interval = 350 };

    // Parameterloser Konstruktor — wird vom WinForms-Designer benötigt
    public FrmProductAttributeList()
    {
        _attributeService = null!;
        InitializeComponent();
        WireUpEvents();
    }

    public FrmProductAttributeList(IProductAttributeService attributeService)
    {
        _attributeService = attributeService;
        InitializeComponent();
        WireUpEvents();
    }

    private static bool IsDesignMode() =>
        LicenseManager.UsageMode == LicenseUsageMode.Designtime;

    // ── Events verdrahten ─────────────────────────────────────────────────────

    private void WireUpEvents()
    {
        Load += FrmProductAttributeList_Load;
        FormClosing += FrmProductAttributeList_FormClosing;

        txtSuche.TextChanged += TxtSuche_TextChanged;
        txtSuche.KeyDown += TxtSuche_KeyDown;
        chkNurAktiv.CheckedChanged += Filter_Changed;

        dgwAttribute.SelectionChanged += DgwAttribute_SelectionChanged;
        dgwAttribute.CellFormatting += DgwAttribute_CellFormatting;

        txtBezeichnung.TextChanged += (_, _) => _isDirty = true;
        cmbFeldtyp.SelectedIndexChanged += CmbFeldtyp_Changed;
        nudMaxLaenge.ValueChanged += (_, _) => _isDirty = true;
        chkAktiv.CheckedChanged += (_, _) => _isDirty = true;
        chkIstVorlage.CheckedChanged += (_, _) => _isDirty = true;   // ← EKLENDİ
        dgwWerte.CellValueChanged += (_, _) => _isDirty = true;
        cmbEntityType.SelectedIndexChanged += (_, _) => _isDirty = true;

        btnNeu.Click += BtnNeu_Click;
        btnSpeichern.Click += BtnSpeichern_Click;
        btnDeaktivieren.Click += BtnDeaktivieren_Click;
        btnWertHinzufuegen.Click += BtnWertHinzufuegen_Click;
        btnWertLoeschen.Click += BtnWertLoeschen_Click;
        btnLoeschen.Click += BtnLoeschen_Click;    // ← Handler binden

        _searchTimer.Tick += async (_, _) =>
        {
            _searchTimer.Stop();
            await LoadListAsync();
        };
    }

    // ── Laden ─────────────────────────────────────────────────────────────────

    // Vor Show() aufrufen — löst kein sofortiges NewAttribute aus
    public void SetPendingNewAttribute(AttributeEntityType entityType)
        => _pendingNewEntityType = entityType;

    private async void FrmProductAttributeList_Load(object? sender, EventArgs e)
    {
        if (IsDesignMode() || _attributeService is null) return;

        WindowState = FormWindowState.Maximized;
        await LoadListAsync(); // ← LoadDetailAsync läuft hier komplett durch

        if (_pendingNewEntityType.HasValue)
        {
            // Nach vollständigem Load: Neu-Modus + Typ vorwählen
            var type = _pendingNewEntityType.Value;
            _pendingNewEntityType = null;
            NewAttribute();
            cmbEntityType.SelectedIndex = type switch
            {
                AttributeEntityType.CustomerProduct => 1,
                AttributeEntityType.Tour => 2,
                AttributeEntityType.KundenGruppe => 3,
                AttributeEntityType.Shared => 4,
                _ => 0
            };
            txtBezeichnung.Focus();
        }
        else if (dgwAttribute.Rows.Count == 0)
        {
            NewAttribute();
        }
    }

    private async Task LoadListAsync()
    {
        IReadOnlyList<ProductAttribute>? all = null;
        int idToLoad = 0;

        await _serviceLock.WaitAsync();
        try
        {
            Cursor = Cursors.WaitCursor;
            _suppressSelectionChanged = true;
            all = await _attributeService.GetAllAttributesAsync(chkNurAktiv.Checked);
        }
        catch (Exception ex) { ShowError($"Ladefehler:\n{ex.Message}"); return; }
        finally
        {
            _suppressSelectionChanged = false;
            Cursor = Cursors.Default;
            _serviceLock.Release();
        }

        var suche = txtSuche.Text.Trim();
        var filtered = string.IsNullOrEmpty(suche)
            ? all.ToList()
            : all.Where(a => a.Bezeichnung.Contains(suche, StringComparison.OrdinalIgnoreCase))
                 .ToList();

        var prevId = SelectedId();
        dgwAttribute.DataSource = null;
        dgwAttribute.DataSource = filtered;
        StyleGrid();

        if (prevId > 0)
            SelectById(prevId);
        else if (dgwAttribute.Rows.Count > 0)
        {
            dgwAttribute.Rows[0].Selected = true;
            var cell = GetFirstVisibleCell(dgwAttribute.Rows[0]);
            if (cell is not null) dgwAttribute.CurrentCell = cell;
        }

        idToLoad = SelectedId();
        if (idToLoad > 0)
            await LoadDetailAsync(idToLoad);
    }

    private void StyleGrid()
    {
        if (dgwAttribute.Columns.Count == 0) return;
        foreach (DataGridViewColumn col in dgwAttribute.Columns) col.Visible = false;
        ShowCol("Bezeichnung", "Attribut", 0, fill: true);
        ShowCol("Feldtyp", "Typ", 85);
        ShowCol("Aktiv", "Aktiv", 50);
    }

    private void ShowCol(string name, string header, int width, bool fill = false)
    {
        if (!dgwAttribute.Columns.Contains(name)) return;
        var col = dgwAttribute.Columns[name];
        col.Visible = true;
        col.HeaderText = header;
        if (fill) col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        else { col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; col.Width = width; }
    }

    // Feldtyp-Enum als lesbaren Text anzeigen
    private void DgwAttribute_CellFormatting(object? s, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0) return;
        if (dgwAttribute.Columns[e.ColumnIndex].Name != "Feldtyp") return;
        if (e.Value is AttributeFieldType ft)
            e.Value = ft == AttributeFieldType.FreeText ? "Freier Text" : "Lookup";
    }

    private async Task LoadDetailAsync(int id)
    {
        ProductAttribute? attr = null;
        IReadOnlyList<ProductAttributeValue>? werte = null;

        await _serviceLock.WaitAsync();
        try
        {
            attr = await _attributeService.GetAttributeByIdAsync(id);
            if (attr is null) return;
            werte = await _attributeService.GetValuesByAttributeAsync(id);
        }
        catch (Exception ex) { ShowError($"Ladefehler:\n{ex.Message}"); return; }
        finally { _serviceLock.Release(); }

        _currentAttribute = attr;
        BindToForm(attr);

        _werte = new BindingList<ProductAttributeValue>(werte.ToList());
        _deletedWertIds.Clear();
        dgwWerte.DataSource = _werte;
        StyleWerteGrid();                                                   // ← EKLENDİ

        SetDetailVisible(true);
        _isDirty = false;
    }

    private void StyleWerteGrid()
    {
        if (dgwWerte.Columns.Count == 0) return;
        foreach (DataGridViewColumn col in dgwWerte.Columns) col.Visible = false;
        ShowWerteCol("Bezeichnung", "Bezeichnung", 0, fill: true);
        ShowWerteCol("Sortierung", "Sort.", 60);
        ShowWerteCol("Aktiv", "Aktiv", 50);
    }

    private void ShowWerteCol(string name, string header, int width, bool fill = false)
    {
        if (!dgwWerte.Columns.Contains(name)) return;
        var col = dgwWerte.Columns[name];
        col.Visible = true;
        col.HeaderText = header;
        if (fill) col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        else { col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; col.Width = width; }
    }

    // Ersetze BindToForm():
    private void BindToForm(ProductAttribute a)
    {
        txtBezeichnung.Text = a.Bezeichnung;
        cmbFeldtyp.SelectedIndex = (int)a.Feldtyp;
        nudMaxLaenge.Value = a.MaxLaenge ?? 0;
        nudMaxLaenge.Enabled = a.Feldtyp == AttributeFieldType.FreeText;

        cmbEntityType.SelectedIndex = a.EntityType switch
        {
            AttributeEntityType.Product => 0,
            AttributeEntityType.CustomerProduct => 1,
            AttributeEntityType.Tour => 2,
            AttributeEntityType.KundenGruppe => 3,
            AttributeEntityType.Shared => 4,
            _ => 0
        };

        chkAktiv.Checked = a.Aktiv;
        chkIstVorlage.Checked = a.IstVorlage;                              // ← EKLENDİ
        btnDeaktivieren.Text = a.Aktiv ? "Deaktivieren" : "Aktivieren";
        _isDirty = false;
    }

    // Ersetze BindFromForm():
    private void BindFromForm(ProductAttribute a)
    {
        a.Bezeichnung = txtBezeichnung.Text.Trim();
        a.Feldtyp = (AttributeFieldType)cmbFeldtyp.SelectedIndex;
        a.EntityType = cmbEntityType.SelectedIndex switch
        {
            1 => AttributeEntityType.CustomerProduct,
            2 => AttributeEntityType.Tour,
            3 => AttributeEntityType.KundenGruppe,
            4 => AttributeEntityType.Shared,
            _ => AttributeEntityType.Product
        };
        a.MaxLaenge = a.Feldtyp == AttributeFieldType.FreeText && nudMaxLaenge.Value > 0
                      ? (int)nudMaxLaenge.Value
                      : null;
        a.Aktiv = chkAktiv.Checked;
        a.IstVorlage = chkIstVorlage.Checked;                             // ← EKLENDİ
    }

    // Neue public Methode — wird nach Show() aufgerufen:
    public void StartNewAttribute(AttributeEntityType? preselectedType = null)
    {
        NewAttribute();
        if (preselectedType.HasValue)
        {
            cmbEntityType.SelectedIndex = preselectedType.Value switch
            {
                AttributeEntityType.CustomerProduct => 1,
                AttributeEntityType.Tour => 2,
                AttributeEntityType.KundenGruppe => 3,
                AttributeEntityType.Shared => 4,
                _ => 0
            };
        }
        txtBezeichnung.Focus();
    }

    // ── Speichern ─────────────────────────────────────────────────────────────

    private async Task SaveAsync()
    {
        if (_currentAttribute is null) return;
        dgwWerte.EndEdit();
        BindFromForm(_currentAttribute);
        try
        {
            // 1. Attribut speichern
            await _serviceLock.WaitAsync();
            try { await _attributeService.SaveAttributeAsync(_currentAttribute); }
            finally { _serviceLock.Release(); }

            // 2. Gelöschte Werte entfernen
            foreach (var deletedId in _deletedWertIds.ToList())
            {
                await _serviceLock.WaitAsync();
                try { await _attributeService.DeleteValueAsync(deletedId); }
                catch (ValidationException ex) { ShowError(ex.Message); }
                finally { _serviceLock.Release(); }
            }
            _deletedWertIds.Clear();

            // 3. Alle Werte in der Liste speichern (neu + geändert)
            foreach (var v in _werte)
            {
                v.AttributId = _currentAttribute.Id;
                await _serviceLock.WaitAsync();
                try { await _attributeService.SaveValueAsync(v); }
                finally { _serviceLock.Release(); }
            }

            _isDirty = false;
            await LoadListAsync();
            SelectById(_currentAttribute.Id);
            ShowSuccess("Attribut gespeichert.");
        }
        catch (ValidationException ex) { ShowError(ex.Message); }
        catch (Exception ex) { ShowError($"Fehler:\n{ex.Message}"); }
    }

    // ── Neu ───────────────────────────────────────────────────────────────────

    private void NewAttribute()
    {
        if (_isDirty && !Confirm("Ungespeicherte Änderungen verwerfen?")) return;
        _currentAttribute = new ProductAttribute();
        _werte = new BindingList<ProductAttributeValue>();
        _deletedWertIds.Clear();
        dgwWerte.DataSource = _werte;
        StyleWerteGrid();                                                   // ← EKLENDİ
        BindToForm(_currentAttribute);
        SetDetailVisible(true);
        txtBezeichnung.Focus();
    }

    // ── Deaktivieren / Aktivieren ─────────────────────────────────────────────

    private async Task ToggleActiveAsync()
    {
        if (_currentAttribute is null || _currentAttribute.Id == 0) return;
        bool newState = !_currentAttribute.Aktiv;
        if (!Confirm($"Attribut '{_currentAttribute.Bezeichnung}' {(newState ? "aktivieren" : "deaktivieren")}?")) return;
        try
        {
            await _serviceLock.WaitAsync();
            try { await _attributeService.SetAttributeActiveAsync(_currentAttribute.Id, newState); }
            finally { _serviceLock.Release(); }

            _currentAttribute.Aktiv = newState;
            chkAktiv.Checked = newState;
            btnDeaktivieren.Text = newState ? "Deaktivieren" : "Aktivieren";
            await LoadListAsync();
        }
        catch (Exception ex) { ShowError($"Fehler:\n{ex.Message}"); }
    }

    // ── Werte: Hinzufügen / Löschen ───────────────────────────────────────────

    private void BtnWertHinzufuegen_Click(object? s, EventArgs e)
    {
        var text = txtNeuerWert.Text.Trim();
        if (string.IsNullOrEmpty(text)) { ShowError("Bitte einen Wert eingeben."); return; }

        if (_currentAttribute?.Feldtyp == AttributeFieldType.FreeText
            && _currentAttribute.MaxLaenge > 0
            && text.Length > _currentAttribute.MaxLaenge)
        {
            ShowError($"Wert darf max. {_currentAttribute.MaxLaenge} Zeichen lang sein.");
            return;
        }

        _werte.Add(new ProductAttributeValue
        {
            Bezeichnung = text,
            Sortierung = (int)nudWertSortierung.Value,
            Aktiv = true,
            //IstVorlage  = false,  // ← BURADA varsayılan değer
            AttributId = _currentAttribute?.Id ?? 0
        });

        txtNeuerWert.Clear();
        nudWertSortierung.Value = 0;
        txtNeuerWert.Focus();
        _isDirty = true;
    }

    private void BtnWertLoeschen_Click(object? s, EventArgs e)
    {
        if (dgwWerte.CurrentRow?.DataBoundItem is not ProductAttributeValue val) return;
        if (!Confirm($"Wert '{val.Bezeichnung}' löschen?")) return;

        if (val.Id > 0) _deletedWertIds.Add(val.Id);
        _werte.Remove(val);
        _isDirty = true;
    }

    // ── Hilfsmethoden ─────────────────────────────────────────────────────────

    private void SetDetailVisible(bool v) => panelDetail.Visible = v;

    private int SelectedId() =>
        dgwAttribute.CurrentRow?.DataBoundItem is ProductAttribute a ? a.Id : 0;

    private void SelectById(int id)
    {
        foreach (DataGridViewRow row in dgwAttribute.Rows)
        {
            if (row.DataBoundItem is not ProductAttribute a || a.Id != id) continue;
            row.Selected = true;
            var cell = GetFirstVisibleCell(row);
            if (cell is not null) dgwAttribute.CurrentCell = cell;
            break;
        }
    }

    private static DataGridViewCell? GetFirstVisibleCell(DataGridViewRow row)
    {
        foreach (DataGridViewCell cell in row.Cells)
            if (cell.OwningColumn?.Visible == true) return cell;
        return null;
    }

    // ── Events ────────────────────────────────────────────────────────────────

    private async void DgwAttribute_SelectionChanged(object? s, EventArgs e)
    {
        if (IsDesignMode() || _suppressSelectionChanged) return;
        var id = SelectedId();
        if (id > 0) await LoadDetailAsync(id);
    }

    private void CmbFeldtyp_Changed(object? s, EventArgs e)
    {
        nudMaxLaenge.Enabled = cmbFeldtyp.SelectedIndex == (int)AttributeFieldType.FreeText;
        _isDirty = true;
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

    private void BtnNeu_Click(object? s, EventArgs e) => NewAttribute();
    private async void BtnSpeichern_Click(object? s, EventArgs e) => await SaveAsync();
    private async void BtnDeaktivieren_Click(object? s, EventArgs e) => await ToggleActiveAsync();
    private async void BtnLoeschen_Click(object? s, EventArgs e)
    {
        if (_currentAttribute is null || _currentAttribute.Id == 0) return;
        if (!Confirm($"Attribut '{_currentAttribute.Bezeichnung}' endgültig löschen?\n" +
                     "Alle zugehörigen Werte werden ebenfalls gelöscht.")) return;
        try
        {
            await _serviceLock.WaitAsync();
            try { await _attributeService.DeleteAttributeAsync(_currentAttribute.Id); }
            finally { _serviceLock.Release(); }

            _currentAttribute = null;
            _isDirty = false;
            await LoadListAsync();

            if (dgwAttribute.Rows.Count == 0)
                NewAttribute();

            ShowSuccess("Attribut gelöscht.");
        }
        catch (ValidationException ex) { ShowError(ex.Message); }
        catch (Exception ex) { ShowError($"Fehler:\n{ex.Message}"); }
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.F2) { NewAttribute(); return true; }
        if (keyData == (Keys.Control | Keys.S)) { _ = SaveAsync(); return true; }
        return base.ProcessCmdKey(ref msg, keyData);
    }

    private void FrmProductAttributeList_FormClosing(object? s, FormClosingEventArgs e)
    {
        if (_isDirty && !Confirm("Ungespeicherte Änderungen verwerfen und schließen?"))
            e.Cancel = true;
    }

}