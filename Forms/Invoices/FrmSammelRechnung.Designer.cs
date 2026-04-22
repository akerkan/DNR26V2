namespace DNR26V2.Forms.Invoices;

partial class FrmSammelRechnung
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
        btnSuchen = new Button();
        btnAlleAuswaehlen = new Button();
        btnAlleAbwaehlen = new Button();
        panelDetail = new Panel();
        lblAusgewaehltLabel = new Label();
        lblAusgewaehltWert = new Label();
        lblGesamtLabel = new Label();
        lblGesamtWert = new Label();
        btnSammelBuchen = new Button();
        panelGridHeader = new Panel();
        lblGridHeader = new Label();
        dgwKunden = new DataGridView();
        colKundeChecked = new DataGridViewCheckBoxColumn();
        panelFilter.SuspendLayout();
        panelDetail.SuspendLayout();
        panelGridHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgwKunden).BeginInit();
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
        panelFilter.Controls.Add(btnAlleAuswaehlen);
        panelFilter.Controls.Add(btnAlleAbwaehlen);
        panelFilter.Dock = DockStyle.Top;
        panelFilter.Location = new Point(0, 0);
        panelFilter.Name = "panelFilter";
        panelFilter.Padding = new Padding(6, 8, 6, 0);
        panelFilter.Size = new Size(1100, 46);
        panelFilter.TabIndex = 3;
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
        // btnAlleAuswaehlen
        // 
        btnAlleAuswaehlen.Location = new Point(424, 9);
        btnAlleAuswaehlen.Name = "btnAlleAuswaehlen";
        btnAlleAuswaehlen.Size = new Size(120, 26);
        btnAlleAuswaehlen.TabIndex = 5;
        btnAlleAuswaehlen.Text = "Alle auswählen";
        btnAlleAuswaehlen.UseVisualStyleBackColor = true;
        // 
        // btnAlleAbwaehlen
        // 
        btnAlleAbwaehlen.Location = new Point(552, 9);
        btnAlleAbwaehlen.Name = "btnAlleAbwaehlen";
        btnAlleAbwaehlen.Size = new Size(120, 26);
        btnAlleAbwaehlen.TabIndex = 6;
        btnAlleAbwaehlen.Text = "Alle abwählen";
        btnAlleAbwaehlen.UseVisualStyleBackColor = true;
        // 
        // panelDetail
        // 
        panelDetail.BackColor = SystemColors.Control;
        panelDetail.Controls.Add(lblAusgewaehltLabel);
        panelDetail.Controls.Add(lblAusgewaehltWert);
        panelDetail.Controls.Add(lblGesamtLabel);
        panelDetail.Controls.Add(lblGesamtWert);
        panelDetail.Controls.Add(btnSammelBuchen);
        panelDetail.Dock = DockStyle.Bottom;
        panelDetail.Location = new Point(0, 610);
        panelDetail.Name = "panelDetail";
        panelDetail.Padding = new Padding(8, 6, 8, 6);
        panelDetail.Size = new Size(1100, 70);
        panelDetail.TabIndex = 2;
        // 
        // lblAusgewaehltLabel
        // 
        lblAusgewaehltLabel.AutoSize = true;
        lblAusgewaehltLabel.Location = new Point(8, 24);
        lblAusgewaehltLabel.Name = "lblAusgewaehltLabel";
        lblAusgewaehltLabel.Size = new Size(72, 15);
        lblAusgewaehltLabel.TabIndex = 0;
        lblAusgewaehltLabel.Text = "Ausgewählt:";
        // 
        // lblAusgewaehltWert
        // 
        lblAusgewaehltWert.AutoSize = true;
        lblAusgewaehltWert.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblAusgewaehltWert.Location = new Point(90, 24);
        lblAusgewaehltWert.Name = "lblAusgewaehltWert";
        lblAusgewaehltWert.Size = new Size(60, 15);
        lblAusgewaehltWert.TabIndex = 1;
        lblAusgewaehltWert.Text = "0 Kunden";
        // 
        // lblGesamtLabel
        // 
        lblGesamtLabel.AutoSize = true;
        lblGesamtLabel.Location = new Point(250, 24);
        lblGesamtLabel.Name = "lblGesamtLabel";
        lblGesamtLabel.Size = new Size(115, 15);
        lblGesamtLabel.TabIndex = 2;
        lblGesamtLabel.Text = "Gesamtbetrag offen:";
        // 
        // lblGesamtWert
        // 
        lblGesamtWert.AutoSize = true;
        lblGesamtWert.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblGesamtWert.ForeColor = Color.DarkGreen;
        lblGesamtWert.Location = new Point(370, 24);
        lblGesamtWert.Name = "lblGesamtWert";
        lblGesamtWert.Size = new Size(41, 15);
        lblGesamtWert.TabIndex = 3;
        lblGesamtWert.Text = "0,00 €";
        // 
        // btnSammelBuchen
        // 
        btnSammelBuchen.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnSammelBuchen.BackColor = Color.FromArgb(0, 122, 204);
        btnSammelBuchen.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnSammelBuchen.ForeColor = Color.White;
        btnSammelBuchen.Location = new Point(908, 11);
        btnSammelBuchen.Name = "btnSammelBuchen";
        btnSammelBuchen.Size = new Size(180, 50);
        btnSammelBuchen.TabIndex = 4;
        btnSammelBuchen.Text = "Sammelrechnung buchen";
        btnSammelBuchen.UseVisualStyleBackColor = false;
        // 
        // panelGridHeader
        // 
        panelGridHeader.BackColor = Color.FromArgb(230, 230, 230);
        panelGridHeader.Controls.Add(lblGridHeader);
        panelGridHeader.Dock = DockStyle.Top;
        panelGridHeader.Location = new Point(0, 46);
        panelGridHeader.Name = "panelGridHeader";
        panelGridHeader.Size = new Size(1100, 26);
        panelGridHeader.TabIndex = 1;
        // 
        // lblGridHeader
        // 
        lblGridHeader.AutoSize = true;
        lblGridHeader.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblGridHeader.Location = new Point(6, 5);
        lblGridHeader.Name = "lblGridHeader";
        lblGridHeader.Size = new Size(276, 15);
        lblGridHeader.TabIndex = 0;
        lblGridHeader.Text = "Kunden mit offenen Lieferscheinen im Zeitraum";
        // 
        // dgwKunden
        // 
        dgwKunden.AllowUserToAddRows = false;
        dgwKunden.AllowUserToDeleteRows = false;
        dgwKunden.AllowUserToResizeRows = false;
        dgwKunden.BackgroundColor = SystemColors.Window;
        dgwKunden.ColumnHeadersHeight = 28;
        dgwKunden.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgwKunden.Columns.AddRange(new DataGridViewColumn[] { colKundeChecked });
        dgwKunden.Dock = DockStyle.Fill;
        dgwKunden.Location = new Point(0, 72);
        dgwKunden.MultiSelect = false;
        dgwKunden.Name = "dgwKunden";
        dgwKunden.RowHeadersVisible = false;
        dgwKunden.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgwKunden.Size = new Size(1100, 538);
        dgwKunden.TabIndex = 0;
        // 
        // colKundeChecked
        // 
        colKundeChecked.HeaderText = "";
        colKundeChecked.Name = "colKundeChecked";
        colKundeChecked.Resizable = DataGridViewTriState.False;
        colKundeChecked.Width = 30;
        // 
        // FrmSammelRechnung
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1100, 680);
        Controls.Add(dgwKunden);
        Controls.Add(panelGridHeader);
        Controls.Add(panelDetail);
        Controls.Add(panelFilter);
        Name = "FrmSammelRechnung";
        Text = "Sammelrechnung";
        panelFilter.ResumeLayout(false);
        panelFilter.PerformLayout();
        panelDetail.ResumeLayout(false);
        panelDetail.PerformLayout();
        panelGridHeader.ResumeLayout(false);
        panelGridHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgwKunden).EndInit();
        ResumeLayout(false);
    }

    private Panel                       panelFilter;
    private Panel                       panelDetail;
    private Panel                       panelGridHeader;
    private Label                       lblGridHeader;
    private DataGridView                dgwKunden;
    private DataGridViewCheckBoxColumn  colKundeChecked;

    private Label          lblVon;
    private DateTimePicker dtpVon;
    private Label          lblBis;
    private DateTimePicker dtpBis;
    private Button         btnSuchen;
    private Button         btnAlleAuswaehlen;
    private Button         btnAlleAbwaehlen;

    private Label          lblAusgewaehltLabel;
    private Label          lblAusgewaehltWert;
    private Label          lblGesamtLabel;
    private Label          lblGesamtWert;
    private Button         btnSammelBuchen;
}