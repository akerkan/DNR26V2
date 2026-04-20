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
        lblVon = new Label();
        dtpVon = new DateTimePicker();
        lblBis = new Label();
        dtpBis = new DateTimePicker();
        lblKunde = new Label();
        txtKunde = new TextBox();
        lblStatus = new Label();
        cmbStatus = new ComboBox();
        btnSuchen = new Button();
        pnlToolbar = new Panel();
        btnFreigeben = new Button();
        btnStornieren = new Button();
        btnLoeschen = new Button();
        btnBuchen = new Button();
        btnAlleMarkieren = new Button();
        dgwAuftraege = new DataGridView();
        colAuftragChecked = new DataGridViewCheckBoxColumn();
        pnlFilter.SuspendLayout();
        pnlToolbar.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgwAuftraege).BeginInit();
        SuspendLayout();
        // 
        // pnlFilter
        // 
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
        pnlFilter.Size = new Size(1100, 44);
        pnlFilter.TabIndex = 0;
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
        // pnlToolbar
        // 
        pnlToolbar.Controls.Add(btnFreigeben);
        pnlToolbar.Controls.Add(btnStornieren);
        pnlToolbar.Controls.Add(btnLoeschen);
        pnlToolbar.Controls.Add(btnBuchen);
        pnlToolbar.Controls.Add(btnAlleMarkieren);
        pnlToolbar.Dock = DockStyle.Bottom;
        pnlToolbar.Location = new Point(0, 614);
        pnlToolbar.Name = "pnlToolbar";
        pnlToolbar.Size = new Size(1100, 36);
        pnlToolbar.TabIndex = 2;
        // 
        // btnFreigeben
        // 
        btnFreigeben.Enabled = false;
        btnFreigeben.Location = new Point(6, 6);
        btnFreigeben.Name = "btnFreigeben";
        btnFreigeben.Size = new Size(105, 26);
        btnFreigeben.TabIndex = 0;
        btnFreigeben.Text = "Freigeben";
        // 
        // btnStornieren
        // 
        btnStornieren.Enabled = false;
        btnStornieren.Location = new Point(117, 6);
        btnStornieren.Name = "btnStornieren";
        btnStornieren.Size = new Size(105, 26);
        btnStornieren.TabIndex = 1;
        btnStornieren.Text = "Öffnen";
        // 
        // btnLoeschen
        // 
        btnLoeschen.Enabled = false;
        btnLoeschen.Location = new Point(228, 6);
        btnLoeschen.Name = "btnLoeschen";
        btnLoeschen.Size = new Size(105, 26);
        btnLoeschen.TabIndex = 2;
        btnLoeschen.Text = "Löschen";
        // 
        // btnBuchen
        // 
        btnBuchen.Location = new Point(339, 6);
        btnBuchen.Name = "btnBuchen";
        btnBuchen.Size = new Size(145, 26);
        btnBuchen.TabIndex = 3;
        btnBuchen.Text = "Buchen (Lieferschein)";
        // 
        // btnAlleMarkieren
        // 
        btnAlleMarkieren.Location = new Point(492, 6);
        btnAlleMarkieren.Name = "btnAlleMarkieren";
        btnAlleMarkieren.Size = new Size(130, 26);
        btnAlleMarkieren.TabIndex = 4;
        btnAlleMarkieren.Text = "Alle markieren";
        // 
        // dgwAuftraege
        // 
        dgwAuftraege.AllowUserToAddRows = false;
        dgwAuftraege.AllowUserToDeleteRows = false;
        dgwAuftraege.Columns.AddRange(new DataGridViewColumn[] { colAuftragChecked });
        dgwAuftraege.Dock = DockStyle.Fill;
        dgwAuftraege.Location = new Point(0, 44);
        dgwAuftraege.MultiSelect = false;
        dgwAuftraege.Name = "dgwAuftraege";
        dgwAuftraege.ReadOnly = true;
        dgwAuftraege.RowHeadersVisible = false;
        dgwAuftraege.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgwAuftraege.Size = new Size(1100, 570);
        dgwAuftraege.TabIndex = 1;
        // 
        // colAuftragChecked
        // 
        colAuftragChecked.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colAuftragChecked.HeaderText = "";
        colAuftragChecked.Name = "colAuftragChecked";
        colAuftragChecked.ReadOnly = true;
        colAuftragChecked.Width = 30;
        // 
        // FrmOrderList
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1100, 650);
        Controls.Add(dgwAuftraege);
        Controls.Add(pnlToolbar);
        Controls.Add(pnlFilter);
        Name = "FrmOrderList";
        Text = "Aufträge";
        pnlFilter.ResumeLayout(false);
        pnlFilter.PerformLayout();
        pnlToolbar.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgwAuftraege).EndInit();
        ResumeLayout(false);
    }

    private System.Windows.Forms.Panel          pnlFilter;
    private System.Windows.Forms.Label          lblVon;
    private System.Windows.Forms.DateTimePicker dtpVon;
    private System.Windows.Forms.Label          lblBis;
    private System.Windows.Forms.DateTimePicker dtpBis;
    private System.Windows.Forms.Label          lblKunde;
    private System.Windows.Forms.TextBox        txtKunde;
    private System.Windows.Forms.Label          lblStatus;
    private System.Windows.Forms.ComboBox       cmbStatus;
    private System.Windows.Forms.Button         btnSuchen;
    private System.Windows.Forms.Panel          pnlToolbar;
    private System.Windows.Forms.Button         btnFreigeben;
    private System.Windows.Forms.Button         btnStornieren;
    private System.Windows.Forms.Button         btnLoeschen;
    private System.Windows.Forms.Button         btnBuchen;
    private System.Windows.Forms.Button         btnAlleMarkieren;
    private System.Windows.Forms.DataGridView   dgwAuftraege;
    private System.Windows.Forms.DataGridViewCheckBoxColumn colAuftragChecked;
}