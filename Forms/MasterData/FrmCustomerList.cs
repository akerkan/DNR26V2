using System.ComponentModel;
using System.Threading;
using DNR26V2.Domain.DTOs;
using DNR26V2.Domain.Entities.MasterData;
using DNR26V2.Domain.Exceptions;
using DNR26V2.Forms.Base;
using DNR26V2.Helpers;
using DNR26V2.Services.MasterData;

namespace DNR26V2.Forms.MasterData;

public partial class FrmCustomerList : BaseListForm
{
    // ── Kolon Kataloğu — TÜM DTO alanlarının Almanca başlıkları ─────────────
    private static readonly Dictionary<string, string> _columnHeaders = new()
    {
        ["Kundennummer"]    = "Kunden-Nr.",
        ["Kundenname"]      = "Name",
        ["Name2"]           = "Name 2",
        ["Inhaber"]         = "Inhaber",
        ["Telefonnummer"]   = "Telefon",
        ["Handynummer"]     = "Handy",
        ["EMail"]           = "E-Mail",
        ["PLZ"]             = "PLZ",
        ["Ort"]             = "Ort",
        ["Adresse"]         = "Adresse",
        ["Tur"]             = "Tour",
        ["AusnahmeTur"]     = "Ausnahme-Tour",
        ["Kundenfilter"]    = "Gruppe",
        ["Routenfolge"]     = "Reihenfolge",
        ["Limit"]           = "Limit",
        ["LiefertMo"]       = "Mo",
        ["LiefertDi"]       = "Di",
        ["LiefertMi"]       = "Mi",
        ["LiefertDo"]       = "Do",
        ["LiefertFr"]       = "Fr",
        ["LiefertSa"]       = "Sa",
        ["LiefertSo"]       = "So",
        ["PreisAusblenden"] = "Preis ausblendn.",
        ["Offen"]           = "Offen",
        ["Aktiv"]           = "Aktiv",
    };

    private readonly ICustomerService _customerService;
    private readonly ICustomerFilterService _filterService;
    private readonly IRouteService _routeService;

    private Customer? _currentCustomer;
    private bool _isDirty;

    // Verhindert parallele DbContext-Zugriffe
    private readonly SemaphoreSlim _serviceLock = new(1, 1);

    // Unterdrückt Event-Kaskaden während Ladevorgängen
    private bool _suppressSelectionChanged;
    private bool _suppressFilterChanged;

    private readonly System.Windows.Forms.Timer _searchTimer = new() { Interval = 350 };

    // Parameterloser Konstruktor — wird vom WinForms-Designer benötigt
    public FrmCustomerList()
    {
        _customerService = null!;
        _filterService = null!;
        _routeService = null!;
        InitializeComponent();
        WireUpEvents();
    }

    public FrmCustomerList(
        ICustomerService customerService,
        ICustomerFilterService filterService,
        IRouteService routeService)
    {
        _customerService = customerService;
        _filterService = filterService;
        _routeService = routeService;
        InitializeComponent();
        WireUpEvents();
    }

    private static bool IsDesignMode() =>
        LicenseManager.UsageMode == LicenseUsageMode.Designtime;

    // ── Events verdrahten ─────────────────────────────────────────────────────

    private void WireUpEvents()
    {
        Load         += FrmCustomerList_Load;
        FormClosing  += FrmCustomerList_FormClosing;

        txtSuche.TextChanged            += TxtSuche_TextChanged;
        txtSuche.KeyDown                += TxtSuche_KeyDown;
        cmbRouteOben.SelectedIndexChanged += Filter_Changed;
        cmbFilterOben.SelectedIndexChanged += Filter_Changed;
        chkNurAktiv.CheckedChanged      += Filter_Changed;

        dgwKunden.SelectionChanged += DgwKunden_SelectionChanged;

        btnNeu.Click          += BtnNeu_Click;
        btnSpeichern.Click    += BtnSpeichern_Click;
        btnDeaktivieren.Click += BtnDeaktivieren_Click;

        chkAbweichend.CheckedChanged += ChkAbweichend_CheckedChanged;

        // AusnahmeTur-Combo: Validierung beim Verlassen
        cmbAusnahmeTur.SelectedIndexChanged += CmbAusnahmeTur_Changed;

        _searchTimer.Tick += async (_, _) =>
        {
            _searchTimer.Stop();
            await LoadListAsync();
        };

        foreach (var tb in new[] {
            txtKundennummer, txtKundenname, txtName2, txtInhaber,
            txtTelefon, txtHandy, txtEmail, txtNotizen,
            txtAdresse, txtAdresse2, txtPLZ, txtOrt, txtLand,
            txtALName2, txtALInhaber, txtALAdresse, txtALAdresse2,
            txtALPLZ, txtALOrt, txtALLand })
            tb.TextChanged += Detail_Changed;

        // Liefertage
        foreach (var chk in new[] { chkMo, chkDi, chkMi, chkDo, chkFr, chkSa, chkSo })
            chk.CheckedChanged += Detail_Changed;

        EnableColumnChooser(dgwKunden);
    }

