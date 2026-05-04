using System.Drawing.Printing;
using DNR26V2.Domain.DTOs.Etikett;
using DNR26V2.Domain.Entities.Etikett;
using DNR26V2.Forms.Base;
using DNR26V2.Services.Etikett;

namespace DNR26V2.Forms.Etikett;

public partial class FrmEtiketDruck : BaseForm
{
    private readonly EtiketDruckData             _data;
    private readonly IReadOnlyList<EtiketLayoutField> _layout;
    private readonly string _printerName;
    private bool _isPrinting;
    private int _remainingCopies;

    public FrmEtiketDruck(
        EtiketDruckData data,
        IReadOnlyList<EtiketLayoutField> layout,
        string printerName)
    {
        _data        = data;
        _layout      = layout;
        _printerName = printerName;
        InitializeComponent();
        nudKopien.Value = Math.Max(1, data.Kopien);
        Load += (_, _) => RenderPreview();
    }

    // ?? Preview ???????????????????????????????????????????????????????????????

    private void RenderPreview()
    {
        var bmp = EtiketRenderer.RenderBitmap(_data, _layout, scale: 1f);
        picturePreview.Image?.Dispose();
        picturePreview.Image = bmp;
    }

    // ?? Print ?????????????????????????????????????????????????????????????????

    private void BtnDrucken_Click(object? sender, EventArgs e)
    {
        if (_isPrinting) return;
        _isPrinting = true;
        btnDrucken.Enabled = false;

        try
        {
            using var doc = new PrintDocument();

            if (!string.IsNullOrWhiteSpace(_printerName))
                doc.PrinterSettings.PrinterName = _printerName;

            _remainingCopies = Math.Max(1, (int)nudKopien.Value);
            doc.PrinterSettings.Copies = 1;

            // 10 × 15 cm in hundredths of an inch: 394 × 591
            doc.DefaultPageSettings.PaperSize = new PaperSize("Etikett 10x15", 394, 591);
            doc.DefaultPageSettings.Margins   = new Margins(0, 0, 0, 0);

            doc.PrintPage += PrintPage;
            doc.EndPrint  += (_, _) => { _isPrinting = false; btnDrucken.Enabled = true; };
            doc.Print();
        }
        catch (Exception ex)
        {
            _isPrinting = false;
            btnDrucken.Enabled = true;
            MessageBox.Show("Druckfehler: " + ex.Message, "Fehler",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void PrintPage(object sender, PrintPageEventArgs e)
    {
        const float scale = 100f / 96f;
        EtiketRenderer.Render(e.Graphics, _data, _layout, scale);

        _remainingCopies--;
        e.HasMorePages = _remainingCopies > 0;
    }

    private void BtnSchliessen_Click(object? sender, EventArgs e) => Close();
}
