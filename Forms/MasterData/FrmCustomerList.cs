using DNR26V2.Domain.DTOs;
using DNR26V2.Domain.Entities.MasterData;
using DNR26V2.Domain.Exceptions;
using DNR26V2.Forms.Base;
using DNR26V2.Helpers;
using DNR26V2.Services.MasterData;

namespace DNR26V2.Forms.MasterData;

public partial class FrmCustomerList : BaseListForm
{
    private readonly ICustomerService       _customerService;
    private readonly ICustomerFilterService _filterService;
    private readonly IRouteService          _routeService;

    private Customer? _currentCustomer;
    private bool      _isNew   = false;
    private bool      _isDirty = false;

    // Debounce-Timer für Suche
    private readonly System.Windows.Forms.Timer _searchTimer = new() { Interval = 350 };

    public FrmCustomerList(
        ICustomerService       customerService,
        ICustomerFilterService filterService,
        IRouteService          routeService)
    {
        _customerService = customerService;
        _filterService   = filterService;
        _routeService    = routeService;

        InitializeComponent();

        _searchTimer.Tick += async (_, _) =>
        {
            _searchTimer.Stop();
            await LoadListAsync();
        };
    }

    // ── Laden ─────────────────────────────────────────────────────────────────

    private async void FrmCustomerList_Load(object sender, EventArgs e)
    {
        await LoadComboBoxesAsync();
        await LoadListAsync();
        SetDetailPanelVisible(false);
    }

    private async Task LoadComboBoxesAsync()
    {
        try
        {
            var routes  = await _routeService.GetAllActiveAsync();
            var filters = await _filterService.GetAllAsync();

            // Filter-ComboBox (oben)
            cmbFilterOben.Items.Clear();
            cmbFilterOben.Items.Add(new ComboItem(0, "(Alle Filter)"));
            foreach (var f in filters)
                cmbFilterOben.Items.Add(new ComboItem(f.Id, f.Kundenfilter));
            cmbFilterOben.SelectedIndex = 0;

            // Route-ComboBox (oben)
            cmbRouteOben.Items.Clear();
            cmbRouteOben.Items.Add(new ComboItem(0, "(Alle Routen)"));
            foreach (var r in routes)
                cmbRouteOben.Items.Add(new ComboItem(r.Id, $"{r.Routencode} – {r.Bezeichnung}"));
            cmbRouteOben.SelectedIndex = 0;

            // Detail-ComboBox für Route
            cmbRoute.Items.Clear();
            cmbRoute.Items.Add(new ComboItem(0, "(keine)"));
            foreach (var r in routes)
                cmbRoute.Items.Add(new ComboItem(r.Id, $"{r.Routencode} – {r.Bezeichnung}"));

            // Detail-ComboBox für Kundenfilter
            cmbKundenfilter.Items.Clear();
            cmbKundenfilter.Items.Add(new ComboItem(0, "(kein)"));
            foreach (var f in filters)
                cmbKundenfilter.Items.Add(new ComboItem(f.Id, f.Kundenfilter));
        }
        catch (Exception ex)
        {
            ShowError($"Fehler beim Laden der Auswahllisten:\n\n{ex.Message}");
        }
    }

