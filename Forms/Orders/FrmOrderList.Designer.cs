namespace DNR26V2.Forms.Orders;

partial class FrmOrderList
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
        btnFreigeben = new Button();
        btnStornieren = new Button();
        lblVon = new Label();
        btnLoeschen = new Button();
        dtpVon = new DateTimePicker();
        btnBuchen = new Button();
        lblBis = new Label();
        btnAlleMarkieren = new Button();
        dtpBis = new DateTimePicker();
        lblKunde = new Label();
        txtKunde = new TextBox();
        lblStatus = new Label();
        cmbStatus = new ComboBox();
        btnSuchen = new Button();
        splitMain = new SplitContainer();
        dgwAuftraege = new DataGridView();
        colAuftragChecked = new DataGridViewCheckBoxColumn();
        pnlDetailHeader = new Panel();
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
        btnAuftragsdruck = new Button();
        dgwAuftragPositionen = new DataGridView();
        pnlFilter.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
        splitMain.Panel1.SuspendLayout();
        splitMain.Panel2.SuspendLayout();
        splitMain.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgwAuftraege).BeginInit();
        pnlDetailHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgwAuftragPositionen).BeginInit();
        SuspendLayout();
        // 
        // pnlFilter
        // 
        pnlFilter.Controls.Add(btnFreigeben);
        pnlFilter.Controls.Add(btnStornieren);
        pnlFilter.Controls.Add(lblVon);
        pnlFilter.Controls.Add(btnLoeschen);
        pnlFilter.Controls.Add(dtpVon);
        pnlFilter.Controls.Add(btnBuchen);
        pnlFilter.Controls.Add(lblBis);
        pnlFilter.Controls.Add(btnAlleMarkieren);
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
        pnlFilter.Size = new Size(1400, 84);
        pnlFilter.TabIndex = 0;
        // 
        // btnFreigeben
        // 
        btnFreigeben.Enabled = false;
        btnFreigeben.Location = new Point(6, 50);
        btnFreigeben.Name = "btnFreigeben";
        btnFreigeben.Size = new Size(105, 26);
        btnFreigeben.TabIndex = 0;
        btnFreigeben.Text = "Freigeben";
        // 
        // btnStornieren
        // 
        btnStornieren.Enabled = false;
        btnStornieren.Location = new Point(117, 50);
        btnStornieren.Name = "btnStornieren";
        btnStornieren.Size = new Size(105, 26);
        btnStornieren.TabIndex = 1;
        btnStornieren.Text = "Öffnen";
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
        // btnLoeschen
        // 
        btnLoeschen.Enabled = false;
        btnLoeschen.Location = new Point(228, 50);
        btnLoeschen.Name = "btnLoeschen";
        btnLoeschen.Size = new Size(105, 26);
        btnLoeschen.TabIndex = 2;
        btnLoeschen.Text = "Löschen";
        // 
        // dtpVon
        // 
        dtpVon.Format = DateTimePickerFormat.Short;
        dtpVon.Location = new Point(76, 10);
        dtpVon.Name = "dtpVon";
        dtpVon.Size = new Size(105, 23);
        dtpVon.TabIndex = 1;
        // 
        // btnBuchen
        // 
        btnBuchen.Location = new Point(339, 50);
        btnBuchen.Name = "btnBuchen";
        btnBuchen.Size = new Size(145, 26);
        btnBuchen.TabIndex = 3;
        btnBuchen.Text = "Buchen (Lieferschein)";
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
        // btnAlleMarkieren
        // 
        btnAlleMarkieren.Location = new Point(492, 50);
        btnAlleMarkieren.Name = "btnAlleMarkieren";
        btnAlleMarkieren.Size = new Size(130, 26);
        btnAlleMarkieren.TabIndex = 4;
        btnAlleMarkieren.Text = "Alle markieren";
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
        cmbStatus.Items.AddRange(new object[] { "Alle", "Offen", "Freigegeben", "Gebucht", "Storniert" });
        cmbStatus.Location = new Point(628, 10);
        cmbStatus.Name = "cmbStatus";
        cmbStatus.Size = new Size(140, 23);
        cmbStatus.TabIndex = 7;
        // 
        // btnSuchen
        // 
        btnSuchen.Location = new Point(776, 9);
        btnSuchen.Name = "btnSuchen";
        btnSuchen.Size = new Size(80, 26);
        btnSuchen.TabIndex = 8;
        btnSuchen.Text = "Suchen";
        // 
        // splitMain
        // 
        splitMain.Dock = DockStyle.Fill;
        splitMain.Location = new Point(0, 84);
        splitMain.Name = "splitMain";
        // 
        // splitMain.Panel1
        // 
        splitMain.Panel1.Controls.Add(dgwAuftraege);
        // 
        // splitMain.Panel2
        // 
        splitMain.Panel2.Controls.Add(pnlDetailHeader);
        splitMain.Panel2.Controls.Add(dgwAuftragPositionen);
        splitMain.Size = new Size(1400, 666);
        splitMain.SplitterDistance = 709;
        splitMain.TabIndex = 1;
        // 
        // dgwAuftraege
        // 
        dgwAuftraege.AllowUserToAddRows = false;
        dgwAuftraege.AllowUserToDeleteRows = false;
        dgwAuftraege.Columns.AddRange(new DataGridViewColumn[] { colAuftragChecked });
        dgwAuftraege.Dock = DockStyle.Fill;
        dgwAuftraege.Location = new Point(0, 0);
        dgwAuftraege.MultiSelect = false;
        dgwAuftraege.Name = "dgwAuftraege";
        dgwAuftraege.ReadOnly = true;
        dgwAuftraege.RowHeadersVisible = false;
        dgwAuftraege.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgwAuftraege.Size = new Size(709, 666);
        dgwAuftraege.TabIndex = 0;
        // 
        // colAuftragChecked
        // 
        colAuftragChecked.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colAuftragChecked.HeaderText = "";
        colAuftragChecked.Name = "colAuftragChecked";
        colAuftragChecked.ReadOnly = true;
        colAuftragChecked.Width = 30;
        // 
        // pnlDetailHeader
        // 
        pnlDetailHeader.BackColor = Color.WhiteSmoke;
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
        pnlDetailHeader.Controls.Add(btnAuftragsdruck);
        pnlDetailHeader.Dock = DockStyle.Top;
        pnlDetailHeader.Location = new Point(0, 0);
        pnlDetailHeader.Name = "pnlDetailHeader";
        pnlDetailHeader.Padding = new Padding(8, 6, 8, 6);
        pnlDetailHeader.Size = new Size(687, 66);
        pnlDetailHeader.TabIndex = 0;
        // 
        // lblDetailAuftragNr
        // 
        lblDetailAuftragNr.AutoSize = true;
        lblDetailAuftragNr.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblDetailAuftragNr.Location = new Point(8, 10);
        lblDetailAuftragNr.Name = "lblDetailAuftragNr";
        lblDetailAuftragNr.Size = new Size(74, 15);
        lblDetailAuftragNr.TabIndex = 0;
        lblDetailAuftragNr.Text = "Auftrag-Nr.:";
        // 
        // lblDetailAuftragNrWert
        // 
        lblDetailAuftragNrWert.AutoSize = true;
        lblDetailAuftragNrWert.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblDetailAuftragNrWert.Location = new Point(88, 10);
        lblDetailAuftragNrWert.Name = "lblDetailAuftragNrWert";
        lblDetailAuftragNrWert.Size = new Size(12, 15);
        lblDetailAuftragNrWert.TabIndex = 1;
        lblDetailAuftragNrWert.Text = "-";
        // 
        // lblDetailKunde
        // 
        lblDetailKunde.AutoSize = true;
        lblDetailKunde.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblDetailKunde.Location = new Point(220, 10);
        lblDetailKunde.Name = "lblDetailKunde";
        lblDetailKunde.Size = new Size(46, 15);
        lblDetailKunde.TabIndex = 2;
        lblDetailKunde.Text = "Kunde:";
        // 
        // lblDetailKundeWert
        // 
        lblDetailKundeWert.AutoSize = true;
        lblDetailKundeWert.Location = new Point(265, 10);
        lblDetailKundeWert.Name = "lblDetailKundeWert";
        lblDetailKundeWert.Size = new Size(12, 15);
        lblDetailKundeWert.TabIndex = 3;
        lblDetailKundeWert.Text = "-";
        // 
        // lblDetailDatum
        // 
        lblDetailDatum.AutoSize = true;
        lblDetailDatum.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblDetailDatum.Location = new Point(490, 10);
        lblDetailDatum.Name = "lblDetailDatum";
        lblDetailDatum.Size = new Size(79, 15);
        lblDetailDatum.TabIndex = 4;
        lblDetailDatum.Text = "Lieferdatum:";
        // 
        // lblDetailDatumWert
        // 
        lblDetailDatumWert.AutoSize = true;
        lblDetailDatumWert.Location = new Point(565, 10);
        lblDetailDatumWert.Name = "lblDetailDatumWert";
        lblDetailDatumWert.Size = new Size(12, 15);
        lblDetailDatumWert.TabIndex = 5;
        lblDetailDatumWert.Text = "-";
        // 
        // lblDetailStatus
        // 
        lblDetailStatus.AutoSize = true;
        lblDetailStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblDetailStatus.Location = new Point(8, 42);
        lblDetailStatus.Name = "lblDetailStatus";
        lblDetailStatus.Size = new Size(45, 15);
        lblDetailStatus.TabIndex = 6;
        lblDetailStatus.Text = "Status:";
        // 
        // lblDetailStatusWert
        // 
        lblDetailStatusWert.AutoSize = true;
        lblDetailStatusWert.Location = new Point(55, 42);
        lblDetailStatusWert.Name = "lblDetailStatusWert";
        lblDetailStatusWert.Size = new Size(12, 15);
        lblDetailStatusWert.TabIndex = 7;
        lblDetailStatusWert.Text = "-";
        // 
        // lblDetailGesamt
        // 
        lblDetailGesamt.AutoSize = true;
        lblDetailGesamt.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblDetailGesamt.Location = new Point(220, 42);
        lblDetailGesamt.Name = "lblDetailGesamt";
        lblDetailGesamt.Size = new Size(90, 15);
        lblDetailGesamt.TabIndex = 8;
        lblDetailGesamt.Text = "Gesamtbetrag:";
        // 
        // lblDetailGesamtWert
        // 
        lblDetailGesamtWert.AutoSize = true;
        lblDetailGesamtWert.Location = new Point(308, 42);
        lblDetailGesamtWert.Name = "lblDetailGesamtWert";
        lblDetailGesamtWert.Size = new Size(12, 15);
        lblDetailGesamtWert.TabIndex = 9;
        lblDetailGesamtWert.Text = "-";
        // 
        // btnAuftragsdruck
        // 
        btnAuftragsdruck.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnAuftragsdruck.Enabled = false;
        btnAuftragsdruck.Location = new Point(496, 34);
        btnAuftragsdruck.Name = "btnAuftragsdruck";
        btnAuftragsdruck.Size = new Size(180, 26);
        btnAuftragsdruck.TabIndex = 10;
        btnAuftragsdruck.Text = "Auftragsbestätigung drucken";
        btnAuftragsdruck.UseVisualStyleBackColor = true;
        // 
        // dgwAuftragPositionen
        // 
        dgwAuftragPositionen.AllowUserToAddRows = false;
        dgwAuftragPositionen.AllowUserToDeleteRows = false;
        dgwAuftragPositionen.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgwAuftragPositionen.Location = new Point(0, 72);
        dgwAuftragPositionen.Name = "dgwAuftragPositionen";
        dgwAuftragPositionen.ReadOnly = true;
        dgwAuftragPositionen.RowHeadersVisible = false;
        dgwAuftragPositionen.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgwAuftragPositionen.Size = new Size(687, 594);
        dgwAuftragPositionen.TabIndex = 1;
        // 
        // FrmOrderList
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1400, 750);
        Controls.Add(splitMain);
        Controls.Add(pnlFilter);
        Name = "FrmOrderList";
        Text = "Aufträge";
        pnlFilter.ResumeLayout(false);
        pnlFilter.PerformLayout();
        splitMain.Panel1.ResumeLayout(false);
        splitMain.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
        splitMain.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgwAuftraege).EndInit();
        pnlDetailHeader.ResumeLayout(false);
        pnlDetailHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgwAuftragPositionen).EndInit();
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
    private System.Windows.Forms.Button                     btnFreigeben;
    private System.Windows.Forms.Button                     btnStornieren;
    private System.Windows.Forms.Button                     btnLoeschen;
    private System.Windows.Forms.Button                     btnBuchen;
    private System.Windows.Forms.Button                     btnAlleMarkieren;
    private System.Windows.Forms.SplitContainer             splitMain;
    private System.Windows.Forms.DataGridView               dgwAuftraege;
    private System.Windows.Forms.DataGridViewCheckBoxColumn colAuftragChecked;
    private System.Windows.Forms.Panel                      pnlDetailHeader;
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
    private System.Windows.Forms.Button                     btnAuftragsdruck;
    private System.Windows.Forms.DataGridView               dgwAuftragPositionen;
}