    // ── Laden ─────────────────────────────────────────────────────────────────

    private async void FrmCustomerList_Load(object? sender, EventArgs e)
    {
        if (IsDesignMode() || _customerService is null) return;

        WindowState = FormWindowState.Maximized;
        // SetDetailVisible(false);  ← Bu satırı kaldır / yorum yap

        await LoadComboBoxesAsync();
        await LoadListAsync();

        // Liste boşsa otomatik "Neu" moduna geç
        if (dgwKunden.Rows.Count == 0)
            NewCustomer();
    }

    private async Task LoadComboBoxesAsync()
    {
        await _serviceLock.WaitAsync();
        try
        {
            _suppressFilterChanged = true;

            var routes  = await _routeService.GetAllActiveAsync();
            var filters = await _filterService.GetAllAsync();

            // ── Filter-Leiste oben ────────────────────────────────────────────
            cmbRouteOben.Items.Clear();
            cmbRouteOben.Items.Add(new ComboItem(0, "(Alle Touren)"));
            foreach (var r in routes)
                cmbRouteOben.Items.Add(new ComboItem(r.Id, $"{r.Routencode} – {r.Bezeichnung}"));
            cmbRouteOben.SelectedIndex = 0;

            cmbFilterOben.Items.Clear();
            cmbFilterOben.Items.Add(new ComboItem(0, "(Alle Gruppen)"));
            foreach (var f in filters)
                cmbFilterOben.Items.Add(new ComboItem(f.Id, f.Kundenfilter));
            cmbFilterOben.SelectedIndex = 0;

            // ── Detail: Standard-Tour ─────────────────────────────────────────
            cmbTur.Items.Clear();
            cmbTur.Items.Add(new ComboItem(0, "(keine)"));
            foreach (var r in routes)
                cmbTur.Items.Add(new ComboItem(r.Id, $"{r.Routencode} – {r.Bezeichnung}"));

            // ── Detail: Ausnahme-Tour ─────────────────────────────────────────
            cmbAusnahmeTur.Items.Clear();
            cmbAusnahmeTur.Items.Add(new ComboItem(0, "(keine)"));
            foreach (var r in routes)
                cmbAusnahmeTur.Items.Add(new ComboItem(r.Id, $"{r.Routencode} – {r.Bezeichnung}"));

            // ── Detail: Kundenfilter ──────────────────────────────────────────
            cmbKundenfilter.Items.Clear();
            cmbKundenfilter.Items.Add(new ComboItem(0, "(keine)"));
            foreach (var f in filters)
                cmbKundenfilter.Items.Add(new ComboItem(f.Id, f.Kundenfilter));
        }
        catch (Exception ex) { ShowError($"Fehler beim Laden der Listen:\n{ex.Message}"); }
        finally
        {
            _suppressFilterChanged = false;
            _serviceLock.Release();
        }
    }

