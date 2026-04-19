using System.ComponentModel;
using DNR26V2.Domain.DTOs;
using DNR26V2.Domain.Enums;
using DNR26V2.Domain.Exceptions;
using DNR26V2.Forms.Base;
using DNR26V2.Services.Orders;

namespace DNR26V2.Forms.Orders;

public partial class FrmOrderEntry : BaseListForm
{
    private readonly IOrderService _orderService;
    private readonly SemaphoreSlim _lock = new(1, 1);

    private DayOfWeek? _selectedDay;
    private Button? _activeDayBtn;
    private bool _suppressKundenChanged;
    private bool _isLoading;
    private bool _suppressFilter;

    private int _selectedKundeId;
    private string _selectedKundename = string.Empty;
    private bool _preisAusblenden;
    private int? _currentAuftragId;
    private OrderStatus? _currentStatus;

    // Full loaded list — filter works in-memory on this
    private IReadOnlyList<OrderKundeListDto> _kundenListe = [];

    // Tracks which KundeIds are checked (checkbox column)
    private readonly HashSet<int> _checkedKundeIds = new();

    // Cached article list for Hinzufügen dialog (refreshed once per form load)
    private IReadOnlyList<ArtikelSuchDto> _artikelCache = [];

    public FrmOrderEntry(IOrderService orderService)
    {
        _orderService = orderService;
        InitializeComponent();
        FixPositionenColumnNames();
        WireUpEvents();
    }

    // Parameterless constructor for Designer
    public FrmOrderEntry()
    {
        _orderService = null!;
        InitializeComponent();
        FixPositionenColumnNames();
        WireUpEvents();
    }

    private void FixPositionenColumnNames()
    {
        colZeileId.Name = "_ZeileId";
        colArtikelId.Name = "_ArtikelId";
        colArtikelnummer.Name = "Artikelnummer";
        colProduktname.Name = "Produktname";
        colMenge.Name = "Menge";
        colGewicht.Name = "Gewicht";
        colPreis.Name = "Preis";
        colNotiz.Name = "Notiz";
    }

    private static bool IsDesignMode() =>
        LicenseManager.UsageMode == LicenseUsageMode.Designtime;

    // ── Events ────────────────────────────────────────────────────────────────

    private void WireUpEvents()
    {
        Load += FrmOrderEntry_Load;

        dtpLieferdatum.ValueChanged += async (_, _) => await ReloadAsync();

        btnMo.Click += (_, _) => SelectDay(btnMo, DayOfWeek.Monday);
        btnDi.Click += (_, _) => SelectDay(btnDi, DayOfWeek.Tuesday);
        btnMi.Click += (_, _) => SelectDay(btnMi, DayOfWeek.Wednesday);
        btnDo.Click += (_, _) => SelectDay(btnDo, DayOfWeek.Thursday);
        btnFr.Click += (_, _) => SelectDay(btnFr, DayOfWeek.Friday);
        btnSa.Click += (_, _) => SelectDay(btnSa, DayOfWeek.Saturday);
        btnSo.Click += (_, _) => SelectDay(btnSo, DayOfWeek.Sunday);
        btnAlle.Click += (_, _) => SelectDay(btnAlle, null);

        txtKundeFilter.TextChanged += (_, _) => { if (!_suppressFilter) OnFilterChanged(); };
        cmbTourFilter.SelectedIndexChanged += (_, _) => { if (!_suppressFilter) OnFilterChanged(); };

        dgwKunden.SelectionChanged += DgwKunden_SelectionChanged;
        dgwKunden.CellFormatting += DgwKunden_CellFormatting;
        dgwKunden.KeyDown += DgwKunden_KeyDown;
        dgwKunden.CurrentCellDirtyStateChanged += DgwKunden_DirtyStateChanged;
        dgwKunden.CellValueChanged += DgwKunden_CheckboxValueChanged;

        dgwPositionen.EditingControlShowing += DgwPositionen_EditingControlShowing;
        dgwPositionen.KeyDown += DgwPositionen_KeyDown;

        btnBuchen.Click += BtnBuchen_Click;
        btnFreigeben.Click += BtnFreigeben_Click;
        btnSpeichern.Click += BtnSpeichern_Click;
        btnHinzufuegen.Click += BtnHinzufuegen_Click;
        btnNachlieferung.Click += BtnNachlieferung_Click;
        btnStornieren.Click += BtnStornieren_Click;
        btnLoeschen.Click += BtnLoeschen_Click;
        btnAlleFreigeben.Click += BtnAlleFreigeben_Click;

        cmsPositionen.Opening += CmsPositionen_Opening;
        cmsMenuZeileLoeschen.Click += (_, _) => DeleteSelectedZeile();
        cmsMenuHinzufuegen.Click += BtnHinzufuegen_Click;
    }

