namespace DNR26V2.Forms.MasterData;

partial class FrmProductList
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        pnlTop = new Panel();
        lblSuche = new Label();
        txtSuche = new TextBox();
        chkNurAktiv = new CheckBox();
        dgwArtikel = new DataGridView();
        panelDetail = new Panel();
        lblArtikelnummer = new Label();
        txtArtikelnummer = new TextBox();
        lblBezeichnung = new Label();
        txtBezeichnung = new TextBox();
        lblBezeichnung2 = new Label();
        txtBezeichnung2 = new TextBox();
        lblEinheit = new Label();
        txtEinheit = new TextBox();
        lblVKPreis = new Label();
        nudVKPreis = new NumericUpDown();
        lblEKPreis = new Label();
        nudEKPreis = new NumericUpDown();
        lblMwstProzent = new Label();
        nudMwstProzent = new NumericUpDown();
        lblBarcode = new Label();
        txtBarcode = new TextBox();
        lblNotizen = new Label();
        txtNotizen = new TextBox();
        chkAktiv = new CheckBox();
        btnNeu = new Button();
        btnSpeichern = new Button();
        btnDeaktivieren = new Button();
        txtFeld1 = new TextBox();
        txtFeld2 = new TextBox();
        txtFeld3 = new TextBox();
        txtFeld4 = new TextBox();
        lblFeld1 = new Label();
        lblFeld2 = new Label();
        lblFeld3 = new Label();
        lblFeld4 = new Label();
        pnlPrintfarbe = new Panel();
        btnPrintfarbe = new Button();
        pnlTop.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgwArtikel).BeginInit();
        panelDetail.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudVKPreis).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudEKPreis).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudMwstProzent).BeginInit();
        SuspendLayout();
        // 
        // pnlTop
        // 
        pnlTop.Controls.Add(lblSuche);
        pnlTop.Controls.Add(txtSuche);
        pnlTop.Controls.Add(chkNurAktiv);
        pnlTop.Dock = DockStyle.Top;
        pnlTop.Location = new Point(0, 0);
        pnlTop.Name = "pnlTop";
        pnlTop.Padding = new Padding(6, 6, 6, 0);
        pnlTop.Size = new Size(1200, 40);
        pnlTop.TabIndex = 2;
        // 
        // lblSuche
        // 
        lblSuche.AutoSize = true;
        lblSuche.Location = new Point(6, 10);
        lblSuche.Name = "lblSuche";
        lblSuche.Size = new Size(42, 15);
        lblSuche.TabIndex = 0;
        lblSuche.Text = "Suche:";
        // 
        // txtSuche
        // 
        txtSuche.Font = new Font("Segoe UI", 10F);
        txtSuche.Location = new Point(55, 7);
        txtSuche.Name = "txtSuche";
        txtSuche.Size = new Size(260, 25);
        txtSuche.TabIndex = 1;
        // 
        // chkNurAktiv
        // 
        chkNurAktiv.AutoSize = true;
        chkNurAktiv.Checked = true;
        chkNurAktiv.CheckState = CheckState.Checked;
        chkNurAktiv.Location = new Point(330, 9);
        chkNurAktiv.Name = "chkNurAktiv";
        chkNurAktiv.Size = new Size(80, 19);
        chkNurAktiv.TabIndex = 2;
        chkNurAktiv.Text = "Nur aktive";
        // 
        // dgwArtikel
        // 
        dgwArtikel.AllowUserToAddRows = false;
        dgwArtikel.AllowUserToDeleteRows = false;
        dgwArtikel.BackgroundColor = SystemColors.Window;
        dgwArtikel.BorderStyle = BorderStyle.None;
        dgwArtikel.Dock = DockStyle.Left;
        dgwArtikel.Font = new Font("Segoe UI", 9.5F);
        dgwArtikel.Location = new Point(0, 40);
        dgwArtikel.MultiSelect = false;
        dgwArtikel.Name = "dgwArtikel";
        dgwArtikel.ReadOnly = true;
        dgwArtikel.RowHeadersVisible = false;
        dgwArtikel.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgwArtikel.Size = new Size(560, 660);
        dgwArtikel.TabIndex = 1;
        // 
        // panelDetail
        // 
        panelDetail.Controls.Add(btnPrintfarbe);
        panelDetail.Controls.Add(pnlPrintfarbe);
        panelDetail.Controls.Add(lblFeld4);
        panelDetail.Controls.Add(lblFeld3);
        panelDetail.Controls.Add(lblFeld2);
        panelDetail.Controls.Add(lblFeld1);
        panelDetail.Controls.Add(txtFeld4);
        panelDetail.Controls.Add(txtFeld3);
        panelDetail.Controls.Add(txtFeld2);
        panelDetail.Controls.Add(txtFeld1);
        panelDetail.Controls.Add(lblArtikelnummer);
        panelDetail.Controls.Add(txtArtikelnummer);
        panelDetail.Controls.Add(lblBezeichnung);
        panelDetail.Controls.Add(txtBezeichnung);
        panelDetail.Controls.Add(lblBezeichnung2);
        panelDetail.Controls.Add(txtBezeichnung2);
        panelDetail.Controls.Add(lblEinheit);
        panelDetail.Controls.Add(txtEinheit);
        panelDetail.Controls.Add(lblVKPreis);
        panelDetail.Controls.Add(nudVKPreis);
        panelDetail.Controls.Add(lblEKPreis);
        panelDetail.Controls.Add(nudEKPreis);
        panelDetail.Controls.Add(lblMwstProzent);
        panelDetail.Controls.Add(nudMwstProzent);
        panelDetail.Controls.Add(lblBarcode);
        panelDetail.Controls.Add(txtBarcode);
        panelDetail.Controls.Add(lblNotizen);
        panelDetail.Controls.Add(txtNotizen);
        panelDetail.Controls.Add(chkAktiv);
        panelDetail.Controls.Add(btnNeu);
        panelDetail.Controls.Add(btnSpeichern);
        panelDetail.Controls.Add(btnDeaktivieren);
        panelDetail.Dock = DockStyle.Fill;
        panelDetail.Location = new Point(560, 40);
        panelDetail.Name = "panelDetail";
        panelDetail.Padding = new Padding(16, 8, 16, 8);
        panelDetail.Size = new Size(640, 660);
        panelDetail.TabIndex = 0;
        panelDetail.Visible = false;
        // 
        // lblArtikelnummer
        // 
        lblArtikelnummer.AutoSize = true;
        lblArtikelnummer.Location = new Point(16, 14);
        lblArtikelnummer.Name = "lblArtikelnummer";
        lblArtikelnummer.Size = new Size(95, 15);
        lblArtikelnummer.TabIndex = 0;
        lblArtikelnummer.Text = "Artikelnummer *";
        // 
        // txtArtikelnummer
        // 
        txtArtikelnummer.CharacterCasing = CharacterCasing.Upper;
        txtArtikelnummer.Font = new Font("Segoe UI", 10F);
        txtArtikelnummer.Location = new Point(16, 32);
        txtArtikelnummer.Name = "txtArtikelnummer";
        txtArtikelnummer.Size = new Size(160, 25);
        txtArtikelnummer.TabIndex = 1;
        // 
        // lblBezeichnung
        // 
        lblBezeichnung.AutoSize = true;
        lblBezeichnung.Location = new Point(16, 64);
        lblBezeichnung.Name = "lblBezeichnung";
        lblBezeichnung.Size = new Size(83, 15);
        lblBezeichnung.TabIndex = 2;
        lblBezeichnung.Text = "Bezeichnung *";
        // 
        // txtBezeichnung
        // 
        txtBezeichnung.Font = new Font("Segoe UI", 10F);
        txtBezeichnung.Location = new Point(16, 82);
        txtBezeichnung.Name = "txtBezeichnung";
        txtBezeichnung.Size = new Size(380, 25);
        txtBezeichnung.TabIndex = 3;
        // 
        // lblBezeichnung2
        // 
        lblBezeichnung2.AutoSize = true;
        lblBezeichnung2.Location = new Point(16, 114);
        lblBezeichnung2.Name = "lblBezeichnung2";
        lblBezeichnung2.Size = new Size(84, 15);
        lblBezeichnung2.TabIndex = 4;
        lblBezeichnung2.Text = "Bezeichnung 2";
        // 
        // txtBezeichnung2
        // 
        txtBezeichnung2.Font = new Font("Segoe UI", 10F);
        txtBezeichnung2.Location = new Point(16, 132);
        txtBezeichnung2.Name = "txtBezeichnung2";
        txtBezeichnung2.Size = new Size(380, 25);
        txtBezeichnung2.TabIndex = 5;
        // 
        // lblEinheit
        // 
        lblEinheit.AutoSize = true;
        lblEinheit.Location = new Point(16, 164);
        lblEinheit.Name = "lblEinheit";
        lblEinheit.Size = new Size(43, 15);
        lblEinheit.TabIndex = 6;
        lblEinheit.Text = "Einheit";
        // 
        // txtEinheit
        // 
        txtEinheit.CharacterCasing = CharacterCasing.Upper;
        txtEinheit.Font = new Font("Segoe UI", 10F);
        txtEinheit.Location = new Point(16, 182);
        txtEinheit.Name = "txtEinheit";
        txtEinheit.Size = new Size(80, 25);
        txtEinheit.TabIndex = 7;
        // 
        // lblVKPreis
        // 
        lblVKPreis.AutoSize = true;
        lblVKPreis.Location = new Point(16, 216);
        lblVKPreis.Name = "lblVKPreis";
        lblVKPreis.Size = new Size(51, 15);
        lblVKPreis.TabIndex = 8;
        lblVKPreis.Text = "VK-Preis";
        // 
        // nudVKPreis
        // 
        nudVKPreis.DecimalPlaces = 4;
        nudVKPreis.Font = new Font("Segoe UI", 10F);
        nudVKPreis.Location = new Point(16, 234);
        nudVKPreis.Maximum = new decimal(new int[] { 999999999, 0, 0, 262144 });
        nudVKPreis.Name = "nudVKPreis";
        nudVKPreis.Size = new Size(110, 25);
        nudVKPreis.TabIndex = 9;
        // 
        // lblEKPreis
        // 
        lblEKPreis.AutoSize = true;
        lblEKPreis.Location = new Point(140, 216);
        lblEKPreis.Name = "lblEKPreis";
        lblEKPreis.Size = new Size(50, 15);
        lblEKPreis.TabIndex = 10;
        lblEKPreis.Text = "EK-Preis";
        // 
        // nudEKPreis
        // 
        nudEKPreis.DecimalPlaces = 4;
        nudEKPreis.Font = new Font("Segoe UI", 10F);
        nudEKPreis.Location = new Point(140, 234);
        nudEKPreis.Maximum = new decimal(new int[] { 999999999, 0, 0, 262144 });
        nudEKPreis.Name = "nudEKPreis";
        nudEKPreis.Size = new Size(110, 25);
        nudEKPreis.TabIndex = 11;
        // 
        // lblMwstProzent
        // 
        lblMwstProzent.AutoSize = true;
        lblMwstProzent.Location = new Point(264, 216);
        lblMwstProzent.Name = "lblMwstProzent";
        lblMwstProzent.Size = new Size(50, 15);
        lblMwstProzent.TabIndex = 12;
        lblMwstProzent.Text = "MwSt %";
        // 
        // nudMwstProzent
        // 
        nudMwstProzent.DecimalPlaces = 2;
        nudMwstProzent.Font = new Font("Segoe UI", 10F);
        nudMwstProzent.Location = new Point(264, 234);
        nudMwstProzent.Name = "nudMwstProzent";
        nudMwstProzent.Size = new Size(80, 25);
        nudMwstProzent.TabIndex = 13;
        // 
        // lblBarcode
        // 
        lblBarcode.AutoSize = true;
        lblBarcode.Location = new Point(16, 270);
        lblBarcode.Name = "lblBarcode";
        lblBarcode.Size = new Size(50, 15);
        lblBarcode.TabIndex = 14;
        lblBarcode.Text = "Barcode";
        // 
        // txtBarcode
        // 
        txtBarcode.Font = new Font("Segoe UI", 10F);
        txtBarcode.Location = new Point(16, 288);
        txtBarcode.Name = "txtBarcode";
        txtBarcode.Size = new Size(200, 25);
        txtBarcode.TabIndex = 15;
        // 
        // lblNotizen
        // 
        lblNotizen.AutoSize = true;
        lblNotizen.Location = new Point(16, 320);
        lblNotizen.Name = "lblNotizen";
        lblNotizen.Size = new Size(48, 15);
        lblNotizen.TabIndex = 16;
        lblNotizen.Text = "Notizen";
        // 
        // txtNotizen
        // 
        txtNotizen.Font = new Font("Segoe UI", 9.5F);
        txtNotizen.Location = new Point(16, 338);
        txtNotizen.Multiline = true;
        txtNotizen.Name = "txtNotizen";
        txtNotizen.ScrollBars = ScrollBars.Vertical;
        txtNotizen.Size = new Size(380, 80);
        txtNotizen.TabIndex = 17;
        // 
        // chkAktiv
        // 
        chkAktiv.AutoSize = true;
        chkAktiv.Checked = true;
        chkAktiv.CheckState = CheckState.Checked;
        chkAktiv.Location = new Point(16, 430);
        chkAktiv.Name = "chkAktiv";
        chkAktiv.Size = new Size(53, 19);
        chkAktiv.TabIndex = 18;
        chkAktiv.Text = "Aktiv";
        // 
        // btnNeu
        // 
        btnNeu.Location = new Point(16, 470);
        btnNeu.Name = "btnNeu";
        btnNeu.Size = new Size(100, 30);
        btnNeu.TabIndex = 19;
        btnNeu.Text = "Neu (F2)";
        // 
        // btnSpeichern
        // 
        btnSpeichern.Location = new Point(124, 470);
        btnSpeichern.Name = "btnSpeichern";
        btnSpeichern.Size = new Size(100, 30);
        btnSpeichern.TabIndex = 20;
        btnSpeichern.Text = "Speichern";
        // 
        // btnDeaktivieren
        // 
        btnDeaktivieren.Location = new Point(232, 470);
        btnDeaktivieren.Name = "btnDeaktivieren";
        btnDeaktivieren.Size = new Size(110, 30);
        btnDeaktivieren.TabIndex = 21;
        btnDeaktivieren.Text = "Deaktivieren";
        // 
        // txtFeld1
        // 
        txtFeld1.Font = new Font("Segoe UI", 10F);
        txtFeld1.Location = new Point(124, 516);
        txtFeld1.Name = "txtFeld1";
        txtFeld1.Size = new Size(380, 25);
        txtFeld1.TabIndex = 22;
        // 
        // txtFeld2
        // 
        txtFeld2.Font = new Font("Segoe UI", 10F);
        txtFeld2.Location = new Point(124, 547);
        txtFeld2.Name = "txtFeld2";
        txtFeld2.Size = new Size(380, 25);
        txtFeld2.TabIndex = 23;
        // 
        // txtFeld3
        // 
        txtFeld3.Font = new Font("Segoe UI", 10F);
        txtFeld3.Location = new Point(124, 578);
        txtFeld3.Name = "txtFeld3";
        txtFeld3.Size = new Size(380, 25);
        txtFeld3.TabIndex = 24;
        // 
        // txtFeld4
        // 
        txtFeld4.Font = new Font("Segoe UI", 10F);
        txtFeld4.Location = new Point(124, 609);
        txtFeld4.Name = "txtFeld4";
        txtFeld4.Size = new Size(380, 25);
        txtFeld4.TabIndex = 25;
        // 
        // lblFeld1
        // 
        lblFeld1.AutoSize = true;
        lblFeld1.Location = new Point(19, 526);
        lblFeld1.Name = "lblFeld1";
        lblFeld1.Size = new Size(38, 15);
        lblFeld1.TabIndex = 26;
        lblFeld1.Text = "Feld 1";
        // 
        // lblFeld2
        // 
        lblFeld2.AutoSize = true;
        lblFeld2.Location = new Point(21, 557);
        lblFeld2.Name = "lblFeld2";
        lblFeld2.Size = new Size(38, 15);
        lblFeld2.TabIndex = 27;
        lblFeld2.Text = "Feld 2";
        // 
        // lblFeld3
        // 
        lblFeld3.AutoSize = true;
        lblFeld3.Location = new Point(24, 588);
        lblFeld3.Name = "lblFeld3";
        lblFeld3.Size = new Size(38, 15);
        lblFeld3.TabIndex = 28;
        lblFeld3.Text = "Feld 3";
        // 
        // lblFeld4
        // 
        lblFeld4.AutoSize = true;
        lblFeld4.Location = new Point(24, 619);
        lblFeld4.Name = "lblFeld4";
        lblFeld4.Size = new Size(38, 15);
        lblFeld4.TabIndex = 29;
        lblFeld4.Text = "Feld 4";
        // 
        // pnlPrintfarbe
        // 
        pnlPrintfarbe.BorderStyle = BorderStyle.FixedSingle;
        pnlPrintfarbe.Location = new Point(475, 404);
        pnlPrintfarbe.Name = "pnlPrintfarbe";
        pnlPrintfarbe.Size = new Size(77, 37);
        pnlPrintfarbe.TabIndex = 30;
        // 
        // btnPrintfarbe
        // 
        btnPrintfarbe.Location = new Point(474, 444);
        btnPrintfarbe.Name = "btnPrintfarbe";
        btnPrintfarbe.Size = new Size(75, 23);
        btnPrintfarbe.TabIndex = 31;
        btnPrintfarbe.Text = "...";
        btnPrintfarbe.UseVisualStyleBackColor = true;
        // 
        // FrmProductList
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1200, 700);
        Controls.Add(panelDetail);
        Controls.Add(dgwArtikel);
        Controls.Add(pnlTop);
        Name = "FrmProductList";
        Text = "Artikelstammdaten";
        pnlTop.ResumeLayout(false);
        pnlTop.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgwArtikel).EndInit();
        panelDetail.ResumeLayout(false);
        panelDetail.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudVKPreis).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudEKPreis).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudMwstProzent).EndInit();
        ResumeLayout(false);
    }

    #endregion

    // ── Control-Deklarationen ─────────────────────────────────────────────────
    private Panel          pnlTop;
    private Label          lblSuche;
    private TextBox        txtSuche;
    private CheckBox       chkNurAktiv;
    private DataGridView   dgwArtikel;
    private Panel          panelDetail;

    private Label          lblArtikelnummer;
    private TextBox        txtArtikelnummer;
    private Label          lblBezeichnung;
    private TextBox        txtBezeichnung;
    private Label          lblBezeichnung2;
    private TextBox        txtBezeichnung2;
    private Label          lblEinheit;
    private TextBox        txtEinheit;
    private Label          lblVKPreis;
    private NumericUpDown  nudVKPreis;
    private Label          lblEKPreis;
    private NumericUpDown  nudEKPreis;
    private Label          lblMwstProzent;
    private NumericUpDown  nudMwstProzent;
    private Label          lblBarcode;
    private TextBox        txtBarcode;
    private Label          lblNotizen;
    private TextBox        txtNotizen;
    private CheckBox       chkAktiv;
    private Button         btnNeu;
    private Button         btnSpeichern;
    private Button         btnDeaktivieren;
    private Button btnPrintfarbe;
    private Panel pnlPrintfarbe;
    private Label lblFeld4;
    private Label lblFeld3;
    private Label lblFeld2;
    private Label lblFeld1;
    private TextBox txtFeld4;
    private TextBox txtFeld3;
    private TextBox txtFeld2;
    private TextBox txtFeld1;
}