namespace DNR26V2.Forms.MasterData;

partial class FrmCustomerList
{
    private System.ComponentModel.IContainer components = null;

    // ── Toolbar ───────────────────────────────────────────────────────────────
    private Panel panelTop = null!;
    private Label lblSuche = null!;
    private TextBox txtSuche = null!;
    private ComboBox cmbRouteOben = null!;
    private Label lblFilter = null!;
    private ComboBox cmbFilterOben = null!;
    private CheckBox chkNurAktiv = null!;

    // ── Layout ────────────────────────────────────────────────────────────────
    private SplitContainer splitContainer = null!;
    private DataGridView dgwKunden = null!;
    private Panel panelDetail = null!;
    private TabControl tabDetail = null!;
    private TabPage tabStamm = null!;
    private TabPage tabAdresse = null!;
    private TabPage tabAltAdresse = null!;
    private TabPage tabEinstellungen = null!;
    private Panel panelButtons = null!;
    private Button btnNeu = null!;
    private Button btnSpeichern = null!;
    private Button btnDeaktivieren = null!;

    // ── Tab: Stammdaten ───────────────────────────────────────────────────────
    private TableLayoutPanel tlpStamm = null!;
    private Label lblKundennummer = null!;
    private TextBox txtKundennummer = null!;
    private Label lblKundenname = null!;
    private TextBox txtKundenname = null!;
    private Label lblName2 = null!;
    private TextBox txtName2 = null!;
    private Label lblInhaber = null!;
    private TextBox txtInhaber = null!;
    private Label lblTelefon = null!;
    private TextBox txtTelefon = null!;
    private Label lblHandy = null!;
    private TextBox txtHandy = null!;
    private Label lblEmail = null!;
    private TextBox txtEmail = null!;
    private Label lblNotizen = null!;
    private TextBox txtNotizen = null!;

    // ── Tab: Adresse ──────────────────────────────────────────────────────────
    private TableLayoutPanel tlpAdresse = null!;
    private Label lblAdresse = null!;
    private TextBox txtAdresse = null!;
    private Label lblAdresse2 = null!;
    private TextBox txtAdresse2 = null!;
    private Label lblPLZ = null!;
    private TextBox txtPLZ = null!;
    private Label lblOrt = null!;
    private TextBox txtOrt = null!;
    private Label lblLand = null!;
    private TextBox txtLand = null!;

    // ── Tab: Alt. Lieferadresse ───────────────────────────────────────────────
    private TableLayoutPanel tlpAltAdresse = null!;
    private Label lblAbweichend = null!;
    private CheckBox chkAbweichend = null!;
    private Label lblALName2 = null!;
    private TextBox txtALName2 = null!;
    private Label lblALInhaber = null!;
    private TextBox txtALInhaber = null!;
    private Label lblALAdresse = null!;
    private TextBox txtALAdresse = null!;
    private Label lblALAdresse2 = null!;
    private TextBox txtALAdresse2 = null!;
    private Label lblALPLZ = null!;
    private TextBox txtALPLZ = null!;
    private Label lblALOrt = null!;
    private TextBox txtALOrt = null!;
    private Label lblALLand = null!;
    private TextBox txtALLand = null!;

    // ── Tab: Einstellungen ────────────────────────────────────────────────────
    private TableLayoutPanel tlpEinstellungen = null!;
    private ComboBox cmbRoute = null!;
    private Label lblKundenfilter = null!;
    private ComboBox cmbKundenfilter = null!;
    private NumericUpDown nudLimit = null!;
    private Label lblPreisAusblenden = null!;
    private CheckBox chkPreisAusblenden = null!;
    private Label lblAktiv = null!;
    private CheckBox chkAktiv = null!;

    private CheckBox chkMo = null!;
    private CheckBox chkDi = null!;
    private CheckBox chkMi = null!;
    private CheckBox chkDo = null!;
    private CheckBox chkFr = null!;
    private CheckBox chkSa = null!;
    private CheckBox chkSo = null!;

    private TextBox txtGeraete1 = null!;
    private TextBox txtGeraete2 = null!;
    private TextBox txtGeraete3 = null!;
    private TextBox txtGeraete4 = null!;
    private TextBox txtGeraete5 = null!;

