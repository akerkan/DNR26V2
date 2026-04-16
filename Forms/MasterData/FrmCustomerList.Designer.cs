namespace DNR26V2.Forms.MasterData;

partial class FrmCustomerList
{
    private System.ComponentModel.IContainer components = null;

    // ── Controls ──────────────────────────────────────────────────────────────
    private SplitContainer splitContainer   = null!;
    private DataGridView   dgwKunden        = null!;
    private TextBox        txtSuche         = null!;
    private ComboBox       cmbRouteOben     = null!;
    private ComboBox       cmbFilterOben    = null!;
    private CheckBox       chkNurAktiv      = null!;

    // Detail – Tab Stammdaten
    private TabControl   tabDetail        = null!;
    private TextBox      txtKundennummer  = null!;
    private TextBox      txtKundenname    = null!;
    private TextBox      txtName2         = null!;
    private TextBox      txtInhaber       = null!;
    private TextBox      txtTelefon       = null!;
    private TextBox      txtHandy         = null!;
    private TextBox      txtEmail         = null!;
    private TextBox      txtNotizen       = null!;

    // Detail – Tab Adresse
    private TextBox txtAdresse  = null!;
    private TextBox txtAdresse2 = null!;
    private TextBox txtPLZ      = null!;
    private TextBox txtOrt      = null!;
    private TextBox txtLand     = null!;

    // Detail – Tab Alt. Lieferadresse
    private CheckBox chkAbweichend = null!;
    private TextBox  txtALName2    = null!;
    private TextBox  txtALInhaber  = null!;
    private TextBox  txtALAdresse  = null!;
    private TextBox  txtALAdresse2 = null!;
    private TextBox  txtALPLZ      = null!;
    private TextBox  txtALOrt      = null!;
    private TextBox  txtALLand     = null!;

    // Detail – Tab Einstellungen
    private ComboBox       cmbRoute            = null!;
    private ComboBox       cmbKundenfilter     = null!;
    private NumericUpDown  nudRoutenfolge      = null!;
    private NumericUpDown  nudLimit            = null!;
    private CheckBox       chkWochenendtour    = null!;
    private CheckBox       chkPreisAusblenden  = null!;
    private CheckBox       chkAktiv            = null!;

