using System.ComponentModel;
using DNR26V2.Domain.DTOs;
using DNR26V2.Domain.DTOs.Payments;
using DNR26V2.Forms.Base;
using DNR26V2.Services.MasterData;
using DNR26V2.Services.Payments;

namespace DNR26V2.Forms.Payments;

public partial class FrmZahlungseingaenge : BaseListForm
{
    // ?? Column-name constants ?????????????????????????????????????????????????
    private const string ColDatum    = "colDatum";
    private const string ColBelegNr  = "colBelegNr";
    private const string ColBelegArt = "colBelegArt";
    private const string ColBetrag   = "colBetrag";
    private const string ColOffen    = "colOffen";
    private const string ColBuchungsdatum   = "colBuchungsdatum";
    private const string ColBar      = "colBar";
    private const string ColBank     = "colBank";
    private const string ColNotiz    = "colNotiz";

    // ?? Hidden data-binding columns (not visible) ?????????????????????????????
    private const string ColReferenzId   = "colReferenzId";
    private const string ColReferenceType = "colReferenceType";

    private readonly ICustomerService       _customerService;
    private readonly ICustomerLedgerService _ledgerService;
    private readonly IPaymentPostingService _postingService;

    private List<CustomerListDto>       _alleKunden    = [];
    private List<CustomerLedgerEntryDto> _ledgerEntries = [];
    private int? _selectedKundeId;

    // neue Feldvariable in der Klasse
    private bool _suppressKundeSelectionChanged;

    public FrmZahlungseingaenge()
    {
        // Designer-only ctor
        _customerService = null!;
        _ledgerService   = null!;
        _postingService  = null!;
        InitializeComponent();
    }

    public FrmZahlungseingaenge(
        ICustomerService       customerService,
        ICustomerLedgerService ledgerService,
        IPaymentPostingService postingService)
    {
        _customerService = customerService;
        _ledgerService   = ledgerService;
        _postingService  = postingService;
        InitializeComponent();
        WireUpEvents();
    }

    private static bool IsDesignMode()
        => LicenseManager.UsageMode == LicenseUsageMode.Designtime;

    // ?? Wire-up ???????????????????????????????????????????????????????????????

    private void WireUpEvents()
    {
        Load += async (_, _) => await OnLoadAsync();

        txtKundeSearch.TextChanged += TxtKundeSearch_TextChanged;
        lstKunden.SelectedIndexChanged += async (_, _) =>
        {
            if (_suppressKundeSelectionChanged) return;
            await OnKundeSelectedAsync();
        };
        btnLaden.Click += async (_, _) => await LoadLedgerAsync();
        btnBuchen.Click += async (_, _) => await BuchenAsync();

        dgwLedger.CellValidating += DgwLedger_CellValidating;
        dgwLedger.CellEndEdit += DgwLedger_CellEndEdit;
    }

    // ?? Load ??????????????????????????????????????????????????????????????????

    private async Task OnLoadAsync()
    {
        if (IsDesignMode()) return;

        dtpVon.Value           = new DateTime(DateTime.Today.Year, 1, 1);
        dtpBis.Value           = DateTime.Today;

        BuildGrid();

        _alleKunden = (await _customerService.GetListAsync(new CustomerListFilter { NurAktiv = true })).ToList();
        FilterKunden(string.Empty);
    }

    // ?? Customer list / filter ????????????????????????????????????????????????

    private void TxtKundeSearch_TextChanged(object? sender, EventArgs e)
        => FilterKunden(txtKundeSearch.Text);

    private void FilterKunden(string filter)
    {
        var filtered = string.IsNullOrWhiteSpace(filter)
            ? _alleKunden
            : _alleKunden
                .Where(k => k.Kundenname.Contains(filter, StringComparison.OrdinalIgnoreCase)
                         || k.Kundennummer.Contains(filter, StringComparison.OrdinalIgnoreCase))
                .ToList();

        lstKunden.DataSource    = null;
        lstKunden.DataSource    = filtered;
        lstKunden.DisplayMember = nameof(CustomerListDto.Kundenname);
    }

    private async Task OnKundeSelectedAsync()
    {
        if (lstKunden.SelectedItem is not CustomerListDto kunde) return;

        if (_selectedKundeId.HasValue
            && kunde.Id != _selectedKundeId.Value
            && HasPendingPaymentInput()
            && !Confirm("Es gibt nicht gebuchte Beträge (Bar/Bank). Änderungen verwerfen und Kunde wechseln?"))
        {
            RestoreCustomerSelection(_selectedKundeId.Value);
            return;
        }

        _selectedKundeId = kunde.Id;
        await LoadLedgerAsync();
    }

