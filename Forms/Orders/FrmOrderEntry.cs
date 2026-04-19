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
    private Button?    _activeDayBtn;
    private bool       _suppressKundenChanged;
    private bool       _isLoading;

    private int          _selectedKundeId;
    private string       _selectedKundename = string.Empty;
    private bool         _preisAusblenden;
    private int?         _currentAuftragId;
    private OrderStatus? _currentStatus;

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
        colZeileId      .Name = "_ZeileId";
        colArtikelId    .Name = "_ArtikelId";
        colArtikelnummer.Name = "Artikelnummer";
        colProduktname  .Name = "Produktname";
        colMenge        .Name = "Menge";
        colGewicht      .Name = "Gewicht";
        colPreis        .Name = "Preis";
        colNotiz        .Name = "Notiz";
    }

    private static bool IsDesignMode() =>
        LicenseManager.UsageMode == LicenseUsageMode.Designtime;

    // ── Events ────────────────────────────────────────────────────────────────

    private void WireUpEvents()
    {
        Load += FrmOrderEntry_Load;

        dtpLieferdatum.ValueChanged += async (_, _) => await ReloadAsync();

        btnMo.Click   += (_, _) => SelectDay(btnMo,   DayOfWeek.Monday);
        btnDi.Click   += (_, _) => SelectDay(btnDi,   DayOfWeek.Tuesday);
        btnMi.Click   += (_, _) => SelectDay(btnMi,   DayOfWeek.Wednesday);
        btnDo.Click   += (_, _) => SelectDay(btnDo,   DayOfWeek.Thursday);
        btnFr.Click   += (_, _) => SelectDay(btnFr,   DayOfWeek.Friday);
        btnSa.Click   += (_, _) => SelectDay(btnSa,   DayOfWeek.Saturday);
        btnSo.Click   += (_, _) => SelectDay(btnSo,   DayOfWeek.Sunday);
        btnAlle.Click += (_, _) => SelectDay(btnAlle, null);

        dgwKunden.SelectionChanged += DgwKunden_SelectionChanged;
        dgwKunden.CellFormatting   += DgwKunden_CellFormatting;

        dgwPositionen.EditingControlShowing += DgwPositionen_EditingControlShowing;
        dgwPositionen.KeyDown += DgwPositionen_KeyDown;

        btnBuchen.Click        += BtnBuchen_Click;
        btnFreigeben.Click     += BtnFreigeben_Click;
        btnSpeichern.Click     += BtnSpeichern_Click;
        btnHinzufuegen.Click   += BtnHinzufuegen_Click;
        btnNachlieferung.Click += BtnNachlieferung_Click;
        btnStornieren.Click    += BtnStornieren_Click;
        btnLoeschen.Click      += BtnLoeschen_Click;

        cmsPositionen.Opening          += CmsPositionen_Opening;
        cmsMenuZeileLoeschen.Click     += (_, _) => DeleteSelectedZeile();
        cmsMenuHinzufuegen.Click       += BtnHinzufuegen_Click;
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
            DayOfWeek.Monday    => btnMo,
            DayOfWeek.Tuesday   => btnDi,
            DayOfWeek.Wednesday => btnMi,
            DayOfWeek.Thursday  => btnDo,
            DayOfWeek.Friday    => btnFr,
            DayOfWeek.Saturday  => btnSa,
            DayOfWeek.Sunday    => btnSo,
            _                   => btnAlle
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
        _selectedDay  = day;

        _ = ReloadAsync();
    }

    // ── Reload customer list ──────────────────────────────────────────────────

    private async Task ReloadAsync()
    {
        if (_orderService is null || _isLoading) return;

        IReadOnlyList<OrderKundeListDto> liste;

        await _lock.WaitAsync();
        try
        {
            Cursor = Cursors.WaitCursor;
            liste  = await _orderService.GetKundenListeAsync(
                dtpLieferdatum.Value.Date, _selectedDay);
        }
        catch (Exception ex) { ShowError($"Ladefehler:\n{ex.Message}"); return; }
        finally { Cursor = Cursors.Default; _lock.Release(); }

        _suppressKundenChanged = true;
        try
        {
            var prevId = SelectedKundeId();
            dgwKunden.DataSource = null;
            dgwKunden.DataSource = liste.ToList();
            StyleKundenGrid();

            if (prevId > 0) SelectKundeById(prevId);
            else if (dgwKunden.Rows.Count > 0)
                dgwKunden.CurrentCell = GetFirstVisibleCell(dgwKunden.Rows[0]);
        }
        finally { _suppressKundenChanged = false; }

        var id = SelectedKundeId();
        if (id > 0) await LoadPositionenAsync(id);
        else        ClearRightPanel();
    }

    private void StyleKundenGrid()
    {
        if (dgwKunden.Columns.Count == 0) return;
        foreach (DataGridViewColumn c in dgwKunden.Columns) c.Visible = false;
        ShowKol("Kundenname", "Kunde", 0, fill: true);
        ShowKol("Tur",        "Tour",  55);
    }

    private void ShowKol(string name, string header, int width, bool fill = false)
    {
        if (!dgwKunden.Columns.Contains(name)) return;
        var col = dgwKunden.Columns[name];
        col.Visible    = true;
        col.HeaderText = header;
        if (fill) col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        else { col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; col.Width = width; }
    }

    // ── Customer grid color coding ────────────────────────────────────────────

    private void DgwKunden_CellFormatting(object? s, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0) return;
        if (dgwKunden.Rows[e.RowIndex].DataBoundItem is not OrderKundeListDto dto) return;

        dgwKunden.Rows[e.RowIndex].DefaultCellStyle.BackColor = dto.AuftragStatus switch
        {
            OrderStatus.Gebucht     => Color.FromArgb(200, 255, 200),   // green
            OrderStatus.Freigegeben => Color.FromArgb(200, 230, 255),   // light blue
            OrderStatus.Offen       => Color.FromArgb(255, 255, 200),   // yellow
            OrderStatus.Storniert   => Color.FromArgb(240, 240, 240),   // grey
            _                       => SystemColors.Window
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

        _selectedKundeId   = kundeId;
        _selectedKundename = kundeDto?.Kundenname      ?? string.Empty;
        _preisAusblenden   = kundeDto?.PreisAusblenden ?? false;
        _currentAuftragId  = kundeDto?.AuftragId;
        _currentStatus     = kundeDto?.AuftragStatus;

        lblKundenname.Text    = _selectedKundename;
        lblAuftragStatus.Text = _currentStatus switch
        {
            OrderStatus.Gebucht     => "✓ GEBUCHT",
            OrderStatus.Freigegeben => "FREIGEGEBEN",
            OrderStatus.Offen       => "OFFEN",
            OrderStatus.Storniert   => "STORNIERT",
            OrderStatus.Geloescht   => "GELÖSCHT",
            _                       => string.Empty
        };
        lblAuftragStatus.ForeColor = _currentStatus switch
        {
            OrderStatus.Gebucht     => Color.DarkGreen,
            OrderStatus.Freigegeben => Color.SteelBlue,
            OrderStatus.Offen       => Color.DarkOrange,
            OrderStatus.Storniert   => Color.Gray,
            _                       => SystemColors.ControlText
        };

        // Load positions and FactBox in parallel
        IReadOnlyList<OrderLineDto> positionen;
        KundenFactBoxDto?            factBox;

        await _lock.WaitAsync();
        try
        {
            var posTask = _orderService.GetPositionenAsync(kundeId, dtpLieferdatum.Value.Date);
            var fbTask  = _orderService.GetKundenFactBoxAsync(kundeId);
            await Task.WhenAll(posTask, fbTask);
            positionen = posTask.Result;
            factBox    = fbTask.Result;
        }
        catch (Exception ex) { ShowError($"Ladefehler:\n{ex.Message}"); return; }
        finally { _lock.Release(); }

        UpdateFactBox(factBox);

        dgwPositionen.Rows.Clear();
        foreach (var pos in positionen)
        {
            var idx = dgwPositionen.Rows.Add();
            var row = dgwPositionen.Rows[idx];
            row.Cells["_ZeileId"]    .Value = pos.OrderLineId;
            row.Cells["_ArtikelId"]  .Value = pos.ArtikelId;
            row.Cells["Artikelnummer"].Value = pos.Artikelnummer;
            row.Cells["Produktname"] .Value = pos.Produktname;
            row.Cells["Menge"]       .Value = pos.Menge;
            row.Cells["Gewicht"]     .Value = pos.Gewicht;
            row.Cells["Preis"]       .Value = pos.Preis;
            row.Cells["Notiz"]       .Value = pos.Notiz;
        }

        colPreis.Visible = !_preisAusblenden;

        // ── Editability ───────────────────────────────────────────────────────
        bool istOffen       = _currentStatus is null or OrderStatus.Offen;
        bool istFreigeben   = _currentStatus == OrderStatus.Freigegeben;
        bool istGebucht     = _currentStatus == OrderStatus.Gebucht;

        // Configure editable columns: only Menge, Gewicht, Preis, Notiz editable when order is Offen
        btnSpeichern.Enabled        = istOffen;
        btnHinzufuegen.Enabled      = istOffen;
        // Ensure grid overall is editable so per-column ReadOnly works
        dgwPositionen.ReadOnly = false;
        // set columns readonly state
        foreach (DataGridViewColumn c in dgwPositionen.Columns)
        {
            if (c.Name is null) { c.ReadOnly = true; continue; }
            var editable = istOffen && (c.Name == "Menge" || c.Name == "Gewicht" || c.Name == "Preis" || c.Name == "Notiz");
            c.ReadOnly = !editable;
        }
        btnFreigeben.Enabled        = istOffen && _currentAuftragId > 0;
        btnFreigeben.Visible        = istOffen;
        btnBuchen.Enabled           = istOffen || istFreigeben;
        btnLoeschen.Enabled         = istOffen && _currentAuftragId > 0;
        btnLoeschen.Visible         = istOffen;
        btnStornieren.Enabled       = istFreigeben || istGebucht;
        btnStornieren.Visible       = istFreigeben || istGebucht;
        btnNachlieferung.Enabled    = istGebucht;
        btnNachlieferung.Visible    = istGebucht;
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
            {
                // let editing control handler manage
                return;
            }
            MoveToNextEditableCell();
        }
    }

    private void MoveToNextEditableCell()
    {
        if (dgwPositionen.CurrentCell is null) return;
        int row = dgwPositionen.CurrentCell.RowIndex;
        int col = dgwPositionen.CurrentCell.ColumnIndex;

        // list of editable column names in display order
        var editableNames = new[] { "Menge", "Gewicht", "Preis", "Notiz" };

        // find current column name
        var curName = dgwPositionen.CurrentCell.OwningColumn?.Name;
        if (curName is null) return;

        // if current is last editable column -> move to next row Menge
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

        // otherwise move to next editable column in same row
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

        // fallback: if none found, go to Menge of next row
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
            lblFBTour.Text            = string.Empty;
            lblFBSaldo.Text           = string.Empty;
            lblFBLetzterAuftrag.Text  = string.Empty;
            lblFBOffeneAuftraege.Text = string.Empty;
            lblFBSaldoCaption.Visible = true;
            lblFBSaldo.Visible        = true;
            return;
        }

        lblFBTour.Text = fb.Tour ?? "—";

        bool showSaldo            = !fb.PreisAusblenden;
        lblFBSaldoCaption.Visible = showSaldo;
        lblFBSaldo.Visible        = showSaldo;

        if (showSaldo)
        {
            lblFBSaldo.Text      = $"{fb.Saldo:N2} €";
            lblFBSaldo.ForeColor = fb.Saldo < 0 ? Color.Crimson : Color.DarkGreen;
        }

        lblFBLetzterAuftrag.Text = fb.LetzterAuftrag.HasValue
            ? fb.LetzterAuftrag.Value.ToString("dd.MM.yyyy")
            : "—";

        lblFBOffeneAuftraege.Text      = fb.OffeneAuftraege.ToString();
        lblFBOffeneAuftraege.ForeColor = fb.OffeneAuftraege > 0
            ? Color.DarkOrange
            : Color.DarkGreen;
    }

    private void ClearRightPanel()
    {
        lblKundenname.Text    = "— Kunden auswählen —";
        lblAuftragStatus.Text = string.Empty;
        lblStatusInfo.Text    = string.Empty;
        dgwPositionen.Rows.Clear();
        UpdateFactBox(null);
        colPreis.Visible         = true;
        _selectedKundeId         = 0;
        _preisAusblenden         = false;
        _currentAuftragId        = null;
        _currentStatus           = null;
        dgwPositionen.ReadOnly   = true;
        btnBuchen.Enabled        = false;
        btnSpeichern.Enabled     = false;
        btnHinzufuegen.Enabled   = false;
        btnFreigeben.Enabled     = false;
        btnFreigeben.Visible     = true;
        btnLoeschen.Enabled      = false;
        btnLoeschen.Visible      = true;
        btnStornieren.Enabled    = false;
        btnStornieren.Visible    = false;
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
        row.Cells["_ZeileId"]    .Value = 0;
        row.Cells["_ArtikelId"]  .Value = art.ArtikelId;
        row.Cells["Artikelnummer"].Value = art.Artikelnummer;
        row.Cells["Produktname"] .Value = art.Produktname;
        row.Cells["Menge"]       .Value = 0m;
        row.Cells["Gewicht"]     .Value = 0m;
        row.Cells["Preis"]       .Value = art.VKPreis;
        row.Cells["Notiz"]       .Value = string.Empty;

        dgwPositionen.CurrentCell = row.Cells["Menge"];
        dgwPositionen.BeginEdit(true);
    }

    // ── Zeile löschen (Context menu + Delete key) ─────────────────────────────

    private void DeleteSelectedZeile()
    {
        if (dgwPositionen.CurrentRow is not DataGridViewRow row) return;
        if (row.IsNewRow) return;
        dgwPositionen.Rows.Remove(row);
    }

    // ── Context menu ──────────────────────────────────────────────────────────

    private void CmsPositionen_Opening(object? s, CancelEventArgs e)
    {
        bool istOffen  = _currentStatus is null or OrderStatus.Offen;
        bool hatZeile  = dgwPositionen.CurrentRow is not null && !dgwPositionen.CurrentRow.IsNewRow;
        cmsMenuZeileLoeschen.Enabled = istOffen && hatZeile;
        cmsMenuHinzufuegen.Enabled   = istOffen && _selectedKundeId > 0;
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
                ParseDecimal(row.Cells["Menge"]  .Value),
                ParseDecimal(row.Cells["Gewicht"] .Value),
                ParseDecimal(row.Cells["Preis"]   .Value),
                row.Cells["Notiz"].Value?.ToString()
            ));
        }
        return result;
    }

    // ── Speichern ─────────────────────────────────────────────────────────────

    private async void BtnSpeichern_Click(object? s, EventArgs e)
    {
        if (_selectedKundeId <= 0) return;

        // Validate lines before calling service
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
                _currentStatus    = order.Status;
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
        catch (Exception ex)           { ShowError($"Fehler:\n{ex.Message}"); }
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

    // ── Advance to next unbooked (async — explicit navigation avoids race) ────

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
                // Suppress SelectionChanged → we load explicitly below
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

        // No unbooked customer found → stay on current selection
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
        catch (Exception ex)           { ShowError($"Fehler:\n{ex.Message}"); }
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
        catch (Exception ex)           { ShowError($"Fehler:\n{ex.Message}"); }
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
        catch (Exception ex)           { ShowError($"Fehler:\n{ex.Message}"); }
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
        // clear right panel immediately to avoid stale status display
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