using DNR26V2.Services.Etikett;

namespace DNR26V2.Forms.Etikett;

partial class FrmEtikettDesigner
{
    private System.ComponentModel.IContainer components = null!;

    // Public so cs can reference
    internal Panel  canvas       = null!;
    private  Button btnSpeichern = null!;
    private  Button btnReset     = null!;
    private  Button btnSchliessen= null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        Text            = "Label-Designer (10 × 15 cm)";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox     = false;
        MinimizeBox     = false;
        StartPosition   = FormStartPosition.CenterParent;
        Size            = new Size(430, 680);

        // Canvas = 378 × 567 px  (10 × 15 cm at 96 DPI)
        canvas = new Panel
        {
            Location    = new Point(12, 10),
            Size        = new Size(EtiketRenderer.CanvasW, EtiketRenderer.CanvasH),
            BackColor   = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
        };

        btnSpeichern  = new Button { Text = "?? Speichern",  Width = 110, Height = 30, Location = new Point(12,  590) };
        btnReset      = new Button { Text = "? Reset",       Width =  80, Height = 30, Location = new Point(132, 590) };
        btnSchliessen = new Button { Text = "Schließen",      Width = 100, Height = 30, Location = new Point(222, 590) };

        btnSpeichern.Click  += BtnSpeichern_Click;
        btnReset.Click      += BtnReset_Click;
        btnSchliessen.Click += BtnSchliessen_Click;

        Controls.AddRange(new Control[] { canvas, btnSpeichern, btnReset, btnSchliessen });
    }
}
