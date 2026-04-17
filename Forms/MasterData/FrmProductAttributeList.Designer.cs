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
        panelTop = new Panel();
        lblSuche = new Label();
        txtSuche = new TextBox();
        chkNurAktiv = new CheckBox();
        dgwAttribute = new DataGridView();
        panelDetail = new Panel();
        dgwWerte = new DataGridView();
        lblWerte = new Label();
        panelAttributFelder = new Panel();
        chkIstVorlage = new CheckBox();
        lblBezeichnung = new Label();
        txtBezeichnung = new TextBox();
        lblFeldtyp = new Label();
        cmbFeldtyp = new ComboBox();
        lblMaxLaenge = new Label();
        nudMaxLaenge = new NumericUpDown();
        chkAktiv = new CheckBox();
        lblEntityType = new Label();
        cmbEntityType = new ComboBox();
        panelWerteEingabe = new Panel();
        lblNeuerWert = new Label();
        txtNeuerWert = new TextBox();
        lblWertSort = new Label();
        nudWertSortierung = new NumericUpDown();
        btnWertHinzufuegen = new Button();
        btnWertLoeschen = new Button();
        panelAktionen = new Panel();
        btnLoeschen = new Button();
        btnNeu = new Button();
        btnSpeichern = new Button();
        btnDeaktivieren = new Button();
        panelTop.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgwAttribute).BeginInit();
        panelDetail.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgwWerte).BeginInit();
        panelAttributFelder.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudMaxLaenge).BeginInit();
        panelWerteEingabe.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudWertSortierung).BeginInit();
        panelAktionen.SuspendLayout();
        SuspendLayout();
        // 
        // panelTop
        // 
        panelTop.Controls.Add(lblSuche);
        panelTop.Controls.Add(txtSuche);
        panelTop.Controls.Add(chkNurAktiv);
        panelTop.Dock = DockStyle.Top;
        panelTop.Location = new Point(0, 0);
        panelTop.Name = "panelTop";
        panelTop.Padding = new Padding(6, 0, 6, 0);
        panelTop.Size = new Size(1100, 38);
        panelTop.TabIndex = 2;
        // 
        // lblSuche
        // 
        lblSuche.AutoSize = true;
        lblSuche.Location = new Point(8, 12);
        lblSuche.Name = "lblSuche";
        lblSuche.Size = new Size(42, 15);
        lblSuche.TabIndex = 0;
        lblSuche.Text = "Suche:";
        // 
        // txtSuche
        // 
        txtSuche.Location = new Point(60, 8);
        txtSuche.Name = "txtSuche";
        txtSuche.Size = new Size(220, 23);
        txtSuche.TabIndex = 1;
        // 
        // chkNurAktiv
        // 
        chkNurAktiv.AutoSize = true;
        chkNurAktiv.Location = new Point(292, 10);
        chkNurAktiv.Name = "chkNurAktiv";
        chkNurAktiv.Size = new Size(80, 19);
        chkNurAktiv.TabIndex = 2;
        chkNurAktiv.Text = "Nur aktive";
        // 
        // dgwAttribute
        // 
        dgwAttribute.AllowUserToAddRows = false;
        dgwAttribute.AllowUserToDeleteRows = false;
        dgwAttribute.BackgroundColor = SystemColors.Window;
        dgwAttribute.BorderStyle = BorderStyle.None;
        dgwAttribute.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgwAttribute.Dock = DockStyle.Left;
        dgwAttribute.Location = new Point(0, 38);
        dgwAttribute.MultiSelect = false;
        dgwAttribute.Name = "dgwAttribute";
        dgwAttribute.ReadOnly = true;
        dgwAttribute.RowHeadersVisible = false;
        dgwAttribute.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgwAttribute.Size = new Size(380, 612);
        dgwAttribute.TabIndex = 1;
        // 
        // panelDetail
        // 
        panelDetail.Controls.Add(dgwWerte);
        panelDetail.Controls.Add(lblWerte);
        panelDetail.Controls.Add(panelAttributFelder);
        panelDetail.Controls.Add(panelWerteEingabe);
        panelDetail.Controls.Add(panelAktionen);
        panelDetail.Dock = DockStyle.Fill;
        panelDetail.Location = new Point(380, 38);
        panelDetail.Name = "panelDetail";
        panelDetail.Padding = new Padding(10, 6, 10, 4);
        panelDetail.Size = new Size(720, 612);
        panelDetail.TabIndex = 0;
        // 
        // dgwWerte
        // 
        dgwWerte.AllowUserToAddRows = false;
        dgwWerte.AllowUserToDeleteRows = false;
        dgwWerte.BackgroundColor = SystemColors.Window;
        dgwWerte.BorderStyle = BorderStyle.None;
        dgwWerte.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgwWerte.Dock = DockStyle.Fill;
        dgwWerte.EditMode = DataGridViewEditMode.EditOnEnter;
        dgwWerte.Location = new Point(10, 175);
        dgwWerte.MultiSelect = false;
        dgwWerte.Name = "dgwWerte";
        dgwWerte.RowHeadersVisible = false;
        dgwWerte.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgwWerte.Size = new Size(700, 350);
        dgwWerte.TabIndex = 0;
        // 
        // lblWerte
        // 
        lblWerte.Dock = DockStyle.Top;
        lblWerte.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
        lblWerte.Location = new Point(10, 151);
        lblWerte.Name = "lblWerte";
        lblWerte.Padding = new Padding(2, 0, 0, 0);
        lblWerte.Size = new Size(700, 24);
        lblWerte.TabIndex = 1;
        lblWerte.Text = "Werte";
        lblWerte.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // panelAttributFelder
        // 
        panelAttributFelder.Controls.Add(chkIstVorlage);
        panelAttributFelder.Controls.Add(lblBezeichnung);
        panelAttributFelder.Controls.Add(txtBezeichnung);
        panelAttributFelder.Controls.Add(lblFeldtyp);
        panelAttributFelder.Controls.Add(cmbFeldtyp);
        panelAttributFelder.Controls.Add(lblMaxLaenge);
        panelAttributFelder.Controls.Add(nudMaxLaenge);
        panelAttributFelder.Controls.Add(chkAktiv);
        panelAttributFelder.Controls.Add(lblEntityType);
        panelAttributFelder.Controls.Add(cmbEntityType);
        panelAttributFelder.Dock = DockStyle.Top;
        panelAttributFelder.Location = new Point(10, 6);
        panelAttributFelder.Name = "panelAttributFelder";
        panelAttributFelder.Size = new Size(700, 145);
        panelAttributFelder.TabIndex = 2;
        // 
        // chkIstVorlage
        // 
        chkIstVorlage.AutoSize = true;
        chkIstVorlage.Location = new Point(196, 78);
        chkIstVorlage.Name = "chkIstVorlage";
        chkIstVorlage.Size = new Size(155, 19);
        chkIstVorlage.TabIndex = 9;
        chkIstVorlage.Text = "Vorlage (Freitext erlaubt)";
        // 
        // lblBezeichnung
        // 
        lblBezeichnung.AutoSize = true;
        lblBezeichnung.Location = new Point(0, 14);
        lblBezeichnung.Name = "lblBezeichnung";
        lblBezeichnung.Size = new Size(83, 15);
        lblBezeichnung.TabIndex = 0;
        lblBezeichnung.Text = "Bezeichnung *";
        // 
        // txtBezeichnung
        // 
        txtBezeichnung.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtBezeichnung.Location = new Point(120, 10);
        txtBezeichnung.Name = "txtBezeichnung";
        txtBezeichnung.Size = new Size(790, 23);
        txtBezeichnung.TabIndex = 1;
        // 
        // lblFeldtyp
        // 
        lblFeldtyp.AutoSize = true;
        lblFeldtyp.Location = new Point(0, 46);
        lblFeldtyp.Name = "lblFeldtyp";
        lblFeldtyp.Size = new Size(46, 15);
        lblFeldtyp.TabIndex = 2;
        lblFeldtyp.Text = "Feldtyp";
        // 
        // cmbFeldtyp
        // 
        cmbFeldtyp.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbFeldtyp.Items.AddRange(new object[] { "Lookup", "Freier Text" });
        cmbFeldtyp.Location = new Point(120, 42);
        cmbFeldtyp.Name = "cmbFeldtyp";
        cmbFeldtyp.Size = new Size(150, 23);
        cmbFeldtyp.TabIndex = 3;
        // 
        // lblMaxLaenge
        // 
        lblMaxLaenge.AutoSize = true;
        lblMaxLaenge.Location = new Point(285, 46);
        lblMaxLaenge.Name = "lblMaxLaenge";
        lblMaxLaenge.Size = new Size(68, 15);
        lblMaxLaenge.TabIndex = 4;
        lblMaxLaenge.Text = "Max. Länge";
        // 
        // nudMaxLaenge
        // 
        nudMaxLaenge.Enabled = false;
        nudMaxLaenge.Location = new Point(375, 42);
        nudMaxLaenge.Maximum = new decimal(new int[] { 500, 0, 0, 0 });
        nudMaxLaenge.Name = "nudMaxLaenge";
        nudMaxLaenge.Size = new Size(70, 23);
        nudMaxLaenge.TabIndex = 5;
        // 
        // chkAktiv
        // 
        chkAktiv.AutoSize = true;
        chkAktiv.Checked = true;
        chkAktiv.CheckState = CheckState.Checked;
        chkAktiv.Location = new Point(120, 78);
        chkAktiv.Name = "chkAktiv";
        chkAktiv.Size = new Size(53, 19);
        chkAktiv.TabIndex = 6;
        chkAktiv.Text = "Aktiv";
        // 
        // lblEntityType
        // 
        lblEntityType.AutoSize = true;
        lblEntityType.Location = new Point(0, 110);
        lblEntityType.Name = "lblEntityType";
        lblEntityType.Size = new Size(73, 15);
        lblEntityType.TabIndex = 7;
        lblEntityType.Text = "Verwendung";
        // 
        // cmbEntityType
        // 
        cmbEntityType.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbEntityType.Items.AddRange(new object[] { "Produkt", "Kundenprodukt", "Tour", "Kundengruppe", "Geteilt (alle)" });
        cmbEntityType.Location = new Point(120, 106);
        cmbEntityType.Name = "cmbEntityType";
        cmbEntityType.Size = new Size(200, 23);
        cmbEntityType.TabIndex = 8;
        // 
        // panelWerteEingabe
        // 
        panelWerteEingabe.Controls.Add(lblNeuerWert);
        panelWerteEingabe.Controls.Add(txtNeuerWert);
        panelWerteEingabe.Controls.Add(lblWertSort);
        panelWerteEingabe.Controls.Add(nudWertSortierung);
        panelWerteEingabe.Controls.Add(btnWertHinzufuegen);
        panelWerteEingabe.Controls.Add(btnWertLoeschen);
        panelWerteEingabe.Dock = DockStyle.Bottom;
        panelWerteEingabe.Location = new Point(10, 525);
        panelWerteEingabe.Name = "panelWerteEingabe";
        panelWerteEingabe.Size = new Size(700, 38);
        panelWerteEingabe.TabIndex = 3;
        // 
        // lblNeuerWert
        // 
        lblNeuerWert.AutoSize = true;
        lblNeuerWert.Location = new Point(0, 12);
        lblNeuerWert.Name = "lblNeuerWert";
        lblNeuerWert.Size = new Size(70, 15);
        lblNeuerWert.TabIndex = 0;
        lblNeuerWert.Text = "Neuer Wert:";
        // 
        // txtNeuerWert
        // 
        txtNeuerWert.Location = new Point(85, 8);
        txtNeuerWert.Name = "txtNeuerWert";
        txtNeuerWert.Size = new Size(200, 23);
        txtNeuerWert.TabIndex = 1;
        // 
        // lblWertSort
        // 
        lblWertSort.AutoSize = true;
        lblWertSort.Location = new Point(295, 12);
        lblWertSort.Name = "lblWertSort";
        lblWertSort.Size = new Size(34, 15);
        lblWertSort.TabIndex = 2;
        lblWertSort.Text = "Sort.:";
        // 
        // nudWertSortierung
        // 
        nudWertSortierung.Location = new Point(333, 8);
        nudWertSortierung.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
        nudWertSortierung.Name = "nudWertSortierung";
        nudWertSortierung.Size = new Size(60, 23);
        nudWertSortierung.TabIndex = 3;
        // 
        // btnWertHinzufuegen
        // 
        btnWertHinzufuegen.Location = new Point(403, 6);
        btnWertHinzufuegen.Name = "btnWertHinzufuegen";
        btnWertHinzufuegen.Size = new Size(100, 23);
        btnWertHinzufuegen.TabIndex = 4;
        btnWertHinzufuegen.Text = "+ Hinzufügen";
        // 
        // btnWertLoeschen
        // 
        btnWertLoeschen.Location = new Point(511, 6);
        btnWertLoeschen.Name = "btnWertLoeschen";
        btnWertLoeschen.Size = new Size(90, 23);
        btnWertLoeschen.TabIndex = 5;
        btnWertLoeschen.Text = "– Löschen";
        // 
        // panelAktionen
        // 
        panelAktionen.Controls.Add(btnLoeschen);
        panelAktionen.Controls.Add(btnNeu);
        panelAktionen.Controls.Add(btnSpeichern);
        panelAktionen.Controls.Add(btnDeaktivieren);
        panelAktionen.Dock = DockStyle.Bottom;
        panelAktionen.Location = new Point(10, 563);
        panelAktionen.Name = "panelAktionen";
        panelAktionen.Size = new Size(700, 45);
        panelAktionen.TabIndex = 4;
        // 
        // btnLoeschen
        // 
        btnLoeschen.Location = new Point(312, 9);
        btnLoeschen.Name = "btnLoeschen";
        btnLoeschen.Size = new Size(110, 23);
        btnLoeschen.TabIndex = 3;
        btnLoeschen.Text = "Löschen";
        // 
        // btnNeu
        // 
        btnNeu.Location = new Point(0, 9);
        btnNeu.Name = "btnNeu";
        btnNeu.Size = new Size(90, 23);
        btnNeu.TabIndex = 0;
        btnNeu.Text = "Neu (F2)";
        // 
        // btnSpeichern
        // 
        btnSpeichern.Location = new Point(98, 9);
        btnSpeichern.Name = "btnSpeichern";
        btnSpeichern.Size = new Size(90, 23);
        btnSpeichern.TabIndex = 1;
        btnSpeichern.Text = "Speichern";
        // 
        // btnDeaktivieren
        // 
        btnDeaktivieren.Location = new Point(196, 9);
        btnDeaktivieren.Name = "btnDeaktivieren";
        btnDeaktivieren.Size = new Size(110, 23);
        btnDeaktivieren.TabIndex = 2;
        btnDeaktivieren.Text = "Deaktivieren";
        // 
        // FrmProductAttributeList
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1100, 650);
        Controls.Add(panelDetail);
        Controls.Add(dgwAttribute);
        Controls.Add(panelTop);
        Name = "FrmProductAttributeList";
        Text = "Attribute";
        panelTop.ResumeLayout(false);
        panelTop.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgwAttribute).EndInit();
        panelDetail.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgwWerte).EndInit();
        panelAttributFelder.ResumeLayout(false);
        panelAttributFelder.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudMaxLaenge).EndInit();
        panelWerteEingabe.ResumeLayout(false);
        panelWerteEingabe.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudWertSortierung).EndInit();
        panelAktionen.ResumeLayout(false);
        ResumeLayout(false);
    }

    private Panel panelTop;
    private Label lblSuche;
    private TextBox txtSuche;
    private CheckBox chkNurAktiv;
    private DataGridView dgwAttribute;
    private Panel panelDetail;
    private Panel panelAttributFelder;
    private Label lblBezeichnung;
    private TextBox txtBezeichnung;
    private Label lblFeldtyp;
    private ComboBox cmbFeldtyp;
    private Label lblMaxLaenge;
    private NumericUpDown nudMaxLaenge;
    private CheckBox chkAktiv;
    private Label lblEntityType;
    private ComboBox cmbEntityType;
    private Label lblWerte;
    private DataGridView dgwWerte;
    private DataGridViewTextBoxColumn colWertBezeichnung;
    private DataGridViewTextBoxColumn colWertSortierung;
    private DataGridViewCheckBoxColumn colWertAktiv;
    private DataGridViewCheckBoxColumn colIstVorlage;  // ← Ekle burada
    private Panel panelWerteEingabe;
    private Label lblNeuerWert;
    private TextBox txtNeuerWert;
    private Label lblWertSort;
    private NumericUpDown nudWertSortierung;
    private Button btnWertHinzufuegen;
    private Button btnWertLoeschen;
    private Panel panelAktionen;
    private Button btnNeu;
    private Button btnSpeichern;
    private Button btnDeaktivieren;
    private CheckBox chkIstVorlage;
    private Button btnLoeschen;
}