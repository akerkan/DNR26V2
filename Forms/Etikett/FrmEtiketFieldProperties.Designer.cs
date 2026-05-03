namespace DNR26V2.Forms.Etikett;

partial class FrmEtiketFieldProperties
{
    private System.ComponentModel.IContainer components = null!;

    private NumericUpDown nudX        = null!;
    private NumericUpDown nudY        = null!;
    private NumericUpDown nudWidth    = null!;
    private NumericUpDown nudHeight   = null!;
    private TextBox       txtFontName = null!;
    private NumericUpDown nudFontSize = null!;
    private CheckBox      chkBold     = null!;
    private CheckBox      chkItalic   = null!;
    private CheckBox      chkVisible  = null!;
    private Button        btnForeColor= null!;
    private Button        btnBackColor= null!;
    private ComboBox      cmbAlign    = null!;
    private Button        btnOK       = null!;
    private Button        btnAbbrechen= null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        Text            = "Feld-Eigenschaften";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox     = false;
        MinimizeBox     = false;
        StartPosition   = FormStartPosition.CenterParent;
        Size            = new Size(360, 400);

        var tbl = new TableLayoutPanel
        {
            Dock        = DockStyle.Fill,
            ColumnCount = 2,
            Padding     = new Padding(12),
            AutoScroll  = true,
        };
        tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
        tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        nudX        = Nud(0, 1000); nudY = Nud(0, 1000);
        nudWidth    = Nud(1, 600);  nudHeight = Nud(1, 600);
        nudFontSize = Nud(4, 120);  nudFontSize.DecimalPlaces = 1;
        txtFontName = new TextBox { Dock = DockStyle.Fill };
        chkBold     = new CheckBox { Dock = DockStyle.Left };
        chkItalic   = new CheckBox { Dock = DockStyle.Left };
        chkVisible  = new CheckBox { Dock = DockStyle.Left, Checked = true };

        btnForeColor = new Button { Text = "Vordergrund", Dock = DockStyle.Fill, Height = 28 };
        btnBackColor = new Button { Text = "Hintergrund", Dock = DockStyle.Fill, Height = 28 };
        btnForeColor.Click += BtnForeColor_Click;
        btnBackColor.Click += BtnBackColor_Click;

        cmbAlign = new ComboBox
        {
            Dock          = DockStyle.Fill,
            DropDownStyle = ComboBoxStyle.DropDownList,
        };
        cmbAlign.Items.AddRange(new object[] { "Links", "Mitte", "Rechts" });
        cmbAlign.SelectedIndex = 0;

        Row(tbl, "X (px):",      nudX);
        Row(tbl, "Y (px):",      nudY);
        Row(tbl, "Breite (px):", nudWidth);
        Row(tbl, "Höhe (px):",   nudHeight);
        Row(tbl, "Schrift:",     txtFontName);
        Row(tbl, "Schriftgröße:",nudFontSize);
        Row(tbl, "Fett:",        chkBold);
        Row(tbl, "Kursiv:",      chkItalic);
        Row(tbl, "Sichtbar:",    chkVisible);
        Row(tbl, "Vordergrundfarbe:", btnForeColor);
        Row(tbl, "Hintergrundfarbe:", btnBackColor);
        Row(tbl, "Ausrichtung:", cmbAlign);

        btnOK        = new Button { Text = "OK",         DialogResult = DialogResult.OK,     Width = 90, Height = 30 };
        btnAbbrechen = new Button { Text = "Abbrechen",  DialogResult = DialogResult.Cancel, Width = 90, Height = 30 };
        btnOK.Click        += BtnOK_Click;
        btnAbbrechen.Click += BtnAbbrechen_Click;

        var btnPanel = new FlowLayoutPanel
        {
            Dock          = DockStyle.Bottom,
            FlowDirection = FlowDirection.RightToLeft,
            Height        = 45,
            Padding       = new Padding(5),
        };
        btnPanel.Controls.AddRange(new Control[] { btnAbbrechen, btnOK });

        Controls.Add(tbl);
        Controls.Add(btnPanel);
    }

    private static NumericUpDown Nud(int min, int max) => new()
    {
        Dock    = DockStyle.Fill,
        Minimum = min,
        Maximum = max,
    };

    private static void Row(TableLayoutPanel tbl, string label, Control ctrl)
    {
        tbl.Controls.Add(new Label
        {
            Text      = label,
            Dock      = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
        });
        tbl.Controls.Add(ctrl);
    }
}