    // ── Load ──────────────────────────────────────────────────────────────────

    private void FrmOrderEntry_Load(object? sender, EventArgs e)
    {
        if (IsDesignMode() || _orderService is null) return;

        WindowState = FormWindowState.Maximized;

        _isLoading = true;
        dtpLieferdatum.Value = DateTime.Today;
        _isLoading = false;

        ClearRightPanel();

        // Pre-load article cache once
        _ = RefreshArtikelCacheAsync();

        var todayBtn = dtpLieferdatum.Value.DayOfWeek switch
        {
            DayOfWeek.Monday => btnMo,
            DayOfWeek.Tuesday => btnDi,
            DayOfWeek.Wednesday => btnMi,
            DayOfWeek.Thursday => btnDo,
            DayOfWeek.Friday => btnFr,
            DayOfWeek.Saturday => btnSa,
            DayOfWeek.Sunday => btnSo,
            _ => btnAlle
        };
        SelectDay(todayBtn, dtpLieferdatum.Value.DayOfWeek);
    }

    // ── Article cache ─────────────────────────────────────────────────────────

    private async Task RefreshArtikelCacheAsync()
    {
        try
        {
            _artikelCache = await _orderService.GetArtikelListeAsync();
        }
        catch { /* non-critical — dialog will show empty list */ }
    }

    // ── Day toggle ────────────────────────────────────────────────────────────

    private void SelectDay(Button btn, DayOfWeek? day)
    {
        if (_activeDayBtn is not null)
        {
            _activeDayBtn.BackColor = SystemColors.Control;
            _activeDayBtn.ForeColor = SystemColors.ControlText;
        }

        btn.BackColor = Color.SteelBlue;
        btn.ForeColor = Color.White;
        _activeDayBtn = btn;
        _selectedDay = day;

        _ = ReloadAsync();
    }

    // ── Reload customer list from DB ──────────────────────────────────────────

    private async Task ReloadAsync()
    {
        if (_orderService is null || _isLoading) return;

        IReadOnlyList<OrderKundeListDto> liste;

        await _lock.WaitAsync();
        try
        {
            Cursor = Cursors.WaitCursor;
            liste = await _orderService.GetKundenListeAsync(
                dtpLieferdatum.Value.Date, _selectedDay);
        }
        catch (Exception ex) { ShowError($"Ladefehler:\n{ex.Message}"); return; }
        finally { Cursor = Cursors.Default; _lock.Release(); }

        // Store full list; auto-check all rows that already have an Auftrag
        _kundenListe = liste;
        _checkedKundeIds.Clear();
        foreach (var k in _kundenListe)
            if (k.AuftragId > 0)
                _checkedKundeIds.Add(k.Id);

        // Rebuild Tour-ComboBox without triggering the filter event
        RefreshTourFilter();

        // Bind filtered grid
        ApplyFilter();

        var id = SelectedKundeId();
        if (id > 0) await LoadPositionenAsync(id);
        else ClearRightPanel();
    }

    // ── Tour filter rebuild ───────────────────────────────────────────────────

    private void RefreshTourFilter()
    {
        _suppressFilter = true;
        try
        {
            var prevTour = cmbTourFilter.SelectedItem as string;
            cmbTourFilter.Items.Clear();
            cmbTourFilter.Items.Add("Alle");

            foreach (var tour in _kundenListe
                .Select(k => k.Tur)
                .Where(t => t is not null)
                .Distinct()
                .OrderBy(t => t))
                cmbTourFilter.Items.Add(tour!);

            if (prevTour is not null && cmbTourFilter.Items.Contains(prevTour))
                cmbTourFilter.SelectedItem = prevTour;
            else
                cmbTourFilter.SelectedIndex = 0;
        }
        finally { _suppressFilter = false; }
    }

    // ── Filter (in-memory, no DB call) ────────────────────────────────────────

    private void ApplyFilter()
    {
        if (_isLoading) return;

        var suche = txtKundeFilter.Text.Trim();
        var tourWahl = cmbTourFilter.SelectedItem as string;

        var gefiltert = _kundenListe
            .Where(k => suche == string.Empty ||
                        k.Kundenname.Contains(suche, StringComparison.OrdinalIgnoreCase))
            .Where(k => tourWahl == null || tourWahl == "Alle" ||
                        k.Tur == tourWahl)
            .ToList();

        _suppressKundenChanged = true;
        try
        {
            var prevId = _selectedKundeId;
            dgwKunden.DataSource = null;
            dgwKunden.DataSource = gefiltert;
            StyleKundenGrid();
            ApplyKundenCheckmarks();

            if (prevId > 0) SelectKundeById(prevId);
            else if (dgwKunden.Rows.Count > 0)
                dgwKunden.CurrentCell = GetFirstVisibleCell(dgwKunden.Rows[0]);
        }
        finally { _suppressKundenChanged = false; }

        UpdateAlleFreigebenButton();
    }

