using DNR26V2.Data.Context;
using DNR26V2.Domain.DTOs.Etikett;
using DNR26V2.Forms.Base;
using DNR26V2.Services.Etikett;
using Microsoft.EntityFrameworkCore;

namespace DNR26V2.Forms.Etikett;

public partial class FrmEtikett : BaseListForm
{
    private readonly IEtiketService _service;
    private readonly AppDbContext _db;

    private IReadOnlyList<EtiketKundeDto> _kunden = [];
    private IReadOnlyList<EtiketProduktDto> _produkte = [];

    public FrmEtikett(IEtiketService service, AppDbContext db)
    {
        _service = service;
        _db = db;
        InitializeComponent();
        WireEvents();
    }

    // Designer ctor
    public FrmEtikett()
    {
        _service = null!;
        _db = null!;
        InitializeComponent();
        WireEvents();
    }

    // ?? Startup ???????????????????????????????????????????????????????????????

    private void WireEvents()
    {
        dgwKunden.SelectionChanged += DgwKunden_SelectionChanged;
        txtKundenSearch.TextChanged += TxtKundenSearch_TextChanged;
        dgwProdukte.CellClick += DgwProdukte_CellClick;
        dgwProdukte.CellEndEdit += DgwProdukte_CellEndEdit;
        Load += FrmEtikett_Load;
    }

    private async void FrmEtikett_Load(object sender, EventArgs e)
    {
        if (DesignMode || _service is null) return;
        StyleGrids();
        await LoadKundenAsync();
    }

    // ?? Data loading ??????????????????????????????????????????????????????????

    private async Task LoadKundenAsync()
    {
        _kunden = await _service.GetKundenAsync();
        ApplyKundenFilter();
    }

    private void ApplyKundenFilter()
    {
        var filter = txtKundenSearch.Text.Trim().ToLower();
        var filtered = string.IsNullOrEmpty(filter)
            ? _kunden
            : _kunden.Where(k => k.Kundenname.ToLower().Contains(filter) ||
                                  k.Kundennummer.ToLower().Contains(filter)).ToList();
        dgwKunden.DataSource = filtered.ToList();
    }

