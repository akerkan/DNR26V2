namespace DNR26V2.Forms.Orders;

partial class FrmOrderEntry
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        splitMain = new SplitContainer();
        pnlLinksTop = new Panel();
        pnlDatum = new Panel();
        dtpLieferdatum = new DateTimePicker();
        lblDatumCaption = new Label();
        pnlTage = new Panel();
        btnMo = new Button();
        btnDi = new Button();
        btnMi = new Button();
        btnDo = new Button();
        btnFr = new Button();
        btnSa = new Button();
        btnSo = new Button();
        btnAlle = new Button();
        dgwKunden = new DataGridView();
        pnlKopf = new Panel();
        lblAuftragStatus = new Label();
        lblSaldo = new Label();
        lblKundenname = new Label();
        pnlAktionen = new Panel();
        lblStatusInfo = new Label();
        btnStornieren = new Button();
        btnSpeichern = new Button();
        btnBuchen = new Button();
        dgwPositionen = new DataGridView();
        colZeileId = new DataGridViewTextBoxColumn();
        colArtikelId = new DataGridViewTextBoxColumn();
        colArtikelnummer = new DataGridViewTextBoxColumn();
        colProduktname = new DataGridViewTextBoxColumn();
        colMenge = new DataGridViewTextBoxColumn();
        colGewicht = new DataGridViewTextBoxColumn();
        colPreis = new DataGridViewTextBoxColumn();
        colNotiz = new DataGridViewTextBoxColumn();
        ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
        splitMain.Panel1.SuspendLayout();
        splitMain.Panel2.SuspendLayout();
        splitMain.SuspendLayout();
        pnlLinksTop.SuspendLayout();
        pnlDatum.SuspendLayout();
        pnlTage.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgwKunden).BeginInit();
        pnlKopf.SuspendLayout();
        pnlAktionen.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgwPositionen).BeginInit();
        SuspendLayout();
        //
        // splitMain
        //
        splitMain.Dock = DockStyle.Fill;
        splitMain.FixedPanel = FixedPanel.Panel1;
        splitMain.Location = new Point(0, 0);
        splitMain.Name = "splitMain";
        splitMain.Panel1.Controls.Add(dgwKunden);
        splitMain.Panel1.Controls.Add(pnlLinksTop);
        splitMain.Panel2.Controls.Add(dgwPositionen);
        splitMain.Panel2.Controls.Add(pnlAktionen);
        splitMain.Panel2.Controls.Add(pnlKopf);
        splitMain.Size = new Size(1300, 720);
        splitMain.SplitterDistance = 420;
        splitMain.TabIndex = 0;
        //
        // pnlLinksTop
        //
        pnlLinksTop.Controls.Add(pnlDatum);
        pnlLinksTop.Controls.Add(pnlTage);
        pnlLinksTop.Dock = DockStyle.Top;
        pnlLinksTop.Location = new Point(0, 0);
        pnlLinksTop.Name = "pnlLinksTop";
        pnlLinksTop.Padding = new Padding(4, 2, 4, 2);
        pnlLinksTop.Size = new Size(420, 76);
        pnlLinksTop.TabIndex = 0;
        //
        // pnlTage
        //
        pnlTage.Controls.Add(btnAlle);
        pnlTage.Controls.Add(btnSo);
        pnlTage.Controls.Add(btnSa);
        pnlTage.Controls.Add(btnFr);
        pnlTage.Controls.Add(btnDo);
        pnlTage.Controls.Add(btnMi);
        pnlTage.Controls.Add(btnDi);
        pnlTage.Controls.Add(btnMo);
        pnlTage.Dock = DockStyle.Top;
        pnlTage.Location = new Point(4, 2);
        pnlTage.Name = "pnlTage";
        pnlTage.Size = new Size(412, 36);
        pnlTage.TabIndex = 0;
        //
        // btnMo
        //
        btnMo.FlatStyle = FlatStyle.Flat;
        btnMo.Location = new Point(0, 5);
        btnMo.Name = "btnMo";
        btnMo.Size = new Size(40, 26);
        btnMo.TabIndex = 0;
        btnMo.Text = "Mo";
        btnMo.UseVisualStyleBackColor = false;
        //
        // btnDi
        //
        btnDi.FlatStyle = FlatStyle.Flat;
        btnDi.Location = new Point(44, 5);
        btnDi.Name = "btnDi";
        btnDi.Size = new Size(40, 26);
        btnDi.TabIndex = 1;
        btnDi.Text = "Di";
        btnDi.UseVisualStyleBackColor = false;
        //
        // btnMi
        //
        btnMi.FlatStyle = FlatStyle.Flat;
        btnMi.Location = new Point(88, 5);
        btnMi.Name = "btnMi";
        btnMi.Size = new Size(40, 26);
        btnMi.TabIndex = 2;
        btnMi.Text = "Mi";
        btnMi.UseVisualStyleBackColor = false;
        //
        // btnDo
        //
        btnDo.FlatStyle = FlatStyle.Flat;
        btnDo.Location = new Point(132, 5);
        btnDo.Name = "btnDo";
        btnDo.Size = new Size(40, 26);
        btnDo.TabIndex = 3;
        btnDo.Text = "Do";
        btnDo.UseVisualStyleBackColor = false;
        //
        // btnFr
        //
        btnFr.FlatStyle = FlatStyle.Flat;
        btnFr.Location = new Point(176, 5);
        btnFr.Name = "btnFr";
        btnFr.Size = new Size(40, 26);
        btnFr.TabIndex = 4;
        btnFr.Text = "Fr";
        btnFr.UseVisualStyleBackColor = false;
        //
        // btnSa
        //
        btnSa.FlatStyle = FlatStyle.Flat;
        btnSa.Location = new Point(220, 5);
        btnSa.Name = "btnSa";
        btnSa.Size = new Size(40, 26);
        btnSa.TabIndex = 5;
        btnSa.Text = "Sa";
        btnSa.UseVisualStyleBackColor = false;
        //
        // btnSo
        //
        btnSo.FlatStyle = FlatStyle.Flat;
        btnSo.Location = new Point(264, 5);
        btnSo.Name = "btnSo";
        btnSo.Size = new Size(40, 26);
        btnSo.TabIndex = 6;
        btnSo.Text = "So";
        btnSo.UseVisualStyleBackColor = false;
        //
        // btnAlle
        //
        btnAlle.FlatStyle = FlatStyle.Flat;
        btnAlle.Location = new Point(316, 5);
        btnAlle.Name = "btnAlle";
        btnAlle.Size = new Size(55, 26);
        btnAlle.TabIndex = 7;
        btnAlle.Text = "Alle";
        btnAlle.UseVisualStyleBackColor = false;
        //
        // pnlDatum
        //
        pnlDatum.Controls.Add(dtpLieferdatum);
        pnlDatum.Controls.Add(lblDatumCaption);
        pnlDatum.Dock = DockStyle.Fill;
        pnlDatum.Location = new Point(4, 38);
        pnlDatum.Name = "pnlDatum";
        pnlDatum.Size = new Size(412, 36);
        pnlDatum.TabIndex = 1;
        //
        // lblDatumCaption
        //
        lblDatumCaption.AutoSize = true;
        lblDatumCaption.Location = new Point(0, 10);
        lblDatumCaption.Name = "lblDatumCaption";
        lblDatumCaption.Size = new Size(83, 15);
        lblDatumCaption.TabIndex = 0;
        lblDatumCaption.Text = "Lieferdatum:";
        //
        // dtpLieferdatum
        //
        dtpLieferdatum.Format = DateTimePickerFormat.Short;
        dtpLieferdatum.Location = new Point(90, 6);
        dtpLieferdatum.Name = "dtpLieferdatum";
        dtpLieferdatum.Size = new Size(130, 23);
        dtpLieferdatum.TabIndex = 1;
        //
        // dgwKunden
        //
        dgwKunden.AllowUserToAddRows = false;
        dgwKunden.AllowUserToDeleteRows = false;
        dgwKunden.BackgroundColor = SystemColors.Window;
        dgwKunden.BorderStyle = BorderStyle.Fixed3D;
        dgwKunden.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgwKunden.Dock = DockStyle.Fill;
        dgwKunden.Location = new Point(0, 76);
        dgwKunden.MultiSelect = false;
        dgwKunden.Name = "dgwKunden";
        dgwKunden.ReadOnly = true;
        dgwKunden.RowHeadersVisible = false;
        dgwKunden.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgwKunden.Size = new Size(420, 644);
        dgwKunden.TabIndex = 1;
        //
        // pnlKopf
        //
        pnlKopf.BackColor = Color.FromArgb(240, 245, 255);
        pnlKopf.Controls.Add(lblAuftragStatus);
        pnlKopf.Controls.Add(lblSaldo);
        pnlKopf.Controls.Add(lblKundenname);
        pnlKopf.Dock = DockStyle.Top;
        pnlKopf.Location = new Point(0, 0);
        pnlKopf.Name = "pnlKopf";
        pnlKopf.Padding = new Padding(8, 4, 8, 4);
        pnlKopf.Size = new Size(876, 52);
        pnlKopf.TabIndex = 0;
        //
        // lblKundenname
        //
        lblKundenname.AutoSize = false;
        lblKundenname.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
        lblKundenname.Location = new Point(8, 8);
        lblKundenname.Name = "lblKundenname";
        lblKundenname.Size = new Size(500, 36);
        lblKundenname.TabIndex = 0;
        lblKundenname.Text = "— Kunden auswählen —";
        lblKundenname.TextAlign = ContentAlignment.MiddleLeft;
        //
        // lblSaldo
        //
        lblSaldo.AutoSize = false;
        lblSaldo.Font = new Font("Segoe UI", 10F);
        lblSaldo.Location = new Point(520, 12);
        lblSaldo.Name = "lblSaldo";
        lblSaldo.Size = new Size(180, 28);
        lblSaldo.TabIndex = 1;
        lblSaldo.TextAlign = ContentAlignment.MiddleLeft;
        //
        // lblAuftragStatus
        //
        lblAuftragStatus.AutoSize = false;
        lblAuftragStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblAuftragStatus.Location = new Point(710, 14);
        lblAuftragStatus.Name = "lblAuftragStatus";
        lblAuftragStatus.Size = new Size(140, 24);
        lblAuftragStatus.TabIndex = 2;
        lblAuftragStatus.TextAlign = ContentAlignment.MiddleCenter;
        //
        // pnlAktionen
        //
        pnlAktionen.Controls.Add(lblStatusInfo);
        pnlAktionen.Controls.Add(btnStornieren);
        pnlAktionen.Controls.Add(btnSpeichern);
        pnlAktionen.Controls.Add(btnBuchen);
        pnlAktionen.Dock = DockStyle.Bottom;
        pnlAktionen.Location = new Point(0, 675);
        pnlAktionen.Name = "pnlAktionen";
        pnlAktionen.Padding = new Padding(6, 8, 6, 6);
        pnlAktionen.Size = new Size(876, 45);
        pnlAktionen.TabIndex = 1;
        //
        // btnBuchen
        //
        btnBuchen.BackColor = Color.SteelBlue;
        btnBuchen.FlatStyle = FlatStyle.Flat;
        btnBuchen.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnBuchen.ForeColor = Color.White;
        btnBuchen.Location = new Point(6, 8);
        btnBuchen.Name = "btnBuchen";
        btnBuchen.Size = new Size(130, 28);
        btnBuchen.TabIndex = 0;
        btnBuchen.Text = "✓ Buchen (F5)";
        btnBuchen.UseVisualStyleBackColor = false;
        //
        // btnSpeichern
        //
        btnSpeichern.Location = new Point(142, 8);
        btnSpeichern.Name = "btnSpeichern";
        btnSpeichern.Size = new Size(110, 28);
        btnSpeichern.TabIndex = 1;
        btnSpeichern.Text = "Speichern";
        //
        // btnStornieren
        //
        btnStornieren.ForeColor = Color.Maroon;
        btnStornieren.Location = new Point(258, 8);
        btnStornieren.Name = "btnStornieren";
        btnStornieren.Size = new Size(110, 28);
        btnStornieren.TabIndex = 2;
        btnStornieren.Text = "Stornieren";
        //
        // lblStatusInfo
        //
        lblStatusInfo.AutoSize = false;
        lblStatusInfo.Location = new Point(380, 12);
        lblStatusInfo.Name = "lblStatusInfo";
        lblStatusInfo.Size = new Size(480, 22);
        lblStatusInfo.TabIndex = 3;
        lblStatusInfo.TextAlign = ContentAlignment.MiddleLeft;
        //
        // dgwPositionen
        //
        dgwPositionen.AllowUserToAddRows = false;
        dgwPositionen.AllowUserToDeleteRows = false;
        dgwPositionen.AutoGenerateColumns = false;
        dgwPositionen.BackgroundColor = SystemColors.Window;
        dgwPositionen.BorderStyle = BorderStyle.Fixed3D;
        dgwPositionen.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgwPositionen.Columns.AddRange(colZeileId, colArtikelId, colArtikelnummer, colProduktname, colMenge, colGewicht, colPreis, colNotiz);
        dgwPositionen.Dock = DockStyle.Fill;
        dgwPositionen.EditMode = DataGridViewEditMode.EditOnEnter;
        dgwPositionen.Location = new Point(0, 52);
        dgwPositionen.MultiSelect = false;
        dgwPositionen.Name = "dgwPositionen";
        dgwPositionen.RowHeadersVisible = false;
        dgwPositionen.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgwPositionen.Size = new Size(876, 623);
        dgwPositionen.TabIndex = 2;
        //
        // colZeileId
        //
        colZeileId.Name = "_ZeileId";
        colZeileId.Visible = false;
        //
        // colArtikelId
        //
        colArtikelId.Name = "_ArtikelId";
        colArtikelId.Visible = false;
        //
        // colArtikelnummer
        //
        colArtikelnummer.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colArtikelnummer.HeaderText = "Art.-Nr.";
        colArtikelnummer.Name = "Artikelnummer";
        colArtikelnummer.ReadOnly = true;
        colArtikelnummer.Width = 90;
        //
        // colProduktname
        //
        colProduktname.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colProduktname.HeaderText = "Produkt";
        colProduktname.Name = "Produktname";
        colProduktname.ReadOnly = true;
        //
        // colMenge
        //
        colMenge.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colMenge.DefaultCellStyle = new DataGridViewCellStyle { Format = "N3", Alignment = DataGridViewContentAlignment.MiddleRight };
        colMenge.HeaderText = "Menge";
        colMenge.Name = "Menge";
        colMenge.Width = 80;
        //
        // colGewicht
        //
        colGewicht.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colGewicht.DefaultCellStyle = new DataGridViewCellStyle { Format = "N3", Alignment = DataGridViewContentAlignment.MiddleRight };
        colGewicht.HeaderText = "Gewicht";
        colGewicht.Name = "Gewicht";
        colGewicht.Width = 80;
        //
        // colPreis
        //
        colPreis.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colPreis.DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight };
        colPreis.HeaderText = "Preis";
        colPreis.Name = "Preis";
        colPreis.Width = 85;
        //
        // colNotiz
        //
        colNotiz.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colNotiz.HeaderText = "Notiz";
        colNotiz.Name = "Notiz";
        colNotiz.Width = 160;
        //
        // FrmOrderEntry
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1300, 720);
        Controls.Add(splitMain);
        Font = new Font("Segoe UI", 9F);
        Name = "FrmOrderEntry";
        Text = "Auftragserfassung";
        splitMain.Panel1.ResumeLayout(false);
        splitMain.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
        splitMain.ResumeLayout(false);
        pnlLinksTop.ResumeLayout(false);
        pnlDatum.ResumeLayout(false);
        pnlDatum.PerformLayout();
        pnlTage.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgwKunden).EndInit();
        pnlKopf.ResumeLayout(false);
        pnlAktionen.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgwPositionen).EndInit();
        ResumeLayout(false);
    }

    private SplitContainer splitMain;
    private Panel pnlLinksTop;
    private Panel pnlTage;
    private Button btnMo;
    private Button btnDi;
    private Button btnMi;
    private Button btnDo;
    private Button btnFr;
    private Button btnSa;
    private Button btnSo;
    private Button btnAlle;
    private Panel pnlDatum;
    private Label lblDatumCaption;
    private DateTimePicker dtpLieferdatum;
    private DataGridView dgwKunden;
    private Panel pnlKopf;
    private Label lblKundenname;
    private Label lblSaldo;
    private Label lblAuftragStatus;
    private Panel pnlAktionen;
    private Button btnBuchen;
    private Button btnSpeichern;
    private Button btnStornieren;
    private Label lblStatusInfo;
    private DataGridView dgwPositionen;
    private DataGridViewTextBoxColumn colZeileId;
    private DataGridViewTextBoxColumn colArtikelId;
    private DataGridViewTextBoxColumn colArtikelnummer;
    private DataGridViewTextBoxColumn colProduktname;
    private DataGridViewTextBoxColumn colMenge;
    private DataGridViewTextBoxColumn colGewicht;
    private DataGridViewTextBoxColumn colPreis;
    private DataGridViewTextBoxColumn colNotiz;
}