    private async Task LoadListAsync()
    {
        int idToLoad = 0;

        await _serviceLock.WaitAsync();
        try
        {
            Cursor = Cursors.WaitCursor;
            _suppressSelectionChanged = true;

            var filter = new CustomerListFilter
            {
                Suche = txtSuche.Text.NullIfEmpty(),
                NurAktiv = chkNurAktiv.Checked ? true : null,
                RouteId = (cmbRouteOben.SelectedItem as ComboItem)?.Id is > 0 and int r ? r : null,
                FilterId = (cmbFilterOben.SelectedItem as ComboItem)?.Id is > 0 and int f ? f : null
            };

            var prevId = SelectedId();
            var liste = await _customerService.GetListAsync(filter);

            dgwKunden.DataSource = null;
            dgwKunden.DataSource = liste.ToList();
            StyleGrid();

            if (prevId > 0)
                SelectById(prevId);
            else if (dgwKunden.Rows.Count > 0)
            {
                var firstRow = dgwKunden.Rows[0];
                firstRow.Selected = true;
                var cell = GetFirstVisibleCell(firstRow);
                if (cell is not null) dgwKunden.CurrentCell = cell;
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

        // Detail nach Lock laden, um Parallelzugriff auf DbContext zu vermeiden
        if (idToLoad > 0)
            await LoadDetailAsync(idToLoad);
    }

    private void StyleGrid()
    {
        if (dgwKunden.Columns.Count == 0) return;

        // 1. Tüm kolonlara Almanca başlık ata (gizli olanlar da dahil)
        ApplyColumnHeaders(dgwKunden, _columnHeaders);

        // 2. Standart görünür kolonları belirle
        foreach (DataGridViewColumn col in dgwKunden.Columns) col.Visible = false;
        ShowCol("Kundennummer", "Kunden-Nr.", 100);
        ShowCol("Kundenname",   "Name",       0, fill: true);
        ShowCol("Tur",          "Tour",       80);
        ShowCol("Kundenfilter", "Gruppe",     90);
        ShowCol("Limit",        "Limit",      80, format: "N2", right: true);
        ShowCol("Aktiv",        "Aktiv",      50);

        // 3. Kullanıcı ayarlarını uygula
        ApplyColumnChooserSettings(dgwKunden);
    }

    private void ShowCol(string name, string header, int width,
        bool fill = false, string? format = null, bool right = false)
    {
        if (!dgwKunden.Columns.Contains(name)) return;
        var col = dgwKunden.Columns[name];
        col.Visible = true;
        col.HeaderText = header;
        if (fill) col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        else { col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; col.Width = width; }
        if (format is not null) col.DefaultCellStyle.Format = format;
        if (right) col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
    }

    private async Task LoadDetailAsync(int id)
    {
        await _serviceLock.WaitAsync();
        try
        {
            _currentCustomer = await _customerService.GetByIdAsync(id);
            if (_currentCustomer is null) return;
            BindToForm(_currentCustomer);
            SetDetailVisible(true);
            _isDirty = false;
        }
        catch (Exception ex) { ShowError($"Ladefehler:\n{ex.Message}"); }
        finally { _serviceLock.Release(); }
    }

    private void BindToForm(Customer c)
    {
        txtKundennummer.Text = c.Kundennummer;
        txtKundenname.Text = c.Kundenname;
        txtName2.Text = c.Name2 ?? "";
        txtInhaber.Text = c.Inhaber ?? "";
        txtTelefon.Text = c.Telefonnummer ?? "";
        txtHandy.Text = c.Handynummer ?? "";
        txtEmail.Text = c.EMail ?? "";
        txtNotizen.Text = c.Notizen ?? "";
        txtAdresse.Text = c.Adresse ?? "";
        txtAdresse2.Text = c.Adresse2 ?? "";
        txtPLZ.Text = c.PLZ ?? "";
        txtOrt.Text = c.Ort ?? "";
        txtLand.Text = c.Land;
        chkAbweichend.Checked = c.AbweichendeLieferadresse;
        txtALName2.Text = c.ALName2 ?? "";
        txtALInhaber.Text = c.ALInhaber ?? "";
        txtALAdresse.Text = c.ALAdresse ?? "";
        txtALAdresse2.Text = c.ALAdresse2 ?? "";
        txtALPLZ.Text = c.ALPLZ ?? "";
        txtALOrt.Text = c.ALOrt ?? "";
        txtALLand.Text = c.ALLand ?? "";

        SelectCombo(cmbTur,          c.TurId);
        SelectCombo(cmbAusnahmeTur,  c.AusnahmeTurId);
        SelectCombo(cmbKundenfilter, c.KundenfilterId);

        //nudRoutenfolge.Value = c.Routenfolge;
        nudLimit.Value       = c.Limit;

        // Liefertage
        chkMo.Checked = c.LiefertMo;
        chkDi.Checked = c.LiefertDi;
        chkMi.Checked = c.LiefertMi;
        chkDo.Checked = c.LiefertDo;
        chkFr.Checked = c.LiefertFr;
        chkSa.Checked = c.LiefertSa;
        chkSo.Checked = c.LiefertSo;

        // Geräte
        txtGeraete1.Text = c.Geraete1 ?? "";
        txtGeraete2.Text = c.Geraete2 ?? "";
        txtGeraete3.Text = c.Geraete3 ?? "";
        txtGeraete4.Text = c.Geraete4 ?? "";
        txtGeraete5.Text = c.Geraete5 ?? "";

        chkPreisAusblenden.Checked = c.PreisAusblenden;
        chkAktiv.Checked           = c.Aktiv;
        txtKundennummer.ReadOnly   = c.Id > 0;
        ToggleAltFields(c.AbweichendeLieferadresse);
        btnDeaktivieren.Text = c.Aktiv ? "Deaktivieren" : "Aktivieren";
        _isDirty = false;
    }

    private void BindFromForm(Customer c)
    {
        c.Kundennummer  = txtKundennummer.Text.Trim();
        c.Kundenname    = txtKundenname.Text.Trim();
        c.Name2         = txtName2.Text.NullIfEmpty();
        c.Inhaber       = txtInhaber.Text.NullIfEmpty();
        c.Telefonnummer = txtTelefon.Text.NullIfEmpty();
        c.Handynummer   = txtHandy.Text.NullIfEmpty();
        c.EMail         = txtEmail.Text.NullIfEmpty();
        c.Notizen       = txtNotizen.Text.NullIfEmpty();
        c.Adresse       = txtAdresse.Text.NullIfEmpty();
        c.Adresse2      = txtAdresse2.Text.NullIfEmpty();
        c.PLZ           = txtPLZ.Text.NullIfEmpty();
        c.Ort           = txtOrt.Text.NullIfEmpty();
        c.Land          = string.IsNullOrWhiteSpace(txtLand.Text) ? "Deutschland" : txtLand.Text.Trim();
        c.AbweichendeLieferadresse = chkAbweichend.Checked;
        c.ALName2    = txtALName2.Text.NullIfEmpty();
        c.ALInhaber  = txtALInhaber.Text.NullIfEmpty();
        c.ALAdresse  = txtALAdresse.Text.NullIfEmpty();
        c.ALAdresse2 = txtALAdresse2.Text.NullIfEmpty();
        c.ALPLZ      = txtALPLZ.Text.NullIfEmpty();
        c.ALOrt      = txtALOrt.Text.NullIfEmpty();
        c.ALLand     = txtALLand.Text.NullIfEmpty();

        c.TurId         = (cmbTur.SelectedItem         as ComboItem)?.Id is > 0 and int tid ? tid : null;
        c.AusnahmeTurId = (cmbAusnahmeTur.SelectedItem  as ComboItem)?.Id is > 0 and int aid ? aid : null;
        c.KundenfilterId = (cmbKundenfilter.SelectedItem as ComboItem)?.Id is > 0 and int fid ? fid : null;

        //c.Routenfolge    = (int)nudRoutenfolge.Value;
        c.Limit          = nudLimit.Value;

        // Liefertage
        c.LiefertMo = chkMo.Checked;
        c.LiefertDi = chkDi.Checked;
        c.LiefertMi = chkMi.Checked;
        c.LiefertDo = chkDo.Checked;
        c.LiefertFr = chkFr.Checked;
        c.LiefertSa = chkSa.Checked;
        c.LiefertSo = chkSo.Checked;

        // Geräte
        c.Geraete1 = txtGeraete1.Text.NullIfEmpty();
        c.Geraete2 = txtGeraete2.Text.NullIfEmpty();
        c.Geraete3 = txtGeraete3.Text.NullIfEmpty();
        c.Geraete4 = txtGeraete4.Text.NullIfEmpty();
        c.Geraete5 = txtGeraete5.Text.NullIfEmpty();

        c.PreisAusblenden = chkPreisAusblenden.Checked;
        c.Aktiv           = chkAktiv.Checked;
    }

    private async Task SaveAsync()
    {
        if (_currentCustomer is null) return;
        BindFromForm(_currentCustomer);
        try
        {
            await _serviceLock.WaitAsync();
            try { await _customerService.SaveAsync(_currentCustomer); _isDirty = false; }
            finally { _serviceLock.Release(); }

            await LoadListAsync();
            SelectById(_currentCustomer.Id);
            ShowSuccess("Kunde gespeichert.");
        }
        catch (ValidationException ex) { ShowError(ex.Message); }
        catch (Exception ex) { ShowError($"Fehler:\n{ex.Message}"); }
    }

    private void NewCustomer()
    {
        if (_isDirty && !Confirm("Ungespeicherte Änderungen verwerfen?")) return;
        _currentCustomer = new Customer();
        BindToForm(_currentCustomer);
        txtKundennummer.ReadOnly = false;
        SetDetailVisible(true);
        txtKundennummer.Focus();
    }

    private async Task ToggleActiveAsync()
    {
        if (_currentCustomer is null || _currentCustomer.Id == 0) return;
        bool newState = !_currentCustomer.Aktiv;
        if (!Confirm($"Kunden '{_currentCustomer.Kundenname}' {(newState ? "aktivieren" : "deaktivieren")}?")) return;
        try
        {
            await _serviceLock.WaitAsync();
            try { await _customerService.SetActiveAsync(_currentCustomer.Id, newState); _currentCustomer.Aktiv = newState; }
            finally { _serviceLock.Release(); }

            chkAktiv.Checked = newState;
            btnDeaktivieren.Text = newState ? "Deaktivieren" : "Aktivieren";
            await LoadListAsync();
        }
        catch (Exception ex) { ShowError($"Fehler:\n{ex.Message}"); }
    }

    // ── Hilfsmethoden ─────────────────────────────────────────────────────────

    private void SetDetailVisible(bool v) => panelDetail.Visible = v;

    private void ToggleAltFields(bool on)
    {
        foreach (var tb in new Control[] {
            txtALName2, txtALInhaber, txtALAdresse,
            txtALAdresse2, txtALPLZ, txtALOrt, txtALLand })
            tb.Enabled = on;
    }

    private int SelectedId() =>
        dgwKunden.CurrentRow?.DataBoundItem is CustomerListDto d ? d.Id : 0;

    private static DataGridViewCell? GetFirstVisibleCell(DataGridViewRow row)
    {
        foreach (DataGridViewCell cell in row.Cells)
            if (cell.OwningColumn?.Visible == true) return cell;
        return null;
    }

    private void SelectById(int id)
    {
        foreach (DataGridViewRow row in dgwKunden.Rows)
        {
            if (row.DataBoundItem is not CustomerListDto d || d.Id != id) continue;
            row.Selected = true;
            var cell = GetFirstVisibleCell(row);
            if (cell is not null) dgwKunden.CurrentCell = cell;
            break;
        }
    }

    private static void SelectCombo(ComboBox cb, int? id)
    {
        if (id is null or 0) { cb.SelectedIndex = 0; return; }
        foreach (var item in cb.Items)
            if (item is ComboItem ci && ci.Id == id) { cb.SelectedItem = ci; return; }
        cb.SelectedIndex = 0;
    }

    // ── Events ────────────────────────────────────────────────────────────────

    private async void DgwKunden_SelectionChanged(object? s, EventArgs e)
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
        if (IsDesignMode() || _suppressFilterChanged) return;
        await LoadListAsync();
    }

    private void BtnNeu_Click(object? s, EventArgs e) => NewCustomer();
    private async void BtnSpeichern_Click(object? s, EventArgs e) => await SaveAsync();
    private async void BtnDeaktivieren_Click(object? s, EventArgs e) => await ToggleActiveAsync();
    private void Detail_Changed(object? s, EventArgs e) => _isDirty = true;

    private void ChkAbweichend_CheckedChanged(object? s, EventArgs e)
    {
        ToggleAltFields(chkAbweichend.Checked);
        _isDirty = true;
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.F2) { NewCustomer(); return true; }
        if (keyData == (Keys.Control | Keys.S)) { _ = SaveAsync(); return true; }
        return base.ProcessCmdKey(ref msg, keyData);
    }

    private void FrmCustomerList_FormClosing(object? s, FormClosingEventArgs e)
    {
        if (_isDirty && !Confirm("Ungespeicherte Änderungen verwerfen und schließen?"))
            e.Cancel = true;
    }

    // ── AusnahmeTur Validierung (live, beim Auswählen) ────────────────────────────
    private void CmbAusnahmeTur_Changed(object? s, EventArgs e)
    {
        _isDirty = true;
        var turId       = (cmbTur.SelectedItem         as ComboItem)?.Id;
        var ausnahmeId  = (cmbAusnahmeTur.SelectedItem  as ComboItem)?.Id;

        if (turId.HasValue && ausnahmeId.HasValue && turId == ausnahmeId && ausnahmeId > 0)
        {
            ShowError("Ausnahme-Tour darf nicht identisch mit der Standard-Tour sein.");
            cmbAusnahmeTur.SelectedIndex = 0;
        }
    }
}