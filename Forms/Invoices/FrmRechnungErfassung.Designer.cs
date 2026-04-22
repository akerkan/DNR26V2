namespace DNR26V2.Forms.Invoices;

partial class FrmRechnungErfassung
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        panelFilter = new Panel();
        lblVon = new Label();
        dtpVon = new DateTimePicker();
        lblBis = new Label();
        dtpBis = new DateTimePicker();
        btnSuchen = new Button();
        panelDetail = new Panel();
        lblRechnungsdatum = new Label();
        dtpRechnungsdatum = new DateTimePicker();
        lblNotizLabel = new Label();
        txtNotiz = new TextBox();
        lblNettoLabel = new Label();
        lblNettoWert = new Label();
        lblBruttoLabel = new Label();
        lblBruttoWert = new Label();
        panelBuchenContainer = new Panel();
        btnBuchen = new Button();
        splitMain = new SplitContainer();
        dgwKunden = new DataGridView();
        panelKundenHeader = new Panel();
        lblKundenHeader = new Label();
        splitRight = new SplitContainer();
        dgwLieferscheine = new DataGridView();
        colLsChecked = new DataGridViewCheckBoxColumn();
        panelLsHeader = new Panel();
        lblLsHeader = new Label();
        dgwZeilen = new DataGridView();
        panelZeilenHeader = new Panel();
        lblZeilenHeader = new Label();
        panelFilter.SuspendLayout();
        panelDetail.SuspendLayout();
        panelBuchenContainer.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
        splitMain.Panel1.SuspendLayout();
        splitMain.Panel2.SuspendLayout();
        splitMain.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgwKunden).BeginInit();
        panelKundenHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitRight).BeginInit();
        splitRight.Panel1.SuspendLayout();
        splitRight.Panel2.SuspendLayout();
        splitRight.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgwLieferscheine).BeginInit();
        panelLsHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgwZeilen).BeginInit();
        panelZeilenHeader.SuspendLayout();
        SuspendLayout();
        // 
        // panelFilter
        // 
        panelFilter.BackColor = SystemColors.Control;
        panelFilter.Controls.Add(lblVon);
        panelFilter.Controls.Add(dtpVon);
        panelFilter.Controls.Add(lblBis);
        panelFilter.Controls.Add(dtpBis);
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
        // btnSuchen
        // 
        btnSuchen.Location = new Point(316, 9);
        btnSuchen.Name = "btnSuchen";
        btnSuchen.Size = new Size(90, 26);
        btnSuchen.TabIndex = 4;
        btnSuchen.Text = "Suchen";
        btnSuchen.UseVisualStyleBackColor = true;
        // 
        // panelDetail
        // 
        panelDetail.BackColor = SystemColors.Control;
        panelDetail.Controls.Add(lblRechnungsdatum);
        panelDetail.Controls.Add(dtpRechnungsdatum);
        panelDetail.Controls.Add(lblNotizLabel);
        panelDetail.Controls.Add(txtNotiz);
        panelDetail.Controls.Add(lblNettoLabel);
        panelDetail.Controls.Add(lblNettoWert);
        panelDetail.Controls.Add(lblBruttoLabel);
        panelDetail.Controls.Add(lblBruttoWert);
        panelDetail.Controls.Add(panelBuchenContainer);
        panelDetail.Dock = DockStyle.Bottom;
        panelDetail.Location = new Point(0, 662);
        panelDetail.Name = "panelDetail";
        panelDetail.Padding = new Padding(8, 6, 8, 6);
        panelDetail.Size = new Size(1200, 88);
        panelDetail.TabIndex = 1;
        // 
        // lblRechnungsdatum
        // 
        lblRechnungsdatum.AutoSize = true;
        lblRechnungsdatum.Location = new Point(8, 16);
        lblRechnungsdatum.Name = "lblRechnungsdatum";
        lblRechnungsdatum.Size = new Size(104, 15);
        lblRechnungsdatum.TabIndex = 0;
        lblRechnungsdatum.Text = "Rechnungsdatum:";
        // 
        // dtpRechnungsdatum
        // 
        dtpRechnungsdatum.Format = DateTimePickerFormat.Short;
        dtpRechnungsdatum.Location = new Point(120, 12);
        dtpRechnungsdatum.Name = "dtpRechnungsdatum";
        dtpRechnungsdatum.Size = new Size(110, 23);
        dtpRechnungsdatum.TabIndex = 1;
        // 
        // lblNotizLabel
        // 
        lblNotizLabel.AutoSize = true;
        lblNotizLabel.Location = new Point(248, 16);
        lblNotizLabel.Name = "lblNotizLabel";
        lblNotizLabel.Size = new Size(38, 15);
        lblNotizLabel.TabIndex = 2;
        lblNotizLabel.Text = "Notiz:";
        // 
        // txtNotiz
        // 
        txtNotiz.Location = new Point(295, 12);
        txtNotiz.Name = "txtNotiz";
        txtNotiz.Size = new Size(360, 23);
        txtNotiz.TabIndex = 3;
        // 
        // lblNettoLabel
        // 
        lblNettoLabel.AutoSize = true;
        lblNettoLabel.Location = new Point(8, 50);
        lblNettoLabel.Name = "lblNettoLabel";
        lblNettoLabel.Size = new Size(40, 15);
        lblNettoLabel.TabIndex = 4;
        lblNettoLabel.Text = "Netto:";
        // 
        // lblNettoWert
        // 
        lblNettoWert.AutoSize = true;
        lblNettoWert.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblNettoWert.Location = new Point(60, 50);
        lblNettoWert.Name = "lblNettoWert";
        lblNettoWert.Size = new Size(41, 15);
        lblNettoWert.TabIndex = 5;
        lblNettoWert.Text = "0,00 €";
        // 
        // lblBruttoLabel
        // 
        lblBruttoLabel.AutoSize = true;
        lblBruttoLabel.Location = new Point(200, 50);
        lblBruttoLabel.Name = "lblBruttoLabel";
        lblBruttoLabel.Size = new Size(43, 15);
        lblBruttoLabel.TabIndex = 6;
        lblBruttoLabel.Text = "Brutto:";
        // 
        // lblBruttoWert
        // 
        lblBruttoWert.AutoSize = true;
        lblBruttoWert.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblBruttoWert.ForeColor = Color.DarkGreen;
        lblBruttoWert.Location = new Point(252, 50);
        lblBruttoWert.Name = "lblBruttoWert";
        lblBruttoWert.Size = new Size(41, 15);
        lblBruttoWert.TabIndex = 7;
        lblBruttoWert.Text = "0,00 €";
        // 
        // panelBuchenContainer
        // 
        panelBuchenContainer.BackColor = Color.Transparent;
        panelBuchenContainer.Controls.Add(btnBuchen);
        panelBuchenContainer.Dock = DockStyle.Right;
        panelBuchenContainer.Location = new Point(1002, 6);
        panelBuchenContainer.Name = "panelBuchenContainer";
        panelBuchenContainer.Size = new Size(190, 76);
        panelBuchenContainer.TabIndex = 8;
        // 
        // btnBuchen
        // 
        btnBuchen.BackColor = Color.FromArgb(0, 122, 204);
        btnBuchen.Dock = DockStyle.Fill;
        btnBuchen.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnBuchen.ForeColor = Color.White;
        btnBuchen.Location = new Point(0, 0);
        btnBuchen.Name = "btnBuchen";
        btnBuchen.Size = new Size(190, 76);
        btnBuchen.TabIndex = 0;
        btnBuchen.Text = "Rechnung buchen";
        btnBuchen.UseVisualStyleBackColor = false;
        // 
        // splitMain
        // 
        splitMain.Dock = DockStyle.Fill;
        splitMain.Location = new Point(0, 46);
        splitMain.Name = "splitMain";
        // 
        // splitMain.Panel1
        // 
        splitMain.Panel1.Controls.Add(dgwKunden);
        splitMain.Panel1.Controls.Add(panelKundenHeader);
        // 
        // splitMain.Panel2
        // 
        splitMain.Panel2.Controls.Add(splitRight);
        splitMain.Size = new Size(1200, 616);
        splitMain.SplitterDistance = 593;
        splitMain.TabIndex = 0;
        // 
        // dgwKunden
        // 
        dgwKunden.AllowUserToAddRows = false;
        dgwKunden.AllowUserToDeleteRows = false;
        dgwKunden.AllowUserToResizeRows = false;
        dgwKunden.BackgroundColor = SystemColors.Window;
        dgwKunden.ColumnHeadersHeight = 28;
        dgwKunden.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgwKunden.Dock = DockStyle.Fill;
        dgwKunden.Location = new Point(0, 26);
        dgwKunden.MultiSelect = false;
        dgwKunden.Name = "dgwKunden";
        dgwKunden.ReadOnly = true;
        dgwKunden.RowHeadersVisible = false;
        dgwKunden.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgwKunden.Size = new Size(593, 590);
        dgwKunden.TabIndex = 0;
        // 
        // panelKundenHeader
        // 
        panelKundenHeader.BackColor = Color.FromArgb(230, 230, 230);
        panelKundenHeader.Controls.Add(lblKundenHeader);
        panelKundenHeader.Dock = DockStyle.Top;
        panelKundenHeader.Location = new Point(0, 0);
        panelKundenHeader.Name = "panelKundenHeader";
        panelKundenHeader.Size = new Size(593, 26);
        panelKundenHeader.TabIndex = 1;
        // 
        // lblKundenHeader
        // 
        lblKundenHeader.AutoSize = true;
        lblKundenHeader.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblKundenHeader.Location = new Point(6, 5);
        lblKundenHeader.Name = "lblKundenHeader";
        lblKundenHeader.Size = new Size(144, 15);
        lblKundenHeader.TabIndex = 0;
        lblKundenHeader.Text = "Kunden (mit offenen LS)";
        // 
        // splitRight
        // 
        splitRight.Dock = DockStyle.Fill;
        splitRight.Location = new Point(0, 0);
        splitRight.Name = "splitRight";
        splitRight.Orientation = Orientation.Horizontal;
        // 
        // splitRight.Panel1
        // 
        splitRight.Panel1.Controls.Add(dgwLieferscheine);
        splitRight.Panel1.Controls.Add(panelLsHeader);
        // 
        // splitRight.Panel2
        // 
        splitRight.Panel2.Controls.Add(dgwZeilen);
        splitRight.Panel2.Controls.Add(panelZeilenHeader);
        splitRight.Size = new Size(603, 616);
        splitRight.SplitterDistance = 437;
        splitRight.TabIndex = 0;
        // 
        // dgwLieferscheine
        // 
        dgwLieferscheine.AllowUserToAddRows = false;
        dgwLieferscheine.AllowUserToDeleteRows = false;
        dgwLieferscheine.AllowUserToResizeRows = false;
        dgwLieferscheine.BackgroundColor = SystemColors.Window;
        dgwLieferscheine.ColumnHeadersHeight = 28;
        dgwLieferscheine.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgwLieferscheine.Columns.AddRange(new DataGridViewColumn[] { colLsChecked });
        dgwLieferscheine.Dock = DockStyle.Fill;
        dgwLieferscheine.Location = new Point(0, 26);
        dgwLieferscheine.MultiSelect = false;
        dgwLieferscheine.Name = "dgwLieferscheine";
        dgwLieferscheine.RowHeadersVisible = false;
        dgwLieferscheine.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgwLieferscheine.Size = new Size(603, 411);
        dgwLieferscheine.TabIndex = 0;
        // 
        // colLsChecked
        // 
        colLsChecked.HeaderText = "";
        colLsChecked.Name = "colLsChecked";
        colLsChecked.Resizable = DataGridViewTriState.False;
        colLsChecked.Width = 30;
        // 
        // panelLsHeader
        // 
        panelLsHeader.BackColor = Color.FromArgb(230, 230, 230);
        panelLsHeader.Controls.Add(lblLsHeader);
        panelLsHeader.Dock = DockStyle.Top;
        panelLsHeader.Location = new Point(0, 0);
        panelLsHeader.Name = "panelLsHeader";
        panelLsHeader.Size = new Size(603, 26);
        panelLsHeader.TabIndex = 1;
        // 
        // lblLsHeader
        // 
        lblLsHeader.AutoSize = true;
        lblLsHeader.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblLsHeader.Location = new Point(6, 5);
        lblLsHeader.Name = "lblLsHeader";
        lblLsHeader.Size = new Size(150, 15);
        lblLsHeader.TabIndex = 0;
        lblLsHeader.Text = "Lieferscheine des Kunden";
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
        dgwZeilen.Size = new Size(603, 149);
        dgwZeilen.TabIndex = 0;
        // 
        // panelZeilenHeader
        // 
        panelZeilenHeader.BackColor = Color.FromArgb(230, 230, 230);
        panelZeilenHeader.Controls.Add(lblZeilenHeader);
        panelZeilenHeader.Dock = DockStyle.Top;
        panelZeilenHeader.Location = new Point(0, 0);
        panelZeilenHeader.Name = "panelZeilenHeader";
        panelZeilenHeader.Size = new Size(603, 26);
        panelZeilenHeader.TabIndex = 1;
        // 
        // lblZeilenHeader
        // 
        lblZeilenHeader.AutoSize = true;
        lblZeilenHeader.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblZeilenHeader.Location = new Point(6, 5);
        lblZeilenHeader.Name = "lblZeilenHeader";
        lblZeilenHeader.Size = new Size(162, 15);
        lblZeilenHeader.TabIndex = 0;
        lblZeilenHeader.Text = "Rechnungszeilen (Vorschau)";
        // 
        // FrmRechnungErfassung
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1200, 750);
        Controls.Add(splitMain);
        Controls.Add(panelDetail);
        Controls.Add(panelFilter);
        Name = "FrmRechnungErfassung";
        Text = "Rechnungserfassung";
        panelFilter.ResumeLayout(false);
        panelFilter.PerformLayout();
        panelDetail.ResumeLayout(false);
        panelDetail.PerformLayout();
        panelBuchenContainer.ResumeLayout(false);
        splitMain.Panel1.ResumeLayout(false);
        splitMain.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
        splitMain.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgwKunden).EndInit();
        panelKundenHeader.ResumeLayout(false);
        panelKundenHeader.PerformLayout();
        splitRight.Panel1.ResumeLayout(false);
        splitRight.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitRight).EndInit();
        splitRight.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgwLieferscheine).EndInit();
        panelLsHeader.ResumeLayout(false);
        panelLsHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgwZeilen).EndInit();
        panelZeilenHeader.ResumeLayout(false);
        panelZeilenHeader.PerformLayout();
        ResumeLayout(false);
    }

    private Panel                       panelFilter;
    private Panel                       panelDetail;
    private Panel                       panelBuchenContainer;
    private SplitContainer              splitMain;
    private SplitContainer              splitRight;
    private Panel                       panelKundenHeader;
    private Label                       lblKundenHeader;
    private DataGridView                dgwKunden;
    private Panel                       panelLsHeader;
    private Label                       lblLsHeader;
    private DataGridView                dgwLieferscheine;
    private DataGridViewCheckBoxColumn  colLsChecked;
    private Panel                       panelZeilenHeader;
    private Label                       lblZeilenHeader;
    private DataGridView                dgwZeilen;
    private Label                       lblVon;
    private DateTimePicker              dtpVon;
    private Label                       lblBis;
    private DateTimePicker              dtpBis;
    private Button                      btnSuchen;
    private Label                       lblRechnungsdatum;
    private DateTimePicker              dtpRechnungsdatum;
    private Label                       lblNotizLabel;
    private TextBox                     txtNotiz;
    private Label                       lblNettoLabel;
    private Label                       lblNettoWert;
    private Label                       lblBruttoLabel;
    private Label                       lblBruttoWert;
    private Button                      btnBuchen;
}