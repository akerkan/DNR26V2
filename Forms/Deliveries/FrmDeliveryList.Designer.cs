namespace DNR26V2.Forms.Deliveries;

partial class FrmDeliveryList
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        pnlFilter = new Panel();
        btnLieferscheineDrucken = new Button();
        btnAlleMarkieren = new Button();
        btnStornieren = new Button();
        lblVon = new Label();
        dtpVon = new DateTimePicker();
        lblBis = new Label();
        dtpBis = new DateTimePicker();
        lblKunde = new Label();
        txtKunde = new TextBox();
        lblStatus = new Label();
        cmbStatus = new ComboBox();
        btnSuchen = new Button();
        splitMain = new SplitContainer();
        dgwLieferscheine = new DataGridView();
        colLsChecked = new DataGridViewCheckBoxColumn();
        pnlDetailHeader = new Panel();
        lblDetailLsNr = new Label();
        lblDetailLsNrWert = new Label();
        lblDetailAuftragNr = new Label();
        lblDetailAuftragNrWert = new Label();
        lblDetailKunde = new Label();
        lblDetailKundeWert = new Label();
        lblDetailDatum = new Label();
        lblDetailDatumWert = new Label();
        lblDetailStatus = new Label();
        lblDetailStatusWert = new Label();
        lblDetailGesamt = new Label();
        lblDetailGesamtWert = new Label();
        dgwLsPositionen = new DataGridView();
        contextMenuLsPositionen = new System.Windows.Forms.ContextMenuStrip();
        ctxZeileStornieren = new System.Windows.Forms.ToolStripMenuItem();
        pnlFilter.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
        splitMain.Panel1.SuspendLayout();
        splitMain.Panel2.SuspendLayout();
        splitMain.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgwLieferscheine).BeginInit();
        pnlDetailHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgwLsPositionen).BeginInit();
        contextMenuLsPositionen.SuspendLayout();
        SuspendLayout();
        // 
        // pnlFilter
        // 
        pnlFilter.Controls.Add(btnLieferscheineDrucken);
        pnlFilter.Controls.Add(btnAlleMarkieren);
        pnlFilter.Controls.Add(btnStornieren);
        pnlFilter.Controls.Add(lblVon);
        pnlFilter.Controls.Add(dtpVon);
        pnlFilter.Controls.Add(lblBis);
        pnlFilter.Controls.Add(dtpBis);
        pnlFilter.Controls.Add(lblKunde);
        pnlFilter.Controls.Add(txtKunde);
        pnlFilter.Controls.Add(lblStatus);
        pnlFilter.Controls.Add(cmbStatus);
        pnlFilter.Controls.Add(btnSuchen);
        pnlFilter.Dock = DockStyle.Top;
        pnlFilter.Location = new Point(0, 0);
        pnlFilter.Name = "pnlFilter";
        pnlFilter.Padding = new Padding(4, 0, 4, 0);
        pnlFilter.Size = new Size(1400, 83);
        pnlFilter.TabIndex = 0;
        // 
        // btnLieferscheineDrucken
        // 
        btnLieferscheineDrucken.Enabled = false;
        btnLieferscheineDrucken.Location = new Point(380, 51);
        btnLieferscheineDrucken.Name = "btnLieferscheineDrucken";
        btnLieferscheineDrucken.Size = new Size(220, 26);
        btnLieferscheineDrucken.TabIndex = 3;
        btnLieferscheineDrucken.Text = "Ausgewählte Lieferscheine drucken";
        btnLieferscheineDrucken.UseVisualStyleBackColor = true;
        // 
        // btnAlleMarkieren
        // 
        btnAlleMarkieren.Enabled = false;
        btnAlleMarkieren.Location = new Point(244, 51);
        btnAlleMarkieren.Name = "btnAlleMarkieren";
        btnAlleMarkieren.Size = new Size(130, 26);
        btnAlleMarkieren.TabIndex = 2;
        btnAlleMarkieren.Text = "Alle markieren";
        // 
        // btnStornieren
        // 
        btnStornieren.Enabled = false;
        btnStornieren.Location = new Point(133, 51);
        btnStornieren.Name = "btnStornieren";
        btnStornieren.Size = new Size(105, 26);
        btnStornieren.TabIndex = 1;
        btnStornieren.Text = "Stornieren";
        // 
        // lblVon
        // 
        lblVon.AutoSize = true;
        lblVon.Location = new Point(6, 13);
        lblVon.Name = "lblVon";
        lblVon.Size = new Size(69, 15);
        lblVon.TabIndex = 0;
        lblVon.Text = "Datum Von:";
        // 
        // dtpVon
        // 
        dtpVon.Format = DateTimePickerFormat.Short;
        dtpVon.Location = new Point(76, 10);
        dtpVon.Name = "dtpVon";
        dtpVon.Size = new Size(105, 23);
        dtpVon.TabIndex = 1;
        // 
        // lblBis
        // 
        lblBis.AutoSize = true;
        lblBis.Location = new Point(188, 13);
        lblBis.Name = "lblBis";
        lblBis.Size = new Size(64, 15);
        lblBis.TabIndex = 2;
        lblBis.Text = "Datum Bis:";
        // 
        // dtpBis
        // 
        dtpBis.Format = DateTimePickerFormat.Short;
        dtpBis.Location = new Point(258, 10);
        dtpBis.Name = "dtpBis";
        dtpBis.Size = new Size(105, 23);
        dtpBis.TabIndex = 3;
        // 
        // lblKunde
        // 
        lblKunde.AutoSize = true;
        lblKunde.Location = new Point(372, 13);
        lblKunde.Name = "lblKunde";
        lblKunde.Size = new Size(44, 15);
        lblKunde.TabIndex = 4;
        lblKunde.Text = "Kunde:";
        // 
        // txtKunde
        // 
        txtKunde.Location = new Point(416, 10);
        txtKunde.Name = "txtKunde";
        txtKunde.Size = new Size(160, 23);
        txtKunde.TabIndex = 5;
        // 
        // lblStatus
        // 
        lblStatus.AutoSize = true;
        lblStatus.Location = new Point(584, 13);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(42, 15);
        lblStatus.TabIndex = 6;
        lblStatus.Text = "Status:";
        // 
        // cmbStatus
        // 
        cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbStatus.Items.AddRange(new object[] { "Alle", "Aktiv", "Teil-Storniert", "Fakturiert", "Storniert" });
        cmbStatus.Location = new Point(628, 10);
        cmbStatus.Name = "cmbStatus";
        cmbStatus.Size = new Size(150, 23);
        cmbStatus.TabIndex = 7;
        // 
        // btnSuchen
        // 
        btnSuchen.Location = new Point(786, 9);
        btnSuchen.Name = "btnSuchen";
        btnSuchen.Size = new Size(80, 26);
        btnSuchen.TabIndex = 8;
        btnSuchen.Text = "Suchen";
        // 
        // splitMain
        // 
        splitMain.Dock = DockStyle.Fill;
        splitMain.Location = new Point(0, 83);
        splitMain.Name = "splitMain";
        // 
        // splitMain.Panel1
        // 
        splitMain.Panel1.Controls.Add(dgwLieferscheine);
        // 
        // splitMain.Panel2
        // 
        splitMain.Panel2.Controls.Add(pnlDetailHeader);
        splitMain.Panel2.Controls.Add(dgwLsPositionen);
        splitMain.Size = new Size(1400, 667);
        splitMain.SplitterDistance = 779;
        splitMain.TabIndex = 1;
        // 
        // dgwLieferscheine
        // 
        dgwLieferscheine.AllowUserToAddRows = false;
        dgwLieferscheine.AllowUserToDeleteRows = false;
        dgwLieferscheine.Columns.AddRange(new DataGridViewColumn[] { colLsChecked });
        dgwLieferscheine.Dock = DockStyle.Fill;
        dgwLieferscheine.Location = new Point(0, 0);
        dgwLieferscheine.MultiSelect = false;
        dgwLieferscheine.Name = "dgwLieferscheine";
        dgwLieferscheine.ReadOnly = true;
        dgwLieferscheine.RowHeadersVisible = false;
        dgwLieferscheine.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgwLieferscheine.Size = new Size(779, 667);
        dgwLieferscheine.TabIndex = 0;
        // 
        // colLsChecked
        // 
        colLsChecked.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colLsChecked.HeaderText = "";
        colLsChecked.Name = "colLsChecked";
        colLsChecked.ReadOnly = true;
        colLsChecked.Width = 30;
        // 
        // pnlDetailHeader
        // 
        pnlDetailHeader.BackColor = Color.WhiteSmoke;
        pnlDetailHeader.Controls.Add(lblDetailLsNr);
        pnlDetailHeader.Controls.Add(lblDetailLsNrWert);
        pnlDetailHeader.Controls.Add(lblDetailAuftragNr);
        pnlDetailHeader.Controls.Add(lblDetailAuftragNrWert);
        pnlDetailHeader.Controls.Add(lblDetailKunde);
        pnlDetailHeader.Controls.Add(lblDetailKundeWert);
        pnlDetailHeader.Controls.Add(lblDetailDatum);
        pnlDetailHeader.Controls.Add(lblDetailDatumWert);
        pnlDetailHeader.Controls.Add(lblDetailStatus);
        pnlDetailHeader.Controls.Add(lblDetailStatusWert);
        pnlDetailHeader.Controls.Add(lblDetailGesamt);
        pnlDetailHeader.Controls.Add(lblDetailGesamtWert);
        pnlDetailHeader.Dock = DockStyle.Top;
        pnlDetailHeader.Location = new Point(0, 0);
        pnlDetailHeader.Name = "pnlDetailHeader";
        pnlDetailHeader.Padding = new Padding(8, 6, 8, 6);
        pnlDetailHeader.Size = new Size(617, 65);
        pnlDetailHeader.TabIndex = 0;
        // 
        // lblDetailLsNr
        // 
        lblDetailLsNr.AutoSize = true;
        lblDetailLsNr.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblDetailLsNr.Location = new Point(8, 10);
        lblDetailLsNr.Name = "lblDetailLsNr";
        lblDetailLsNr.Size = new Size(99, 15);
        lblDetailLsNr.TabIndex = 0;
        lblDetailLsNr.Text = "Lieferschein-Nr.:";
        // 
        // lblDetailLsNrWert
        // 
        lblDetailLsNrWert.AutoSize = true;
        lblDetailLsNrWert.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblDetailLsNrWert.Location = new Point(108, 10);
        lblDetailLsNrWert.Name = "lblDetailLsNrWert";
        lblDetailLsNrWert.Size = new Size(12, 15);
        lblDetailLsNrWert.TabIndex = 1;
        lblDetailLsNrWert.Text = "-";
        // 
        // lblDetailAuftragNr
        // 
        lblDetailAuftragNr.AutoSize = true;
        lblDetailAuftragNr.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblDetailAuftragNr.Location = new Point(220, 10);
        lblDetailAuftragNr.Name = "lblDetailAuftragNr";
        lblDetailAuftragNr.Size = new Size(74, 15);
        lblDetailAuftragNr.TabIndex = 2;
        lblDetailAuftragNr.Text = "Auftrag-Nr.:";
        // 
        // lblDetailAuftragNrWert
        // 
        lblDetailAuftragNrWert.AutoSize = true;
        lblDetailAuftragNrWert.Location = new Point(300, 10);
        lblDetailAuftragNrWert.Name = "lblDetailAuftragNrWert";
        lblDetailAuftragNrWert.Size = new Size(12, 15);
        lblDetailAuftragNrWert.TabIndex = 3;
        lblDetailAuftragNrWert.Text = "-";
        // 
        // lblDetailKunde
        // 
        lblDetailKunde.AutoSize = true;
        lblDetailKunde.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblDetailKunde.Location = new Point(430, 10);
        lblDetailKunde.Name = "lblDetailKunde";
        lblDetailKunde.Size = new Size(46, 15);
        lblDetailKunde.TabIndex = 4;
        lblDetailKunde.Text = "Kunde:";
        // 
        // lblDetailKundeWert
        // 
        lblDetailKundeWert.AutoSize = true;
        lblDetailKundeWert.Location = new Point(475, 10);
        lblDetailKundeWert.Name = "lblDetailKundeWert";
        lblDetailKundeWert.Size = new Size(12, 15);
        lblDetailKundeWert.TabIndex = 5;
        lblDetailKundeWert.Text = "-";
        // 
        // lblDetailDatum
        // 
        lblDetailDatum.AutoSize = true;
        lblDetailDatum.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblDetailDatum.Location = new Point(8, 42);
        lblDetailDatum.Name = "lblDetailDatum";
        lblDetailDatum.Size = new Size(79, 15);
        lblDetailDatum.TabIndex = 6;
        lblDetailDatum.Text = "Lieferdatum:";
        // 
        // lblDetailDatumWert
        // 
        lblDetailDatumWert.AutoSize = true;
        lblDetailDatumWert.Location = new Point(80, 42);
        lblDetailDatumWert.Name = "lblDetailDatumWert";
        lblDetailDatumWert.Size = new Size(12, 15);
        lblDetailDatumWert.TabIndex = 7;
        lblDetailDatumWert.Text = "-";
        // 
        // lblDetailStatus
        // 
        lblDetailStatus.AutoSize = true;
        lblDetailStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblDetailStatus.Location = new Point(220, 42);
        lblDetailStatus.Name = "lblDetailStatus";
        lblDetailStatus.Size = new Size(45, 15);
        lblDetailStatus.TabIndex = 8;
        lblDetailStatus.Text = "Status:";
        // 
        // lblDetailStatusWert
        // 
        lblDetailStatusWert.AutoSize = true;
        lblDetailStatusWert.Location = new Point(265, 42);
        lblDetailStatusWert.Name = "lblDetailStatusWert";
        lblDetailStatusWert.Size = new Size(12, 15);
        lblDetailStatusWert.TabIndex = 9;
        lblDetailStatusWert.Text = "-";
        // 
        // lblDetailGesamt
        // 
        lblDetailGesamt.AutoSize = true;
        lblDetailGesamt.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblDetailGesamt.Location = new Point(430, 42);
        lblDetailGesamt.Name = "lblDetailGesamt";
        lblDetailGesamt.Size = new Size(90, 15);
        lblDetailGesamt.TabIndex = 10;
        lblDetailGesamt.Text = "Gesamtbetrag:";
        // 
        // lblDetailGesamtWert
        // 
        lblDetailGesamtWert.AutoSize = true;
        lblDetailGesamtWert.Location = new Point(518, 42);
        lblDetailGesamtWert.Name = "lblDetailGesamtWert";
        lblDetailGesamtWert.Size = new Size(12, 15);
        lblDetailGesamtWert.TabIndex = 11;
        lblDetailGesamtWert.Text = "-";
        // 
        // dgwLsPositionen
        // 
        dgwLsPositionen.AllowUserToAddRows = false;
        dgwLsPositionen.AllowUserToDeleteRows = false;
        dgwLsPositionen.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgwLsPositionen.Location = new Point(0, 71);
        dgwLsPositionen.Name = "dgwLsPositionen";
        dgwLsPositionen.ReadOnly = true;
        dgwLsPositionen.RowHeadersVisible = false;
        dgwLsPositionen.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgwLsPositionen.Size = new Size(617, 596);
        dgwLsPositionen.TabIndex = 1;
        dgwLsPositionen.ContextMenuStrip = contextMenuLsPositionen;
        // 
        // contextMenuLsPositionen
        // 
        contextMenuLsPositionen.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            ctxZeileStornieren});
        contextMenuLsPositionen.Name = "contextMenuLsPositionen";
        // 
        // ctxZeileStornieren
        // 
        ctxZeileStornieren.Name = "ctxZeileStornieren";
        ctxZeileStornieren.Text = "Zeile stornieren";
        // 
        // FrmDeliveryList
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1400, 750);
        Controls.Add(splitMain);
        Controls.Add(pnlFilter);
        Name = "FrmDeliveryList";
        Text = "Lieferungen";
        pnlFilter.ResumeLayout(false);
        pnlFilter.PerformLayout();
        splitMain.Panel1.ResumeLayout(false);
        splitMain.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
        splitMain.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgwLieferscheine).EndInit();
        pnlDetailHeader.ResumeLayout(false);
        pnlDetailHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgwLsPositionen).EndInit();
        contextMenuLsPositionen.ResumeLayout(false);
        ResumeLayout(false);
    }

    private System.Windows.Forms.Panel                      pnlFilter;
    private System.Windows.Forms.Label                      lblVon;
    private System.Windows.Forms.DateTimePicker             dtpVon;
    private System.Windows.Forms.Label                      lblBis;
    private System.Windows.Forms.DateTimePicker             dtpBis;
    private System.Windows.Forms.Label                      lblKunde;
    private System.Windows.Forms.TextBox                    txtKunde;
    private System.Windows.Forms.Label                      lblStatus;
    private System.Windows.Forms.ComboBox                   cmbStatus;
    private System.Windows.Forms.Button                     btnSuchen;
    private System.Windows.Forms.Button                     btnStornieren;
    private System.Windows.Forms.Button                     btnAlleMarkieren;
    private System.Windows.Forms.Button                     btnLieferscheineDrucken;
    private System.Windows.Forms.SplitContainer             splitMain;
    private System.Windows.Forms.DataGridView               dgwLieferscheine;
    private System.Windows.Forms.DataGridViewCheckBoxColumn colLsChecked;
    private System.Windows.Forms.Panel                      pnlDetailHeader;
    private System.Windows.Forms.Label                      lblDetailLsNr;
    private System.Windows.Forms.Label                      lblDetailLsNrWert;
    private System.Windows.Forms.Label                      lblDetailAuftragNr;
    private System.Windows.Forms.Label                      lblDetailAuftragNrWert;
    private System.Windows.Forms.Label                      lblDetailKunde;
    private System.Windows.Forms.Label                      lblDetailKundeWert;
    private System.Windows.Forms.Label                      lblDetailDatum;
    private System.Windows.Forms.Label                      lblDetailDatumWert;
    private System.Windows.Forms.Label                      lblDetailStatus;
    private System.Windows.Forms.Label                      lblDetailStatusWert;
    private System.Windows.Forms.Label                      lblDetailGesamt;
    private System.Windows.Forms.Label                      lblDetailGesamtWert;
    private System.Windows.Forms.DataGridView               dgwLsPositionen;
    private System.Windows.Forms.ContextMenuStrip          contextMenuLsPositionen;
    private System.Windows.Forms.ToolStripMenuItem         ctxZeileStornieren;
}