    private async void DgwKunden_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgwKunden.CurrentRow?.DataBoundItem is not EtiketKundeDto kunde) return;
        await LoadProdukteAsync(kunde.Id);
    }

    private async Task LoadProdukteAsync(int kundeId)
    {
        _produkte = await _service.GetProdukteByKundeAsync(kundeId);

        // Build mutable list for grid (user can edit Menge/Gewicht)
        var rows = _produkte.Select(p => new EtiketProduktRow
        {
            KundeId = p.KundeId,
            Kundenname = p.Kundenname,
            ArtikelId = p.ArtikelId,
            Artikelnummer = p.Artikelnummer,
            Produktname = p.Produktname,
            Menge = p.Menge,
            Gewicht = p.Gewicht,
            Printfarbe = p.Printfarbe,
            Feld1 = p.Feld1,
            Feld2 = p.Feld2,
            Feld3 = p.Feld3,
            Barcode = p.Barcode,
        }).ToList();

        dgwProdukte.DataSource = rows;
    }

    private void TxtKundenSearch_TextChanged(object? sender, EventArgs e)
        => ApplyKundenFilter();

    // ?? Grid events ???????????????????????????????????????????????????????????

    private async void DgwProdukte_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        if (dgwProdukte.Columns[e.ColumnIndex].Name != colEtikett.Name) return;

        if (dgwProdukte.Rows[e.RowIndex].DataBoundItem is not EtiketProduktRow row) return;

        await OpenPrintFormAsync(row);
    }

    private void DgwProdukte_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
    {
        // Changes are auto-reflected in the bound EtiketProduktRow
    }

    private async Task OpenPrintFormAsync(EtiketProduktRow row)
    {
        var setup = await _db.AppSetup.FirstOrDefaultAsync();

        var kopienAusMenge = Math.Max(1, (int)Math.Round(row.Menge, MidpointRounding.AwayFromZero));

        var data = new EtiketDruckData
        {
            Kundenname    = row.Kundenname,
            Produktname   = row.Produktname,
            Untertitel    = row.Feld1,
            Zutaten       = row.Feld2,
            Hinweis       = row.Feld3,
            Printfarbe    = row.Printfarbe,
            Barcode       = row.Barcode,
            LieferDatum   = dtpLieferDatum.Value,
            HerstellDatum = dtpHerstellDatum.Value,
            Menge         = row.Menge,
            Gewicht       = row.Gewicht,
            Kopien        = kopienAusMenge,
            Firmenname    = setup?.Firmenname    ?? string.Empty,
            FirmenAdresse = setup?.Firmenadresse ?? string.Empty,
            FirmenTelefon = setup?.FirmenTelefon ?? string.Empty,
            FirmenEmail   = setup?.FirmenEmail   ?? string.Empty,
            LogoPfad      = setup?.LogoPfad,
        };

        var layout = await _service.GetLayoutAsync();
        var printerName = setup?.DruckerEtikett ?? string.Empty;

        using var frm = new FrmEtiketDruck(data, layout, printerName);
        frm.ShowDialog(this);
    }

    // ?? Grid styling ????????????????????????????????????????????????????????

    private void StyleGrids()
    {
        // ?? Kunden grid ??????????????????????????????????????????????????????
        ConfigureGrid(dgwKunden);
        dgwKunden.AutoGenerateColumns = false;
        dgwKunden.Columns.Clear();
        dgwKunden.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "Kundenname",
            HeaderText       = "Kundenname",
            Name             = "colKdName",
            AutoSizeMode     = DataGridViewAutoSizeColumnMode.Fill,
            ReadOnly         = true,
        });
        dgwKunden.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "Tur",
            HeaderText       = "Tur",
            Name             = "colKdTur",
            Width            = 55,
            ReadOnly         = true,
        });

        // ? Produkte grid editable for Menge/Gewicht
        ConfigureGrid(dgwProdukte);
        dgwProdukte.ReadOnly = false;
        dgwProdukte.AutoGenerateColumns = false;
        dgwProdukte.Columns.Clear();
        dgwProdukte.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "Artikelnummer", HeaderText = "Art.-Nr.",
            Name = "colArtikelnummer", Width = 90, ReadOnly = true,
        });
        dgwProdukte.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "Produktname", HeaderText = "Produktname",
            Name = "colProduktname",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true,
        });
        dgwProdukte.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "Menge", HeaderText = "Menge",
            Name = "colMenge", Width = 75, ReadOnly = false,
        });
        dgwProdukte.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "Gewicht", HeaderText = "Gewicht Kg",
            Name = "colGewicht", Width = 95, ReadOnly = false,
        });
        dgwProdukte.Columns.Add(new DataGridViewButtonColumn
        {
            HeaderText = "Etikett", Name = "colEtikettBtn",
            Text = "Drucken", UseColumnTextForButtonValue = true,
            Width = 72, ReadOnly = true,
        });

        // Update the field reference so CellClick still works
        colEtikett = (DataGridViewButtonColumn)dgwProdukte.Columns["colEtikettBtn"];
    }
}

// ?? Row model for the product grid ??????????????????????????????????????????

internal sealed class EtiketProduktRow
{
    public int     KundeId       { get; set; }
    public string  Kundenname    { get; set; } = string.Empty;
    public int     ArtikelId     { get; set; }
    public string  Artikelnummer { get; set; } = string.Empty;
    public string  Produktname   { get; set; } = string.Empty;
    public decimal Menge         { get; set; }
    public decimal Gewicht       { get; set; }
    public string? Printfarbe    { get; set; }
    public string? Feld1         { get; set; }
    public string? Feld2         { get; set; }
    public string? Feld3         { get; set; }
    public string? Barcode       { get; set; }
}