    private async Task LoadListAsync()
    {
        try
        {
            Cursor = Cursors.WaitCursor;

            var filter = BuildFilter();
            var liste  = await _customerService.GetListAsync(filter);

            var prevId = GetSelectedCustomerId();
            dgwKunden.DataSource = null;
            dgwKunden.DataSource = liste.ToList();
            ConfigureListGrid();

            // Selektion wiederherstellen
            if (prevId > 0)
                SelectRowById(prevId);
        }
        catch (Exception ex)
        {
            ShowError($"Fehler beim Laden der Kundenliste:\n\n{ex.Message}");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private CustomerListFilter BuildFilter()
    {
        var routeId  = (cmbRouteOben.SelectedItem  as ComboItem)?.Id;
        var filterId = (cmbFilterOben.SelectedItem as ComboItem)?.Id;

        return new CustomerListFilter
        {
            Suche    = string.IsNullOrWhiteSpace(txtSuche.Text) ? null : txtSuche.Text.Trim(),
            NurAktiv = chkNurAktiv.Checked ? true : null,
            RouteId  = routeId  > 0 ? routeId  : null,
            FilterId = filterId > 0 ? filterId : null
        };
    }

    private void ConfigureListGrid()
    {
        ConfigureGrid(dgwKunden);  // BaseListForm-Methode

        var hidden = new[] { "Id", "Wochenendtour" };
        foreach (var col in hidden)
            if (dgwKunden.Columns.Contains(col))
                dgwKunden.Columns[col].Visible = false;

        if (dgwKunden.Columns.Contains("Kundennummer"))
        {
            dgwKunden.Columns["Kundennummer"].HeaderText = "Kunden-Nr.";
            dgwKunden.Columns["Kundennummer"].Width      = 100;
        }
        if (dgwKunden.Columns.Contains("Kundenname"))
        {
            dgwKunden.Columns["Kundenname"].HeaderText = "Kundenname";
            dgwKunden.Columns["Kundenname"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
        if (dgwKunden.Columns.Contains("Routencode"))
        {
            dgwKunden.Columns["Routencode"].HeaderText = "Route";
            dgwKunden.Columns["Routencode"].Width      = 80;
        }
        if (dgwKunden.Columns.Contains("Kundenfilter"))
        {
            dgwKunden.Columns["Kundenfilter"].HeaderText = "Gruppe";
            dgwKunden.Columns["Kundenfilter"].Width      = 90;
        }
        if (dgwKunden.Columns.Contains("Limit"))
        {
            dgwKunden.Columns["Limit"].HeaderText = "Limit";
            dgwKunden.Columns["Limit"].Width      = 80;
            dgwKunden.Columns["Limit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgwKunden.Columns["Limit"].DefaultCellStyle.Format    = "N2";
        }
        if (dgwKunden.Columns.Contains("Aktiv"))
        {
            dgwKunden.Columns["Aktiv"].HeaderText = "Aktiv";
            dgwKunden.Columns["Aktiv"].Width      = 50;
        }
    }

    // ── Detail laden ──────────────────────────────────────────────────────────

    private async Task LoadDetailAsync(int id)
    {
        try
        {
            _currentCustomer = await _customerService.GetByIdAsync(id);
            if (_currentCustomer is null) return;

            BindToForm(_currentCustomer);
            SetDetailPanelVisible(true);
            _isNew   = false;
            _isDirty = false;
        }
        catch (Exception ex)
        {
            ShowError($"Fehler beim Laden des Kunden:\n\n{ex.Message}");
        }
    }

    private void BindToForm(Customer c)
    {
        // Tab: Stammdaten
        txtKundennummer .Text    = c.Kundennummer;
        txtKundenname   .Text    = c.Kundenname;
        txtName2        .Text    = c.Name2    ?? string.Empty;
        txtInhaber      .Text    = c.Inhaber  ?? string.Empty;
        txtTelefon      .Text    = c.Telefonnummer ?? string.Empty;
        txtHandy        .Text    = c.Handynummer   ?? string.Empty;
        txtEmail        .Text    = c.EMail         ?? string.Empty;
        txtNotizen      .Text    = c.Notizen   ?? string.Empty;

        // Tab: Adresse
        txtAdresse  .Text = c.Adresse  ?? string.Empty;
        txtAdresse2 .Text = c.Adresse2 ?? string.Empty;
        txtPLZ      .Text = c.PLZ      ?? string.Empty;
        txtOrt      .Text = c.Ort      ?? string.Empty;
        txtLand     .Text = c.Land;

        // Tab: Alt. Lieferadresse
        chkAbweichend.Checked = c.AbweichendeLieferadresse;
        txtALName2   .Text    = c.ALName2   ?? string.Empty;
        txtALInhaber .Text    = c.ALInhaber ?? string.Empty;
        txtALAdresse .Text    = c.ALAdresse ?? string.Empty;
        txtALAdresse2.Text    = c.ALAdresse2 ?? string.Empty;
        txtALPLZ     .Text    = c.ALPLZ     ?? string.Empty;
        txtALOrt     .Text    = c.ALOrt     ?? string.Empty;
        txtALLand    .Text    = c.ALLand    ?? string.Empty;
        ToggleAltAdresseFields(c.AbweichendeLieferadresse);

        // Tab: Einstellungen
        SelectComboById(cmbRoute,         c.RouteId);
        SelectComboById(cmbKundenfilter,  c.KundenfilterId);
        nudRoutenfolge   .Value   = c.Routenfolge;
        nudLimit         .Value   = c.Limit;
        chkWochenendtour .Checked = c.Wochenendtour;
        chkPreisAusblenden.Checked = c.PreisAusblenden;
        chkAktiv         .Checked = c.Aktiv;

        // Kundennummer im Edit-Modus readonly
        txtKundennummer.ReadOnly = c.Id > 0;
    }

    private void BindFromForm(Customer c)
    {
        c.Kundennummer = txtKundennummer.Text.Trim();
        c.Kundenname   = txtKundenname.Text.Trim();
        c.Name2        = txtName2.Text.NullIfEmpty();
        c.Inhaber      = txtInhaber.Text.NullIfEmpty();
        c.Telefonnummer = txtTelefon.Text.NullIfEmpty();
        c.Handynummer  = txtHandy.Text.NullIfEmpty();
        c.EMail        = txtEmail.Text.NullIfEmpty();
        c.Notizen      = txtNotizen.Text.NullIfEmpty();

        c.Adresse  = txtAdresse.Text.NullIfEmpty();
        c.Adresse2 = txtAdresse2.Text.NullIfEmpty();
        c.PLZ      = txtPLZ.Text.NullIfEmpty();
        c.Ort      = txtOrt.Text.NullIfEmpty();
        c.Land     = string.IsNullOrWhiteSpace(txtLand.Text) ? "Deutschland" : txtLand.Text.Trim();

        c.AbweichendeLieferadresse = chkAbweichend.Checked;
        c.ALName2    = txtALName2.Text.NullIfEmpty();
        c.ALInhaber  = txtALInhaber.Text.NullIfEmpty();
        c.ALAdresse  = txtALAdresse.Text.NullIfEmpty();
        c.ALAdresse2 = txtALAdresse2.Text.NullIfEmpty();
        c.ALPLZ      = txtALPLZ.Text.NullIfEmpty();
        c.ALOrt      = txtALOrt.Text.NullIfEmpty();
        c.ALLand     = txtALLand.Text.NullIfEmpty();

        c.RouteId        = (cmbRoute.SelectedItem        as ComboItem)?.Id is > 0 and int rid ? rid : null;
        c.KundenfilterId = (cmbKundenfilter.SelectedItem as ComboItem)?.Id is > 0 and int fid ? fid : null;
        c.Routenfolge    = (int)nudRoutenfolge.Value;
        c.Limit          = nudLimit.Value;
        c.Wochenendtour  = chkWochenendtour.Checked;
        c.PreisAusblenden = chkPreisAusblenden.Checked;
        c.Aktiv          = chkAktiv.Checked;
    }

    // ── Speichern ─────────────────────────────────────────────────────────────

    private async Task SaveAsync()
    {
        if (_currentCustomer is null) return;

        BindFromForm(_currentCustomer);

        try
        {
            await _customerService.SaveAsync(_currentCustomer);
            _isDirty = false;
            _isNew   = false;
            await LoadListAsync();
            SelectRowById(_currentCustomer.Id);
            ShowSuccess("Kunde erfolgreich gespeichert.");
        }
        catch (ValidationException ex)
        {
            ShowError(ex.Message);
        }
        catch (Exception ex)
        {
            ShowError($"Fehler beim Speichern:\n\n{ex.Message}");
        }
    }

    // ── Neu ───────────────────────────────────────────────────────────────────

    private void NewCustomer()
    {
        if (_isDirty && !Confirm("Es gibt ungespeicherte Änderungen. Trotzdem verwerfen?"))
            return;

        _currentCustomer             = new Customer();
        _isNew                       = true;
        _isDirty                     = false;
        txtKundennummer.ReadOnly     = false;

        BindToForm(_currentCustomer);
        SetDetailPanelVisible(true);
        txtKundennummer.Focus();
    }

    // ── Deaktivieren ──────────────────────────────────────────────────────────

    private async Task ToggleActiveAsync()
    {
        if (_currentCustomer is null || _currentCustomer.Id == 0) return;

        bool newState   = !_currentCustomer.Aktiv;
        string aktion   = newState ? "aktivieren" : "deaktivieren";

        if (!Confirm($"Kunden '{_currentCustomer.Kundenname}' wirklich {aktion}?")) return;

        try
        {
            await _customerService.SetActiveAsync(_currentCustomer.Id, newState);
            _currentCustomer.Aktiv = newState;
            chkAktiv.Checked       = newState;
            btnDeaktivieren.Text   = newState ? "Deaktivieren" : "Aktivieren";
            await LoadListAsync();
        }
        catch (Exception ex)
        {
            ShowError($"Fehler:\n\n{ex.Message}");
        }
    }

    // ── Hilfsmethoden ─────────────────────────────────────────────────────────

    private void SetDetailPanelVisible(bool visible)
    {
        splitContainer.Panel2.Visible = visible;
        if (!visible) _currentCustomer = null;
    }

    private void ToggleAltAdresseFields(bool enabled)
    {
        txtALName2.Enabled    = enabled;
        txtALInhaber.Enabled  = enabled;
        txtALAdresse.Enabled  = enabled;
        txtALAdresse2.Enabled = enabled;
        txtALPLZ.Enabled      = enabled;
        txtALOrt.Enabled      = enabled;
        txtALLand.Enabled     = enabled;
    }

    private int GetSelectedCustomerId()
    {
        if (dgwKunden.CurrentRow?.DataBoundItem is CustomerListDto dto)
            return dto.Id;
        return 0;
    }

    private void SelectRowById(int id)
    {
        foreach (DataGridViewRow row in dgwKunden.Rows)
        {
            if (row.DataBoundItem is CustomerListDto dto && dto.Id == id)
            {
                row.Selected           = true;
                dgwKunden.CurrentCell  = row.Cells[0];
                dgwKunden.FirstDisplayedScrollingRowIndex = row.Index;
                break;
            }
        }
    }

    private static void SelectComboById(ComboBox combo, int? id)
    {
        if (id is null or 0) { combo.SelectedIndex = 0; return; }
        foreach (var item in combo.Items)
        {
            if (item is ComboItem ci && ci.Id == id)
            {
                combo.SelectedItem = ci;
                return;
            }
        }
        combo.SelectedIndex = 0;
    }

    // ── Events ────────────────────────────────────────────────────────────────

    private async void DgwKunden_SelectionChanged(object sender, EventArgs e)
    {
        int id = GetSelectedCustomerId();
        if (id > 0)
            await LoadDetailAsync(id);
    }

    private void TxtSuche_TextChanged(object sender, EventArgs e)
    {
        _searchTimer.Stop();
        _searchTimer.Start();
    }

    private async void TxtSuche_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            _searchTimer.Stop();
            await LoadListAsync();
            e.Handled = true;
        }
    }

    private async void Filter_Changed(object sender, EventArgs e)
        => await LoadListAsync();

    private async void BtnNeu_Click(object sender, EventArgs e)
        => NewCustomer();

    private async void BtnSpeichern_Click(object sender, EventArgs e)
        => await SaveAsync();

    private async void BtnDeaktivieren_Click(object sender, EventArgs e)
        => await ToggleActiveAsync();

    private void Detail_Changed(object sender, EventArgs e)
        => _isDirty = true;

    private void ChkAbweichend_CheckedChanged(object sender, EventArgs e)
    {
        ToggleAltAdresseFields(chkAbweichend.Checked);
        _isDirty = true;
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.F2)          { NewCustomer();  return true; }
        if (keyData == (Keys.Control | Keys.S)) { _ = SaveAsync(); return true; }
        return base.ProcessCmdKey(ref msg, keyData);
    }

    private void FrmCustomerList_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (_isDirty && !Confirm("Es gibt ungespeicherte Änderungen. Trotzdem schließen?"))
            e.Cancel = true;
    }
}

/// <summary>Hilfsobjekt für ComboBox-Einträge mit Id.</summary>
internal sealed record ComboItem(int Id, string Text)
{
    public override string ToString() => Text;
}