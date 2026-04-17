namespace DNR26V2.Forms.MasterData;

partial class FrmProductAttributeList
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        panelTop           = new Panel();
        lblSuche           = new Label();
        txtSuche           = new TextBox();
        chkNurAktiv        = new CheckBox();

        dgwAttribute       = new DataGridView();

        panelDetail        = new Panel();

        panelAttributFelder = new Panel();
        lblBezeichnung     = new Label();
        txtBezeichnung     = new TextBox();
        lblFeldtyp         = new Label();
        cmbFeldtyp         = new ComboBox();
        lblMaxLaenge       = new Label();
        nudMaxLaenge       = new NumericUpDown();
        chkAktiv           = new CheckBox();

        lblWerte           = new Label();

        dgwWerte           = new DataGridView();
        colWertBezeichnung = new DataGridViewTextBoxColumn();
        colWertSortierung  = new DataGridViewTextBoxColumn();
        colWertAktiv       = new DataGridViewCheckBoxColumn();

        panelWerteEingabe  = new Panel();
        lblNeuerWert       = new Label();
        txtNeuerWert       = new TextBox();
        lblWertSort        = new Label();
        nudWertSortierung  = new NumericUpDown();
        btnWertHinzufuegen = new Button();
        btnWertLoeschen    = new Button();

        panelAktionen      = new Panel();
        btnNeu             = new Button();
        btnSpeichern       = new Button();
        btnDeaktivieren    = new Button();

        SuspendLayout();

        // ── panelTop ──────────────────────────────────────────────────────────
        panelTop.Dock    = DockStyle.Top;
        panelTop.Height  = 38;
        panelTop.Padding = new Padding(6, 0, 6, 0);

        lblSuche.Text     = "Suche:";
        lblSuche.Location = new Point(8, 12);
        lblSuche.AutoSize = true;

        txtSuche.Location = new Point(60, 8);
        txtSuche.Width    = 220;
        txtSuche.Name     = "txtSuche";

        chkNurAktiv.Text     = "Nur aktive";
        chkNurAktiv.Location = new Point(292, 10);
        chkNurAktiv.AutoSize = true;
        chkNurAktiv.Checked  = false;
        chkNurAktiv.Name     = "chkNurAktiv";

        panelTop.Controls.AddRange(new Control[] { lblSuche, txtSuche, chkNurAktiv });

        // ── dgwAttribute ──────────────────────────────────────────────────────
        dgwAttribute.Dock                        = DockStyle.Left;
        dgwAttribute.Width                       = 380;
        dgwAttribute.ReadOnly                    = true;
        dgwAttribute.AllowUserToAddRows          = false;
        dgwAttribute.AllowUserToDeleteRows       = false;
        dgwAttribute.SelectionMode               = DataGridViewSelectionMode.FullRowSelect;
        dgwAttribute.MultiSelect                 = false;
        dgwAttribute.AutoGenerateColumns         = true;
        dgwAttribute.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgwAttribute.BackgroundColor             = SystemColors.Window;
        dgwAttribute.BorderStyle                 = BorderStyle.None;
        dgwAttribute.RowHeadersVisible           = false;
        dgwAttribute.Name                        = "dgwAttribute";

        // ── panelDetail ───────────────────────────────────────────────────────
        panelDetail.Dock    = DockStyle.Fill;
        panelDetail.Padding = new Padding(10, 6, 10, 4);
        panelDetail.Name    = "panelDetail";

        // ── panelAttributFelder (Dock=Top) ────────────────────────────────────
        panelAttributFelder.Dock   = DockStyle.Top;
        panelAttributFelder.Height = 115;
        panelAttributFelder.Name   = "panelAttributFelder";

        lblBezeichnung.Text     = "Bezeichnung *";
        lblBezeichnung.Location = new Point(0, 14);
        lblBezeichnung.AutoSize = true;

        txtBezeichnung.Location = new Point(120, 10);
        txtBezeichnung.Width    = 290;
        txtBezeichnung.Anchor   = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtBezeichnung.Name     = "txtBezeichnung";

        lblFeldtyp.Text     = "Feldtyp";
        lblFeldtyp.Location = new Point(0, 46);
        lblFeldtyp.AutoSize = true;

        cmbFeldtyp.Location      = new Point(120, 42);
        cmbFeldtyp.Width         = 150;
        cmbFeldtyp.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbFeldtyp.Name          = "cmbFeldtyp";
        cmbFeldtyp.Items.AddRange(new object[] { "Lookup", "Freier Text" });
        cmbFeldtyp.SelectedIndex = 0;

        lblMaxLaenge.Text     = "Max. Länge";
        lblMaxLaenge.Location = new Point(285, 46);
        lblMaxLaenge.AutoSize = true;

        nudMaxLaenge.Location = new Point(375, 42);
        nudMaxLaenge.Width    = 70;
        nudMaxLaenge.Minimum  = 0;
        nudMaxLaenge.Maximum  = 500;
        nudMaxLaenge.Value    = 0;
        nudMaxLaenge.Enabled  = false;
        nudMaxLaenge.Name     = "nudMaxLaenge";

        chkAktiv.Text     = "Aktiv";
        chkAktiv.Location = new Point(120, 78);
        chkAktiv.AutoSize = true;
        chkAktiv.Checked  = true;
        chkAktiv.Name     = "chkAktiv";

        panelAttributFelder.Controls.AddRange(new Control[]
        {
            lblBezeichnung, txtBezeichnung,
            lblFeldtyp, cmbFeldtyp,
            lblMaxLaenge, nudMaxLaenge,
            chkAktiv
        });

        // ── lblWerte (Dock=Top) ───────────────────────────────────────────────
        lblWerte.Text      = "Werte";
        lblWerte.Dock      = DockStyle.Top;
        lblWerte.Height    = 24;
        lblWerte.TextAlign = ContentAlignment.MiddleLeft;
        lblWerte.Font      = new Font(SystemFonts.DefaultFont, FontStyle.Bold);
        lblWerte.Padding   = new Padding(2, 0, 0, 0);
        lblWerte.Name      = "lblWerte";

        // ── dgwWerte (Dock=Fill) ──────────────────────────────────────────────
        dgwWerte.Dock                        = DockStyle.Fill;
        dgwWerte.AutoGenerateColumns         = false;
        dgwWerte.AllowUserToAddRows          = false;
        dgwWerte.AllowUserToDeleteRows       = false;
        dgwWerte.SelectionMode               = DataGridViewSelectionMode.FullRowSelect;
        dgwWerte.MultiSelect                 = false;
        dgwWerte.BackgroundColor             = SystemColors.Window;
        dgwWerte.BorderStyle                 = BorderStyle.None;
        dgwWerte.EditMode                    = DataGridViewEditMode.EditOnEnter;
        dgwWerte.RowHeadersVisible           = false;
        dgwWerte.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgwWerte.Name                        = "dgwWerte";

        colWertBezeichnung.Name             = "ColBezeichnung";
        colWertBezeichnung.HeaderText       = "Wert";
        colWertBezeichnung.DataPropertyName = "Bezeichnung";
        colWertBezeichnung.AutoSizeMode     = DataGridViewAutoSizeColumnMode.Fill;

        colWertSortierung.Name              = "ColSortierung";
        colWertSortierung.HeaderText        = "Sort.";
        colWertSortierung.DataPropertyName  = "Sortierung";
        colWertSortierung.Width             = 60;

        colWertAktiv.Name                   = "ColAktiv";
        colWertAktiv.HeaderText             = "Aktiv";
        colWertAktiv.DataPropertyName       = "Aktiv";
        colWertAktiv.Width                  = 55;
        colWertAktiv.TrueValue              = true;
        colWertAktiv.FalseValue             = false;

        dgwWerte.Columns.AddRange(new DataGridViewColumn[]
        {
            colWertBezeichnung, colWertSortierung, colWertAktiv
        });

        // ── panelWerteEingabe (Dock=Bottom) ───────────────────────────────────
        panelWerteEingabe.Dock   = DockStyle.Bottom;
        panelWerteEingabe.Height = 38;
        panelWerteEingabe.Name   = "panelWerteEingabe";

        lblNeuerWert.Text     = "Neuer Wert:";
        lblNeuerWert.Location = new Point(0, 12);
        lblNeuerWert.AutoSize = true;

        txtNeuerWert.Location = new Point(85, 8);
        txtNeuerWert.Width    = 200;
        txtNeuerWert.Name     = "txtNeuerWert";

        lblWertSort.Text     = "Sort.:";
        lblWertSort.Location = new Point(295, 12);
        lblWertSort.AutoSize = true;

        nudWertSortierung.Location = new Point(333, 8);
        nudWertSortierung.Width    = 60;
        nudWertSortierung.Minimum  = 0;
        nudWertSortierung.Maximum  = 9999;
        nudWertSortierung.Name     = "nudWertSortierung";

        btnWertHinzufuegen.Text     = "+ Hinzufügen";
        btnWertHinzufuegen.Location = new Point(403, 6);
        btnWertHinzufuegen.Width    = 100;
        btnWertHinzufuegen.Name     = "btnWertHinzufuegen";

        btnWertLoeschen.Text     = "– Löschen";
        btnWertLoeschen.Location = new Point(511, 6);
        btnWertLoeschen.Width    = 90;
        btnWertLoeschen.Name     = "btnWertLoeschen";

        panelWerteEingabe.Controls.AddRange(new Control[]
        {
            lblNeuerWert, txtNeuerWert,
            lblWertSort, nudWertSortierung,
            btnWertHinzufuegen, btnWertLoeschen
        });

        // ── panelAktionen (Dock=Bottom) ───────────────────────────────────────
        panelAktionen.Dock   = DockStyle.Bottom;
        panelAktionen.Height = 45;
        panelAktionen.Name   = "panelAktionen";

        btnNeu.Text     = "Neu (F2)";
        btnNeu.Location = new Point(0, 9);
        btnNeu.Width    = 90;
        btnNeu.Name     = "btnNeu";

        btnSpeichern.Text     = "Speichern";
        btnSpeichern.Location = new Point(98, 9);
        btnSpeichern.Width    = 90;
        btnSpeichern.Name     = "btnSpeichern";

        btnDeaktivieren.Text     = "Deaktivieren";
        btnDeaktivieren.Location = new Point(196, 9);
        btnDeaktivieren.Width    = 110;
        btnDeaktivieren.Name     = "btnDeaktivieren";

        panelAktionen.Controls.AddRange(new Control[]
        {
            btnNeu, btnSpeichern, btnDeaktivieren
        });

        // ── panelDetail: Controls.Add order = z-order (last added = front = docked first)
        panelDetail.Controls.Add(dgwWerte);             // Fill  → innermost (added first = back)
        panelDetail.Controls.Add(lblWerte);             // Top   → below panelAttributFelder
        panelDetail.Controls.Add(panelAttributFelder);  // Top   → topmost
        panelDetail.Controls.Add(panelWerteEingabe);    // Bottom→ above panelAktionen
        panelDetail.Controls.Add(panelAktionen);        // Bottom→ very bottom (added last = front)

        // ── Form ──────────────────────────────────────────────────────────────
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode       = AutoScaleMode.Font;
        ClientSize          = new Size(1100, 650);
        Text                = "Artikelattribute";
        Name                = "FrmProductAttributeList";

        // Last added = Dock=Top gets the top edge (panelTop)
        Controls.Add(panelDetail);
        Controls.Add(dgwAttribute);
        Controls.Add(panelTop);

        ResumeLayout(false);
        PerformLayout();
    }

    private Panel                      panelTop;
    private Label                      lblSuche;
    private TextBox                    txtSuche;
    private CheckBox                   chkNurAktiv;
    private DataGridView               dgwAttribute;
    private Panel                      panelDetail;
    private Panel                      panelAttributFelder;
    private Label                      lblBezeichnung;
    private TextBox                    txtBezeichnung;
    private Label                      lblFeldtyp;
    private ComboBox                   cmbFeldtyp;
    private Label                      lblMaxLaenge;
    private NumericUpDown              nudMaxLaenge;
    private CheckBox                   chkAktiv;
    private Label                      lblWerte;
    private DataGridView               dgwWerte;
    private DataGridViewTextBoxColumn  colWertBezeichnung;
    private DataGridViewTextBoxColumn  colWertSortierung;
    private DataGridViewCheckBoxColumn colWertAktiv;
    private Panel                      panelWerteEingabe;
    private Label                      lblNeuerWert;
    private TextBox                    txtNeuerWert;
    private Label                      lblWertSort;
    private NumericUpDown              nudWertSortierung;
    private Button                     btnWertHinzufuegen;
    private Button                     btnWertLoeschen;
    private Panel                      panelAktionen;
    private Button                     btnNeu;
    private Button                     btnSpeichern;
    private Button                     btnDeaktivieren;
}