    // --- Liefertage - Konstanten für Tabelle und Steuerelemente ---
    private FlowLayoutPanel flpLiefertage = null!;
    private Label           lblLiefertageTitel  = null!;
    private TabPage         tabLeihgeraete      = null!;
    private TableLayoutPanel tlpLeihgeraete     = null!;
    private Label           lblLeihgeraeteTitel = null!;
    private Label           lblLeihgeraet1      = null!;
    private Label           lblLeihgeraet2      = null!;
    private Label           lblLeihgeraet3      = null!;
    private Label           lblLeihgeraet4      = null!;
    private Label           lblLeihgeraet5      = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        panelTop = new Panel();
        chkNurAktiv = new CheckBox();
        cmbFilterOben = new ComboBox();
        lblFilter = new Label();
        cmbRouteOben = new ComboBox();
        txtSuche = new TextBox();
        lblSuche = new Label();
        splitContainer = new SplitContainer();
        dgwKunden = new DataGridView();
        panelDetail = new Panel();
        tabDetail = new TabControl();
        tabStamm = new TabPage();
        tlpStamm = new TableLayoutPanel();
        lblKundennummer = new Label();
        txtKundennummer = new TextBox();
        lblKundenname = new Label();
        txtKundenname = new TextBox();
        lblName2 = new Label();
        txtName2 = new TextBox();
        lblInhaber = new Label();
        txtInhaber = new TextBox();
        lblTelefon = new Label();
        txtTelefon = new TextBox();
        lblHandy = new Label();
        txtHandy = new TextBox();
        lblEmail = new Label();
        txtEmail = new TextBox();
        lblNotizen = new Label();
        txtNotizen = new TextBox();
        tabAdresse = new TabPage();
        tlpAdresse = new TableLayoutPanel();
        lblAdresse = new Label();
        txtAdresse = new TextBox();
        lblAdresse2 = new Label();
        txtAdresse2 = new TextBox();
        lblPLZ = new Label();
        txtPLZ = new TextBox();
        lblOrt = new Label();
        txtOrt = new TextBox();
        lblLand = new Label();
        txtLand = new TextBox();
        tabAltAdresse = new TabPage();
        tlpAltAdresse = new TableLayoutPanel();
        lblAbweichend = new Label();
        chkAbweichend = new CheckBox();
        lblALName2 = new Label();
        txtALName2 = new TextBox();
        lblALInhaber = new Label();
        txtALInhaber = new TextBox();
        lblALAdresse = new Label();
        txtALAdresse = new TextBox();
        lblALAdresse2 = new Label();
        txtALAdresse2 = new TextBox();
        lblALPLZ = new Label();
        txtALPLZ = new TextBox();
        lblALOrt = new Label();
        txtALOrt = new TextBox();
        lblALLand = new Label();
        txtALLand = new TextBox();
        tabEinstellungen = new TabPage();
        tlpEinstellungen = new TableLayoutPanel();
        cmbTur = new ComboBox();
        lblStandartTour = new Label();
        lblRoutenfolge = new Label();
        cmbRoute = new ComboBox();
        lblKundenfilter = new Label();
        cmbKundenfilter = new ComboBox();
        lblPreisAusblenden = new Label();
        chkPreisAusblenden = new CheckBox();
        lblAktiv = new Label();
        chkAktiv = new CheckBox();
        cmbAusnahmeTur = new ComboBox();
        nudLimit = new NumericUpDown();
        lblLimit = new Label();
        lblAusnahmeTour = new Label();
        tabLiefertage = new TabPage();
        tlpLiefertage = new TableLayoutPanel();
        lblLiefertageTitel = new Label();
        flpLiefertage = new FlowLayoutPanel();
        lblLiefertage = new Label();
        flpDays = new FlowLayoutPanel();
        chkMo = new CheckBox();
        chkDi = new CheckBox();
        chkMi = new CheckBox();
        chkDo = new CheckBox();
        chkFr = new CheckBox();
        chkSa = new CheckBox();
        chkSo = new CheckBox();
        spacer = new Panel();
        tabLeihgeraete = new TabPage();
        tlpLeihgeraete = new TableLayoutPanel();
        lblLeihgeraeteTitel = new Label();
        lblLeihgeraet1 = new Label();
        txtGeraete1 = new TextBox();
        lblLeihgeraet2 = new Label();
        txtGeraete2 = new TextBox();
        lblLeihgeraet3 = new Label();
        txtGeraete3 = new TextBox();
        lblLeihgeraet4 = new Label();
        txtGeraete4 = new TextBox();
        lblLeihgeraet5 = new Label();
        txtGeraete5 = new TextBox();
        panelButtons = new Panel();
        btnDeaktivieren = new Button();
        btnSpeichern = new Button();
        btnNeu = new Button();
        panelTop.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
        splitContainer.Panel1.SuspendLayout();
        splitContainer.Panel2.SuspendLayout();
        splitContainer.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgwKunden).BeginInit();
        panelDetail.SuspendLayout();
        tabDetail.SuspendLayout();
        tabStamm.SuspendLayout();
        tlpStamm.SuspendLayout();
        tabAdresse.SuspendLayout();
        tlpAdresse.SuspendLayout();
        tabAltAdresse.SuspendLayout();
        tlpAltAdresse.SuspendLayout();
        tabEinstellungen.SuspendLayout();
        tlpEinstellungen.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudLimit).BeginInit();
        tabLiefertage.SuspendLayout();
        tlpLiefertage.SuspendLayout();
        flpDays.SuspendLayout();
        tabLeihgeraete.SuspendLayout();
        tlpLeihgeraete.SuspendLayout();
        panelButtons.SuspendLayout();
        SuspendLayout();
        // 
        // panelTop
        // 
        panelTop.Controls.Add(chkNurAktiv);
        panelTop.Controls.Add(cmbFilterOben);
        panelTop.Controls.Add(lblFilter);
        panelTop.Controls.Add(cmbRouteOben);
        panelTop.Controls.Add(txtSuche);
        panelTop.Controls.Add(lblSuche);
        panelTop.Dock = DockStyle.Top;
        panelTop.Location = new Point(0, 0);
        panelTop.Name = "panelTop";
        panelTop.Size = new Size(1081, 46);
        panelTop.TabIndex = 0;
        // 
        // chkNurAktiv
        // 
        chkNurAktiv.AutoSize = true;
        chkNurAktiv.Checked = true;
        chkNurAktiv.CheckState = CheckState.Checked;
        chkNurAktiv.Location = new Point(704, 12);
        chkNurAktiv.Name = "chkNurAktiv";
        chkNurAktiv.Size = new Size(74, 19);
        chkNurAktiv.TabIndex = 3;
        chkNurAktiv.Text = "Nur aktiv";
        // 
        // cmbFilterOben
        // 
        cmbFilterOben.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbFilterOben.Location = new Point(548, 10);
        cmbFilterOben.Name = "cmbFilterOben";
        cmbFilterOben.Size = new Size(140, 23);
        cmbFilterOben.TabIndex = 2;
        // 
        // lblFilter
        // 
        lblFilter.Location = new Point(492, 13);
        lblFilter.Name = "lblFilter";
        lblFilter.Size = new Size(52, 20);
        lblFilter.TabIndex = 2;
        lblFilter.Text = "Gruppe:";
        // 
        // cmbRouteOben
        // 
        cmbRouteOben.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbRouteOben.Location = new Point(318, 10);
        cmbRouteOben.Name = "cmbRouteOben";
        cmbRouteOben.Size = new Size(160, 23);
        cmbRouteOben.TabIndex = 1;
        // 
        // txtSuche
        // 
        txtSuche.Location = new Point(56, 10);
        txtSuche.Name = "txtSuche";
        txtSuche.PlaceholderText = "Name oder Nummer…";
        txtSuche.Size = new Size(200, 23);
        txtSuche.TabIndex = 0;
        // 
        // lblSuche
        // 
        lblSuche.Location = new Point(8, 13);
        lblSuche.Name = "lblSuche";
        lblSuche.Size = new Size(44, 20);
        lblSuche.TabIndex = 0;
        lblSuche.Text = "Suche:";
        // 
        // splitContainer
        // 
        splitContainer.Dock = DockStyle.Fill;
        splitContainer.Location = new Point(0, 46);
        splitContainer.Name = "splitContainer";
        // 
        // splitContainer.Panel1
        // 
        splitContainer.Panel1.Controls.Add(dgwKunden);
        splitContainer.Panel1MinSize = 300;
        // 
        // splitContainer.Panel2
        // 
        splitContainer.Panel2.Controls.Add(panelDetail);
        splitContainer.Panel2MinSize = 380;
        splitContainer.Size = new Size(1081, 626);
        splitContainer.SplitterDistance = 659;
        splitContainer.TabIndex = 1;
        // 
        // dgwKunden
        // 
        dgwKunden.AllowUserToAddRows = false;
        dgwKunden.AllowUserToDeleteRows = false;
        dgwKunden.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgwKunden.BackgroundColor = SystemColors.Window;
        dgwKunden.Dock = DockStyle.Fill;
        dgwKunden.Location = new Point(0, 0);
        dgwKunden.MultiSelect = false;
        dgwKunden.Name = "dgwKunden";
        dgwKunden.ReadOnly = true;
        dgwKunden.RowHeadersVisible = false;
        dgwKunden.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgwKunden.Size = new Size(659, 626);
        dgwKunden.TabIndex = 0;
        // 
        // panelDetail
        // 
        panelDetail.Controls.Add(tabDetail);
        panelDetail.Controls.Add(panelButtons);
        panelDetail.Dock = DockStyle.Fill;
        panelDetail.Location = new Point(0, 0);
        panelDetail.Name = "panelDetail";
        panelDetail.Padding = new Padding(4);
        panelDetail.Size = new Size(418, 626);
        panelDetail.TabIndex = 0;
        // 
        // tabDetail
        // 
        tabDetail.Controls.Add(tabStamm);
        tabDetail.Controls.Add(tabAdresse);
        tabDetail.Controls.Add(tabAltAdresse);
        tabDetail.Controls.Add(tabEinstellungen);
        tabDetail.Controls.Add(tabLiefertage);
        tabDetail.Controls.Add(tabLeihgeraete);
        tabDetail.Dock = DockStyle.Fill;
        tabDetail.Location = new Point(4, 4);
        tabDetail.Name = "tabDetail";
        tabDetail.SelectedIndex = 0;
        tabDetail.Size = new Size(410, 570);
        tabDetail.TabIndex = 0;
        // 
        // tabStamm
        // 
        tabStamm.Controls.Add(tlpStamm);
        tabStamm.Location = new Point(4, 24);
        tabStamm.Name = "tabStamm";
        tabStamm.Padding = new Padding(3);
        tabStamm.Size = new Size(402, 542);
        tabStamm.TabIndex = 0;
        tabStamm.Text = "Stammdaten";
        // 
        // tlpStamm
        // 
        tlpStamm.AutoScroll = true;
        tlpStamm.AutoSize = true;
        tlpStamm.ColumnCount = 2;
        tlpStamm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
        tlpStamm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpStamm.Controls.Add(lblKundennummer, 0, 0);
        tlpStamm.Controls.Add(txtKundennummer, 1, 0);
        tlpStamm.Controls.Add(lblKundenname, 0, 1);
        tlpStamm.Controls.Add(txtKundenname, 1, 1);
        tlpStamm.Controls.Add(lblName2, 0, 2);
        tlpStamm.Controls.Add(txtName2, 1, 2);
        tlpStamm.Controls.Add(lblInhaber, 0, 3);
        tlpStamm.Controls.Add(txtInhaber, 1, 3);
        tlpStamm.Controls.Add(lblTelefon, 0, 4);
        tlpStamm.Controls.Add(txtTelefon, 1, 4);
        tlpStamm.Controls.Add(lblHandy, 0, 5);
        tlpStamm.Controls.Add(txtHandy, 1, 5);
        tlpStamm.Controls.Add(lblEmail, 0, 6);
        tlpStamm.Controls.Add(txtEmail, 1, 6);
        tlpStamm.Controls.Add(lblNotizen, 0, 7);
        tlpStamm.Controls.Add(txtNotizen, 1, 7);
        tlpStamm.Dock = DockStyle.Fill;
        tlpStamm.Location = new Point(3, 3);
        tlpStamm.Name = "tlpStamm";
        tlpStamm.Padding = new Padding(8);
        tlpStamm.RowCount = 9;
        tlpStamm.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpStamm.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpStamm.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpStamm.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpStamm.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpStamm.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpStamm.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpStamm.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpStamm.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        tlpStamm.Size = new Size(396, 536);
        tlpStamm.TabIndex = 0;
        // 
        // lblKundennummer
        // 
        lblKundennummer.Dock = DockStyle.Fill;
        lblKundennummer.Location = new Point(8, 13);
        lblKundennummer.Margin = new Padding(0, 5, 8, 5);
        lblKundennummer.Name = "lblKundennummer";
        lblKundennummer.Size = new Size(152, 23);
        lblKundennummer.TabIndex = 0;
        lblKundennummer.Text = "Kundennummer *";
        lblKundennummer.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtKundennummer
        // 
        txtKundennummer.CharacterCasing = CharacterCasing.Upper;
        txtKundennummer.Dock = DockStyle.Fill;
        txtKundennummer.Location = new Point(168, 13);
        txtKundennummer.Margin = new Padding(0, 5, 0, 5);
        txtKundennummer.Name = "txtKundennummer";
        txtKundennummer.Size = new Size(220, 23);
        txtKundennummer.TabIndex = 1;
        // 
        // lblKundenname
        // 
        lblKundenname.Dock = DockStyle.Fill;
        lblKundenname.Location = new Point(8, 46);
        lblKundenname.Margin = new Padding(0, 5, 8, 5);
        lblKundenname.Name = "lblKundenname";
        lblKundenname.Size = new Size(152, 23);
        lblKundenname.TabIndex = 2;
        lblKundenname.Text = "Kundenname *";
        lblKundenname.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtKundenname
        // 
        txtKundenname.Dock = DockStyle.Fill;
        txtKundenname.Location = new Point(168, 46);
        txtKundenname.Margin = new Padding(0, 5, 0, 5);
        txtKundenname.Name = "txtKundenname";
        txtKundenname.Size = new Size(220, 23);
        txtKundenname.TabIndex = 3;
        // 
        // lblName2
        // 
        lblName2.Dock = DockStyle.Fill;
        lblName2.Location = new Point(8, 79);
        lblName2.Margin = new Padding(0, 5, 8, 5);
        lblName2.Name = "lblName2";
        lblName2.Size = new Size(152, 23);
        lblName2.TabIndex = 4;
        lblName2.Text = "Name 2";
        lblName2.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtName2
        // 
        txtName2.Dock = DockStyle.Fill;
        txtName2.Location = new Point(168, 79);
        txtName2.Margin = new Padding(0, 5, 0, 5);
        txtName2.Name = "txtName2";
        txtName2.Size = new Size(220, 23);
        txtName2.TabIndex = 5;
        // 
        // lblInhaber
        // 
        lblInhaber.Dock = DockStyle.Fill;
        lblInhaber.Location = new Point(8, 112);
        lblInhaber.Margin = new Padding(0, 5, 8, 5);
        lblInhaber.Name = "lblInhaber";
        lblInhaber.Size = new Size(152, 23);
        lblInhaber.TabIndex = 6;
        lblInhaber.Text = "Inhaber";
        lblInhaber.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtInhaber
        // 
        txtInhaber.Dock = DockStyle.Fill;
        txtInhaber.Location = new Point(168, 112);
        txtInhaber.Margin = new Padding(0, 5, 0, 5);
        txtInhaber.Name = "txtInhaber";
        txtInhaber.Size = new Size(220, 23);
        txtInhaber.TabIndex = 7;
        // 
        // lblTelefon
        // 
        lblTelefon.Dock = DockStyle.Fill;
        lblTelefon.Location = new Point(8, 145);
        lblTelefon.Margin = new Padding(0, 5, 8, 5);
        lblTelefon.Name = "lblTelefon";
        lblTelefon.Size = new Size(152, 23);
        lblTelefon.TabIndex = 8;
        lblTelefon.Text = "Telefon";
        lblTelefon.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtTelefon
        // 
        txtTelefon.Dock = DockStyle.Fill;
        txtTelefon.Location = new Point(168, 145);
        txtTelefon.Margin = new Padding(0, 5, 0, 5);
        txtTelefon.Name = "txtTelefon";
        txtTelefon.Size = new Size(220, 23);
        txtTelefon.TabIndex = 9;
        // 
        // lblHandy
        // 
        lblHandy.Dock = DockStyle.Fill;
        lblHandy.Location = new Point(8, 178);
        lblHandy.Margin = new Padding(0, 5, 8, 5);
        lblHandy.Name = "lblHandy";
        lblHandy.Size = new Size(152, 23);
        lblHandy.TabIndex = 10;
        lblHandy.Text = "Handy";
        lblHandy.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtHandy
        // 
        txtHandy.Dock = DockStyle.Fill;
        txtHandy.Location = new Point(168, 178);
        txtHandy.Margin = new Padding(0, 5, 0, 5);
        txtHandy.Name = "txtHandy";
        txtHandy.Size = new Size(220, 23);
        txtHandy.TabIndex = 11;
        // 
        // lblEmail
        // 
        lblEmail.Dock = DockStyle.Fill;
        lblEmail.Location = new Point(8, 211);
        lblEmail.Margin = new Padding(0, 5, 8, 5);
        lblEmail.Name = "lblEmail";
        lblEmail.Size = new Size(152, 23);
        lblEmail.TabIndex = 12;
        lblEmail.Text = "E-Mail";
        lblEmail.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtEmail
        // 
        txtEmail.Dock = DockStyle.Fill;
        txtEmail.Location = new Point(168, 211);
        txtEmail.Margin = new Padding(0, 5, 0, 5);
        txtEmail.Name = "txtEmail";
        txtEmail.Size = new Size(220, 23);
        txtEmail.TabIndex = 13;
        // 
        // lblNotizen
        // 
        lblNotizen.Dock = DockStyle.Fill;
        lblNotizen.Location = new Point(8, 244);
        lblNotizen.Margin = new Padding(0, 5, 8, 5);
        lblNotizen.Name = "lblNotizen";
        lblNotizen.Size = new Size(152, 23);
        lblNotizen.TabIndex = 14;
        lblNotizen.Text = "Notizen";
        lblNotizen.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtNotizen
        // 
        txtNotizen.Dock = DockStyle.Fill;
        txtNotizen.Location = new Point(168, 244);
        txtNotizen.Margin = new Padding(0, 5, 0, 5);
        txtNotizen.Multiline = true;
        txtNotizen.Name = "txtNotizen";
        txtNotizen.Size = new Size(220, 23);
        txtNotizen.TabIndex = 15;
        // 
        // tabAdresse
        // 
        tabAdresse.Controls.Add(tlpAdresse);
        tabAdresse.Location = new Point(4, 24);
        tabAdresse.Name = "tabAdresse";
        tabAdresse.Padding = new Padding(3);
        tabAdresse.Size = new Size(402, 542);
        tabAdresse.TabIndex = 1;
        tabAdresse.Text = "Adresse";
        // 
        // tlpAdresse
        // 
        tlpAdresse.AutoScroll = true;
        tlpAdresse.AutoSize = true;
        tlpAdresse.ColumnCount = 2;
        tlpAdresse.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
        tlpAdresse.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpAdresse.Controls.Add(lblAdresse, 0, 0);
        tlpAdresse.Controls.Add(txtAdresse, 1, 0);
        tlpAdresse.Controls.Add(lblAdresse2, 0, 1);
        tlpAdresse.Controls.Add(txtAdresse2, 1, 1);
        tlpAdresse.Controls.Add(lblPLZ, 0, 2);
        tlpAdresse.Controls.Add(txtPLZ, 1, 2);
        tlpAdresse.Controls.Add(lblOrt, 0, 3);
        tlpAdresse.Controls.Add(txtOrt, 1, 3);
        tlpAdresse.Controls.Add(lblLand, 0, 4);
        tlpAdresse.Controls.Add(txtLand, 1, 4);
        tlpAdresse.Dock = DockStyle.Fill;
        tlpAdresse.Location = new Point(3, 3);
        tlpAdresse.Name = "tlpAdresse";
        tlpAdresse.Padding = new Padding(8);
        tlpAdresse.RowCount = 6;
        tlpAdresse.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpAdresse.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpAdresse.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpAdresse.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpAdresse.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpAdresse.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpAdresse.Size = new Size(396, 536);
        tlpAdresse.TabIndex = 0;
        // 
        // lblAdresse
        // 
        lblAdresse.Dock = DockStyle.Fill;
        lblAdresse.Location = new Point(8, 13);
        lblAdresse.Margin = new Padding(0, 5, 8, 5);
        lblAdresse.Name = "lblAdresse";
        lblAdresse.Size = new Size(152, 23);
        lblAdresse.TabIndex = 0;
        lblAdresse.Text = "Adresse";
        lblAdresse.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtAdresse
        // 
        txtAdresse.Dock = DockStyle.Fill;
        txtAdresse.Location = new Point(168, 13);
        txtAdresse.Margin = new Padding(0, 5, 0, 5);
        txtAdresse.Name = "txtAdresse";
        txtAdresse.Size = new Size(220, 23);
        txtAdresse.TabIndex = 1;
        // 
        // lblAdresse2
        // 
        lblAdresse2.Dock = DockStyle.Fill;
        lblAdresse2.Location = new Point(8, 46);
        lblAdresse2.Margin = new Padding(0, 5, 8, 5);
        lblAdresse2.Name = "lblAdresse2";
        lblAdresse2.Size = new Size(152, 23);
        lblAdresse2.TabIndex = 2;
        lblAdresse2.Text = "Adresse 2";
        lblAdresse2.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtAdresse2
        // 
        txtAdresse2.Dock = DockStyle.Fill;
        txtAdresse2.Location = new Point(168, 46);
        txtAdresse2.Margin = new Padding(0, 5, 0, 5);
        txtAdresse2.Name = "txtAdresse2";
        txtAdresse2.Size = new Size(220, 23);
        txtAdresse2.TabIndex = 3;
        // 
        // lblPLZ
        // 
        lblPLZ.Dock = DockStyle.Fill;
        lblPLZ.Location = new Point(8, 79);
        lblPLZ.Margin = new Padding(0, 5, 8, 5);
        lblPLZ.Name = "lblPLZ";
        lblPLZ.Size = new Size(152, 23);
        lblPLZ.TabIndex = 4;
        lblPLZ.Text = "PLZ";
        lblPLZ.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtPLZ
        // 
        txtPLZ.Dock = DockStyle.Fill;
        txtPLZ.Location = new Point(168, 79);
        txtPLZ.Margin = new Padding(0, 5, 0, 5);
        txtPLZ.Name = "txtPLZ";
        txtPLZ.Size = new Size(220, 23);
        txtPLZ.TabIndex = 5;
        // 
        // lblOrt
        // 
        lblOrt.Dock = DockStyle.Fill;
        lblOrt.Location = new Point(8, 112);
        lblOrt.Margin = new Padding(0, 5, 8, 5);
        lblOrt.Name = "lblOrt";
        lblOrt.Size = new Size(152, 23);
        lblOrt.TabIndex = 6;
        lblOrt.Text = "Ort";
        lblOrt.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtOrt
        // 
        txtOrt.Dock = DockStyle.Fill;
        txtOrt.Location = new Point(168, 112);
        txtOrt.Margin = new Padding(0, 5, 0, 5);
        txtOrt.Name = "txtOrt";
        txtOrt.Size = new Size(220, 23);
        txtOrt.TabIndex = 7;
        // 
        // lblLand
        // 
        lblLand.Dock = DockStyle.Fill;
        lblLand.Location = new Point(8, 145);
        lblLand.Margin = new Padding(0, 5, 8, 5);
        lblLand.Name = "lblLand";
        lblLand.Size = new Size(152, 23);
        lblLand.TabIndex = 8;
        lblLand.Text = "Land";
        lblLand.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtLand
        // 
        txtLand.Dock = DockStyle.Fill;
        txtLand.Location = new Point(168, 145);
        txtLand.Margin = new Padding(0, 5, 0, 5);
        txtLand.Name = "txtLand";
        txtLand.Size = new Size(220, 23);
        txtLand.TabIndex = 9;
        // 
        // tabAltAdresse
        // 
        tabAltAdresse.Controls.Add(tlpAltAdresse);
        tabAltAdresse.Location = new Point(4, 24);
        tabAltAdresse.Name = "tabAltAdresse";
        tabAltAdresse.Padding = new Padding(3);
        tabAltAdresse.Size = new Size(402, 542);
        tabAltAdresse.TabIndex = 2;
        tabAltAdresse.Text = "Alt. Lieferadresse";
        // 
        // tlpAltAdresse
        // 
        tlpAltAdresse.AutoScroll = true;
        tlpAltAdresse.AutoSize = true;
        tlpAltAdresse.ColumnCount = 2;
        tlpAltAdresse.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
        tlpAltAdresse.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpAltAdresse.Controls.Add(lblAbweichend, 0, 0);
        tlpAltAdresse.Controls.Add(chkAbweichend, 1, 0);
        tlpAltAdresse.Controls.Add(lblALName2, 0, 1);
        tlpAltAdresse.Controls.Add(txtALName2, 1, 1);
        tlpAltAdresse.Controls.Add(lblALInhaber, 0, 2);
        tlpAltAdresse.Controls.Add(txtALInhaber, 1, 2);
        tlpAltAdresse.Controls.Add(lblALAdresse, 0, 3);
        tlpAltAdresse.Controls.Add(txtALAdresse, 1, 3);
        tlpAltAdresse.Controls.Add(lblALAdresse2, 0, 4);
        tlpAltAdresse.Controls.Add(txtALAdresse2, 1, 4);
        tlpAltAdresse.Controls.Add(lblALPLZ, 0, 5);
        tlpAltAdresse.Controls.Add(txtALPLZ, 1, 5);
        tlpAltAdresse.Controls.Add(lblALOrt, 0, 6);
        tlpAltAdresse.Controls.Add(txtALOrt, 1, 6);
        tlpAltAdresse.Controls.Add(lblALLand, 0, 7);
        tlpAltAdresse.Controls.Add(txtALLand, 1, 7);
        tlpAltAdresse.Dock = DockStyle.Fill;
        tlpAltAdresse.Location = new Point(3, 3);
        tlpAltAdresse.Name = "tlpAltAdresse";
        tlpAltAdresse.Padding = new Padding(8);
        tlpAltAdresse.RowCount = 9;
        tlpAltAdresse.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpAltAdresse.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpAltAdresse.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpAltAdresse.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpAltAdresse.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpAltAdresse.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpAltAdresse.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpAltAdresse.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpAltAdresse.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        tlpAltAdresse.Size = new Size(396, 536);
        tlpAltAdresse.TabIndex = 0;
        // 
        // lblAbweichend
        // 
        lblAbweichend.Dock = DockStyle.Fill;
        lblAbweichend.Location = new Point(8, 13);
        lblAbweichend.Margin = new Padding(0, 5, 8, 5);
        lblAbweichend.Name = "lblAbweichend";
        lblAbweichend.Size = new Size(152, 23);
        lblAbweichend.TabIndex = 0;
        lblAbweichend.Text = "Abweichende Adresse";
        lblAbweichend.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // chkAbweichend
        // 
        chkAbweichend.Location = new Point(168, 16);
        chkAbweichend.Margin = new Padding(0, 8, 0, 5);
        chkAbweichend.Name = "chkAbweichend";
        chkAbweichend.Size = new Size(104, 20);
        chkAbweichend.TabIndex = 1;
        // 
        // lblALName2
        // 
        lblALName2.Dock = DockStyle.Fill;
        lblALName2.Location = new Point(8, 46);
        lblALName2.Margin = new Padding(0, 5, 8, 5);
        lblALName2.Name = "lblALName2";
        lblALName2.Size = new Size(152, 23);
        lblALName2.TabIndex = 2;
        lblALName2.Text = "Name 2";
        lblALName2.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtALName2
        // 
        txtALName2.Dock = DockStyle.Fill;
        txtALName2.Location = new Point(168, 46);
        txtALName2.Margin = new Padding(0, 5, 0, 5);
        txtALName2.Name = "txtALName2";
        txtALName2.Size = new Size(220, 23);
        txtALName2.TabIndex = 3;
        // 
        // lblALInhaber
        // 
        lblALInhaber.Dock = DockStyle.Fill;
        lblALInhaber.Location = new Point(8, 79);
        lblALInhaber.Margin = new Padding(0, 5, 8, 5);
        lblALInhaber.Name = "lblALInhaber";
        lblALInhaber.Size = new Size(152, 23);
        lblALInhaber.TabIndex = 4;
        lblALInhaber.Text = "Inhaber";
        lblALInhaber.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtALInhaber
        // 
        txtALInhaber.Dock = DockStyle.Fill;
        txtALInhaber.Location = new Point(168, 79);
        txtALInhaber.Margin = new Padding(0, 5, 0, 5);
        txtALInhaber.Name = "txtALInhaber";
        txtALInhaber.Size = new Size(220, 23);
        txtALInhaber.TabIndex = 5;
        // 
        // lblALAdresse
        // 
        lblALAdresse.Dock = DockStyle.Fill;
        lblALAdresse.Location = new Point(8, 112);
        lblALAdresse.Margin = new Padding(0, 5, 8, 5);
        lblALAdresse.Name = "lblALAdresse";
        lblALAdresse.Size = new Size(152, 23);
        lblALAdresse.TabIndex = 6;
        lblALAdresse.Text = "Adresse";
        lblALAdresse.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtALAdresse
        // 
        txtALAdresse.Dock = DockStyle.Fill;
        txtALAdresse.Location = new Point(168, 112);
        txtALAdresse.Margin = new Padding(0, 5, 0, 5);
        txtALAdresse.Name = "txtALAdresse";
        txtALAdresse.Size = new Size(220, 23);
        txtALAdresse.TabIndex = 7;
        // 
        // lblALAdresse2
        // 
        lblALAdresse2.Dock = DockStyle.Fill;
        lblALAdresse2.Location = new Point(8, 145);
        lblALAdresse2.Margin = new Padding(0, 5, 8, 5);
        lblALAdresse2.Name = "lblALAdresse2";
        lblALAdresse2.Size = new Size(152, 23);
        lblALAdresse2.TabIndex = 8;
        lblALAdresse2.Text = "Adresse 2";
        lblALAdresse2.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtALAdresse2
        // 
        txtALAdresse2.Dock = DockStyle.Fill;
        txtALAdresse2.Location = new Point(168, 145);
        txtALAdresse2.Margin = new Padding(0, 5, 0, 5);
        txtALAdresse2.Name = "txtALAdresse2";
        txtALAdresse2.Size = new Size(220, 23);
        txtALAdresse2.TabIndex = 9;
        // 
        // lblALPLZ
        // 
        lblALPLZ.Dock = DockStyle.Fill;
        lblALPLZ.Location = new Point(8, 178);
        lblALPLZ.Margin = new Padding(0, 5, 8, 5);
        lblALPLZ.Name = "lblALPLZ";
        lblALPLZ.Size = new Size(152, 23);
        lblALPLZ.TabIndex = 10;
        lblALPLZ.Text = "PLZ";
        lblALPLZ.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtALPLZ
        // 
        txtALPLZ.Dock = DockStyle.Fill;
        txtALPLZ.Location = new Point(168, 178);
        txtALPLZ.Margin = new Padding(0, 5, 0, 5);
        txtALPLZ.Name = "txtALPLZ";
        txtALPLZ.Size = new Size(220, 23);
        txtALPLZ.TabIndex = 11;
        // 
        // lblALOrt
        // 
        lblALOrt.Dock = DockStyle.Fill;
        lblALOrt.Location = new Point(8, 211);
        lblALOrt.Margin = new Padding(0, 5, 8, 5);
        lblALOrt.Name = "lblALOrt";
        lblALOrt.Size = new Size(152, 23);
        lblALOrt.TabIndex = 12;
        lblALOrt.Text = "Ort";
        lblALOrt.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtALOrt
        // 
        txtALOrt.Dock = DockStyle.Fill;
        txtALOrt.Location = new Point(168, 211);
        txtALOrt.Margin = new Padding(0, 5, 0, 5);
        txtALOrt.Name = "txtALOrt";
        txtALOrt.Size = new Size(220, 23);
        txtALOrt.TabIndex = 13;
        // 
        // lblALLand
        // 
        lblALLand.Dock = DockStyle.Fill;
        lblALLand.Location = new Point(8, 244);
        lblALLand.Margin = new Padding(0, 5, 8, 5);
        lblALLand.Name = "lblALLand";
        lblALLand.Size = new Size(152, 23);
        lblALLand.TabIndex = 14;
        lblALLand.Text = "Land";
        lblALLand.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtALLand
        // 
        txtALLand.Dock = DockStyle.Fill;
        txtALLand.Location = new Point(168, 244);
        txtALLand.Margin = new Padding(0, 5, 0, 5);
        txtALLand.Name = "txtALLand";
        txtALLand.Size = new Size(220, 23);
        txtALLand.TabIndex = 15;
        // 
        // tabEinstellungen
        // 
        tabEinstellungen.Controls.Add(tlpEinstellungen);
        tabEinstellungen.Location = new Point(4, 24);
        tabEinstellungen.Name = "tabEinstellungen";
        tabEinstellungen.Padding = new Padding(3);
        tabEinstellungen.Size = new Size(402, 542);
        tabEinstellungen.TabIndex = 3;
        tabEinstellungen.Text = "Einstellungen";
        // 
        // tlpEinstellungen
        // 
        tlpEinstellungen.AutoScroll = true;
        tlpEinstellungen.AutoSize = true;
        tlpEinstellungen.ColumnCount = 2;
        tlpEinstellungen.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
        tlpEinstellungen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpEinstellungen.Controls.Add(cmbTur, 1, 2);
        tlpEinstellungen.Controls.Add(lblStandartTour, 0, 2);
        tlpEinstellungen.Controls.Add(lblRoutenfolge, 0, 0);
        tlpEinstellungen.Controls.Add(cmbRoute, 1, 0);
        tlpEinstellungen.Controls.Add(lblKundenfilter, 0, 1);
        tlpEinstellungen.Controls.Add(cmbKundenfilter, 1, 1);
        tlpEinstellungen.Controls.Add(lblPreisAusblenden, 0, 5);
        tlpEinstellungen.Controls.Add(chkPreisAusblenden, 1, 5);
        tlpEinstellungen.Controls.Add(lblAktiv, 0, 6);
        tlpEinstellungen.Controls.Add(chkAktiv, 1, 6);
        tlpEinstellungen.Controls.Add(cmbAusnahmeTur, 1, 3);
        tlpEinstellungen.Controls.Add(nudLimit, 1, 4);
        tlpEinstellungen.Controls.Add(lblLimit, 0, 4);
        tlpEinstellungen.Controls.Add(lblAusnahmeTour, 0, 3);
        tlpEinstellungen.Dock = DockStyle.Fill;
        tlpEinstellungen.Location = new Point(3, 3);
        tlpEinstellungen.Name = "tlpEinstellungen";
        tlpEinstellungen.Padding = new Padding(8);
        tlpEinstellungen.RowCount = 8;
        tlpEinstellungen.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpEinstellungen.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpEinstellungen.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpEinstellungen.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpEinstellungen.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpEinstellungen.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpEinstellungen.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpEinstellungen.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpEinstellungen.Size = new Size(396, 536);
        tlpEinstellungen.TabIndex = 0;
        // 
        // cmbTur
        // 
        cmbTur.Dock = DockStyle.Fill;
        cmbTur.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbTur.Location = new Point(168, 79);
        cmbTur.Margin = new Padding(0, 5, 0, 5);
        cmbTur.Name = "cmbTur";
        cmbTur.Size = new Size(220, 23);
        cmbTur.TabIndex = 19;
        // 
        // lblStandartTour
        // 
        lblStandartTour.Dock = DockStyle.Fill;
        lblStandartTour.Location = new Point(8, 79);
        lblStandartTour.Margin = new Padding(0, 5, 8, 5);
        lblStandartTour.Name = "lblStandartTour";
        lblStandartTour.Size = new Size(152, 23);
        lblStandartTour.TabIndex = 18;
        lblStandartTour.Text = "Standard-Tour";
        lblStandartTour.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblRoutenfolge
        // 
        lblRoutenfolge.Dock = DockStyle.Fill;
        lblRoutenfolge.Location = new Point(8, 13);
        lblRoutenfolge.Margin = new Padding(0, 5, 8, 5);
        lblRoutenfolge.Name = "lblRoutenfolge";
        lblRoutenfolge.Size = new Size(152, 23);
        lblRoutenfolge.TabIndex = 17;
        lblRoutenfolge.Text = "Routenfolge";
        lblRoutenfolge.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cmbRoute
        // 
        cmbRoute.Dock = DockStyle.Fill;
        cmbRoute.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbRoute.Location = new Point(168, 13);
        cmbRoute.Margin = new Padding(0, 5, 0, 5);
        cmbRoute.Name = "cmbRoute";
        cmbRoute.Size = new Size(220, 23);
        cmbRoute.TabIndex = 1;
        // 
        // lblKundenfilter
        // 
        lblKundenfilter.Dock = DockStyle.Fill;
        lblKundenfilter.Location = new Point(8, 46);
        lblKundenfilter.Margin = new Padding(0, 5, 8, 5);
        lblKundenfilter.Name = "lblKundenfilter";
        lblKundenfilter.Size = new Size(152, 23);
        lblKundenfilter.TabIndex = 2;
        lblKundenfilter.Text = "Kundengruppe";
        lblKundenfilter.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cmbKundenfilter
        // 
        cmbKundenfilter.Dock = DockStyle.Fill;
        cmbKundenfilter.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbKundenfilter.Location = new Point(168, 46);
        cmbKundenfilter.Margin = new Padding(0, 5, 0, 5);
        cmbKundenfilter.Name = "cmbKundenfilter";
        cmbKundenfilter.Size = new Size(220, 23);
        cmbKundenfilter.TabIndex = 3;
        // 
        // lblPreisAusblenden
        // 
        lblPreisAusblenden.Dock = DockStyle.Fill;
        lblPreisAusblenden.Location = new Point(8, 178);
        lblPreisAusblenden.Margin = new Padding(0, 5, 8, 5);
        lblPreisAusblenden.Name = "lblPreisAusblenden";
        lblPreisAusblenden.Size = new Size(152, 23);
        lblPreisAusblenden.TabIndex = 10;
        lblPreisAusblenden.Text = "Preise ausblenden";
        lblPreisAusblenden.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // chkPreisAusblenden
        // 
        chkPreisAusblenden.Location = new Point(168, 181);
        chkPreisAusblenden.Margin = new Padding(0, 8, 0, 5);
        chkPreisAusblenden.Name = "chkPreisAusblenden";
        chkPreisAusblenden.Size = new Size(104, 20);
        chkPreisAusblenden.TabIndex = 11;
        // 
        // lblAktiv
        // 
        lblAktiv.Dock = DockStyle.Fill;
        lblAktiv.Location = new Point(8, 211);
        lblAktiv.Margin = new Padding(0, 5, 8, 5);
        lblAktiv.Name = "lblAktiv";
        lblAktiv.Size = new Size(152, 23);
        lblAktiv.TabIndex = 12;
        lblAktiv.Text = "Aktiv";
        lblAktiv.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // chkAktiv
        // 
        chkAktiv.Location = new Point(168, 214);
        chkAktiv.Margin = new Padding(0, 8, 0, 5);
        chkAktiv.Name = "chkAktiv";
        chkAktiv.Size = new Size(104, 20);
        chkAktiv.TabIndex = 13;
        // 
        // cmbAusnahmeTur
        // 
        cmbAusnahmeTur.Dock = DockStyle.Fill;
        cmbAusnahmeTur.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbAusnahmeTur.Location = new Point(168, 112);
        cmbAusnahmeTur.Margin = new Padding(0, 5, 0, 5);
        cmbAusnahmeTur.Name = "cmbAusnahmeTur";
        cmbAusnahmeTur.Size = new Size(220, 23);
        cmbAusnahmeTur.TabIndex = 15;
        // 
        // nudLimit
        // 
        nudLimit.DecimalPlaces = 2;
        nudLimit.Dock = DockStyle.Fill;
        nudLimit.Location = new Point(168, 145);
        nudLimit.Margin = new Padding(0, 5, 0, 5);
        nudLimit.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
        nudLimit.Name = "nudLimit";
        nudLimit.Size = new Size(220, 23);
        nudLimit.TabIndex = 7;
        // 
        // lblLimit
        // 
        lblLimit.Dock = DockStyle.Fill;
        lblLimit.Location = new Point(8, 145);
        lblLimit.Margin = new Padding(0, 5, 8, 5);
        lblLimit.Name = "lblLimit";
        lblLimit.Size = new Size(152, 23);
        lblLimit.TabIndex = 20;
        lblLimit.Text = "Kreditlimit";
        lblLimit.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblAusnahmeTour
        // 
        lblAusnahmeTour.Dock = DockStyle.Fill;
        lblAusnahmeTour.Location = new Point(8, 112);
        lblAusnahmeTour.Margin = new Padding(0, 5, 8, 5);
        lblAusnahmeTour.Name = "lblAusnahmeTour";
        lblAusnahmeTour.Size = new Size(152, 23);
        lblAusnahmeTour.TabIndex = 21;
        lblAusnahmeTour.Text = "Ausnahme-Tour";
        lblAusnahmeTour.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // tabLiefertage
        // 
        tabLiefertage.Controls.Add(tlpLiefertage);
        tabLiefertage.Location = new Point(4, 24);
        tabLiefertage.Name = "tabLiefertage";
        tabLiefertage.Padding = new Padding(3);
        tabLiefertage.Size = new Size(402, 542);
        tabLiefertage.TabIndex = 4;
        tabLiefertage.Text = "Liefertage";
        tabLiefertage.UseVisualStyleBackColor = true;
        // 
        // tlpLiefertage
        // 
        tlpLiefertage.ColumnCount = 2;
        tlpLiefertage.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
        tlpLiefertage.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpLiefertage.Controls.Add(lblLiefertageTitel, 0, 0);
        tlpLiefertage.Controls.Add(flpLiefertage, 0, 1);
        tlpLiefertage.Controls.Add(lblLiefertage, 0, 0);
        tlpLiefertage.Controls.Add(flpDays, 0, 1);
        tlpLiefertage.Controls.Add(spacer, 0, 7);
        tlpLiefertage.Dock = DockStyle.Fill;
        tlpLiefertage.Location = new Point(3, 3);
        tlpLiefertage.Name = "tlpLiefertage";
        tlpLiefertage.Padding = new Padding(8);
        tlpLiefertage.RowCount = 13;
        tlpLiefertage.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tlpLiefertage.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tlpLiefertage.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tlpLiefertage.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tlpLiefertage.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tlpLiefertage.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tlpLiefertage.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tlpLiefertage.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tlpLiefertage.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tlpLiefertage.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tlpLiefertage.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tlpLiefertage.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tlpLiefertage.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tlpLiefertage.Size = new Size(396, 536);
        tlpLiefertage.TabIndex = 0;
        // 
        // lblLiefertageTitel
        // 
        tlpLiefertage.SetColumnSpan(lblLiefertageTitel, 2);
        lblLiefertageTitel.Dock = DockStyle.Fill;
        lblLiefertageTitel.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
        lblLiefertageTitel.Location = new Point(11, 38);
        lblLiefertageTitel.Name = "lblLiefertageTitel";
        lblLiefertageTitel.Size = new Size(374, 30);
        lblLiefertageTitel.TabIndex = 0;
        lblLiefertageTitel.Text = "Liefertage";
        lblLiefertageTitel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // flpLiefertage
        // 
        flpLiefertage.AutoSize = true;
        tlpLiefertage.SetColumnSpan(flpLiefertage, 2);
        flpLiefertage.Dock = DockStyle.Fill;
        flpLiefertage.Location = new Point(11, 101);
        flpLiefertage.Name = "flpLiefertage";
        flpLiefertage.Size = new Size(374, 24);
        flpLiefertage.TabIndex = 1;
        // 
        // lblLiefertage
        // 
        tlpLiefertage.SetColumnSpan(lblLiefertage, 2);
        lblLiefertage.Location = new Point(11, 8);
        lblLiefertage.Name = "lblLiefertage";
        lblLiefertage.Size = new Size(100, 23);
        lblLiefertage.TabIndex = 2;
        // 
        // flpDays
        // 
        tlpLiefertage.SetColumnSpan(flpDays, 2);
        flpDays.Controls.Add(chkMo);
        flpDays.Controls.Add(chkDi);
        flpDays.Controls.Add(chkMi);
        flpDays.Controls.Add(chkDo);
        flpDays.Controls.Add(chkFr);
        flpDays.Controls.Add(chkSa);
        flpDays.Controls.Add(chkSo);
        flpDays.Dock = DockStyle.Fill;
        flpDays.Location = new Point(11, 71);
        flpDays.Name = "flpDays";
        flpDays.Size = new Size(374, 24);
        flpDays.TabIndex = 3;
        // 
        // chkMo
        // 
        chkMo.AutoSize = true;
        chkMo.Location = new Point(4, 6);
        chkMo.Margin = new Padding(4, 6, 4, 0);
        chkMo.Name = "chkMo";
        chkMo.Size = new Size(44, 19);
        chkMo.TabIndex = 16;
        chkMo.Text = "Mo";
        // 
        // chkDi
        // 
        chkDi.AutoSize = true;
        chkDi.Location = new Point(56, 6);
        chkDi.Margin = new Padding(4, 6, 4, 0);
        chkDi.Name = "chkDi";
        chkDi.Size = new Size(37, 19);
        chkDi.TabIndex = 17;
        chkDi.Text = "Di";
        // 
        // chkMi
        // 
        chkMi.AutoSize = true;
        chkMi.Location = new Point(101, 6);
        chkMi.Margin = new Padding(4, 6, 4, 0);
        chkMi.Name = "chkMi";
        chkMi.Size = new Size(40, 19);
        chkMi.TabIndex = 18;
        chkMi.Text = "Mi";
        // 
        // chkDo
        // 
        chkDo.AutoSize = true;
        chkDo.Location = new Point(149, 6);
        chkDo.Margin = new Padding(4, 6, 4, 0);
        chkDo.Name = "chkDo";
        chkDo.Size = new Size(41, 19);
        chkDo.TabIndex = 19;
        chkDo.Text = "Do";
        // 
        // chkFr
        // 
        chkFr.AutoSize = true;
        chkFr.Location = new Point(198, 6);
        chkFr.Margin = new Padding(4, 6, 4, 0);
        chkFr.Name = "chkFr";
        chkFr.Size = new Size(36, 19);
        chkFr.TabIndex = 20;
        chkFr.Text = "Fr";
        // 
        // chkSa
        // 
        chkSa.AutoSize = true;
        chkSa.Location = new Point(242, 6);
        chkSa.Margin = new Padding(4, 6, 4, 0);
        chkSa.Name = "chkSa";
        chkSa.Size = new Size(38, 19);
        chkSa.TabIndex = 21;
        chkSa.Text = "Sa";
        // 
        // chkSo
        // 
        chkSo.AutoSize = true;
        chkSo.Location = new Point(288, 6);
        chkSo.Margin = new Padding(4, 6, 4, 0);
        chkSo.Name = "chkSo";
        chkSo.Size = new Size(39, 19);
        chkSo.TabIndex = 22;
        chkSo.Text = "So";
        // 
        // spacer
        // 
        tlpLiefertage.SetColumnSpan(spacer, 2);
        spacer.Location = new Point(11, 221);
        spacer.Name = "spacer";
        spacer.Size = new Size(200, 24);
        spacer.TabIndex = 4;
        // 
        // tabLeihgeraete
        // 
        tabLeihgeraete.Controls.Add(tlpLeihgeraete);
        tabLeihgeraete.Location = new Point(4, 24);
        tabLeihgeraete.Name = "tabLeihgeraete";
        tabLeihgeraete.Padding = new Padding(3);
        tabLeihgeraete.Size = new Size(402, 542);
        tabLeihgeraete.TabIndex = 5;
        tabLeihgeraete.Text = "Leihgeräte";
        tabLeihgeraete.UseVisualStyleBackColor = true;
        // 
        // tlpLeihgeraete
        // 
        tlpLeihgeraete.ColumnCount = 2;
        tlpLeihgeraete.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
        tlpLeihgeraete.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpLeihgeraete.Controls.Add(lblLeihgeraeteTitel, 0, 0);
        tlpLeihgeraete.Controls.Add(lblLeihgeraet1, 0, 1);
        tlpLeihgeraete.Controls.Add(txtGeraete1, 1, 1);
        tlpLeihgeraete.Controls.Add(lblLeihgeraet2, 0, 2);
        tlpLeihgeraete.Controls.Add(txtGeraete2, 1, 2);
        tlpLeihgeraete.Controls.Add(lblLeihgeraet3, 0, 3);
        tlpLeihgeraete.Controls.Add(txtGeraete3, 1, 3);
        tlpLeihgeraete.Controls.Add(lblLeihgeraet4, 0, 4);
        tlpLeihgeraete.Controls.Add(txtGeraete4, 1, 4);
        tlpLeihgeraete.Controls.Add(lblLeihgeraet5, 0, 5);
        tlpLeihgeraete.Controls.Add(txtGeraete5, 1, 5);
        tlpLeihgeraete.Dock = DockStyle.Fill;
        tlpLeihgeraete.Location = new Point(3, 3);
        tlpLeihgeraete.Name = "tlpLeihgeraete";
        tlpLeihgeraete.Padding = new Padding(8);
        tlpLeihgeraete.RowCount = 7;
        tlpLeihgeraete.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tlpLeihgeraete.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpLeihgeraete.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpLeihgeraete.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpLeihgeraete.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpLeihgeraete.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
        tlpLeihgeraete.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        tlpLeihgeraete.Size = new Size(396, 536);
        tlpLeihgeraete.TabIndex = 0;
        // 
        // lblLeihgeraeteTitel
        // 
        tlpLeihgeraete.SetColumnSpan(lblLeihgeraeteTitel, 2);
        lblLeihgeraeteTitel.Dock = DockStyle.Fill;
        lblLeihgeraeteTitel.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
        lblLeihgeraeteTitel.Location = new Point(11, 8);
        lblLeihgeraeteTitel.Name = "lblLeihgeraeteTitel";
        lblLeihgeraeteTitel.Size = new Size(374, 30);
        lblLeihgeraeteTitel.TabIndex = 0;
        lblLeihgeraeteTitel.Text = "Leihgeräte beim Kunden";
        lblLeihgeraeteTitel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblLeihgeraet1
        // 
        lblLeihgeraet1.Dock = DockStyle.Fill;
        lblLeihgeraet1.Location = new Point(8, 43);
        lblLeihgeraet1.Margin = new Padding(0, 5, 8, 5);
        lblLeihgeraet1.Name = "lblLeihgeraet1";
        lblLeihgeraet1.Size = new Size(152, 23);
        lblLeihgeraet1.TabIndex = 1;
        lblLeihgeraet1.Text = "Leihgerät 1";
        lblLeihgeraet1.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtGeraete1
        // 
        txtGeraete1.Dock = DockStyle.Fill;
        txtGeraete1.Location = new Point(168, 43);
        txtGeraete1.Margin = new Padding(0, 5, 0, 5);
        txtGeraete1.Name = "txtGeraete1";
        txtGeraete1.Size = new Size(220, 23);
        txtGeraete1.TabIndex = 23;
        // 
        // lblLeihgeraet2
        // 
        lblLeihgeraet2.Dock = DockStyle.Fill;
        lblLeihgeraet2.Location = new Point(8, 76);
        lblLeihgeraet2.Margin = new Padding(0, 5, 8, 5);
        lblLeihgeraet2.Name = "lblLeihgeraet2";
        lblLeihgeraet2.Size = new Size(152, 23);
        lblLeihgeraet2.TabIndex = 24;
        lblLeihgeraet2.Text = "Leihgerät 2";
        lblLeihgeraet2.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtGeraete2
        // 
        txtGeraete2.Dock = DockStyle.Fill;
        txtGeraete2.Location = new Point(168, 76);
        txtGeraete2.Margin = new Padding(0, 5, 0, 5);
        txtGeraete2.Name = "txtGeraete2";
        txtGeraete2.Size = new Size(220, 23);
        txtGeraete2.TabIndex = 24;
        // 
        // lblLeihgeraet3
        // 
        lblLeihgeraet3.Dock = DockStyle.Fill;
        lblLeihgeraet3.Location = new Point(8, 109);
        lblLeihgeraet3.Margin = new Padding(0, 5, 8, 5);
        lblLeihgeraet3.Name = "lblLeihgeraet3";
        lblLeihgeraet3.Size = new Size(152, 23);
        lblLeihgeraet3.TabIndex = 25;
        lblLeihgeraet3.Text = "Leihgerät 3";
        lblLeihgeraet3.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtGeraete3
        // 
        txtGeraete3.Dock = DockStyle.Fill;
        txtGeraete3.Location = new Point(168, 109);
        txtGeraete3.Margin = new Padding(0, 5, 0, 5);
        txtGeraete3.Name = "txtGeraete3";
        txtGeraete3.Size = new Size(220, 23);
        txtGeraete3.TabIndex = 25;
        // 
        // lblLeihgeraet4
        // 
        lblLeihgeraet4.Dock = DockStyle.Fill;
        lblLeihgeraet4.Location = new Point(8, 142);
        lblLeihgeraet4.Margin = new Padding(0, 5, 8, 5);
        lblLeihgeraet4.Name = "lblLeihgeraet4";
        lblLeihgeraet4.Size = new Size(152, 23);
        lblLeihgeraet4.TabIndex = 26;
        lblLeihgeraet4.Text = "Leihgerät 4";
        lblLeihgeraet4.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtGeraete4
        // 
        txtGeraete4.Dock = DockStyle.Fill;
        txtGeraete4.Location = new Point(168, 142);
        txtGeraete4.Margin = new Padding(0, 5, 0, 5);
        txtGeraete4.Name = "txtGeraete4";
        txtGeraete4.Size = new Size(220, 23);
        txtGeraete4.TabIndex = 26;
        // 
        // lblLeihgeraet5
        // 
        lblLeihgeraet5.Dock = DockStyle.Fill;
        lblLeihgeraet5.Location = new Point(8, 175);
        lblLeihgeraet5.Margin = new Padding(0, 5, 8, 5);
        lblLeihgeraet5.Name = "lblLeihgeraet5";
        lblLeihgeraet5.Size = new Size(152, 23);
        lblLeihgeraet5.TabIndex = 27;
        lblLeihgeraet5.Text = "Leihgerät 5";
        lblLeihgeraet5.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtGeraete5
        // 
        txtGeraete5.Dock = DockStyle.Fill;
        txtGeraete5.Location = new Point(168, 175);
        txtGeraete5.Margin = new Padding(0, 5, 0, 5);
        txtGeraete5.Name = "txtGeraete5";
        txtGeraete5.Size = new Size(220, 23);
        txtGeraete5.TabIndex = 27;
        // 
        // panelButtons
        // 
        panelButtons.Controls.Add(btnDeaktivieren);
        panelButtons.Controls.Add(btnSpeichern);
        panelButtons.Controls.Add(btnNeu);
        panelButtons.Dock = DockStyle.Bottom;
        panelButtons.Location = new Point(4, 574);
        panelButtons.Name = "panelButtons";
        panelButtons.Size = new Size(410, 48);
        panelButtons.TabIndex = 1;
        // 
        // btnDeaktivieren
        // 
        btnDeaktivieren.Location = new Point(286, 7);
        btnDeaktivieren.Name = "btnDeaktivieren";
        btnDeaktivieren.Size = new Size(110, 34);
        btnDeaktivieren.TabIndex = 2;
        btnDeaktivieren.Text = "Deaktivieren";
        // 
        // btnSpeichern
        // 
        btnSpeichern.Location = new Point(120, 7);
        btnSpeichern.Name = "btnSpeichern";
        btnSpeichern.Size = new Size(160, 34);
        btnSpeichern.TabIndex = 1;
        btnSpeichern.Text = "Speichern (Strg+S)";
        // 
        // btnNeu
        // 
        btnNeu.Location = new Point(4, 7);
        btnNeu.Name = "btnNeu";
        btnNeu.Size = new Size(110, 34);
        btnNeu.TabIndex = 0;
        btnNeu.Text = "Neu (F2)";
        // 
        // FrmCustomerList
        // 
        ClientSize = new Size(1081, 672);
        Controls.Add(splitContainer);
        Controls.Add(panelTop);
        MinimumSize = new Size(900, 500);
        Name = "FrmCustomerList";
        Text = "Kundenstammdaten";
        panelTop.ResumeLayout(false);
        panelTop.PerformLayout();
        splitContainer.Panel1.ResumeLayout(false);
        splitContainer.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
        splitContainer.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgwKunden).EndInit();
        panelDetail.ResumeLayout(false);
        tabDetail.ResumeLayout(false);
        tabStamm.ResumeLayout(false);
        tabStamm.PerformLayout();
        tlpStamm.ResumeLayout(false);
        tlpStamm.PerformLayout();
        tabAdresse.ResumeLayout(false);
        tabAdresse.PerformLayout();
        tlpAdresse.ResumeLayout(false);
        tlpAdresse.PerformLayout();
        tabAltAdresse.ResumeLayout(false);
        tabAltAdresse.PerformLayout();
        tlpAltAdresse.ResumeLayout(false);
        tlpAltAdresse.PerformLayout();
        tabEinstellungen.ResumeLayout(false);
        tabEinstellungen.PerformLayout();
        tlpEinstellungen.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)nudLimit).EndInit();
        tabLiefertage.ResumeLayout(false);
        tlpLiefertage.ResumeLayout(false);
        tlpLiefertage.PerformLayout();
        flpDays.ResumeLayout(false);
        flpDays.PerformLayout();
        tabLeihgeraete.ResumeLayout(false);
        tlpLeihgeraete.ResumeLayout(false);
        tlpLeihgeraete.PerformLayout();
        panelButtons.ResumeLayout(false);
        ResumeLayout(false);
    }
    private ComboBox cmbTur;
    private Label lblStandartTour;
    private Label lblRoutenfolge;
    private ComboBox cmbAusnahmeTur;
    private Label lblAusnahmeTour;
    private Label lblLimit;
    private TabPage tabLiefertage;
    private TableLayoutPanel tlpLiefertage;
    private Label lblLiefertage;
    private FlowLayoutPanel flpDays;
    private Panel spacer;
}