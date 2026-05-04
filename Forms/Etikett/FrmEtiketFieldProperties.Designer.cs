namespace DNR26V2.Forms.Etikett;

partial class FrmEtiketFieldProperties
{
    private System.ComponentModel.IContainer components = null!;

    private ComboBox      cmbFontName  = null!;
    private NumericUpDown nudFontSize  = null!;
    private CheckBox      chkBold      = null!;
    private CheckBox      chkItalic    = null!;
    private CheckBox      chkVisible   = null!;
    private Button        btnForeColor = null!;
    private Button        btnBackColor = null!;
    private ComboBox      cmbAlign     = null!;
    private Button        btnOK        = null!;
    private Button        btnAbbrechen = null!;

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
        Size            = new Size(340, 320);

        var tbl = new TableLayoutPanel
        {
            Dock        = DockStyle.Fill,
            ColumnCount = 2,
            Padding     = new Padding(12),
        };
        tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
        tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        cmbFontName = new ComboBox
        {
            Dock          = DockStyle.Fill,
            DropDownStyle = ComboBoxStyle.DropDown,
            AutoCompleteMode   = AutoCompleteMode.SuggestAppend,
            AutoCompleteSource = AutoCompleteSource.ListItems,
        };

        nudFontSize = new NumericUpDown
        {
            Dock          = DockStyle.Fill,
            Minimum       = 4,
            Maximum       = 120,
            DecimalPlaces = 1,
            Increment     = 0.5m,
        };

        chkBold    = new CheckBox { Dock = DockStyle.Left, Text = "Fett"    };
        chkItalic  = new CheckBox { Dock = DockStyle.Left, Text = "Kursiv"  };
        chkVisible = new CheckBox { Dock = DockStyle.Left, Text = "Sichtbar", Checked = true };

        btnForeColor = new Button { Dock = DockStyle.Fill, Text = "Vordergrundfarbe", Height = 28 };
        btnBackColor = new Button { Dock = DockStyle.Fill, Text = "Hintergrundfarbe", Height = 28 };
        btnForeColor.Click += BtnForeColor_Click;
        btnBackColor.Click += BtnBackColor_Click;

        cmbAlign = new ComboBox
        {
            Dock          = DockStyle.Fill,
            DropDownStyle = ComboBoxStyle.DropDownList,
        };
        cmbAlign.Items.AddRange(new object[] { "Links", "Mitte", "Rechts" });
        cmbAlign.SelectedIndex = 0;

        Row(tbl, "Schrift:",        cmbFontName);
        Row(tbl, "Schriftgröße:",   nudFontSize);
        Row(tbl, "Stil:",           chkBold);
        Row(tbl, string.Empty,      chkItalic);
        Row(tbl, "Sichtbar:",       chkVisible);
        Row(tbl, "Ausrichtung:",    cmbAlign);
        Row(tbl, "Textfarbe:",      btnForeColor);
        Row(tbl, "Hintergrundfarbe:", btnBackColor);

        btnOK        = new Button { Text = "OK",        Width = 90, Height = 30 };
        btnAbbrechen = new Button { Text = "Abbrechen", Width = 90, Height = 30 };
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
