namespace DNR26V2.Forms.Settings;

partial class FrmLocationSetup
{
    private System.ComponentModel.IContainer components = null;

    private TextBox     txtStandortcode = null!;
    private TextBox     txtBezeichnung  = null!;
    private TextBox     txtAdresse      = null!;
    private TextBox     txtPLZ          = null!;
    private TextBox     txtOrt          = null!;
    private TextBox     txtLand         = null!;
    private CheckBox    chkAktiv        = null!;
    private Button      btnSpeichern    = null!;
    private Button      btnSchliessen   = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        Text            = "Standortverwaltung";
        Size            = new Size(480, 400);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox     = false;
        MinimizeBox     = false;
        StartPosition   = FormStartPosition.CenterParent;

        var panel = new TableLayoutPanel
        {
            Dock        = DockStyle.Fill,
            ColumnCount = 2,
            Padding     = new Padding(20),
            AutoScroll  = false
        };
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        txtStandortcode = new TextBox { Dock = DockStyle.Fill, CharacterCasing = CharacterCasing.Upper };
        txtBezeichnung  = new TextBox { Dock = DockStyle.Fill };
        txtAdresse      = new TextBox { Dock = DockStyle.Fill };
        txtPLZ          = new TextBox { Dock = DockStyle.Fill };
        txtOrt          = new TextBox { Dock = DockStyle.Fill };
        txtLand         = new TextBox { Dock = DockStyle.Fill };
        chkAktiv        = new CheckBox { Text = "Aktiv", Checked = true, AutoSize = true };

        AddRow(panel, "Standortcode *",  txtStandortcode);
        AddRow(panel, "Bezeichnung *",   txtBezeichnung);
        AddRow(panel, "Adresse",         txtAdresse);
        AddRow(panel, "PLZ",             txtPLZ);
        AddRow(panel, "Ort",             txtOrt);
        AddRow(panel, "Land",            txtLand);
        AddRow(panel, string.Empty,      chkAktiv);

        btnSpeichern  = new Button { Text = "Speichern",  Width = 120, Height = 35 };
        btnSchliessen = new Button { Text = "Schlieﬂen",  Width = 120, Height = 35 };
        btnSpeichern.Click  += BtnSpeichern_Click;
        btnSchliessen.Click += BtnSchliessen_Click;

        var panelButtons = new FlowLayoutPanel
        {
            Dock          = DockStyle.Bottom,
            FlowDirection = FlowDirection.RightToLeft,
            Height        = 50,
            Padding       = new Padding(5)
        };
        panelButtons.Controls.AddRange(new Control[] { btnSchliessen, btnSpeichern });

        Controls.Add(panel);
        Controls.Add(panelButtons);

        Load += FrmLocationSetup_Load;
    }

    private static void AddRow(TableLayoutPanel panel, string label, Control control)
    {
        var lbl = new Label
        {
            Text      = label,
            Dock      = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin    = new Padding(0, 5, 10, 5)
        };
        control.Margin = new Padding(0, 5, 0, 5);
        panel.Controls.Add(lbl);
        panel.Controls.Add(control);
    }
}