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
        DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
        splitMain = new SplitContainer();
        dgwKunden = new DataGridView();
        colKundeChecked = new DataGridViewCheckBoxColumn();
        pnlLinksTop = new Panel();
        pnlFilter = new Panel();
        btnAlleFreigeben = new Button();
        cmbTourFilter = new ComboBox();
        txtKundeFilter = new TextBox();
        pnlDatum = new Panel();
        dtpLieferdatum = new DateTimePicker();
        lblDatumCaption = new Label();
        pnlTage = new Panel();
        btnAlle = new Button();
        btnSo = new Button();
        btnSa = new Button();
        btnFr = new Button();
        btnDo = new Button();
        btnMi = new Button();
        btnDi = new Button();
        btnMo = new Button();
        dgwPositionen = new DataGridView();
        colZeileId = new DataGridViewTextBoxColumn();
        colArtikelId = new DataGridViewTextBoxColumn();
        colArtikelnummer = new DataGridViewTextBoxColumn();
        colProduktname = new DataGridViewTextBoxColumn();
        colMenge = new DataGridViewTextBoxColumn();
        colGewicht = new DataGridViewTextBoxColumn();
        colPreis = new DataGridViewTextBoxColumn();
        colNotiz = new DataGridViewTextBoxColumn();
        cmsPositionen = new ContextMenuStrip(components);
        cmsMenuZeileLoeschen = new ToolStripMenuItem();
        cmsMenuTrenner = new ToolStripSeparator();
        cmsMenuHinzufuegen = new ToolStripMenuItem();
        pnlAktionen = new Panel();
        lblStatusInfo = new Label();
        btnStornieren = new Button();
        btnLoeschen = new Button();
        btnNachlieferung = new Button();
        btnHinzufuegen = new Button();
        btnFreigeben = new Button();
        btnSpeichern = new Button();
        btnBuchen = new Button();
        pnlFactBox = new Panel();
        lblFBOffeneAuftraege = new Label();
        lblFBOffenCaption = new Label();
        lblFBLetzterAuftrag = new Label();
        lblFBLetzterCaption = new Label();
        lblFBSaldo = new Label();
        lblFBSaldoCaption = new Label();
        lblFBTour = new Label();
        lblFBTourCaption = new Label();
        lblFBTitle = new Label();
        pnlKopf = new Panel();
        lblAuftragStatus = new Label();
        lblKundenname = new Label();
        btnAlleAuswaehlenGespeichert = new Button();
        ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
        splitMain.Panel1.SuspendLayout();
        splitMain.Panel2.SuspendLayout();
        splitMain.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgwKunden).BeginInit();
        pnlLinksTop.SuspendLayout();
        pnlFilter.SuspendLayout();
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
        splitMain.Margin = new Padding(3, 4, 3, 4);
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
        splitMain.Size = new Size(1486, 960);
        splitMain.SplitterDistance = 480;
        splitMain.SplitterWidth = 5;
        splitMain.TabIndex = 0;
        // 
        // dgwKunden
        // 
        dgwKunden.AllowUserToAddRows = false;
        dgwKunden.AllowUserToDeleteRows = false;
        dgwKunden.BackgroundColor = SystemColors.Window;
        dgwKunden.BorderStyle = BorderStyle.Fixed3D;
        dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle1.BackColor = SystemColors.Control;
        dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
        dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
        dgwKunden.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
        dgwKunden.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgwKunden.Columns.AddRange(new DataGridViewColumn[] { colKundeChecked });
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle2.BackColor = SystemColors.Window;
        dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
        dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
        dgwKunden.DefaultCellStyle = dataGridViewCellStyle2;
        dgwKunden.Dock = DockStyle.Fill;
        dgwKunden.Location = new Point(0, 149);
        dgwKunden.Margin = new Padding(3, 4, 3, 4);
        dgwKunden.MultiSelect = false;
        dgwKunden.Name = "dgwKunden";
        dgwKunden.RowHeadersVisible = false;
        dgwKunden.RowHeadersWidth = 51;
        dgwKunden.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgwKunden.Size = new Size(480, 811);
        dgwKunden.TabIndex = 1;
        // 
        // colKundeChecked
        // 
        colKundeChecked.HeaderText = "";
        colKundeChecked.MinimumWidth = 28;
        colKundeChecked.Name = "colKundeChecked";
        colKundeChecked.Width = 28;
        // 
        // pnlLinksTop
        // 
        pnlLinksTop.Controls.Add(pnlFilter);
        pnlLinksTop.Controls.Add(pnlDatum);
        pnlLinksTop.Controls.Add(pnlTage);
        pnlLinksTop.Dock = DockStyle.Top;
        pnlLinksTop.Location = new Point(0, 0);
        pnlLinksTop.Margin = new Padding(3, 4, 3, 4);
        pnlLinksTop.Name = "pnlLinksTop";
        pnlLinksTop.Padding = new Padding(5, 3, 5, 3);
        pnlLinksTop.Size = new Size(480, 149);
        pnlLinksTop.TabIndex = 0;
        // 
        // pnlFilter
        // 
        pnlFilter.Controls.Add(btnAlleAuswaehlenGespeichert);
        pnlFilter.Controls.Add(btnAlleFreigeben);
        pnlFilter.Controls.Add(cmbTourFilter);
        pnlFilter.Controls.Add(txtKundeFilter);
        pnlFilter.Dock = DockStyle.Fill;
        pnlFilter.Location = new Point(5, 99);
        pnlFilter.Margin = new Padding(3, 4, 3, 4);
        pnlFilter.Name = "pnlFilter";
        pnlFilter.Size = new Size(470, 47);
        pnlFilter.TabIndex = 2;
        // 
        // btnAlleFreigeben
        // 
        btnAlleFreigeben.Location = new Point(361, 7);
        btnAlleFreigeben.Margin = new Padding(3, 4, 3, 4);
        btnAlleFreigeben.Name = "btnAlleFreigeben";
        btnAlleFreigeben.Size = new Size(106, 35);
        btnAlleFreigeben.TabIndex = 2;
        btnAlleFreigeben.Text = "Freigeben";
        // 
        // cmbTourFilter
        // 
        cmbTourFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbTourFilter.Location = new Point(177, 7);
        cmbTourFilter.Margin = new Padding(3, 4, 3, 4);
        cmbTourFilter.Name = "cmbTourFilter";
        cmbTourFilter.Size = new Size(83, 28);
        cmbTourFilter.TabIndex = 1;
        // 
        // txtKundeFilter
        // 
        txtKundeFilter.Location = new Point(0, 8);
        txtKundeFilter.Margin = new Padding(3, 4, 3, 4);
        txtKundeFilter.Name = "txtKundeFilter";
        txtKundeFilter.PlaceholderText = "Kunde...";
        txtKundeFilter.Size = new Size(171, 27);
        txtKundeFilter.TabIndex = 0;
        // 
        // pnlDatum
        // 
        pnlDatum.Controls.Add(dtpLieferdatum);
        pnlDatum.Controls.Add(lblDatumCaption);
        pnlDatum.Dock = DockStyle.Top;
        pnlDatum.Location = new Point(5, 51);
        pnlDatum.Margin = new Padding(3, 4, 3, 4);
        pnlDatum.Name = "pnlDatum";
        pnlDatum.Size = new Size(470, 48);
        pnlDatum.TabIndex = 1;
        // 
        // dtpLieferdatum
        // 
        dtpLieferdatum.Format = DateTimePickerFormat.Short;
        dtpLieferdatum.Location = new Point(103, 8);
        dtpLieferdatum.Margin = new Padding(3, 4, 3, 4);
        dtpLieferdatum.Name = "dtpLieferdatum";
        dtpLieferdatum.Size = new Size(148, 27);
        dtpLieferdatum.TabIndex = 1;
        // 
        // lblDatumCaption
        // 
        lblDatumCaption.AutoSize = true;
        lblDatumCaption.Location = new Point(0, 13);
        lblDatumCaption.Name = "lblDatumCaption";
        lblDatumCaption.Size = new Size(92, 20);
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
        pnlTage.Location = new Point(5, 3);
        pnlTage.Margin = new Padding(3, 4, 3, 4);
        pnlTage.Name = "pnlTage";
        pnlTage.Size = new Size(470, 48);
        pnlTage.TabIndex = 0;
        // 
        // btnAlle
        // 
        btnAlle.FlatStyle = FlatStyle.Flat;
        btnAlle.Location = new Point(361, 7);
        btnAlle.Margin = new Padding(3, 4, 3, 4);
        btnAlle.Name = "btnAlle";
        btnAlle.Size = new Size(63, 35);
        btnAlle.TabIndex = 7;
        btnAlle.Text = "Alle";
        btnAlle.UseVisualStyleBackColor = false;
        // 
        // btnSo
        // 
        btnSo.FlatStyle = FlatStyle.Flat;
        btnSo.Location = new Point(302, 7);
        btnSo.Margin = new Padding(3, 4, 3, 4);
        btnSo.Name = "btnSo";
        btnSo.Size = new Size(46, 35);
        btnSo.TabIndex = 6;
        btnSo.Text = "So";
        btnSo.UseVisualStyleBackColor = false;
        // 
        // btnSa
        // 
        btnSa.FlatStyle = FlatStyle.Flat;
        btnSa.Location = new Point(251, 7);
        btnSa.Margin = new Padding(3, 4, 3, 4);
        btnSa.Name = "btnSa";
        btnSa.Size = new Size(46, 35);
        btnSa.TabIndex = 5;
        btnSa.Text = "Sa";
        btnSa.UseVisualStyleBackColor = false;
        // 
        // btnFr
        // 
        btnFr.FlatStyle = FlatStyle.Flat;
        btnFr.Location = new Point(201, 7);
        btnFr.Margin = new Padding(3, 4, 3, 4);
        btnFr.Name = "btnFr";
        btnFr.Size = new Size(46, 35);
        btnFr.TabIndex = 4;
        btnFr.Text = "Fr";
        btnFr.UseVisualStyleBackColor = false;
        // 
        // btnDo
        // 
        btnDo.FlatStyle = FlatStyle.Flat;
        btnDo.Location = new Point(151, 7);
        btnDo.Margin = new Padding(3, 4, 3, 4);
        btnDo.Name = "btnDo";
        btnDo.Size = new Size(46, 35);
        btnDo.TabIndex = 3;
        btnDo.Text = "Do";
        btnDo.UseVisualStyleBackColor = false;
        // 
        // btnMi
        // 
        btnMi.FlatStyle = FlatStyle.Flat;
        btnMi.Location = new Point(101, 7);
        btnMi.Margin = new Padding(3, 4, 3, 4);
        btnMi.Name = "btnMi";
        btnMi.Size = new Size(46, 35);
        btnMi.TabIndex = 2;
        btnMi.Text = "Mi";
        btnMi.UseVisualStyleBackColor = false;
        // 
        // btnDi
        // 
        btnDi.FlatStyle = FlatStyle.Flat;
        btnDi.Location = new Point(50, 7);
        btnDi.Margin = new Padding(3, 4, 3, 4);
        btnDi.Name = "btnDi";
        btnDi.Size = new Size(46, 35);
        btnDi.TabIndex = 1;
        btnDi.Text = "Di";
        btnDi.UseVisualStyleBackColor = false;
        // 
        // btnMo
        // 
        btnMo.FlatStyle = FlatStyle.Flat;
        btnMo.Location = new Point(0, 7);
        btnMo.Margin = new Padding(3, 4, 3, 4);
        btnMo.Name = "btnMo";
        btnMo.Size = new Size(46, 35);
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
        dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle3.BackColor = SystemColors.Control;
        dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
        dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
        dgwPositionen.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
        dgwPositionen.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgwPositionen.Columns.AddRange(new DataGridViewColumn[] { colZeileId, colArtikelId, colArtikelnummer, colProduktname, colMenge, colGewicht, colPreis, colNotiz });
        dgwPositionen.ContextMenuStrip = cmsPositionen;
        dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle4.BackColor = SystemColors.Window;
        dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle4.ForeColor = SystemColors.ControlText;
        dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
        dgwPositionen.DefaultCellStyle = dataGridViewCellStyle4;
        dgwPositionen.Dock = DockStyle.Fill;
        dgwPositionen.EditMode = DataGridViewEditMode.EditOnEnter;
        dgwPositionen.Location = new Point(0, 59);
        dgwPositionen.Margin = new Padding(3, 4, 3, 4);
        dgwPositionen.MultiSelect = false;
        dgwPositionen.Name = "dgwPositionen";
        dgwPositionen.RowHeadersVisible = false;
        dgwPositionen.RowHeadersWidth = 51;
        dgwPositionen.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgwPositionen.Size = new Size(768, 841);
        dgwPositionen.TabIndex = 2;
        // 
        // colZeileId
        // 
        colZeileId.MinimumWidth = 6;
        colZeileId.Name = "colZeileId";
        colZeileId.Visible = false;
        colZeileId.Width = 125;
        // 
        // colArtikelId
        // 
        colArtikelId.MinimumWidth = 6;
        colArtikelId.Name = "colArtikelId";
        colArtikelId.Visible = false;
        colArtikelId.Width = 125;
        // 
        // colArtikelnummer
        // 
        colArtikelnummer.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colArtikelnummer.HeaderText = "Art.-Nr.";
        colArtikelnummer.MinimumWidth = 6;
        colArtikelnummer.Name = "colArtikelnummer";
        colArtikelnummer.ReadOnly = true;
        colArtikelnummer.Width = 90;
        // 
        // colProduktname
        // 
        colProduktname.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colProduktname.HeaderText = "Produkt";
        colProduktname.MinimumWidth = 6;
        colProduktname.Name = "colProduktname";
        colProduktname.ReadOnly = true;
        // 
        // colMenge
        // 
        colMenge.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colMenge.HeaderText = "Menge";
        colMenge.MinimumWidth = 6;
        colMenge.Name = "colMenge";
        colMenge.Width = 80;
        // 
        // colGewicht
        // 
        colGewicht.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colGewicht.HeaderText = "Gewicht";
        colGewicht.MinimumWidth = 6;
        colGewicht.Name = "colGewicht";
        colGewicht.Width = 80;
        // 
        // colPreis
        // 
        colPreis.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colPreis.HeaderText = "Preis";
        colPreis.MinimumWidth = 6;
        colPreis.Name = "colPreis";
        colPreis.Width = 85;
        // 
        // colNotiz
        // 
        colNotiz.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colNotiz.HeaderText = "Notiz";
        colNotiz.MinimumWidth = 6;
        colNotiz.Name = "colNotiz";
        colNotiz.Width = 160;
        // 
        // cmsPositionen
        // 
        cmsPositionen.ImageScalingSize = new Size(20, 20);
        cmsPositionen.Items.AddRange(new ToolStripItem[] { cmsMenuZeileLoeschen, cmsMenuTrenner, cmsMenuHinzufuegen });
        cmsPositionen.Name = "cmsPositionen";
        cmsPositionen.Size = new Size(199, 58);
        // 
        // cmsMenuZeileLoeschen
        // 
        cmsMenuZeileLoeschen.Name = "cmsMenuZeileLoeschen";
        cmsMenuZeileLoeschen.Size = new Size(198, 24);
        cmsMenuZeileLoeschen.Text = "Zeile löschen";
        // 
        // cmsMenuTrenner
        // 
        cmsMenuTrenner.Name = "cmsMenuTrenner";
        cmsMenuTrenner.Size = new Size(195, 6);
        // 
        // cmsMenuHinzufuegen
        // 
        cmsMenuHinzufuegen.Name = "cmsMenuHinzufuegen";
        cmsMenuHinzufuegen.Size = new Size(198, 24);
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
        pnlAktionen.Location = new Point(0, 900);
        pnlAktionen.Margin = new Padding(3, 4, 3, 4);
        pnlAktionen.Name = "pnlAktionen";
        pnlAktionen.Padding = new Padding(7, 11, 7, 8);
        pnlAktionen.Size = new Size(768, 60);
        pnlAktionen.TabIndex = 1;
        // 
        // lblStatusInfo
        // 
        lblStatusInfo.Location = new Point(658, 16);
        lblStatusInfo.Name = "lblStatusInfo";
        lblStatusInfo.Size = new Size(251, 29);
        lblStatusInfo.TabIndex = 7;
        lblStatusInfo.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // btnStornieren
        // 
        btnStornieren.ForeColor = Color.Maroon;
        btnStornieren.Location = new Point(537, 11);
        btnStornieren.Margin = new Padding(3, 4, 3, 4);
        btnStornieren.Name = "btnStornieren";
        btnStornieren.Size = new Size(114, 37);
        btnStornieren.TabIndex = 5;
        btnStornieren.Text = "Stornieren";
        // 
        // btnLoeschen
        // 
        btnLoeschen.BackColor = Color.Firebrick;
        btnLoeschen.FlatStyle = FlatStyle.Flat;
        btnLoeschen.ForeColor = Color.White;
        btnLoeschen.Location = new Point(537, 11);
        btnLoeschen.Margin = new Padding(3, 4, 3, 4);
        btnLoeschen.Name = "btnLoeschen";
        btnLoeschen.Size = new Size(109, 37);
        btnLoeschen.TabIndex = 6;
        btnLoeschen.Text = "Löschen";
        btnLoeschen.UseVisualStyleBackColor = false;
        // 
        // btnNachlieferung
        // 
        btnNachlieferung.Location = new Point(416, 11);
        btnNachlieferung.Margin = new Padding(3, 4, 3, 4);
        btnNachlieferung.Name = "btnNachlieferung";
        btnNachlieferung.Size = new Size(137, 37);
        btnNachlieferung.TabIndex = 4;
        btnNachlieferung.Text = "+ Nachlieferung";
        // 
        // btnHinzufuegen
        // 
        btnHinzufuegen.Location = new Point(283, 11);
        btnHinzufuegen.Margin = new Padding(3, 4, 3, 4);
        btnHinzufuegen.Name = "btnHinzufuegen";
        btnHinzufuegen.Size = new Size(126, 37);
        btnHinzufuegen.TabIndex = 2;
        btnHinzufuegen.Text = "+ Hinzufügen";
        // 
        // btnFreigeben
        // 
        btnFreigeben.Location = new Point(416, 11);
        btnFreigeben.Margin = new Padding(3, 4, 3, 4);
        btnFreigeben.Name = "btnFreigeben";
        btnFreigeben.Size = new Size(114, 37);
        btnFreigeben.TabIndex = 3;
        btnFreigeben.Text = "Freigeben";
        // 
        // btnSpeichern
        // 
        btnSpeichern.Location = new Point(162, 11);
        btnSpeichern.Margin = new Padding(3, 4, 3, 4);
        btnSpeichern.Name = "btnSpeichern";
        btnSpeichern.Size = new Size(114, 37);
        btnSpeichern.TabIndex = 1;
        btnSpeichern.Text = "Speichern";
        // 
        // btnBuchen
        // 
        btnBuchen.BackColor = Color.SteelBlue;
        btnBuchen.FlatStyle = FlatStyle.Flat;
        btnBuchen.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnBuchen.ForeColor = Color.White;
        btnBuchen.Location = new Point(7, 11);
        btnBuchen.Margin = new Padding(3, 4, 3, 4);
        btnBuchen.Name = "btnBuchen";
        btnBuchen.Size = new Size(149, 37);
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
        pnlFactBox.Location = new Point(768, 59);
        pnlFactBox.Margin = new Padding(3, 4, 3, 4);
        pnlFactBox.Name = "pnlFactBox";
        pnlFactBox.Padding = new Padding(9, 8, 9, 8);
        pnlFactBox.Size = new Size(233, 901);
        pnlFactBox.TabIndex = 3;
        // 
        // lblFBOffeneAuftraege
        // 
        lblFBOffeneAuftraege.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblFBOffeneAuftraege.Location = new Point(9, 261);
        lblFBOffeneAuftraege.Name = "lblFBOffeneAuftraege";
        lblFBOffeneAuftraege.Size = new Size(213, 24);
        lblFBOffeneAuftraege.TabIndex = 8;
        // 
        // lblFBOffenCaption
        // 
        lblFBOffenCaption.AutoSize = true;
        lblFBOffenCaption.ForeColor = Color.Gray;
        lblFBOffenCaption.Location = new Point(9, 240);
        lblFBOffenCaption.Name = "lblFBOffenCaption";
        lblFBOffenCaption.Size = new Size(119, 20);
        lblFBOffenCaption.TabIndex = 7;
        lblFBOffenCaption.Text = "Offene Aufträge:";
        // 
        // lblFBLetzterAuftrag
        // 
        lblFBLetzterAuftrag.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblFBLetzterAuftrag.Location = new Point(9, 200);
        lblFBLetzterAuftrag.Name = "lblFBLetzterAuftrag";
        lblFBLetzterAuftrag.Size = new Size(213, 24);
        lblFBLetzterAuftrag.TabIndex = 6;
        // 
        // lblFBLetzterCaption
        // 
        lblFBLetzterCaption.AutoSize = true;
        lblFBLetzterCaption.ForeColor = Color.Gray;
        lblFBLetzterCaption.Location = new Point(9, 179);
        lblFBLetzterCaption.Name = "lblFBLetzterCaption";
        lblFBLetzterCaption.Size = new Size(111, 20);
        lblFBLetzterCaption.TabIndex = 5;
        lblFBLetzterCaption.Text = "Letzter Auftrag:";
        // 
        // lblFBSaldo
        // 
        lblFBSaldo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblFBSaldo.Location = new Point(9, 131);
        lblFBSaldo.Name = "lblFBSaldo";
        lblFBSaldo.Size = new Size(213, 32);
        lblFBSaldo.TabIndex = 4;
        // 
        // lblFBSaldoCaption
        // 
        lblFBSaldoCaption.AutoSize = true;
        lblFBSaldoCaption.ForeColor = Color.Gray;
        lblFBSaldoCaption.Location = new Point(9, 109);
        lblFBSaldoCaption.Name = "lblFBSaldoCaption";
        lblFBSaldoCaption.Size = new Size(50, 20);
        lblFBSaldoCaption.TabIndex = 3;
        lblFBSaldoCaption.Text = "Saldo:";
        // 
        // lblFBTour
        // 
        lblFBTour.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblFBTour.Location = new Point(9, 69);
        lblFBTour.Name = "lblFBTour";
        lblFBTour.Size = new Size(213, 24);
        lblFBTour.TabIndex = 2;
        // 
        // lblFBTourCaption
        // 
        lblFBTourCaption.AutoSize = true;
        lblFBTourCaption.ForeColor = Color.Gray;
        lblFBTourCaption.Location = new Point(9, 48);
        lblFBTourCaption.Name = "lblFBTourCaption";
        lblFBTourCaption.Size = new Size(41, 20);
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
        lblFBTitle.Padding = new Padding(7, 0, 0, 0);
        lblFBTitle.Size = new Size(231, 35);
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
        pnlKopf.Margin = new Padding(3, 4, 3, 4);
        pnlKopf.Name = "pnlKopf";
        pnlKopf.Padding = new Padding(9, 5, 9, 5);
        pnlKopf.Size = new Size(1001, 59);
        pnlKopf.TabIndex = 0;
        // 
        // lblAuftragStatus
        // 
        lblAuftragStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblAuftragStatus.Location = new Point(571, 13);
        lblAuftragStatus.Name = "lblAuftragStatus";
        lblAuftragStatus.Size = new Size(183, 32);
        lblAuftragStatus.TabIndex = 1;
        lblAuftragStatus.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblKundenname
        // 
        lblKundenname.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblKundenname.Location = new Point(9, 8);
        lblKundenname.Name = "lblKundenname";
        lblKundenname.Size = new Size(549, 43);
        lblKundenname.TabIndex = 0;
        lblKundenname.Text = "— Kunden auswählen —";
        lblKundenname.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // btnAlleAuswaehlenGespeichert
        // 
        btnAlleAuswaehlenGespeichert.Location = new Point(266, 7);
        btnAlleAuswaehlenGespeichert.Name = "btnAlleAuswaehlenGespeichert";
        btnAlleAuswaehlenGespeichert.Size = new Size(82, 35);
        btnAlleAuswaehlenGespeichert.TabIndex = 3;
        btnAlleAuswaehlenGespeichert.Text = "Alle";
        btnAlleAuswaehlenGespeichert.UseVisualStyleBackColor = true;
        // 
        // FrmOrderEntry
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1486, 960);
        Controls.Add(splitMain);
        Font = new Font("Segoe UI", 9F);
        Margin = new Padding(3, 4, 3, 4);
        Name = "FrmOrderEntry";
        Text = "Auftragserfassung";
        splitMain.Panel1.ResumeLayout(false);
        splitMain.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
        splitMain.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgwKunden).EndInit();
        pnlLinksTop.ResumeLayout(false);
        pnlFilter.ResumeLayout(false);
        pnlFilter.PerformLayout();
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
    private Panel pnlFilter;
    private TextBox txtKundeFilter;
    private ComboBox cmbTourFilter;
    private Button btnAlleFreigeben;
    private DataGridView dgwKunden;
    private DataGridViewCheckBoxColumn colKundeChecked;
    private Panel pnlKopf;
    private Label lblKundenname;
    private Label lblAuftragStatus;
    private Panel pnlAktionen;
    private Button btnBuchen;
    private Button btnSpeichern;
    private Button btnFreigeben;
    private Button btnHinzufuegen;
    private Button btnNachlieferung;
    private Button btnStornieren;
    private Button btnLoeschen;
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
    // ── ContextMenu ───────────────────────────────────────────────────────────
    private ContextMenuStrip cmsPositionen;
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
    private Button btnAlleAuswaehlenGespeichert;
}