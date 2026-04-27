using System.ComponentModel;
using DNR26V2.Domain.DTOs;
using DNR26V2.Domain.DTOs.Payments;
using DNR26V2.Forms.Base;
using DNR26V2.Services.MasterData;
using DNR26V2.Services.Payments;

namespace DNR26V2.Forms.Payments;

public partial class FrmKundenkonto : BaseListForm
{
    private readonly ICustomerService _customerService;
    private readonly ICustomerAccountService _customerAccountService;

    private List<CustomerListDto> _allCustomers = [];
    private bool _suppressCustomerSelectionChanged;

    public FrmKundenkonto()
    {
        _customerService = null!;
        _customerAccountService = null!;
        InitializeComponent();
    }

    public FrmKundenkonto(
        ICustomerService customerService,
        ICustomerAccountService customerAccountService)
    {
        _customerService = customerService;
        _customerAccountService = customerAccountService;

        InitializeComponent();
        WireUpEvents();
    }

    private static bool IsDesignMode()
        => LicenseManager.UsageMode == LicenseUsageMode.Designtime;

    private void WireUpEvents()
    {
        Load += async (_, _) => await OnLoadAsync();
        txtKundeSuche.TextChanged += (_, _) => FilterCustomers();
        dgwKunden.SelectionChanged += async (_, _) =>
        {
            if (_suppressCustomerSelectionChanged) return;
            await OnKundeGridSelectionChangedAsync();
        };
        btnLaden.Click += async (_, _) => await LoadKundenkontoAsync();
    }

    private async Task OnLoadAsync()
    {
        if (IsDesignMode())
            return;

        dtpVon.Value = new DateTime(DateTime.Today.Year, 1, 1);
        dtpBis.Value = DateTime.Today;

        ConfigureKundenGrid();

        dgwKundenkonto.AutoGenerateColumns = false;
        colDatum.DefaultCellStyle.Format = "dd.MM.yyyy";
        colSoll.DefaultCellStyle.Format = "N2";
        colHaben.DefaultCellStyle.Format = "N2";
        colSaldo.DefaultCellStyle.Format = "N2";
        colSoll.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        colHaben.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        colSaldo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

        _allCustomers = (await _customerService
            .GetListAsync(new CustomerListFilter { NurAktiv = true }))
            .OrderBy(x => x.Kundenname)
            .ToList();

        FilterCustomers();
    }

    private void ConfigureKundenGrid()
    {
        dgwKunden.AutoGenerateColumns = false;
        dgwKunden.Columns.Clear();

        dgwKunden.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colKundenNr",
            DataPropertyName = nameof(CustomerListDto.Kundennummer),
            HeaderText = "Nr.",
            FillWeight = 80,
            ReadOnly = true
        });

        dgwKunden.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colKundenName",
            DataPropertyName = nameof(CustomerListDto.Kundenname),
            HeaderText = "Kunde",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            ReadOnly = true
        });
    }
    private void FilterCustomers()
    {
        var filter = txtKundeSuche.Text?.Trim() ?? string.Empty;

        var filtered = string.IsNullOrWhiteSpace(filter)
            ? _allCustomers
            : _allCustomers
                    .Where(k => (k.Kundenname ?? string.Empty).Contains(filter, StringComparison.OrdinalIgnoreCase)
                    || (k.Kundennummer ?? string.Empty).Contains(filter, StringComparison.OrdinalIgnoreCase))
                .ToList();

        var currentId = (cboKunde.SelectedItem as CustomerListDto)?.Id;
        cboKunde.DataSource = null;
        cboKunde.DataSource = filtered;
        cboKunde.DisplayMember = nameof(CustomerListDto.Kundenname);

        _suppressCustomerSelectionChanged = true;
        try
        {
            dgwKunden.DataSource = null;
            //ConfigureKundenGrid();
            dgwKunden.DataSource = filtered;

            // restore selection if possible
            if (currentId.HasValue)
            {
                foreach (DataGridViewRow r in dgwKunden.Rows)
                {
                    if (r.DataBoundItem is CustomerListDto dto && dto.Id == currentId.Value)
                    {
                        r.Selected = true;
                        dgwKunden.CurrentCell = r.Cells[0];
                        break;
                    }
                }
            }
        }
        finally
        {
            _suppressCustomerSelectionChanged = false;
        }
    }

    private async Task LoadKundenkontoAsync()
    {
        CustomerListDto? kunde = null;

        // Prefer grid selection (new UI), fall back to combo for compatibility
        if (dgwKunden.CurrentRow?.DataBoundItem is CustomerListDto gridKunde)
            kunde = gridKunde;
        else if (cboKunde.SelectedItem is CustomerListDto cb)
            kunde = cb;

        if (kunde is null)
        {
            ShowError("Bitte einen Kunden auswählen.");
            return;
        }

        try
        {
            Cursor = Cursors.WaitCursor;

            var entries = await _customerAccountService.GetEntriesAsync(
                kunde.Id,
                dtpVon.Value.Date,
                dtpBis.Value.Date);

            dgwKundenkonto.DataSource = entries.ToList();

            lblKundennameValue.Text = $"{kunde.Kundennummer} - {kunde.Kundenname}";
            lblZeitraumValue.Text = $"{dtpVon.Value:dd.MM.yyyy} - {dtpBis.Value:dd.MM.yyyy}";

            var saldo = entries.LastOrDefault()?.Saldo ?? 0m;
            lblSaldoValue.Text = $"{saldo:N2} €";
            lblSaldoValue.ForeColor = saldo >= 0m ? Color.DarkRed : Color.DarkGreen;
        }
        catch (Exception ex)
        {
            ShowError($"Kundenkonto konnte nicht geladen werden:\n{ex.Message}");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private async Task OnKundeGridSelectionChangedAsync()
    {
        // when user selects a customer in the left grid, auto-load the account
        if (dgwKunden.CurrentRow?.DataBoundItem is not CustomerListDto selected) return;

        // keep combo in sync
        _suppressCustomerSelectionChanged = true;
        try
        {
            cboKunde.SelectedItem = selected;
        }
        finally
        {
            _suppressCustomerSelectionChanged = false;
        }

        await LoadKundenkontoAsync();
    }
}
