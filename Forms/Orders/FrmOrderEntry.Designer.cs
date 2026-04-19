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
        components = new System.ComponentModel.Container();
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
        splitMain             = new SplitContainer();
        dgwKunden             = new DataGridView();
        pnlLinksTop           = new Panel();
        pnlDatum              = new Panel();
        dtpLieferdatum        = new DateTimePicker();
        lblDatumCaption       = new Label();
        pnlTage               = new Panel();
        btnAlle               = new Button();
        btnSo                 = new Button();
        btnSa                 = new Button();
        btnFr                 = new Button();
        btnDo                 = new Button();
        btnMi                 = new Button();
        btnDi                 = new Button();
        btnMo                 = new Button();
        dgwPositionen         = new DataGridView();
        colZeileId            = new DataGridViewTextBoxColumn();
        colArtikelId          = new DataGridViewTextBoxColumn();
        colArtikelnummer      = new DataGridViewTextBoxColumn();
        colProduktname        = new DataGridViewTextBoxColumn();
        colMenge              = new DataGridViewTextBoxColumn();
        colGewicht            = new DataGridViewTextBoxColumn();
        colPreis              = new DataGridViewTextBoxColumn();
        colNotiz              = new DataGridViewTextBoxColumn();
        cmsPositionen         = new ContextMenuStrip(components);
        cmsMenuZeileLoeschen  = new ToolStripMenuItem();
        cmsMenuTrenner        = new ToolStripSeparator();
        cmsMenuHinzufuegen    = new ToolStripMenuItem();
        pnlAktionen           = new Panel();
        lblStatusInfo         = new Label();
        btnStornieren         = new Button();
        btnLoeschen           = new Button();
        btnNachlieferung      = new Button();
        btnHinzufuegen        = new Button();
        btnFreigeben          = new Button();
        btnSpeichern          = new Button();
        btnBuchen             = new Button();
        pnlFactBox            = new Panel();
        lblFBOffeneAuftraege  = new Label();
        lblFBOffenCaption     = new Label();
        lblFBLetzterAuftrag   = new Label();
        lblFBLetzterCaption   = new Label();
        lblFBSaldo            = new Label();
        lblFBSaldoCaption     = new Label();
        lblFBTour             = new Label();
        lblFBTourCaption      = new Label();
        lblFBTitle            = new Label();
        pnlKopf               = new Panel();
        lblAuftragStatus      = new Label();
        lblKundenname         = new Label();
        ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
        splitMain.Panel1.SuspendLayout();
        splitMain.Panel2.SuspendLayout();
        splitMain.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgwKunden).BeginInit();
        pnlLinksTop.SuspendLayout();
        pnlDatum.SuspendLayout();
        pnlTage.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgwPositionen).BeginInit();
        cmsPositionen.SuspendLayout();
        pnlAktionen.SuspendLayout();
        pnlFactBox.SuspendLayout();
        pnlKopf.SuspendLayout();
        SuspendLayout();
        // 
        // splitMain
        // 
        splitMain.Dock = DockStyle.Fill;
        splitMain.FixedPanel = FixedPanel.Panel1;
        splitMain.Location = new Point(0, 0);
        splitMain.Name = "splitMain";
        // 
        // splitMain.Panel1
        // 
        splitMain.Panel1.Controls.Add(dgwKunden);
        splitMain.Panel1.Controls.Add(pnlLinksTop);
        // 
        // splitMain.Panel2
        // 
        splitMain.Panel2.Controls.Add(dgwPositionen);
        splitMain.Panel2.Controls.Add(pnlAktionen);
        splitMain.Panel2.Controls.Add(pnlFactBox);
        splitMain.Panel2.Controls.Add(pnlKopf);
        splitMain.Size = new Size(1300, 720);
        splitMain.SplitterDistance = 420;
        splitMain.TabIndex = 0;
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
        // dtpLieferdatum
        // 
        dtpLieferdatum.Format = DateTimePickerFormat.Short;
        dtpLieferdatum.Location = new Point(90, 6);
        dtpLieferdatum.Name = "dtpLieferdatum";
        dtpLieferdatum.Size = new Size(130, 23);
        dtpLieferdatum.TabIndex = 1;
        // 
        // lblDatumCaption
        // 
        lblDatumCaption.AutoSize = true;
        lblDatumCaption.Location = new Point(0, 10);
        lblDatumCaption.Name = "lblDatumCaption";
        lblDatumCaption.Size = new Size(74, 15);
        lblDatumCaption.TabIndex = 0;
        lblDatumCaption.Text = "Lieferdatum:";
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
        // dgwPositionen
        // 
        dgwPositionen.AllowUserToAddRows = false;
        dgwPositionen.AllowUserToDeleteRows = false;
        dgwPositionen.BackgroundColor = SystemColors.Window;
        dgwPositionen.BorderStyle = BorderStyle.Fixed3D;
        dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle1.BackColor = SystemColors.Control;
        dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
        dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
        dgwPositionen.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
        dgwPositionen.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgwPositionen.Columns.AddRange(new DataGridViewColumn[] { colZeileId, colArtikelId, colArtikelnummer, colProduktname, colMenge, colGewicht, colPreis, colNotiz });
        dgwPositionen.ContextMenuStrip = cmsPositionen;
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle2.BackColor = SystemColors.Window;
        dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
        dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
        dgwPositionen.DefaultCellStyle = dataGridViewCellStyle2;
        dgwPositionen.Dock = DockStyle.Fill;
        dgwPositionen.EditMode = DataGridViewEditMode.EditOnEnter;
        dgwPositionen.Location = new Point(0, 44);
        dgwPositionen.MultiSelect = false;
        dgwPositionen.Name = "dgwPositionen";
        dgwPositionen.RowHeadersVisible = false;
        dgwPositionen.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgwPositionen.Size = new Size(672, 631);
        dgwPositionen.TabIndex = 2;
        // 
        // colZeileId
        // 
        colZeileId.Name = "colZeileId";
        colZeileId.Visible = false;
        // 
        // colArtikelId
        // 
        colArtikelId.Name = "colArtikelId";
        colArtikelId.Visible = false;
        // 
        // colArtikelnummer
        // 
        colArtikelnummer.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colArtikelnummer.HeaderText = "Art.-Nr.";
        colArtikelnummer.Name = "colArtikelnummer";
        colArtikelnummer.ReadOnly = true;
        colArtikelnummer.Width = 90;
        // 
        // colProduktname
        // 
        colProduktname.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colProduktname.HeaderText = "Produkt";
        colProduktname.Name = "colProduktname";
        colProduktname.ReadOnly = true;
        // 
        // colMenge
        // 
        colMenge.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colMenge.HeaderText = "Menge";
        colMenge.Name = "colMenge";
        colMenge.Width = 80;
        // 
        // colGewicht
        // 
        colGewicht.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colGewicht.HeaderText = "Gewicht";
        colGewicht.Name = "colGewicht";
        colGewicht.Width = 80;
        // 
        // colPreis
        // 
        colPreis.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colPreis.HeaderText = "Preis";
        colPreis.Name = "colPreis";
        colPreis.Width = 85;
        // 
        // colNotiz
        // 
        colNotiz.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colNotiz.HeaderText = "Notiz";
        colNotiz.Name = "colNotiz";
        colNotiz.Width = 160;
        // 
        // cmsPositionen
        // 
        cmsPositionen.Items.AddRange(new ToolStripItem[] { cmsMenuZeileLoeschen, cmsMenuTrenner, cmsMenuHinzufuegen });
        cmsPositionen.Name = "cmsPositionen";
        cmsPositionen.Size = new Size(181, 76);
        // 
        // cmsMenuZeileLoeschen
        // 
        cmsMenuZeileLoeschen.Name = "cmsMenuZeileLoeschen";
        cmsMenuZeileLoeschen.Size = new Size(180, 22);
        cmsMenuZeileLoeschen.Text = "Zeile löschen";
        // 
        // cmsMenuTrenner
        // 
        cmsMenuTrenner.Name = "cmsMenuTrenner";
        cmsMenuTrenner.Size = new Size(177, 6);
        // 
        // cmsMenuHinzufuegen
        // 
        cmsMenuHinzufuegen.Name = "cmsMenuHinzufuegen";
        cmsMenuHinzufuegen.Size = new Size(180, 22);
        cmsMenuHinzufuegen.Text = "Artikel hinzufügen";
        // 
        // pnlAktionen
        // 
        pnlAktionen.Controls.Add(lblStatusInfo);
        pnlAktionen.Controls.Add(btnStornieren);
        pnlAktionen.Controls.Add(btnLoeschen);
        pnlAktionen.Controls.Add(btnNachlieferung);
        pnlAktionen.Controls.Add(btnHinzufuegen);
        pnlAktionen.Controls.Add(btnFreigeben);
        pnlAktionen.Controls.Add(btnSpeichern);
        pnlAktionen.Controls.Add(btnBuchen);
        pnlAktionen.Dock = DockStyle.Bottom;
        pnlAktionen.Location = new Point(0, 675);
        pnlAktionen.Name = "pnlAktionen";
        pnlAktionen.Padding = new Padding(6, 8, 6, 6);
        pnlAktionen.Size = new Size(876, 45);
        pnlAktionen.TabIndex = 1;
        // 
        // lblStatusInfo
        // 
        lblStatusInfo.Location = new Point(576, 12);
        lblStatusInfo.Name = "lblStatusInfo";
        lblStatusInfo.Size = new Size(220, 22);
        lblStatusInfo.TabIndex = 7;
        lblStatusInfo.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // btnStornieren
        // 
        btnStornieren.ForeColor = Color.Maroon;
        btnStornieren.Location = new Point(470, 8);
        btnStornieren.Name = "btnStornieren";
        btnStornieren.Size = new Size(100, 28);
        btnStornieren.TabIndex = 5;
        btnStornieren.Text = "Stornieren";
        // 
        // btnLoeschen  (same position as btnStornieren — mutually exclusive visibility)
        // 
        btnLoeschen.BackColor = Color.Firebrick;
        btnLoeschen.FlatStyle = FlatStyle.Flat;
        btnLoeschen.ForeColor = Color.White;
        btnLoeschen.Location = new Point(470, 8);
        btnLoeschen.Name = "btnLoeschen";
        btnLoeschen.Size = new Size(95, 28);
        btnLoeschen.TabIndex = 6;
        btnLoeschen.Text = "Löschen";
        btnLoeschen.UseVisualStyleBackColor = false;
        // 
        // btnNachlieferung  (same position as btnFreigeben — mutually exclusive visibility)
        // 
        btnNachlieferung.Location = new Point(364, 8);
        btnNachlieferung.Name = "btnNachlieferung";
        btnNachlieferung.Size = new Size(120, 28);
        btnNachlieferung.TabIndex = 4;
        btnNachlieferung.Text = "+ Nachlieferung";
        // 
        // btnHinzufuegen
        // 
        btnHinzufuegen.Location = new Point(248, 8);
        btnHinzufuegen.Name = "btnHinzufuegen";
        btnHinzufuegen.Size = new Size(110, 28);
        btnHinzufuegen.TabIndex = 2;
        btnHinzufuegen.Text = "+ Hinzufügen";
        // 
        // btnFreigeben  (same position as btnNachlieferung — mutually exclusive visibility)
        // 
        btnFreigeben.Location = new Point(364, 8);
        btnFreigeben.Name = "btnFreigeben";
        btnFreigeben.Size = new Size(100, 28);
        btnFreigeben.TabIndex = 3;
        btnFreigeben.Text = "Freigeben";
        // 
        // btnSpeichern
        // 
        btnSpeichern.Location = new Point(142, 8);
        btnSpeichern.Name = "btnSpeichern";
        btnSpeichern.Size = new Size(100, 28);
        btnSpeichern.TabIndex = 1;
        btnSpeichern.Text = "Speichern";
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
        // pnlFactBox
        // 
        pnlFactBox.BackColor = Color.FromArgb(245, 248, 255);
        pnlFactBox.BorderStyle = BorderStyle.FixedSingle;
        pnlFactBox.Controls.Add(lblFBOffeneAuftraege);
        pnlFactBox.Controls.Add(lblFBOffenCaption);
        pnlFactBox.Controls.Add(lblFBLetzterAuftrag);
        pnlFactBox.Controls.Add(lblFBLetzterCaption);
        pnlFactBox.Controls.Add(lblFBSaldo);
        pnlFactBox.Controls.Add(lblFBSaldoCaption);
        pnlFactBox.Controls.Add(lblFBTour);
        pnlFactBox.Controls.Add(lblFBTourCaption);
        pnlFactBox.Controls.Add(lblFBTitle);
        pnlFactBox.Dock = DockStyle.Right;
        pnlFactBox.Location = new Point(672, 44);
        pnlFactBox.Name = "pnlFactBox";
        pnlFactBox.Padding = new Padding(8, 6, 8, 6);
        pnlFactBox.Size = new Size(204, 676);
        pnlFactBox.TabIndex = 3;
        // 
        // lblFBOffeneAuftraege
        // 
        lblFBOffeneAuftraege.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblFBOffeneAuftraege.Location = new Point(8, 196);
        lblFBOffeneAuftraege.Name = "lblFBOffeneAuftraege";
        lblFBOffeneAuftraege.Size = new Size(186, 18);
        lblFBOffeneAuftraege.TabIndex = 8;
        // 
        // lblFBOffenCaption
        // 
        lblFBOffenCaption.AutoSize = true;
        lblFBOffenCaption.ForeColor = Color.Gray;
        lblFBOffenCaption.Location = new Point(8, 180);
        lblFBOffenCaption.Name = "lblFBOffenCaption";
        lblFBOffenCaption.Size = new Size(95, 15);
        lblFBOffenCaption.TabIndex = 7;
        lblFBOffenCaption.Text = "Offene Aufträge:";
        // 
        // lblFBLetzterAuftrag
        // 
        lblFBLetzterAuftrag.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblFBLetzterAuftrag.Location = new Point(8, 150);
        lblFBLetzterAuftrag.Name = "lblFBLetzterAuftrag";
        lblFBLetzterAuftrag.Size = new Size(186, 18);
        lblFBLetzterAuftrag.TabIndex = 6;
        // 
        // lblFBLetzterCaption
        // 
        lblFBLetzterCaption.AutoSize = true;
        lblFBLetzterCaption.ForeColor = Color.Gray;
        lblFBLetzterCaption.Location = new Point(8, 134);
        lblFBLetzterCaption.Name = "lblFBLetzterCaption";
        lblFBLetzterCaption.Size = new Size(88, 15);
        lblFBLetzterCaption.TabIndex = 5;
        lblFBLetzterCaption.Text = "Letzter Auftrag:";
        // 
        // lblFBSaldo
        // 
        lblFBSaldo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblFBSaldo.Location = new Point(8, 98);
        lblFBSaldo.Name = "lblFBSaldo";
        lblFBSaldo.Size = new Size(186, 24);
        lblFBSaldo.TabIndex = 4;
        // 
        // lblFBSaldoCaption
        // 
        lblFBSaldoCaption.AutoSize = true;
        lblFBSaldoCaption.ForeColor = Color.Gray;
        lblFBSaldoCaption.Location = new Point(8, 82);
        lblFBSaldoCaption.Name = "lblFBSaldoCaption";
        lblFBSaldoCaption.Size = new Size(39, 15);
        lblFBSaldoCaption.TabIndex = 3;
        lblFBSaldoCaption.Text = "Saldo:";
        // 
        // lblFBTour
        // 
        lblFBTour.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblFBTour.Location = new Point(8, 52);
        lblFBTour.Name = "lblFBTour";
        lblFBTour.Size = new Size(186, 18);
        lblFBTour.TabIndex = 2;
        // 
        // lblFBTourCaption
        // 
        lblFBTourCaption.AutoSize = true;
        lblFBTourCaption.ForeColor = Color.Gray;
        lblFBTourCaption.Location = new Point(8, 36);
        lblFBTourCaption.Name = "lblFBTourCaption";
        lblFBTourCaption.Size = new Size(33, 15);
        lblFBTourCaption.TabIndex = 1;
        lblFBTourCaption.Text = "Tour:";
        // 
        // lblFBTitle
        // 
        lblFBTitle.BackColor = Color.SteelBlue;
        lblFBTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblFBTitle.ForeColor = Color.White;
        lblFBTitle.Location = new Point(0, 0);
        lblFBTitle.Name = "lblFBTitle";
        lblFBTitle.Padding = new Padding(6, 0, 0, 0);
        lblFBTitle.Size = new Size(202, 26);
        lblFBTitle.TabIndex = 0;
        lblFBTitle.Text = "Kundeninfo";
        lblFBTitle.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // pnlKopf
        // 
        pnlKopf.BackColor = Color.FromArgb(240, 245, 255);
        pnlKopf.Controls.Add(lblAuftragStatus);
        pnlKopf.Controls.Add(lblKundenname);
        pnlKopf.Dock = DockStyle.Top;
        pnlKopf.Location = new Point(0, 0);
        pnlKopf.Name = "pnlKopf";
        pnlKopf.Padding = new Padding(8, 4, 8, 4);
        pnlKopf.Size = new Size(876, 44);
        pnlKopf.TabIndex = 0;
        // 
        // lblAuftragStatus
        // 
        lblAuftragStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblAuftragStatus.Location = new Point(500, 10);
        lblAuftragStatus.Name = "lblAuftragStatus";
        lblAuftragStatus.Size = new Size(160, 24);
        lblAuftragStatus.TabIndex = 1;
        lblAuftragStatus.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblKundenname
        // 
        lblKundenname.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblKundenname.Location = new Point(8, 6);
        lblKundenname.Name = "lblKundenname";
        lblKundenname.Size = new Size(480, 32);
        lblKundenname.TabIndex = 0;
        lblKundenname.Text = "— Kunden auswählen —";
        lblKundenname.TextAlign = ContentAlignment.MiddleLeft;
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
        ((System.ComponentModel.ISupportInitialize)dgwKunden).EndInit();
        pnlLinksTop.ResumeLayout(false);
        pnlDatum.ResumeLayout(false);
        pnlDatum.PerformLayout();
        pnlTage.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgwPositionen).EndInit();
        cmsPositionen.ResumeLayout(false);
        pnlAktionen.ResumeLayout(false);
        pnlFactBox.ResumeLayout(false);
        pnlFactBox.PerformLayout();
        pnlKopf.ResumeLayout(false);
        ResumeLayout(false);
    }

    // ── Fields ────────────────────────────────────────────────────────────────
    private SplitContainer splitMain;
    private Panel          pnlLinksTop;
    private Panel          pnlTage;
    private Button         btnMo;
    private Button         btnDi;
    private Button         btnMi;
    private Button         btnDo;
    private Button         btnFr;
    private Button         btnSa;
    private Button         btnSo;
    private Button         btnAlle;
    private Panel          pnlDatum;
    private Label          lblDatumCaption;
    private DateTimePicker dtpLieferdatum;
    private DataGridView   dgwKunden;
    private Panel          pnlKopf;
    private Label          lblKundenname;
    private Label          lblAuftragStatus;
    private Panel          pnlAktionen;
    private Button         btnBuchen;
    private Button         btnSpeichern;
    private Button         btnFreigeben;
    private Button         btnHinzufuegen;
    private Button         btnNachlieferung;
    private Button         btnStornieren;
    private Button         btnLoeschen;
    private Label          lblStatusInfo;
    private DataGridView   dgwPositionen;
    private DataGridViewTextBoxColumn colZeileId;
    private DataGridViewTextBoxColumn colArtikelId;
    private DataGridViewTextBoxColumn colArtikelnummer;
    private DataGridViewTextBoxColumn colProduktname;
    private DataGridViewTextBoxColumn colMenge;
    private DataGridViewTextBoxColumn colGewicht;
    private DataGridViewTextBoxColumn colPreis;
    private DataGridViewTextBoxColumn colNotiz;
    // ── ContextMenu ───────────────────────────────────────────────────────────
    private ContextMenuStrip  cmsPositionen;
    private ToolStripMenuItem cmsMenuZeileLoeschen;
    private ToolStripSeparator cmsMenuTrenner;
    private ToolStripMenuItem cmsMenuHinzufuegen;
    // ── FactBox ───────────────────────────────────────────────────────────────
    private Panel pnlFactBox;
    private Label lblFBTitle;
    private Label lblFBTourCaption;
    private Label lblFBTour;
    private Label lblFBSaldoCaption;
    private Label lblFBSaldo;
    private Label lblFBLetzterCaption;
    private Label lblFBLetzterAuftrag;
    private Label lblFBOffenCaption;
    private Label lblFBOffeneAuftraege;
}