    private bool HasPendingPaymentInput()
    {
        return dgwLedger.Rows
            .Cast<DataGridViewRow>()
            .Any(r => GetDecimal(r, ColBar) > 0m || GetDecimal(r, ColBank) > 0m);
    }

    private void RestoreCustomerSelection(int kundeId)
    {
        _suppressKundeSelectionChanged = true;
        try
        {
            foreach (var item in lstKunden.Items)
            {
                if (item is CustomerListDto kunde && kunde.Id == kundeId)
                {
                    lstKunden.SelectedItem = item;
                    break;
                }
            }
        }
        finally
        {
            _suppressKundeSelectionChanged = false;
        }
    }


    // ?? Ledger load ???????????????????????????????????????????????????????????

    private async Task LoadLedgerAsync()
    {
        if (_selectedKundeId is null) return;

        try
        {
            Cursor = Cursors.WaitCursor;
            _ledgerEntries = (await _ledgerService.GetLedgerAsync(
                _selectedKundeId.Value,
                dtpVon.Value.Date,
                dtpBis.Value.Date)).ToList();

            await UpdateSaldoAsync();
            await LoadPaymentHistoryAsync();
        }
        catch (Exception ex) { ShowError($"Ladefehler:\n{ex.Message}"); return; }
        finally { Cursor = Cursors.Default; }

        PopulateGrid();
        btnBuchen.Enabled = false;
    }

    private async Task UpdateSaldoAsync()
    {
        if (_selectedKundeId is null) return;
        var saldo = await _ledgerService.GetSaldoAsync(
            _selectedKundeId.Value,
            dtpVon.Value.Date,
            dtpBis.Value.Date);

        lblSaldo.Text      = $"Saldo: {saldo:N2} €";
        lblSaldo.ForeColor = saldo > 0 ? Color.DarkRed : Color.DarkGreen;
    }

    private async Task LoadPaymentHistoryAsync()
    {
        if (_selectedKundeId is null) return;

        var history = await _ledgerService.GetPaymentHistoryAsync(
            _selectedKundeId.Value,
            dtpVon.Value.Date,
            dtpBis.Value.Date);

        BuildHistoryGrid();
        dgwHistory.Rows.Clear();
        foreach (var h in history)
        {
            var idx = dgwHistory.Rows.Add(
                h.PaymentLineId,
                h.Zahlungsnummer,
                h.Buchungsdatum.ToString("dd.MM.yyyy"),
                h.BelegNr,
                h.Zahlungsart,
                h.Amount,
                h.Notiz ?? string.Empty,
                h.ErstelltVon);

            // Storno rows in grey italic
            if (h.IsStorno)
            {
                var r = dgwHistory.Rows[idx];
                r.DefaultCellStyle.ForeColor = Color.Gray;
                r.DefaultCellStyle.Font      = new Font(dgwHistory.Font, FontStyle.Italic);
            }
        }
    }

    // ?? Grid ?????????????????????????????????????????????????????????????????

