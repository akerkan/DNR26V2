using DNR26V2.Domain.DTOs;

namespace DNR26V2.Forms.Orders;

/// <summary>
/// Product selection dialog — used to add extra lines to an order.
/// Receives the full list and filters client-side (no extra DB calls).
/// </summary>
public partial class FrmArtikelAuswahl : Form
{
    private readonly IReadOnlyList<ArtikelSuchDto> _alle;

    /// <summary>Set after OK is clicked — the article the user chose.</summary>
    public ArtikelSuchDto? SelectedArtikel { get; private set; }

    public FrmArtikelAuswahl(IReadOnlyList<ArtikelSuchDto> artikel)
    {
        _alle = artikel;
        InitializeComponent();
        WireUpEvents();
    }

    private void WireUpEvents()
    {
        Load += FrmArtikelAuswahl_Load;
        txtSuche.TextChanged       += TxtSuche_TextChanged;
        dgwArtikel.CellDoubleClick += DgwArtikel_CellDoubleClick;
        btnOk.Click                += BtnOk_Click;
    }

    private void FrmArtikelAuswahl_Load(object? sender, EventArgs e)
    {
        FillGrid(_alle);
        txtSuche.Focus();
    }

    // ── Filter ────────────────────────────────────────────────────────────────

    private void TxtSuche_TextChanged(object? sender, EventArgs e)
    {
        var text = txtSuche.Text.Trim();

        if (string.IsNullOrEmpty(text))
        {
            FillGrid(_alle);
            return;
        }

        var filtered = _alle
            .Where(a => a.Produktname  .Contains(text, StringComparison.OrdinalIgnoreCase)
                     || a.Artikelnummer.Contains(text, StringComparison.OrdinalIgnoreCase))
            .ToList();

        FillGrid(filtered);
    }

    private void FillGrid(IEnumerable<ArtikelSuchDto> source)
    {
        dgwArtikel.Rows.Clear();

        foreach (var a in source)
        {
            var idx = dgwArtikel.Rows.Add();
            var row = dgwArtikel.Rows[idx];
            row.Cells["_ArtikelId"]  .Value = a.ArtikelId;
            row.Cells["Artikelnummer"].Value = a.Artikelnummer;
            row.Cells["Produktname"] .Value = a.Produktname;
            row.Cells["VKPreis"]     .Value = a.VKPreis;
        }

        // Auto-select first row
        if (dgwArtikel.Rows.Count > 0)
        {
            dgwArtikel.Rows[0].Selected = true;
            dgwArtikel.CurrentCell      = dgwArtikel.Rows[0].Cells["Artikelnummer"];
        }
    }

    // ── OK / Double-click ─────────────────────────────────────────────────────

    private void BtnOk_Click(object? sender, EventArgs e)
        => ConfirmSelection();

    private void DgwArtikel_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        ConfirmSelection();
        DialogResult = DialogResult.OK;
        Close();
    }

    private void ConfirmSelection()
    {
        if (dgwArtikel.CurrentRow is null) return;

        SelectedArtikel = new ArtikelSuchDto
        {
            ArtikelId     = (int)(dgwArtikel.CurrentRow.Cells["_ArtikelId"]  .Value ?? 0),
            Artikelnummer =       dgwArtikel.CurrentRow.Cells["Artikelnummer"].Value?.ToString() ?? string.Empty,
            Produktname   =       dgwArtikel.CurrentRow.Cells["Produktname"] .Value?.ToString() ?? string.Empty,
            VKPreis       = ParseDecimal(dgwArtikel.CurrentRow.Cells["VKPreis"].Value)
        };
    }

    private static decimal ParseDecimal(object? val)
    {
        if (val is decimal d) return d;
        if (val is string  s && decimal.TryParse(s,
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.CurrentCulture, out var r)) return r;
        return 0;
    }
}