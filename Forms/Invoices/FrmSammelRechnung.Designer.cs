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
        components = new System.ComponentModel.Container();

        panelFilter      = new Panel();
        panelDetail      = new Panel();
        panelGridHeader  = new Panel();
        lblGridHeader    = new Label();
        dgwKunden        = new DataGridView();
        colKundeChecked  = new DataGridViewCheckBoxColumn();

        lblVon              = new Label();
        dtpVon              = new DateTimePicker();
        lblBis              = new Label();
        dtpBis              = new DateTimePicker();
        btnSuchen           = new Button();
        btnAlleAuswaehlen   = new Button();
        btnAlleAbwaehlen    = new Button();

        lblAusgewaehltLabel = new Label();
        lblAusgewaehltWert  = new Label();
        lblGesamtLabel      = new Label();
        lblGesamtWert       = new Label();
        btnSammelBuchen     = new Button();

        panelFilter.SuspendLayout();
        panelDetail.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgwKunden).BeginInit();
        SuspendLayout();

        // ── panelFilter ───────────────────────────────────────────────────────
        panelFilter.BackColor = SystemColors.Control;
        panelFilter.Dock      = DockStyle.Top;
        panelFilter.Height    = 46;
        panelFilter.Padding   = new Padding(6, 8, 6, 0);
        panelFilter.Controls.AddRange([
            lblVon, dtpVon, lblBis, dtpBis, btnSuchen,
            btnAlleAuswaehlen, btnAlleAbwaehlen]);

        lblVon.AutoSize = true;
        lblVon.Location = new Point(8, 14);
        lblVon.Text     = "Von:";

        dtpVon.Format   = DateTimePickerFormat.Short;
        dtpVon.Location = new Point(42, 10);
        dtpVon.Size     = new Size(110, 23);

        lblBis.AutoSize = true;
        lblBis.Location = new Point(162, 14);
        lblBis.Text     = "Bis:";

        dtpBis.Format   = DateTimePickerFormat.Short;
        dtpBis.Location = new Point(193, 10);
        dtpBis.Size     = new Size(110, 23);

        btnSuchen.Location  = new Point(316, 9);
        btnSuchen.Size      = new Size(90, 26);
        btnSuchen.Text      = "Suchen";
        btnSuchen.UseVisualStyleBackColor = true;

        btnAlleAuswaehlen.Location  = new Point(424, 9);
        btnAlleAuswaehlen.Size      = new Size(120, 26);
        btnAlleAuswaehlen.Text      = "Alle auswählen";
        btnAlleAuswaehlen.UseVisualStyleBackColor = true;

        btnAlleAbwaehlen.Location  = new Point(552, 9);
        btnAlleAbwaehlen.Size      = new Size(120, 26);
        btnAlleAbwaehlen.Text      = "Alle abwählen";
        btnAlleAbwaehlen.UseVisualStyleBackColor = true;

        // ── panelDetail ───────────────────────────────────────────────────────
        panelDetail.BackColor = SystemColors.Control;
        panelDetail.Dock      = DockStyle.Bottom;
        panelDetail.Height    = 70;
        panelDetail.Padding   = new Padding(8, 6, 8, 6);
        panelDetail.Controls.AddRange([
            lblAusgewaehltLabel, lblAusgewaehltWert,
            lblGesamtLabel, lblGesamtWert,
            btnSammelBuchen]);

        lblAusgewaehltLabel.AutoSize = true;
        lblAusgewaehltLabel.Location = new Point(8, 24);
        lblAusgewaehltLabel.Text     = "Ausgewählt:";

        lblAusgewaehltWert.AutoSize  = true;
        lblAusgewaehltWert.Font      = new Font("Segoe UI", 9f, FontStyle.Bold);
        lblAusgewaehltWert.Location  = new Point(90, 24);
        lblAusgewaehltWert.Text      = "0 Kunden";

        lblGesamtLabel.AutoSize = true;
        lblGesamtLabel.Location = new Point(250, 24);
        lblGesamtLabel.Text     = "Gesamtbetrag offen:";

        lblGesamtWert.AutoSize  = true;
        lblGesamtWert.Font      = new Font("Segoe UI", 9f, FontStyle.Bold);
        lblGesamtWert.ForeColor = Color.DarkGreen;
        lblGesamtWert.Location  = new Point(370, 24);
        lblGesamtWert.Text      = "0,00 €";

        btnSammelBuchen.Anchor    = AnchorStyles.Top | AnchorStyles.Right;
        btnSammelBuchen.BackColor = Color.FromArgb(0, 122, 204);
        btnSammelBuchen.ForeColor = Color.White;
        btnSammelBuchen.Font      = new Font("Segoe UI", 9f, FontStyle.Bold);
        btnSammelBuchen.Location  = new Point(900, 8);
        btnSammelBuchen.Size      = new Size(180, 50);
        btnSammelBuchen.Text      = "Sammelrechnung buchen";
        btnSammelBuchen.UseVisualStyleBackColor = false;

        // ── panelGridHeader ───────────────────────────────────────────────────
        panelGridHeader.Dock      = DockStyle.Top;
        panelGridHeader.Height    = 26;
        panelGridHeader.BackColor = Color.FromArgb(230, 230, 230);
        panelGridHeader.Controls.Add(lblGridHeader);

        lblGridHeader.AutoSize = true;
        lblGridHeader.Font     = new Font("Segoe UI", 8.5f, FontStyle.Bold);
        lblGridHeader.Location = new Point(6, 5);
        lblGridHeader.Text     = "Kunden mit offenen Lieferscheinen im Zeitraum";

        // ── dgwKunden ─────────────────────────────────────────────────────────
        dgwKunden.Dock                        = DockStyle.Fill;
        dgwKunden.AllowUserToAddRows          = false;
        dgwKunden.AllowUserToDeleteRows       = false;
        dgwKunden.AllowUserToResizeRows       = false;
        dgwKunden.AutoSizeColumnsMode         = DataGridViewAutoSizeColumnsMode.None;
        dgwKunden.BackgroundColor             = SystemColors.Window;
        dgwKunden.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgwKunden.ColumnHeadersHeight         = 28;
        dgwKunden.MultiSelect                 = false;
        dgwKunden.ReadOnly                    = false;
        dgwKunden.RowHeadersVisible           = false;
        dgwKunden.SelectionMode               = DataGridViewSelectionMode.FullRowSelect;

        colKundeChecked.HeaderText = "";
        colKundeChecked.Name       = "colKundeChecked";
        colKundeChecked.Width      = 30;
        colKundeChecked.ReadOnly   = false;
        colKundeChecked.Resizable  = DataGridViewTriState.False;
        dgwKunden.Columns.Add(colKundeChecked);

        // ── Form ──────────────────────────────────────────────────────────────
        AutoScaleDimensions = new SizeF(7f, 15f);
        AutoScaleMode       = AutoScaleMode.Font;
        ClientSize          = new Size(1100, 680);
        Controls.Add(dgwKunden);
        Controls.Add(panelGridHeader);
        Controls.Add(panelDetail);
        Controls.Add(panelFilter);
        Name        = "FrmSammelRechnung";
        Text        = "Sammelrechnung";
        WindowState = FormWindowState.Normal;

        panelFilter.ResumeLayout(false);
        panelFilter.PerformLayout();
        panelDetail.ResumeLayout(false);
        panelDetail.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgwKunden).EndInit();
        ResumeLayout(false);
        PerformLayout();
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