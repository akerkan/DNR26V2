namespace DNR26V2.Forms.Settings;

partial class FrmAppSetup
{
    private System.ComponentModel.IContainer components = null;

    // Controls
    private TabControl  tabControl       = null!;
    private TabPage     tabFirma         = null!;
    private TabPage     tabDrucker       = null!;
    private TabPage     tabNoSeries      = null!;
    private TextBox     txtFirmenname    = null!;
    private TextBox     txtFirmenadresse = null!;
    private TextBox     txtFirmenPLZ     = null!;
    private TextBox     txtFirmenOrt     = null!;
    private TextBox     txtFirmenLand    = null!;
    private TextBox     txtFirmenTelefon = null!;
    private TextBox     txtFirmenEmail   = null!;
    private TextBox     txtSteuernummer  = null!;
    private TextBox     txtUStIdNr       = null!;
    private TextBox     txtDrucker1      = null!;
    private TextBox     txtDrucker2      = null!;
    private NumericUpDown nudMwst        = null!;
    private NumericUpDown nudSeitengroesse = null!;
    private DataGridView  dgwNoSeries    = null!;
    private Button      btnSpeichern     = null!;
    private Button      btnSchliessen    = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        Text            = "Systemeinstellungen";
        Size            = new Size(680, 620);
        MinimumSize     = new Size(680, 620);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox     = false;
        MinimizeBox     = false;
        StartPosition   = FormStartPosition.CenterParent;

        // ── TabControl ────────────────────────────────────────────────────────
        tabControl = new TabControl { Dock = DockStyle.Fill, Padding = new Point(10, 5) };

        tabFirma    = new TabPage("Firmendaten");
        tabDrucker  = new TabPage("Drucker & Druck");
        tabNoSeries = new TabPage("Nummernserien");

        tabControl.TabPages.AddRange(new[] { tabFirma, tabDrucker, tabNoSeries });

        // ── Tab 1: Firmendaten ────────────────────────────────────────────────
        var panelFirma = new TableLayoutPanel
        {
            Dock        = DockStyle.Fill,
            ColumnCount = 2,
            Padding     = new Padding(15),
            AutoScroll  = true
        };
        panelFirma.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180));
        panelFirma.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        txtFirmenname    = new TextBox { Dock = DockStyle.Fill };
        txtFirmenadresse = new TextBox { Dock = DockStyle.Fill };
        txtFirmenPLZ     = new TextBox { Dock = DockStyle.Fill };
        txtFirmenOrt     = new TextBox { Dock = DockStyle.Fill };
        txtFirmenLand    = new TextBox { Dock = DockStyle.Fill };
        txtFirmenTelefon = new TextBox { Dock = DockStyle.Fill };
        txtFirmenEmail   = new TextBox { Dock = DockStyle.Fill };
        txtSteuernummer  = new TextBox { Dock = DockStyle.Fill };
        txtUStIdNr       = new TextBox { Dock = DockStyle.Fill };

        nudMwst = new NumericUpDown
        {
            Dock          = DockStyle.Fill,
            DecimalPlaces = 2,
            Minimum       = 0,
            Maximum       = 100,
            Value         = 7
        };
        nudSeitengroesse = new NumericUpDown
        {
            Dock    = DockStyle.Fill,
            Minimum = 10,
            Maximum = 500,
            Value   = 20
        };

        AddLabelAndControl(panelFirma, "Firmenname *",    txtFirmenname);
        AddLabelAndControl(panelFirma, "Adresse",         txtFirmenadresse);
        AddLabelAndControl(panelFirma, "PLZ",             txtFirmenPLZ);
        AddLabelAndControl(panelFirma, "Ort",             txtFirmenOrt);
        AddLabelAndControl(panelFirma, "Land",            txtFirmenLand);
        AddLabelAndControl(panelFirma, "Telefon",         txtFirmenTelefon);
        AddLabelAndControl(panelFirma, "E-Mail",          txtFirmenEmail);
        AddLabelAndControl(panelFirma, "Steuernummer",    txtSteuernummer);
        AddLabelAndControl(panelFirma, "USt-IdNr.",       txtUStIdNr);
        AddLabelAndControl(panelFirma, "MwSt. %",         nudMwst);
        AddLabelAndControl(panelFirma, "Zeilen / Seite",  nudSeitengroesse);

        tabFirma.Controls.Add(panelFirma);

        // ── Tab 2: Drucker ────────────────────────────────────────────────────
        var panelDrucker = new TableLayoutPanel
        {
            Dock        = DockStyle.Fill,
            ColumnCount = 2,
            Padding     = new Padding(15)
        };
        panelDrucker.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200));
        panelDrucker.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        txtDrucker1 = new TextBox { Dock = DockStyle.Fill };
        txtDrucker2 = new TextBox { Dock = DockStyle.Fill };

        AddLabelAndControl(panelDrucker, "Drucker (weißes Papier)", txtDrucker1);
        AddLabelAndControl(panelDrucker, "Drucker (mit Logo)",      txtDrucker2);

        var lblHinweis = new Label
        {
            Text      = "Druckernamen exakt wie in Windows angeben.",
            ForeColor = Color.Gray,
            AutoSize  = true,
            Margin    = new Padding(5, 15, 0, 0)
        };
        panelDrucker.Controls.Add(lblHinweis);
        panelDrucker.SetColumnSpan(lblHinweis, 2);
        tabDrucker.Controls.Add(panelDrucker);

        // ── Tab 3: Nummernserien ──────────────────────────────────────────────
        dgwNoSeries = new DataGridView
        {
            Dock                = DockStyle.Fill,
            ReadOnly            = true,
            AllowUserToAddRows  = false,
            SelectionMode       = DataGridViewSelectionMode.FullRowSelect,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            RowHeadersVisible   = false,
            BackgroundColor     = SystemColors.Window
        };
        SetDoubleBuffered(dgwNoSeries);
        tabNoSeries.Controls.Add(dgwNoSeries);

        // ── Buttons ───────────────────────────────────────────────────────────
        btnSpeichern  = new Button { Text = "Speichern",  Width = 120, Height = 35 };
        btnSchliessen = new Button { Text = "Schließen",  Width = 120, Height = 35 };

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

        // ── Form zusammenbauen ────────────────────────────────────────────────
        Controls.Add(tabControl);
        Controls.Add(panelButtons);

        Load += FrmAppSetup_Load;
    }

    private static void AddLabelAndControl(TableLayoutPanel panel, string labelText, Control control)
    {
        var lbl = new Label
        {
            Text      = labelText,
            Dock      = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin    = new Padding(0, 5, 10, 5)
        };
        control.Margin = new Padding(0, 5, 0, 5);
        panel.Controls.Add(lbl);
        panel.Controls.Add(control);
    }
}