    // Buttons
    private Button btnNeu           = null!;
    private Button btnSpeichern     = null!;
    private Button btnDeaktivieren  = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) { components?.Dispose(); _searchTimer.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        Text = "Kundenstammdaten";

        // ── Top-Filterleiste ──────────────────────────────────────────────────
        var panelTop = new Panel { Dock = DockStyle.Top, Height = 46, Padding = new Padding(6, 8, 6, 0) };

        var lblSuche = new Label { Text = "Suche:", AutoSize = true, Top = 12, Left = 0 };
        txtSuche     = new TextBox { Width = 200, Top = 8, Left = 52, PlaceholderText = "Name oder Nummer…" };

        var lblRoute = new Label { Text = "Route:", AutoSize = true, Top = 12, Left = 265 };
        cmbRouteOben = new ComboBox { Width = 160, Top = 8, Left = 308, DropDownStyle = ComboBoxStyle.DropDownList };

        var lblFilter = new Label { Text = "Gruppe:", AutoSize = true, Top = 12, Left = 478 };
        cmbFilterOben = new ComboBox { Width = 140, Top = 8, Left = 530, DropDownStyle = ComboBoxStyle.DropDownList };

        chkNurAktiv = new CheckBox { Text = "Nur aktiv", Top = 12, Left = 684, AutoSize = true, Checked = true };

        panelTop.Controls.AddRange(new Control[]
            { lblSuche, txtSuche, lblRoute, cmbRouteOben, lblFilter, cmbFilterOben, chkNurAktiv });

        txtSuche.TextChanged      += TxtSuche_TextChanged;
        txtSuche.KeyDown          += TxtSuche_KeyDown;
        cmbRouteOben.SelectedIndexChanged  += Filter_Changed;
        cmbFilterOben.SelectedIndexChanged += Filter_Changed;
        chkNurAktiv.CheckedChanged         += Filter_Changed;

        // ── SplitContainer ────────────────────────────────────────────────────
        splitContainer = new SplitContainer
        {
            Dock             = DockStyle.Fill,
            SplitterDistance = 440,
            Panel2MinSize    = 420
        };

        // ── Links: Kundenliste ────────────────────────────────────────────────
        dgwKunden = new DataGridView { Dock = DockStyle.Fill };
        dgwKunden.SelectionChanged += DgwKunden_SelectionChanged;
        splitContainer.Panel1.Controls.Add(dgwKunden);

        // ── Rechts: Detail ────────────────────────────────────────────────────
        splitContainer.Panel2.Padding = new Padding(6, 0, 6, 0);

        tabDetail = new TabControl { Dock = DockStyle.Fill };
        var tabStamm  = new TabPage("Stammdaten");
        var tabAdress = new TabPage("Adresse");
        var tabAltAdr = new TabPage("Alt. Lieferadresse");
        var tabEinst  = new TabPage("Einstellungen");
        tabDetail.TabPages.AddRange(new[] { tabStamm, tabAdress, tabAltAdr, tabEinst });

        // Tab 1: Stammdaten
        var pStamm = BuildTwoColumnPanel();
        txtKundennummer = AddRow(pStamm, "Kundennummer *"); txtKundennummer.CharacterCasing = CharacterCasing.Upper;
        txtKundenname   = AddRow(pStamm, "Kundenname *");
        txtName2        = AddRow(pStamm, "Name 2");
        txtInhaber      = AddRow(pStamm, "Inhaber");
        txtTelefon      = AddRow(pStamm, "Telefon");
        txtHandy        = AddRow(pStamm, "Handy");
        txtEmail        = AddRow(pStamm, "E-Mail");

        var lblNotizen = new Label { Text = "Notizen", Dock = DockStyle.Fill, Margin = new Padding(0, 5, 10, 5) };
        txtNotizen = new TextBox  { Dock = DockStyle.Fill, Multiline = true, Height = 60, Margin = new Padding(0, 5, 0, 5) };
        pStamm.Controls.Add(lblNotizen);
        pStamm.Controls.Add(txtNotizen);
        pStamm.SetColumnSpan(txtNotizen, 1);
        tabStamm.Controls.Add(pStamm);

        // Tab 2: Adresse
        var pAdress = BuildTwoColumnPanel();
        txtAdresse  = AddRow(pAdress, "Adresse");
        txtAdresse2 = AddRow(pAdress, "Adresse 2");
        txtPLZ      = AddRow(pAdress, "PLZ");
        txtOrt      = AddRow(pAdress, "Ort");
        txtLand     = AddRow(pAdress, "Land");
        tabAdress.Controls.Add(pAdress);

        // Tab 3: Alt. Lieferadresse
        var pAlt = BuildTwoColumnPanel();
        chkAbweichend = new CheckBox { Text = "Abweichende Lieferadresse", AutoSize = true, Margin = new Padding(0, 8, 0, 8) };
        pAlt.Controls.Add(chkAbweichend);
        pAlt.SetColumnSpan(chkAbweichend, 2);
        txtALName2    = AddRow(pAlt, "Name 2");
        txtALInhaber  = AddRow(pAlt, "Inhaber");
        txtALAdresse  = AddRow(pAlt, "Adresse");
        txtALAdresse2 = AddRow(pAlt, "Adresse 2");
        txtALPLZ      = AddRow(pAlt, "PLZ");
        txtALOrt      = AddRow(pAlt, "Ort");
        txtALLand     = AddRow(pAlt, "Land");
        chkAbweichend.CheckedChanged += ChkAbweichend_CheckedChanged;
        tabAltAdr.Controls.Add(pAlt);

        // Tab 4: Einstellungen
        var pEinst = BuildTwoColumnPanel();
        cmbRoute           = AddComboRow(pEinst, "Route");
        cmbKundenfilter    = AddComboRow(pEinst, "Kundengruppe");
        nudRoutenfolge     = AddNudRow(pEinst,   "Reihenfolge",  0, 9999);
        nudLimit           = AddNudRow(pEinst,   "Kreditlimit",  0, 999999, 2);
        chkWochenendtour   = AddCheckRow(pEinst, "Wochenendtour");
        chkPreisAusblenden = AddCheckRow(pEinst, "Preise ausblenden");
        chkAktiv           = AddCheckRow(pEinst, "Aktiv");
        tabEinst.Controls.Add(pEinst);

        // Dirty-Tracking auf allen Eingabefeldern
        foreach (var tb in new[] { txtKundennummer, txtKundenname, txtName2, txtInhaber,
                                   txtTelefon, txtHandy, txtEmail, txtNotizen,
                                   txtAdresse, txtAdresse2, txtPLZ, txtOrt, txtLand,
                                   txtALName2, txtALInhaber, txtALAdresse, txtALAdresse2,
                                   txtALPLZ, txtALOrt, txtALLand })
            tb.TextChanged += Detail_Changed;

        // ── Buttons ───────────────────────────────────────────────────────────
        btnNeu          = new Button { Text = "Neu (F2)",          Width = 110, Height = 34 };
        btnSpeichern    = new Button { Text = "Speichern (Strg+S)",Width = 140, Height = 34 };
        btnDeaktivieren = new Button { Text = "Deaktivieren",      Width = 110, Height = 34 };

        btnNeu.Click          += BtnNeu_Click;
        btnSpeichern.Click    += BtnSpeichern_Click;
        btnDeaktivieren.Click += BtnDeaktivieren_Click;

        var panelButtons = new FlowLayoutPanel
        {
            Dock          = DockStyle.Bottom,
            Height        = 48,
            FlowDirection = FlowDirection.LeftToRight,
            Padding       = new Padding(4, 6, 4, 0)
        };
        panelButtons.Controls.AddRange(new Control[] { btnNeu, btnSpeichern, btnDeaktivieren });

        splitContainer.Panel2.Controls.Add(tabDetail);
        splitContainer.Panel2.Controls.Add(panelButtons);

        // ── Form ──────────────────────────────────────────────────────────────
        Controls.Add(splitContainer);
        Controls.Add(panelTop);

        Load         += FrmCustomerList_Load;
        FormClosing  += FrmCustomerList_FormClosing;
    }

    // ── Designer-Hilfen ───────────────────────────────────────────────────────

    private static TableLayoutPanel BuildTwoColumnPanel() =>
        new()
        {
            Dock        = DockStyle.Fill,
            ColumnCount = 2,
            Padding     = new Padding(12),
            AutoScroll  = true,
            ColumnStyles = { new ColumnStyle(SizeType.Absolute, 150),
                             new ColumnStyle(SizeType.Percent, 100) }
        };

    private static TextBox AddRow(TableLayoutPanel panel, string label)
    {
        panel.Controls.Add(new Label
        {
            Text = label, Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin    = new Padding(0, 5, 8, 5)
        });
        var tb = new TextBox { Dock = DockStyle.Fill, Margin = new Padding(0, 5, 0, 5) };
        panel.Controls.Add(tb);
        return tb;
    }

    private static ComboBox AddComboRow(TableLayoutPanel panel, string label)
    {
        panel.Controls.Add(new Label
        {
            Text = label, Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin    = new Padding(0, 5, 8, 5)
        });
        var cb = new ComboBox { Dock = DockStyle.Fill, Margin = new Padding(0, 5, 0, 5), DropDownStyle = ComboBoxStyle.DropDownList };
        panel.Controls.Add(cb);
        return cb;
    }

    private static NumericUpDown AddNudRow(TableLayoutPanel panel, string label,
        decimal min, decimal max, int decimals = 0)
    {
        panel.Controls.Add(new Label
        {
            Text = label, Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin    = new Padding(0, 5, 8, 5)
        });
        var nud = new NumericUpDown
        {
            Dock = DockStyle.Fill, Margin = new Padding(0, 5, 0, 5),
            Minimum = min, Maximum = max, DecimalPlaces = decimals
        };
        panel.Controls.Add(nud);
        return nud;
    }

    private static CheckBox AddCheckRow(TableLayoutPanel panel, string label)
    {
        panel.Controls.Add(new Label
        {
            Text = label, Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin    = new Padding(0, 5, 8, 5)
        });
        var chk = new CheckBox { AutoSize = true, Margin = new Padding(0, 8, 0, 5) };
        panel.Controls.Add(chk);
        return chk;
    }
}