    // Triggered by user changing filter controls
    private void OnFilterChanged()
    {
        var prevId = _selectedKundeId;
        ApplyFilter();
        var newId = SelectedKundeId();
        if (newId != prevId)
        {
            if (newId > 0) _ = LoadPositionenAsync(newId);
            else ClearRightPanel();
        }
    }

    private void StyleKundenGrid()
    {
        if (dgwKunden.Columns.Count == 0) return;

        // Hide all except the checkbox column; mark bound columns read-only
        foreach (DataGridViewColumn c in dgwKunden.Columns)
        {
            if (c.Name == "colKundeChecked") { c.Visible = true; continue; }
            c.Visible = false;
            c.ReadOnly = true;
        }

        ShowKol("Kundenname", "Kunde", 0, fill: true);
        ShowKol("Tur", "Tour", 55);
    }

    private void ShowKol(string name, string header, int width, bool fill = false)
    {
        if (!dgwKunden.Columns.Contains(name)) return;
        var col = dgwKunden.Columns[name];
        col.Visible = true;
        col.HeaderText = header;
        if (fill) col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        else { col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; col.Width = width; }
    }

    // ── Checkbox: apply state from _checkedKundeIds to visible rows ───────────

    private void ApplyKundenCheckmarks()
    {
        foreach (DataGridViewRow row in dgwKunden.Rows)
        {
            if (row.DataBoundItem is not OrderKundeListDto dto) continue;
            row.Cells["colKundeChecked"].Value = _checkedKundeIds.Contains(dto.Id);
        }
    }

    // ── Checkbox: commit on single click ──────────────────────────────────────

    private void DgwKunden_DirtyStateChanged(object? s, EventArgs e)
    {
        if (dgwKunden.IsCurrentCellDirty &&
            dgwKunden.CurrentCell?.OwningColumn?.Name == "colKundeChecked")
            dgwKunden.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }

    private void DgwKunden_CheckboxValueChanged(object? s, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        if (dgwKunden.Columns[e.ColumnIndex]?.Name != "colKundeChecked") return;
        if (dgwKunden.Rows[e.RowIndex].DataBoundItem is not OrderKundeListDto dto) return;

        bool isChecked = dgwKunden.Rows[e.RowIndex].Cells["colKundeChecked"].Value is true;
        if (isChecked) _checkedKundeIds.Add(dto.Id);
        else _checkedKundeIds.Remove(dto.Id);

        UpdateAlleFreigebenButton();
    }

    // ── Spacebar: toggle checkbox on current row ──────────────────────────────

    private void DgwKunden_KeyDown(object? s, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Space) return;
        if (dgwKunden.CurrentRow?.DataBoundItem is not OrderKundeListDto dto) return;

        e.Handled = true;
        e.SuppressKeyPress = true;

        bool current = _checkedKundeIds.Contains(dto.Id);
        if (current) _checkedKundeIds.Remove(dto.Id);
        else _checkedKundeIds.Add(dto.Id);

