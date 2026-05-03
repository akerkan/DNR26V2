namespace DNR26V2.Forms.Etikett;

partial class FrmEtiketDruck
{
    private System.ComponentModel.IContainer components = null!;

    private PictureBox    picturePreview = null!;
    private NumericUpDown nudKopien      = null!;
    private Button        btnDrucken     = null!;
    private Button        btnSchliessen  = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        Text            = "Etikett Vorschau & Druck";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox     = false;
        MinimizeBox     = false;
        StartPosition   = FormStartPosition.CenterParent;
        Size            = new Size(420, 680);

        // Preview (10×15 cm = 378×567 px at 96 DPI)
        picturePreview = new PictureBox
        {
            Location    = new Point(12, 10),
            Size        = new Size(378, 567),
            BorderStyle = BorderStyle.FixedSingle,
            SizeMode    = PictureBoxSizeMode.Zoom,
        };

        var lblKopien = new Label
        {
            Text     = "Kopien:",
            AutoSize = true,
            Location = new Point(12, 590),
        };

        nudKopien = new NumericUpDown
        {
            Minimum  = 1,
            Maximum  = 99,
            Value    = 1,
            Width    = 60,
            Location = new Point(65, 587),
        };

        btnDrucken = new Button
        {
            Text     = "?? Drucken",
            Width    = 120,
            Height   = 30,
            Location = new Point(145, 585),
        };

        btnSchliessen = new Button
        {
            Text     = "Schließen",
            Width    = 100,
            Height   = 30,
            Location = new Point(275, 585),
        };

        btnDrucken.Click    += BtnDrucken_Click;
        btnSchliessen.Click += BtnSchliessen_Click;

        Controls.AddRange(new Control[]
            { picturePreview, lblKopien, nudKopien, btnDrucken, btnSchliessen });
    }
}
