namespace DNR26V2.Forms.Invoices;

partial class FrmRechnungList
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        panelFilter = new Panel();
        lblVon = new Label();
        dtpVon = new DateTimePicker();
        lblBis = new Label();
        dtpBis = new DateTimePicker();
        lblKundeFilter = new Label();
        txtKundeFilter = new TextBox();
        lblStatusFilter = new Label();
        cmbStatus = new ComboBox();
        btnSuchen = new Button();
        panelDetail = new Panel();
        lblRechnungsnrLabel = new Label();
        lblRechnungsnrWert = new Label();
        lblNettoLabel = new Label();
        lblNettoWert = new Label();
        lblBruttoLabel = new Label();
        lblBruttoWert = new Label();
        lblStatusLabel = new Label();
        lblStatusWert = new Label();
        btnStornieren = new Button();
        btnGutschrift = new Button();
        splitMain = new SplitContainer();
        dgwRechnungen = new DataGridView();
        panelListHeader = new Panel();
        lblListHeader = new Label();
        dgwZeilen = new DataGridView();
        panelZeilHeader = new Panel();
        lblZeilHeader = new Label();
        panelFilter.SuspendLayout();
        panelDetail.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
        splitMain.Panel1.SuspendLayout();
        splitMain.Panel2.SuspendLayout();
        splitMain.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgwRechnungen).BeginInit();
        panelListHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgwZeilen).BeginInit();
        panelZeilHeader.SuspendLayout();
        SuspendLayout();
        // 
        // panelFilter
        // 
        panelFilter.BackColor = SystemColors.Control;
        panelFilter.Controls.Add(lblVon);
        panelFilter.Controls.Add(dtpVon);
        panelFilter.Controls.Add(lblBis);
        panelFilter.Controls.Add(dtpBis);
        panelFilter.Controls.Add(lblKundeFilter);
        panelFilter.Controls.Add(txtKundeFilter);
        panelFilter.Controls.Add(lblStatusFilter);
        panelFilter.Controls.Add(cmbStatus);
        panelFilter.Controls.Add(btnSuchen);
        panelFilter.Dock = DockStyle.Top;
        panelFilter.Location = new Point(0, 0);
        panelFilter.Name = "panelFilter";
        panelFilter.Padding = new Padding(6, 8, 6, 0);
        panelFilter.Size = new Size(1200, 46);
        panelFilter.TabIndex = 2;
        // 
        // lblVon
        // 
        lblVon.AutoSize = true;
        lblVon.Location = new Point(8, 14);
        lblVon.Name = "lblVon";
        lblVon.Size = new Size(30, 15);
        lblVon.TabIndex = 0;
        lblVon.Text = "Von:";
        // 
        // dtpVon
        // 
        dtpVon.Format = DateTimePickerFormat.Short;
        dtpVon.Location = new Point(42, 10);
        dtpVon.Name = "dtpVon";
        dtpVon.Size = new Size(110, 23);
        dtpVon.TabIndex = 1;
        // 
        // lblBis
        // 
        lblBis.AutoSize = true;
        lblBis.Location = new Point(162, 14);
        lblBis.Name = "lblBis";
        lblBis.Size = new Size(25, 15);
        lblBis.TabIndex = 2;
        lblBis.Text = "Bis:";
        // 
        // dtpBis
        // 
        dtpBis.Format = DateTimePickerFormat.Short;
        dtpBis.Location = new Point(193, 10);
        dtpBis.Name = "dtpBis";
        dtpBis.Size = new Size(110, 23);
        dtpBis.TabIndex = 3;
        // 
        // lblKundeFilter
        // 
        lblKundeFilter.AutoSize = true;
        lblKundeFilter.Location = new Point(318, 14);
        lblKundeFilter.Name = "lblKundeFilter";
        lblKundeFilter.Size = new Size(44, 15);
        lblKundeFilter.TabIndex = 4;
        lblKundeFilter.Text = "Kunde:";
        // 
        // txtKundeFilter
        // 
        txtKundeFilter.Location = new Point(360, 10);
        txtKundeFilter.Name = "txtKundeFilter";
        txtKundeFilter.Size = new Size(180, 23);
        txtKundeFilter.TabIndex = 5;
        // 
        // lblStatusFilter
        // 
        lblStatusFilter.AutoSize = true;
        lblStatusFilter.Location = new Point(552, 14);
        lblStatusFilter.Name = "lblStatusFilter";
        lblStatusFilter.Size = new Size(42, 15);
        lblStatusFilter.TabIndex = 6;
        lblStatusFilter.Text = "Status:";
        // 
        // cmbStatus
        // 
        cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbStatus.Location = new Point(598, 10);
        cmbStatus.Name = "cmbStatus";
        cmbStatus.Size = new Size(130, 23);
        cmbStatus.TabIndex = 7;
        // 
        // btnSuchen
        // 
        btnSuchen.Location = new Point(742, 9);
        btnSuchen.Name = "btnSuchen";
        btnSuchen.Size = new Size(90, 26);
        btnSuchen.TabIndex = 8;
        btnSuchen.Text = "Suchen";
        btnSuchen.UseVisualStyleBackColor = true;
        // 
        // panelDetail
        // 
        panelDetail.BackColor = SystemColors.Control;
        panelDetail.Controls.Add(lblRechnungsnrLabel);
        panelDetail.Controls.Add(lblRechnungsnrWert);
        panelDetail.Controls.Add(lblNettoLabel);
        panelDetail.Controls.Add(lblNettoWert);
        panelDetail.Controls.Add(lblBruttoLabel);
        panelDetail.Controls.Add(lblBruttoWert);
        panelDetail.Controls.Add(lblStatusLabel);
        panelDetail.Controls.Add(lblStatusWert);
        panelDetail.Controls.Add(btnStornieren);
        panelDetail.Controls.Add(btnGutschrift);
        panelDetail.Dock = DockStyle.Bottom;
        panelDetail.Location = new Point(0, 692);
        panelDetail.Name = "panelDetail";
        panelDetail.Padding = new Padding(8, 6, 8, 6);
        panelDetail.Size = new Size(1200, 58);
        panelDetail.TabIndex = 1;
        // 
        // lblRechnungsnrLabel
        // 
        lblRechnungsnrLabel.AutoSize = true;
        lblRechnungsnrLabel.Location = new Point(8, 20);
        lblRechnungsnrLabel.Name = "lblRechnungsnrLabel";
        lblRechnungsnrLabel.Size = new Size(64, 15);
        lblRechnungsnrLabel.TabIndex = 0;
        lblRechnungsnrLabel.Text = "Rechnung:";
        // 
        // lblRechnungsnrWert
        // 
        lblRechnungsnrWert.AutoSize = true;
        lblRechnungsnrWert.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblRechnungsnrWert.Location = new Point(72, 20);
        lblRechnungsnrWert.Name = "lblRechnungsnrWert";
        lblRechnungsnrWert.Size = new Size(13, 15);
        lblRechnungsnrWert.TabIndex = 1;
        lblRechnungsnrWert.Text = "–";
        // 
        // lblNettoLabel
        // 
        lblNettoLabel.AutoSize = true;
        lblNettoLabel.Location = new Point(230, 20);
        lblNettoLabel.Name = "lblNettoLabel";
        lblNettoLabel.Size = new Size(40, 15);
        lblNettoLabel.TabIndex = 2;
        lblNettoLabel.Text = "Netto:";
        // 
        // lblNettoWert
        // 
        lblNettoWert.AutoSize = true;
        lblNettoWert.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblNettoWert.Location = new Point(272, 20);
        lblNettoWert.Name = "lblNettoWert";
        lblNettoWert.Size = new Size(13, 15);
        lblNettoWert.TabIndex = 3;
        lblNettoWert.Text = "–";
        // 
        // lblBruttoLabel
        // 
        lblBruttoLabel.AutoSize = true;
        lblBruttoLabel.Location = new Point(400, 20);
        lblBruttoLabel.Name = "lblBruttoLabel";
        lblBruttoLabel.Size = new Size(43, 15);
        lblBruttoLabel.TabIndex = 4;
        lblBruttoLabel.Text = "Brutto:";
        // 
        // lblBruttoWert
        // 
        lblBruttoWert.AutoSize = true;
        lblBruttoWert.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblBruttoWert.ForeColor = Color.DarkGreen;
        lblBruttoWert.Location = new Point(444, 20);
        lblBruttoWert.Name = "lblBruttoWert";
        lblBruttoWert.Size = new Size(13, 15);
        lblBruttoWert.TabIndex = 5;
        lblBruttoWert.Text = "–";
        // 
        // lblStatusLabel
        // 
        lblStatusLabel.AutoSize = true;
        lblStatusLabel.Location = new Point(600, 20);
        lblStatusLabel.Name = "lblStatusLabel";
        lblStatusLabel.Size = new Size(42, 15);
        lblStatusLabel.TabIndex = 6;
        lblStatusLabel.Text = "Status:";
        // 
        // lblStatusWert
        // 
        lblStatusWert.AutoSize = true;
        lblStatusWert.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblStatusWert.Location = new Point(645, 20);
        lblStatusWert.Name = "lblStatusWert";
        lblStatusWert.Size = new Size(13, 15);
        lblStatusWert.TabIndex = 7;
        lblStatusWert.Text = "–";
        // 
        // btnGutschrift
        // 
        btnGutschrift.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnGutschrift.BackColor = Color.FromArgb(30, 100, 160);
        btnGutschrift.Enabled = false;
        btnGutschrift.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnGutschrift.ForeColor = Color.White;
        btnGutschrift.Location = new Point(919, 5);
        btnGutschrift.Name = "btnGutschrift";
        btnGutschrift.Size = new Size(130, 44);
        btnGutschrift.TabIndex = 9;
        btnGutschrift.Text = "Gutschrift";
        btnGutschrift.UseVisualStyleBackColor = false;
        // 
        // btnStornieren
        // 
        btnStornieren.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnStornieren.BackColor = Color.FromArgb(180, 30, 30);
        btnStornieren.Enabled = false;
        btnStornieren.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnStornieren.ForeColor = Color.White;
        btnStornieren.Location = new Point(1059, 5);
        btnStornieren.Name = "btnStornieren";
        btnStornieren.Size = new Size(130, 44);
        btnStornieren.TabIndex = 8;
        btnStornieren.Text = "Stornieren";
        btnStornieren.UseVisualStyleBackColor = false;
        // 
        // splitMain
        // 
        splitMain.Dock = DockStyle.Fill;
        splitMain.Location = new Point(0, 46);
        splitMain.Name = "splitMain";
        splitMain.Orientation = Orientation.Horizontal;
        // 
        // splitMain.Panel1
        // 
        splitMain.Panel1.Controls.Add(dgwRechnungen);
        splitMain.Panel1.Controls.Add(panelListHeader);
        // 
        // splitMain.Panel2
        // 
        splitMain.Panel2.Controls.Add(dgwZeilen);
        splitMain.Panel2.Controls.Add(panelZeilHeader);
        splitMain.Size = new Size(1200, 646);
        splitMain.SplitterDistance = 458;
        splitMain.TabIndex = 0;
        // 
        // dgwRechnungen
        // 
        dgwRechnungen.AllowUserToAddRows = false;
        dgwRechnungen.AllowUserToDeleteRows = false;
        dgwRechnungen.AllowUserToResizeRows = false;
        dgwRechnungen.BackgroundColor = SystemColors.Window;
        dgwRechnungen.ColumnHeadersHeight = 28;
        dgwRechnungen.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgwRechnungen.Dock = DockStyle.Fill;
        dgwRechnungen.Location = new Point(0, 26);
        dgwRechnungen.MultiSelect = false;
        dgwRechnungen.Name = "dgwRechnungen";
        dgwRechnungen.ReadOnly = true;
        dgwRechnungen.RowHeadersVisible = false;
        dgwRechnungen.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgwRechnungen.Size = new Size(1200, 432);
        dgwRechnungen.TabIndex = 0;
        // 
        // panelListHeader
        // 
        panelListHeader.BackColor = Color.FromArgb(230, 230, 230);
        panelListHeader.Controls.Add(lblListHeader);
        panelListHeader.Dock = DockStyle.Top;
        panelListHeader.Location = new Point(0, 0);
        panelListHeader.Name = "panelListHeader";
        panelListHeader.Size = new Size(1200, 26);
        panelListHeader.TabIndex = 1;
        // 
        // lblListHeader
        // 
        lblListHeader.AutoSize = true;
        lblListHeader.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblListHeader.Location = new Point(6, 5);
        lblListHeader.Name = "lblListHeader";
        lblListHeader.Size = new Size(91, 15);
        lblListHeader.TabIndex = 0;
        lblListHeader.Text = "Rechnungsliste";
        // 
        // dgwZeilen
        // 
        dgwZeilen.AllowUserToAddRows = false;
        dgwZeilen.AllowUserToDeleteRows = false;
        dgwZeilen.AllowUserToResizeRows = false;
        dgwZeilen.BackgroundColor = SystemColors.Window;
        dgwZeilen.ColumnHeadersHeight = 28;
        dgwZeilen.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgwZeilen.Dock = DockStyle.Fill;
        dgwZeilen.Location = new Point(0, 26);
        dgwZeilen.MultiSelect = false;
        dgwZeilen.Name = "dgwZeilen";
        dgwZeilen.ReadOnly = true;
        dgwZeilen.RowHeadersVisible = false;
        dgwZeilen.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgwZeilen.Size = new Size(1200, 158);
        dgwZeilen.TabIndex = 0;
        // 
        // panelZeilHeader
        // 
        panelZeilHeader.BackColor = Color.FromArgb(230, 230, 230);
        panelZeilHeader.Controls.Add(lblZeilHeader);
        panelZeilHeader.Dock = DockStyle.Top;
        panelZeilHeader.Location = new Point(0, 0);
        panelZeilHeader.Name = "panelZeilHeader";
        panelZeilHeader.Size = new Size(1200, 26);
        panelZeilHeader.TabIndex = 1;
        // 
        // lblZeilHeader
        // 
        lblZeilHeader.AutoSize = true;
        lblZeilHeader.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblZeilHeader.Location = new Point(6, 5);
        lblZeilHeader.Name = "lblZeilHeader";
        lblZeilHeader.Size = new Size(101, 15);
        lblZeilHeader.TabIndex = 0;
        lblZeilHeader.Text = "Rechnungszeilen";
        // 
        // FrmRechnungList
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1200, 750);
        Controls.Add(splitMain);
        Controls.Add(panelDetail);
        Controls.Add(panelFilter);
        Name = "FrmRechnungList";
        Text = "Rechnungsliste";
        panelFilter.ResumeLayout(false);
        panelFilter.PerformLayout();
        panelDetail.ResumeLayout(false);
        panelDetail.PerformLayout();
        splitMain.Panel1.ResumeLayout(false);
        splitMain.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
        splitMain.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgwRechnungen).EndInit();
        panelListHeader.ResumeLayout(false);
        panelListHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgwZeilen).EndInit();
        panelZeilHeader.ResumeLayout(false);
        panelZeilHeader.PerformLayout();
        ResumeLayout(false);
    }

    private Panel          panelFilter;
    private Panel          panelDetail;
    private SplitContainer splitMain;
    private Panel          panelListHeader;
    private Label          lblListHeader;
    private DataGridView   dgwRechnungen;
    private Panel          panelZeilHeader;
    private Label          lblZeilHeader;
    private DataGridView   dgwZeilen;

    private Label          lblVon;
    private DateTimePicker dtpVon;
    private Label          lblBis;
    private DateTimePicker dtpBis;
    private Label          lblKundeFilter;
    private TextBox        txtKundeFilter;
    private Label          lblStatusFilter;
    private ComboBox       cmbStatus;
    private Button         btnSuchen;

    private Label          lblRechnungsnrLabel;
    private Label          lblRechnungsnrWert;
    private Label          lblNettoLabel;
    private Label          lblNettoWert;
    private Label          lblBruttoLabel;
    private Label          lblBruttoWert;
    private Label          lblStatusLabel;
    private Label          lblStatusWert;
    private Button         btnStornieren;
    private Button         btnGutschrift;
}