        dgwKunden.CurrentRow.Cells["colKundeChecked"].Value = !current;
        UpdateAlleFreigebenButton();
    }

    // ── "Alle Freigeben" button state ─────────────────────────────────────────

    private void UpdateAlleFreigebenButton()
    {
        btnAlleFreigeben.Enabled = _kundenListe.Any(k =>
            _checkedKundeIds.Contains(k.Id) &&
            k.AuftragId > 0 &&
            (k.AuftragStatus is null || k.AuftragStatus == OrderStatus.Offen));
    }

    // ── Customer grid color coding ────────────────────────────────────────────

    private void DgwKunden_CellFormatting(object? s, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0) return;
        if (dgwKunden.Rows[e.RowIndex].DataBoundItem is not OrderKundeListDto dto) return;

        dgwKunden.Rows[e.RowIndex].DefaultCellStyle.BackColor = dto.AuftragStatus switch
        {
            OrderStatus.Gebucht => Color.FromArgb(200, 255, 200),   // green
            OrderStatus.Freigegeben => Color.FromArgb(200, 230, 255),   // light blue
            OrderStatus.Offen => Color.FromArgb(255, 255, 200),   // yellow
            OrderStatus.Storniert => Color.FromArgb(240, 240, 240),   // grey
            _ => SystemColors.Window
        };
    }

    // ── Load order positions + FactBox ────────────────────────────────────────

    private async Task LoadPositionenAsync(int kundeId)
    {
        // Clear previous status immediately to avoid showing stale labels while loading
        lblAuftragStatus.Text = string.Empty;
        // ensure grid is not left in edit-mode from previous selection
        try { dgwPositionen.EndEdit(); } catch { }
        dgwPositionen.ClearSelection();
        dgwPositionen.ReadOnly = true;

        var kundeDto = dgwKunden.CurrentRow?.DataBoundItem as OrderKundeListDto;

        _selectedKundeId = kundeId;
        _selectedKundename = kundeDto?.Kundenname ?? string.Empty;
        _preisAusblenden = kundeDto?.PreisAusblenden ?? false;
        _currentAuftragId = kundeDto?.AuftragId;
        _currentStatus = kundeDto?.AuftragStatus;

        lblKundenname.Text = _selectedKundename;
        lblAuftragStatus.Text = _currentStatus switch
        {
            OrderStatus.Gebucht => "✓ GEBUCHT",
            OrderStatus.Freigegeben => "FREIGEGEBEN",
            OrderStatus.Offen => "OFFEN",
            OrderStatus.Storniert => "STORNIERT",
            OrderStatus.Geloescht => "GELÖSCHT",
            _ => string.Empty
        };
        lblAuftragStatus.ForeColor = _currentStatus switch
        {
            OrderStatus.Gebucht => Color.DarkGreen,
            OrderStatus.Freigegeben => Color.SteelBlue,
            OrderStatus.Offen => Color.DarkOrange,
            OrderStatus.Storniert => Color.Gray,
            _ => SystemColors.ControlText
        };

        // Load positions and FactBox in parallel
        IReadOnlyList<OrderLineDto> positionen;
        KundenFactBoxDto? factBox;

        await _lock.WaitAsync();
        try
        {
            var posTask = _orderService.GetPositionenAsync(kundeId, dtpLieferdatum.Value.Date);
            var fbTask = _orderService.GetKundenFactBoxAsync(kundeId);
            await Task.WhenAll(posTask, fbTask);
            positionen = posTask.Result;
            factBox = fbTask.Result;
        }
        catch (Exception ex) { ShowError($"Ladefehler:\n{ex.Message}"); return; }
        finally { _lock.Release(); }

        UpdateFactBox(factBox);

        dgwPositionen.Rows.Clear();
        foreach (var pos in positionen)
        {
            var idx = dgwPositionen.Rows.Add();
            var row = dgwPositionen.Rows[idx];
            row.Cells["_ZeileId"].Value = pos.OrderLineId;
            row.Cells["_ArtikelId"].Value = pos.ArtikelId;
            row.Cells["Artikelnummer"].Value = pos.Artikelnummer;
            row.Cells["Produktname"].Value = pos.Produktname;
            row.Cells["Menge"].Value = pos.Menge;
            row.Cells["Gewicht"].Value = pos.Gewicht;
            row.Cells["Preis"].Value = pos.Preis;
            row.Cells["Notiz"].Value = pos.Notiz;
        }

        // Set focus to first editable cell (Menge) in first row and begin edit
        if (dgwPositionen.Rows.Count > 0)
        {
            var startCell = GetFirstEditableCell(dgwPositionen.Rows[0]);
            if (startCell is not null)
            {
                dgwPositionen.CurrentCell = startCell;
                dgwPositionen.Focus();
                dgwPositionen.BeginEdit(true);
            }
        }

        colPreis.Visible = !_preisAusblenden;

        // ── Editability ───────────────────────────────────────────────────────
        bool istOffen = _currentStatus is null or OrderStatus.Offen;
        bool istFreigeben = _currentStatus == OrderStatus.Freigegeben;
        bool istGebucht = _currentStatus == OrderStatus.Gebucht;

        btnSpeichern.Enabled = istOffen;
        btnHinzufuegen.Enabled = istOffen;
        dgwPositionen.ReadOnly = false;

        foreach (DataGridViewColumn c in dgwPositionen.Columns)
        {
            if (c.Name is null) { c.ReadOnly = true; continue; }
            var editable = istOffen && (c.Name == "Menge" || c.Name == "Gewicht" || c.Name == "Preis" || c.Name == "Notiz");
            c.ReadOnly = !editable;
        }

        btnFreigeben.Enabled = istOffen && _currentAuftragId > 0;
        btnFreigeben.Visible = istOffen;
        btnBuchen.Enabled = istOffen || istFreigeben;
        btnLoeschen.Enabled = istOffen && _currentAuftragId > 0;
        btnLoeschen.Visible = istOffen;
        btnStornieren.Enabled = istFreigeben || istGebucht;
        btnStornieren.Visible = istFreigeben || istGebucht;
        btnNachlieferung.Enabled = istGebucht;
        btnNachlieferung.Visible = istGebucht;
    }

    // Handle Enter navigation inside editing control
    private void DgwPositionen_EditingControlShowing(object? sender, DataGridViewEditingControlShowingEventArgs e)
    {
        if (e.Control is TextBox tb)
        {
            tb.KeyDown -= EditingControl_KeyDown;
            tb.KeyDown += EditingControl_KeyDown;
        }
        else if (e.Control is ComboBox cb)
        {
            cb.KeyDown -= EditingControl_KeyDown;
            cb.KeyDown += EditingControl_KeyDown;
        }
    }

    private void EditingControl_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;
        e.Handled = true;
        e.SuppressKeyPress = true;
        MoveToNextEditableCell();
    }

    private void DgwPositionen_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.Handled = true;
            e.SuppressKeyPress = true;
            if (dgwPositionen.CurrentCell is not null && dgwPositionen.IsCurrentCellInEditMode)
                return;
            MoveToNextEditableCell();
        }
    }

    private void MoveToNextEditableCell()
    {
        if (dgwPositionen.CurrentCell is null) return;
        int row = dgwPositionen.CurrentCell.RowIndex;

        var editableNames = new[] { "Menge", "Gewicht", "Preis", "Notiz" };
        var curName = dgwPositionen.CurrentCell.OwningColumn?.Name;
        if (curName is null) return;

        // Last editable column → jump to Menge of next row
        if (curName == "Notiz")
        {
            int nextRow = row + 1;
            if (nextRow >= dgwPositionen.Rows.Count) return;
            var target = dgwPositionen.Rows[nextRow].Cells["Menge"];
            if (target.OwningColumn.ReadOnly) return;
            dgwPositionen.CurrentCell = target;
            dgwPositionen.BeginEdit(true);
            return;
        }

        // Move to next editable column in same row
        int startIndex = Array.IndexOf(editableNames, curName);
        for (int i = startIndex + 1; i < editableNames.Length; i++)
        {
            var name = editableNames[i];
            if (!dgwPositionen.Columns.Contains(name)) continue;
            var c = dgwPositionen.Rows[row].Cells[name];
            if (c.OwningColumn.ReadOnly) continue;
            dgwPositionen.CurrentCell = c;
            dgwPositionen.BeginEdit(true);
            return;
        }

        // Fallback: Menge of next row
        int nr = row + 1;
        if (nr < dgwPositionen.Rows.Count)
        {
            var t = dgwPositionen.Rows[nr].Cells["Menge"];
            if (!t.OwningColumn.ReadOnly)
            {
                dgwPositionen.CurrentCell = t;
                dgwPositionen.BeginEdit(true);
            }
        }
    }

    // ── FactBox update ────────────────────────────────────────────────────────

    private void UpdateFactBox(KundenFactBoxDto? fb)
    {
        if (fb is null)
        {
            lblFBTour.Text = string.Empty;
            lblFBSaldo.Text = string.Empty;
            lblFBLetzterAuftrag.Text = string.Empty;
            lblFBOffeneAuftraege.Text = string.Empty;
            lblFBSaldoCaption.Visible = true;
            lblFBSaldo.Visible = true;
            return;
        }

        lblFBTour.Text = fb.Tour ?? "—";

        bool showSaldo = !fb.PreisAusblenden;
        lblFBSaldoCaption.Visible = showSaldo;
        lblFBSaldo.Visible = showSaldo;

        if (showSaldo)
        {
            lblFBSaldo.Text = $"{fb.Saldo:N2} €";
            lblFBSaldo.ForeColor = fb.Saldo < 0 ? Color.Crimson : Color.DarkGreen;
        }

        lblFBLetzterAuftrag.Text = fb.LetzterAuftrag.HasValue
            ? fb.LetzterAuftrag.Value.ToString("dd.MM.yyyy")
            : "—";

        lblFBOffeneAuftraege.Text = fb.OffeneAuftraege.ToString();
        lblFBOffeneAuftraege.ForeColor = fb.OffeneAuftraege > 0
            ? Color.DarkOrange
            : Color.DarkGreen;
    }

    private void ClearRightPanel()
    {
        lblKundenname.Text = "— Kunden auswählen —";
        lblAuftragStatus.Text = string.Empty;
        lblStatusInfo.Text = string.Empty;
        dgwPositionen.Rows.Clear();
        UpdateFactBox(null);
        colPreis.Visible = true;
        _selectedKundeId = 0;
        _preisAusblenden = false;
        _currentAuftragId = null;
        _currentStatus = null;
        dgwPositionen.ReadOnly = true;
        btnBuchen.Enabled = false;
        btnSpeichern.Enabled = false;
        btnHinzufuegen.Enabled = false;
        btnFreigeben.Enabled = false;
        btnFreigeben.Visible = true;
        btnLoeschen.Enabled = false;
        btnLoeschen.Visible = true;
        btnStornieren.Enabled = false;
        btnStornieren.Visible = false;
        btnNachlieferung.Enabled = false;
        btnNachlieferung.Visible = false;
    }

    // ── Hinzufügen ────────────────────────────────────────────────────────────

    private void BtnHinzufuegen_Click(object? s, EventArgs e)
    {
        if (_selectedKundeId <= 0) return;

        if (_artikelCache.Count == 0)
            _ = RefreshArtikelCacheAsync();

        using var dlg = new FrmArtikelAuswahl(_artikelCache);
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        if (dlg.SelectedArtikel is not ArtikelSuchDto art) return;

        var idx = dgwPositionen.Rows.Add();
        var row = dgwPositionen.Rows[idx];
        row.Cells["_ZeileId"].Value = 0;
        row.Cells["_ArtikelId"].Value = art.ArtikelId;
        row.Cells["Artikelnummer"].Value = art.Artikelnummer;
        row.Cells["Produktname"].Value = art.Produktname;
        row.Cells["Menge"].Value = 0m;
        row.Cells["Gewicht"].Value = 0m;
        row.Cells["Preis"].Value = art.VKPreis;
        row.Cells["Notiz"].Value = string.Empty;

        dgwPositionen.CurrentCell = row.Cells["Menge"];
        dgwPositionen.BeginEdit(true);
    }

    // ── Zeile löschen ────────────────────────────────────────────────────────

    private void DeleteSelectedZeile()
    {
        if (dgwPositionen.CurrentRow is not DataGridViewRow row) return;
        if (row.IsNewRow) return;
        dgwPositionen.Rows.Remove(row);
    }

    // ── Context menu ──────────────────────────────────────────────────────────

    private void CmsPositionen_Opening(object? s, CancelEventArgs e)
    {
        bool istOffen = _currentStatus is null or OrderStatus.Offen;
        bool hatZeile = dgwPositionen.CurrentRow is not null && !dgwPositionen.CurrentRow.IsNewRow;
        cmsMenuZeileLoeschen.Enabled = istOffen && hatZeile;
        cmsMenuHinzufuegen.Enabled = istOffen && _selectedKundeId > 0;
    }

    // ── Collect grid rows ─────────────────────────────────────────────────────

    private List<(int ArtikelId, decimal Menge, decimal Gewicht, decimal Preis, string? Notiz)> CollectPositionen()
    {
        dgwPositionen.EndEdit();
        var result = new List<(int, decimal, decimal, decimal, string?)>();

        foreach (DataGridViewRow row in dgwPositionen.Rows)
        {
            if (row.IsNewRow) continue;
            if (row.Cells["_ArtikelId"].Value is not int artikelId) continue;

            result.Add((
                artikelId,
                ParseDecimal(row.Cells["Menge"].Value),
                ParseDecimal(row.Cells["Gewicht"].Value),
                ParseDecimal(row.Cells["Preis"].Value),
                row.Cells["Notiz"].Value?.ToString()
            ));
        }
        return result;
    }

    // ── Speichern ─────────────────────────────────────────────────────────────

    private async void BtnSpeichern_Click(object? s, EventArgs e)
    {
        if (_selectedKundeId <= 0) return;

        dgwPositionen.EndEdit();
        if (dgwPositionen.Rows.Count == 0)
        {
            ShowError("Mindestens eine Position ist erforderlich.");
            return;
        }

        try
        {
            await _lock.WaitAsync();
            try
            {
                var order = await _orderService.SaveAuftragAsync(
                    _selectedKundeId,
                    dtpLieferdatum.Value.Date,
                    CollectPositionen());
                _currentAuftragId = order.Id;
                _currentStatus = order.Status;
            }
            finally { _lock.Release(); }

            lblStatusInfo.Text = "Auftrag gespeichert.";
            await ReloadAsync();
            SelectKundeById(_selectedKundeId);
        }
        catch (ValidationException ex) { ShowError(ex.Message); }
        catch (Exception ex)
        {
            var msg = ex.InnerException?.InnerException?.Message
                   ?? ex.InnerException?.Message
                   ?? ex.Message;
            ShowError($"Fehler:\n{msg}");
        }
    }

    // ── Freigeben ─────────────────────────────────────────────────────────────

    private async void BtnFreigeben_Click(object? s, EventArgs e)
    {
        if (_currentAuftragId is not int id || id <= 0) return;

        try
        {
            await _lock.WaitAsync();
            try { await _orderService.FreigebenAsync(id); }
            finally { _lock.Release(); }

            lblStatusInfo.Text = "Auftrag freigegeben.";
            await ReloadAsync();
            SelectKundeById(_selectedKundeId);
        }
        catch (ValidationException ex) { ShowError(ex.Message); }
        catch (Exception ex) { ShowError($"Fehler:\n{ex.Message}"); }
    }

    // ── Alle Freigeben (checked Offen rows) ───────────────────────────────────

    private async void BtnAlleFreigeben_Click(object? s, EventArgs e)
    {
        var targets = _kundenListe
            .Where(k => _checkedKundeIds.Contains(k.Id) &&
                        k.AuftragId > 0 &&
                        (k.AuftragStatus is null || k.AuftragStatus == OrderStatus.Offen))
            .Select(k => k.AuftragId!.Value)
            .ToList();

        if (targets.Count == 0)
        {
            ShowError("Keine freigebbaren Aufträge ausgewählt.\n(Nur gespeicherte Aufträge mit Status 'Offen' können freigegeben werden.)");
            return;
        }

        try
        {
            await _lock.WaitAsync();
            try
            {
                foreach (var id in targets)
                    await _orderService.FreigebenAsync(id);
            }
            finally { _lock.Release(); }

            lblStatusInfo.Text = $"{targets.Count} Auftrag/Aufträge freigegeben.";
            await ReloadAsync();
        }
        catch (ValidationException ex) { ShowError(ex.Message); }
        catch (Exception ex) { ShowError($"Fehler:\n{ex.Message}"); }
    }

    // ── Buchen ────────────────────────────────────────────────────────────────

    private async void BtnBuchen_Click(object? s, EventArgs e)
    {
        if (_selectedKundeId <= 0) return;

        try
        {
            await _lock.WaitAsync();
            try
            {
                int auftragId;

                if (_currentStatus == OrderStatus.Freigegeben && _currentAuftragId > 0)
                {
                    // Freigegeben: lines are read-only → skip SaveAuftragAsync
                    auftragId = _currentAuftragId!.Value;
                }
                else
                {
                    // Offen: save grid lines first
                    dgwPositionen.EndEdit();
                    if (dgwPositionen.Rows.Count == 0)
                    {
                        ShowError("Mindestens eine Position ist erforderlich.");
                        return;
                    }
                    var saved = await _orderService.SaveAuftragAsync(
                        _selectedKundeId,
                        dtpLieferdatum.Value.Date,
                        CollectPositionen());
                    auftragId = saved.Id;
                }

                await _orderService.BuchenAsync(auftragId);
            }
            finally { _lock.Release(); }

            lblStatusInfo.Text = "✓ Auftrag gebucht — Lieferschein erstellt.";
            var prevKundeId = _selectedKundeId;
            await ReloadAsync();
            await AdvanceToNextUnbookedAsync(prevKundeId);
        }
        catch (ValidationException ex) { ShowError(ex.Message); }
        catch (Exception ex)
        {
            var msg = ex.InnerException?.InnerException?.Message
                   ?? ex.InnerException?.Message
                   ?? ex.Message;
            ShowError($"Fehler:\n{msg}");
        }
    }

    // ── Advance to next unbooked ──────────────────────────────────────────────

    private async Task AdvanceToNextUnbookedAsync(int justBookedKundeId)
    {
        int startIdx = 0;

        for (int i = 0; i < dgwKunden.Rows.Count; i++)
        {
            if (dgwKunden.Rows[i].DataBoundItem is OrderKundeListDto d && d.Id == justBookedKundeId)
            {
                startIdx = i + 1;
                break;
            }
        }

        for (int offset = 0; offset < dgwKunden.Rows.Count; offset++)
        {
            int idx = (startIdx + offset) % dgwKunden.Rows.Count;
            if (dgwKunden.Rows[idx].DataBoundItem is OrderKundeListDto dto
                && dto.AuftragStatus is null)
            {
                _suppressKundenChanged = true;
                try
                {
                    var cell = GetFirstVisibleCell(dgwKunden.Rows[idx]);
                    if (cell is not null) dgwKunden.CurrentCell = cell;
                }
                finally { _suppressKundenChanged = false; }

                await LoadPositionenAsync(dto.Id);
                return;
            }
        }

        var id = SelectedKundeId();
        if (id > 0) await LoadPositionenAsync(id);
    }

    // ── Löschen ───────────────────────────────────────────────────────────────

    private async void BtnLoeschen_Click(object? s, EventArgs e)
    {
        if (_currentAuftragId is not int id || id <= 0) return;
        if (!Confirm("Auftrag löschen?")) return;

        try
        {
            await _lock.WaitAsync();
            try { await _orderService.LoeschenAsync(id); }
            finally { _lock.Release(); }

            lblStatusInfo.Text = "Auftrag gelöscht.";
            await ReloadAsync();
            ClearRightPanel();
        }
        catch (ValidationException ex) { ShowError(ex.Message); }
        catch (Exception ex) { ShowError($"Fehler:\n{ex.Message}"); }
    }

    // ── Stornieren ────────────────────────────────────────────────────────────

    private async void BtnStornieren_Click(object? s, EventArgs e)
    {
        if (_currentAuftragId is not int auftragId || auftragId <= 0) return;
        if (!Confirm("Auftrag stornieren?")) return;

        try
        {
            await _lock.WaitAsync();
            try { await _orderService.StornierenAsync(auftragId); }
            finally { _lock.Release(); }

            lblStatusInfo.Text = "Auftrag storniert.";
            await ReloadAsync();
            SelectKundeById(_selectedKundeId);
        }
        catch (ValidationException ex) { ShowError(ex.Message); }
        catch (Exception ex) { ShowError($"Fehler:\n{ex.Message}"); }
    }

    // ── Nachlieferung ─────────────────────────────────────────────────────────

    private async void BtnNachlieferung_Click(object? s, EventArgs e)
    {
        if (_selectedKundeId <= 0) return;

        try
        {
            await _lock.WaitAsync();
            try { await _orderService.NachlieferungAsync(_selectedKundeId, dtpLieferdatum.Value.Date); }
            finally { _lock.Release(); }

            lblStatusInfo.Text = "+ Nachlieferung erstellt.";
            await ReloadAsync();
            SelectKundeById(_selectedKundeId);
        }
        catch (ValidationException ex) { ShowError(ex.Message); }
        catch (Exception ex) { ShowError($"Fehler:\n{ex.Message}"); }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private int SelectedKundeId() =>
        dgwKunden.CurrentRow?.DataBoundItem is OrderKundeListDto d ? d.Id : 0;

    private void SelectKundeById(int id)
    {
        foreach (DataGridViewRow row in dgwKunden.Rows)
        {
            if (row.DataBoundItem is OrderKundeListDto d && d.Id == id)
            {
                row.Selected = true;
                var cell = GetFirstVisibleCell(row);
                if (cell is not null) dgwKunden.CurrentCell = cell;
                break;
            }
        }
    }

    private static DataGridViewCell? GetFirstVisibleCell(DataGridViewRow row)
    {
        foreach (DataGridViewCell c in row.Cells)
            if (c.OwningColumn?.Visible == true) return c;
        return null;
    }

    private DataGridViewCell? GetFirstEditableCell(DataGridViewRow row)
    {
        var editableNames = new[] { "Menge", "Gewicht", "Preis", "Notiz" };
        foreach (var name in editableNames)
        {
            if (!dgwPositionen.Columns.Contains(name)) continue;
            var cell = row.Cells[name];
            if (cell.OwningColumn?.Visible == true && !cell.OwningColumn.ReadOnly)
                return cell;
        }
        foreach (DataGridViewCell c in row.Cells)
            if (c.OwningColumn?.Visible == true && !c.OwningColumn.ReadOnly) return c;
        return null;
    }

    private static decimal ParseDecimal(object? val)
    {
        if (val is decimal d) return d;
        if (val is string s && decimal.TryParse(s,
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.CurrentCulture, out var r)) return r;
        return 0;
    }

    // ── Grid / keyboard ───────────────────────────────────────────────────────

    private async void DgwKunden_SelectionChanged(object? s, EventArgs e)
    {
        if (IsDesignMode() || _suppressKundenChanged) return;
        var id = SelectedKundeId();
        ClearRightPanel();
        if (id > 0) await LoadPositionenAsync(id);
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.F5)
        { BtnBuchen_Click(null, EventArgs.Empty); return true; }
        if (keyData == (Keys.Control | Keys.S))
        { BtnSpeichern_Click(null, EventArgs.Empty); return true; }
        if (keyData == Keys.Insert)
        { BtnHinzufuegen_Click(null, EventArgs.Empty); return true; }
        if (keyData == Keys.Delete && dgwPositionen.Focused)
        { DeleteSelectedZeile(); return true; }
        return base.ProcessCmdKey(ref msg, keyData);
    }
}