    private void BuildGrid()
    {
        dgwLedger.Columns.Clear();

        // Hidden key columns
        dgwLedger.Columns.Add(new DataGridViewTextBoxColumn
            { Name = ColReferenzId, Visible = false });
        dgwLedger.Columns.Add(new DataGridViewTextBoxColumn
            { Name = ColReferenceType, Visible = false });

        // Read-only display columns
        dgwLedger.Columns.Add(new DataGridViewTextBoxColumn
            { Name = ColDatum,    HeaderText = "Datum",    ReadOnly = true, Width = 90 });
        dgwLedger.Columns.Add(new DataGridViewTextBoxColumn
            { Name = ColBelegNr,  HeaderText = "Beleg-Nr", ReadOnly = true, Width = 110 });
        dgwLedger.Columns.Add(new DataGridViewTextBoxColumn
            { Name = ColBelegArt, HeaderText = "Art",      ReadOnly = true, Width = 80 });
        dgwLedger.Columns.Add(new DataGridViewTextBoxColumn
            { Name = ColBetrag,   HeaderText = "Betrag",   ReadOnly = true, Width = 90,
              DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2" } });
        dgwLedger.Columns.Add(new DataGridViewTextBoxColumn
            { Name = ColOffen,    HeaderText = "Offen",    ReadOnly = true, Width = 90,
              DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2" } });

        // Buchungsdatum — DateTimePicker per row
        dgwLedger.Columns.Add(new DataGridViewCalendarColumn
            { Name = ColBuchungsdatum, HeaderText = "Buchungsdatum", ReadOnly = false, Width = 110 });

        // Editable payment columns
        dgwLedger.Columns.Add(new DataGridViewTextBoxColumn
            { Name = ColBar,  HeaderText = "Bar",  ReadOnly = false, Width = 85,
              DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2" } });
        dgwLedger.Columns.Add(new DataGridViewTextBoxColumn
            { Name = ColBank, HeaderText = "Bank", ReadOnly = false, Width = 85,
              DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2" } });
        dgwLedger.Columns.Add(new DataGridViewTextBoxColumn
            { Name = ColNotiz, HeaderText = "Notiz", ReadOnly = false, Width = 160 });
    }

    private void BuildHistoryGrid()
    {
        if (dgwHistory.Columns.Count > 0) return; // built once

        dgwHistory.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "hLineId",      Visible = false });                                  // hidden key
        dgwHistory.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "hZahlungsNr",  HeaderText = "Zahlungs-Nr",  Width = 120, ReadOnly = true });
        dgwHistory.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "hDatum",       HeaderText = "Datum",         Width = 90,  ReadOnly = true });
        dgwHistory.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "hBelegNr",     HeaderText = "Beleg-Nr",      Width = 110, ReadOnly = true });
        dgwHistory.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "hZahlungsart", HeaderText = "Zahlungsart",   Width = 80,  ReadOnly = true });
        dgwHistory.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "hBetrag",      HeaderText = "Betrag",        Width = 90,  ReadOnly = true,
              DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2" } });
        dgwHistory.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "hNotiz",       HeaderText = "Notiz",         Width = 160, ReadOnly = true });
        dgwHistory.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "hBenutzer",    HeaderText = "Benutzer",      Width = 100, ReadOnly = true });

        // Context menu
        var ctxHistory = new ContextMenuStrip();
        var miStorno   = new ToolStripMenuItem("Zeile stornieren");
        miStorno.Click += MiStorno_Click;
        ctxHistory.Items.Add(miStorno);
        dgwHistory.ContextMenuStrip = ctxHistory;
    }

    private void PopulateGrid()
    {
        dgwLedger.Rows.Clear();

        foreach (var entry in _ledgerEntries)
        {
            bool payable = entry.OffenerBetrag > 0 && entry.BelegArt != "Gutschrift";

            var row = new DataGridViewRow();
            row.CreateCells(dgwLedger,
                entry.ReferenzId,
                ResolveReferenceType(entry.BelegArt),
                entry.Datum.ToString("dd.MM.yyyy"),
                entry.BelegNr,
                entry.BelegArt,
                entry.Betrag,
                entry.OffenerBetrag,
                DateTime.Today,         // Buchungsdatum — DateTime for CalendarColumn
                0m,   // Bar
                0m,   // Bank
                entry.Notiz ?? string.Empty);

            dgwLedger.Rows.Add(row);

            if (!payable)
            {
                row.DefaultCellStyle.ForeColor        = Color.Gray;
                row.Cells[ColBuchungsdatum].ReadOnly  = true;
                row.Cells[ColBar  ].ReadOnly          = true;
                row.Cells[ColBank ].ReadOnly          = true;
                row.Cells[ColNotiz].ReadOnly          = true;
            }
        }
    }

    private static int ResolveReferenceType(string belegArt)
        => belegArt.Equals("Lieferschein", StringComparison.OrdinalIgnoreCase) ? 1 : 0;

    // ?? Cell validation — Bar + Bank ? Offen ?????????????????????????????????

    private void DgwLedger_CellValidating(object? sender, DataGridViewCellValidatingEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var colName = dgwLedger.Columns[e.ColumnIndex].Name;
        if (colName is not (ColBar or ColBank)) return;

        var raw = e.FormattedValue?.ToString() ?? "0";
        if (!decimal.TryParse(raw, out var value))
        {
            e.Cancel = true;
            ShowError("Bitte einen gültigen Betrag eingeben.");
            return;
        }

        if (value < 0)
        {
            e.Cancel = true;
            ShowError("Negative Beträge sind nicht erlaubt.");
            return;
        }

        var row   = dgwLedger.Rows[e.RowIndex];
        var offen = GetDecimal(row, ColOffen);

        decimal bar  = colName == ColBar  ? value : GetDecimal(row, ColBar);
        decimal bank = colName == ColBank ? value : GetDecimal(row, ColBank);

        if (bar + bank > offen)
        {
            e.Cancel = true;
            ShowError($"Bar + Bank ({bar + bank:N2}) übersteigt den offenen Betrag ({offen:N2}).");
        }
    }

    private void DgwLedger_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var colName = dgwLedger.Columns[e.ColumnIndex].Name;
        if (colName is not (ColBar or ColBank)) return;

        bool anyAmount = dgwLedger.Rows
            .Cast<DataGridViewRow>()
            .Any(r => GetDecimal(r, ColBar) > 0 || GetDecimal(r, ColBank) > 0);

        btnBuchen.Enabled = anyAmount;
    }

    // ?? Buchen ????????????????????????????????????????????????????????????????

    private async Task BuchenAsync()
    {
        if (_selectedKundeId is null) return;

        // Commit any active cell edit
        dgwLedger.CommitEdit(DataGridViewDataErrorContexts.Commit);

        var rows = dgwLedger.Rows
            .Cast<DataGridViewRow>()
            .Where(r => GetDecimal(r, ColBar) > 0 || GetDecimal(r, ColBank) > 0)
            .Select(r => new PaymentPostingRowItem
            {
                ReferenceId   = (int)r.Cells[ColReferenzId].Value,
                ReferenceType = (int)r.Cells[ColReferenceType].Value,
                BelegArt      = r.Cells[ColBelegArt].Value?.ToString() ?? string.Empty,
                OffenerBetrag = GetDecimal(r, ColOffen),
                Bar           = GetDecimal(r, ColBar),
                Bank          = GetDecimal(r, ColBank),
                Notiz         = r.Cells[ColNotiz].Value?.ToString()
            })
            .ToList();

        if (rows.Count == 0) return;

        // Use the Buchungsdatum from the first active row (all rows share one header per booking).
        var firstRow      = dgwLedger.Rows.Cast<DataGridViewRow>()
                                .First(r => GetDecimal(r, ColBar) > 0 || GetDecimal(r, ColBank) > 0);
        var buchungsdatum = firstRow.Cells[ColBuchungsdatum].Value is DateTime bd
                                ? bd.Date
                                : DateTime.Today;

        var request = new PaymentPostingRequest
        {
            KundeId       = _selectedKundeId.Value,
            Buchungsdatum = buchungsdatum,
            Rows          = rows
        };

        try
        {
            Cursor = Cursors.WaitCursor;
            btnBuchen.Enabled = false;
            await _postingService.BuchenAsync(request);
        }
        catch (Exception ex) { ShowError($"Buchungsfehler:\n{ex.Message}"); return; }
        finally { Cursor = Cursors.Default; }

        MessageBox.Show("Zahlung wurde erfolgreich gebucht.", "Buchung",
            MessageBoxButtons.OK, MessageBoxIcon.Information);

        // Reload so open amounts update
        await LoadLedgerAsync();
    }

    // ?? Storno ????????????????????????????????????????????????????????????????

    private async void MiStorno_Click(object? sender, EventArgs e)
    {
        if (dgwHistory.CurrentRow is not DataGridViewRow row) return;

        // Block storno of already-storno rows
        if (row.Cells["hBetrag"].Value is decimal amt && amt < 0)
        {
            ShowError("Storno-Zeilen können nicht erneut storniert werden.");
            return;
        }

        var lineIdVal = row.Cells["hLineId"].Value;
        if (lineIdVal is not int paymentLineId || paymentLineId <= 0) return;

        var zahlungsNr = row.Cells["hZahlungsNr"].Value?.ToString() ?? string.Empty;

        var notiz = Microsoft.VisualBasic.Interaction.InputBox(
            $"Stornogrund für Zahlung {zahlungsNr}:",
            "Zeile stornieren", "");

        if (string.IsNullOrWhiteSpace(notiz)) return; // user cancelled

        try
        {
            Cursor = Cursors.WaitCursor;
            await _postingService.StornierenAsync(paymentLineId, notiz);
        }
        catch (Exception ex) { ShowError($"Stornofehler:\n{ex.Message}"); return; }
        finally { Cursor = Cursors.Default; }

        await LoadLedgerAsync();
    }

    // ?? Helpers ???????????????????????????????????????????????????????????????

    private static decimal GetDecimal(DataGridViewRow row, string colName)
    {
        var val = row.Cells[colName].Value;
        if (val is decimal d) return d;
        if (val is string s && decimal.TryParse(s, out var parsed)) return parsed;
        return 0m;
    }
}
