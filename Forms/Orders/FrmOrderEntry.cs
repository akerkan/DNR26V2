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

    private int          _selectedKundeId;
    private string       _selectedKundename = string.Empty;
    private int?         _currentAuftragId;
    private OrderStatus? _currentStatus;

    // Parameterless constructor for Designer
    public FrmOrderEntry()
    {
        _orderService = null!;
        InitializeComponent();
        WireUpEvents();
    }

    public FrmOrderEntry(IOrderService orderService)
    {
        _orderService = orderService;
        InitializeComponent();
        WireUpEvents();
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
        btnAlle.Click += (_, _) => SelectDay(btnAlle,  null);

        dgwKunden.SelectionChanged += DgwKunden_SelectionChanged;
        dgwKunden.CellFormatting   += DgwKunden_CellFormatting;

        btnBuchen.Click     += BtnBuchen_Click;
        btnSpeichern.Click  += BtnSpeichern_Click;
        btnStornieren.Click += BtnStornieren_Click;
    }

    // ── Load ──────────────────────────────────────────────────────────────────

    private async void FrmOrderEntry_Load(object? sender, EventArgs e)
    {
        if (IsDesignMode() || _orderService is null) return;

        WindowState = FormWindowState.Maximized;
        dtpLieferdatum.Value = DateTime.Today;

        // Auto-select today's weekday
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

    // ── Day toggle buttons ────────────────────────────────────────────────────

    private void SelectDay(Button btn, DayOfWeek? day)
    {
        // Deselect previous
        if (_activeDayBtn is not null)
        {
            _activeDayBtn.BackColor = SystemColors.Control;
            _activeDayBtn.ForeColor = SystemColors.ControlText;
        }

        // Select new
        btn.BackColor  = Color.SteelBlue;
        btn.ForeColor  = Color.White;
        _activeDayBtn  = btn;
        _selectedDay   = day;

        _ = ReloadAsync();
    }

    // ── Reload customer list ──────────────────────────────────────────────────

    private async Task ReloadAsync()
    {
        if (_orderService is null) return;

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

        _suppressKundenChanged = true;
        try
        {
            var prevId = SelectedKundeId();
            dgwKunden.DataSource = null;
            dgwKunden.DataSource = liste.ToList();
            StyleKundenGrid();

            if (prevId > 0) SelectKundeById(prevId);
            else if (dgwKunden.Rows.Count > 0)
            {
                dgwKunden.CurrentCell = GetFirstVisibleCell(dgwKunden.Rows[0]);
            }
        }
        finally { _suppressKundenChanged = false; }

        var id = SelectedKundeId();
        if (id > 0) await LoadPositionenAsync(id);
        else ClearRightPanel();
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

        var row = dgwKunden.Rows[e.RowIndex];
        row.DefaultCellStyle.BackColor = dto.AuftragStatus switch
        {
            OrderStatus.Bestaetigt => Color.FromArgb(200, 255, 200),  // green = booked
            OrderStatus.Offen      => Color.FromArgb(255, 255, 200),  // yellow = saved but not booked
            _                      => SystemColors.Window              // white = no order
        };
    }

    // ── Load order positions ──────────────────────────────────────────────────

    private async Task LoadPositionenAsync(int kundeId)
    {
        var kundeDto = dgwKunden.CurrentRow?.DataBoundItem as OrderKundeListDto;
        _selectedKundeId   = kundeId;
        _selectedKundename = kundeDto?.Kundenname ?? string.Empty;
        _currentAuftragId  = kundeDto?.AuftragId;
        _currentStatus     = kundeDto?.AuftragStatus;

        lblKundenname.Text    = _selectedKundename;
        lblAuftragStatus.Text = _currentStatus switch
        {
            OrderStatus.Bestaetigt => "✓ GEBUCHT",
            OrderStatus.Offen      => "OFFEN",
            _                      => string.Empty
        };
        lblAuftragStatus.ForeColor = _currentStatus switch
        {
            OrderStatus.Bestaetigt => Color.DarkGreen,
            OrderStatus.Offen      => Color.DarkOrange,
            _                      => SystemColors.ControlText
        };
        lblSaldo.Text = string.Empty;

        IReadOnlyList<OrderLineDto> positionen;

        await _lock.WaitAsync();
        try
        {
            positionen = await _orderService.GetPositionenAsync(kundeId, dtpLieferdatum.Value.Date);
        }
        catch (Exception ex) { ShowError($"Ladefehler:\n{ex.Message}"); return; }
        finally { _lock.Release(); }

        dgwPositionen.Rows.Clear();
        foreach (var pos in positionen)
        {
            var idx = dgwPositionen.Rows.Add();
            var row = dgwPositionen.Rows[idx];
            row.Cells["_ZeileId"].Value    = pos.OrderLineId;
            row.Cells["_ArtikelId"].Value  = pos.ArtikelId;
            row.Cells["Artikelnummer"].Value = pos.Artikelnummer;
            row.Cells["Produktname"].Value = pos.Produktname;
            row.Cells["Menge"].Value       = pos.Menge;
            row.Cells["Gewicht"].Value     = pos.Gewicht;
            row.Cells["Preis"].Value       = pos.Preis;
            row.Cells["Notiz"].Value       = pos.Notiz;
        }

        // Read-only if already booked
        bool editable = _currentStatus is null or OrderStatus.Offen;
        dgwPositionen.ReadOnly = !editable;
        btnBuchen.Enabled      = editable;
        btnSpeichern.Enabled   = editable;
        btnStornieren.Enabled  = _currentStatus is OrderStatus.Offen or OrderStatus.Bestaetigt;
    }

    private void ClearRightPanel()
    {
        lblKundenname.Text    = "— Kunden auswählen —";
        lblSaldo.Text         = string.Empty;
        lblAuftragStatus.Text = string.Empty;
        lblStatusInfo.Text    = string.Empty;
        dgwPositionen.Rows.Clear();
        _selectedKundeId  = 0;
        _currentAuftragId = null;
        _currentStatus    = null;
    }

    // ── Collect grid rows as input tuples ─────────────────────────────────────

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

    // ── Save (without booking) ────────────────────────────────────────────────

    private async void BtnSpeichern_Click(object? s, EventArgs e)
    {
        if (_selectedKundeId <= 0) return;

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
        catch (Exception ex)           { ShowError($"Fehler:\n{ex.Message}"); }
    }

    // ── Buchen = Save + Book + Create Delivery + Advance to next ──────────────

    private async void BtnBuchen_Click(object? s, EventArgs e)
    {
        if (_selectedKundeId <= 0) return;

        try
        {
            await _lock.WaitAsync();
            try
            {
                // Save first (creates order if not exists)
                var order = await _orderService.SaveAuftragAsync(
                    _selectedKundeId,
                    dtpLieferdatum.Value.Date,
                    CollectPositionen());

                // Book → creates delivery
                await _orderService.BuchenAsync(order.Id);
            }
            finally { _lock.Release(); }

            lblStatusInfo.Text = $"✓ Auftrag gebucht — Lieferschein erstellt.";

            // Reload list and advance to next unbooked customer
            var prevKundeId = _selectedKundeId;
            await ReloadAsync();
            AdvanceToNextUnbooked(prevKundeId);
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

    // ── Advance to next unbooked customer ─────────────────────────────────────

    private void AdvanceToNextUnbooked(int justBookedKundeId)
    {
        int startIdx = 0;

        // Find the row we just booked
        for (int i = 0; i < dgwKunden.Rows.Count; i++)
        {
            if (dgwKunden.Rows[i].DataBoundItem is OrderKundeListDto d && d.Id == justBookedKundeId)
            {
                startIdx = i + 1;
                break;
            }
        }

        // Find next unbooked row (wrap around)
        for (int offset = 0; offset < dgwKunden.Rows.Count; offset++)
        {
            int idx = (startIdx + offset) % dgwKunden.Rows.Count;
            if (dgwKunden.Rows[idx].DataBoundItem is OrderKundeListDto dto
                && dto.AuftragStatus is null)
            {
                dgwKunden.CurrentCell = GetFirstVisibleCell(dgwKunden.Rows[idx]);
                return;
            }
        }

        // All booked — stay on current
        if (dgwKunden.Rows.Count > 0)
            SelectKundeById(justBookedKundeId);
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
        if (val is string s && decimal.TryParse(
                s, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.CurrentCulture, out var r)) return r;
        return 0;
    }

    // ── Customer grid events ──────────────────────────────────────────────────

    private async void DgwKunden_SelectionChanged(object? s, EventArgs e)
    {
        if (IsDesignMode() || _suppressKundenChanged) return;
        var id = SelectedKundeId();
        if (id > 0) await LoadPositionenAsync(id);
        else ClearRightPanel();
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.F5) { BtnBuchen_Click(null, EventArgs.Empty); return true; }
        if (keyData == (Keys.Control | Keys.S)) { BtnSpeichern_Click(null, EventArgs.Empty); return true; }
        return base.ProcessCmdKey(ref msg, keyData);
    }
}