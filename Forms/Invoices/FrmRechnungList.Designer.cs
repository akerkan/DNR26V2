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
        components = new System.ComponentModel.Container();

        panelFilter     = new Panel();
        panelDetail     = new Panel();
        splitMain       = new SplitContainer();
        panelListHeader = new Panel();
        lblListHeader   = new Label();
        dgwRechnungen   = new DataGridView();
        panelZeilHeader = new Panel();
        lblZeilHeader   = new Label();
        dgwZeilen       = new DataGridView();

        lblVon          = new Label();
        dtpVon          = new DateTimePicker();
        lblBis          = new Label();
        dtpBis          = new DateTimePicker();
        lblKundeFilter  = new Label();
        txtKundeFilter  = new TextBox();
        lblStatusFilter = new Label();
        cmbStatus       = new ComboBox();
        btnSuchen       = new Button();

        lblRechnungsnrLabel  = new Label();
        lblRechnungsnrWert   = new Label();
        lblNettoLabel        = new Label();
        lblNettoWert         = new Label();
        lblBruttoLabel       = new Label();
        lblBruttoWert        = new Label();
        lblStatusLabel       = new Label();
        lblStatusWert        = new Label();
        btnStornieren        = new Button();

        panelFilter.SuspendLayout();
        panelDetail.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
        splitMain.Panel1.SuspendLayout();
        splitMain.Panel2.SuspendLayout();
        splitMain.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgwRechnungen).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgwZeilen).BeginInit();
        SuspendLayout();

        // ── panelFilter ───────────────────────────────────────────────────────
        panelFilter.BackColor = SystemColors.Control;
        panelFilter.Dock      = DockStyle.Top;
        panelFilter.Height    = 46;
        panelFilter.Padding   = new Padding(6, 8, 6, 0);
        panelFilter.Controls.AddRange([
            lblVon, dtpVon, lblBis, dtpBis,
            lblKundeFilter, txtKundeFilter,
            lblStatusFilter, cmbStatus,
            btnSuchen]);

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

        lblKundeFilter.AutoSize = true;
        lblKundeFilter.Location = new Point(318, 14);
        lblKundeFilter.Text     = "Kunde:";

        txtKundeFilter.Location = new Point(360, 10);
        txtKundeFilter.Size     = new Size(180, 23);

        lblStatusFilter.AutoSize = true;
        lblStatusFilter.Location = new Point(552, 14);
        lblStatusFilter.Text     = "Status:";

        cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbStatus.Location      = new Point(598, 10);
        cmbStatus.Size          = new Size(130, 23);

        btnSuchen.Location  = new Point(742, 9);
        btnSuchen.Size      = new Size(90, 26);
        btnSuchen.Text      = "Suchen";
        btnSuchen.UseVisualStyleBackColor = true;

        // ── panelDetail ───────────────────────────────────────────────────────
        panelDetail.BackColor = SystemColors.Control;
        panelDetail.Dock      = DockStyle.Bottom;
        panelDetail.Height    = 58;
        panelDetail.Padding   = new Padding(8, 6, 8, 6);
        panelDetail.Controls.AddRange([
            lblRechnungsnrLabel, lblRechnungsnrWert,
            lblNettoLabel, lblNettoWert,
            lblBruttoLabel, lblBruttoWert,
            lblStatusLabel, lblStatusWert,
            btnStornieren]);

        lblRechnungsnrLabel.AutoSize = true;
        lblRechnungsnrLabel.Location = new Point(8, 20);
        lblRechnungsnrLabel.Text     = "Rechnung:";

        lblRechnungsnrWert.AutoSize  = true;
        lblRechnungsnrWert.Font      = new Font("Segoe UI", 9f, FontStyle.Bold);
        lblRechnungsnrWert.Location  = new Point(72, 20);
        lblRechnungsnrWert.Text      = "–";

        lblNettoLabel.AutoSize = true;
        lblNettoLabel.Location = new Point(230, 20);
        lblNettoLabel.Text     = "Netto:";

        lblNettoWert.AutoSize  = true;
        lblNettoWert.Font      = new Font("Segoe UI", 9f, FontStyle.Bold);
        lblNettoWert.Location  = new Point(272, 20);
        lblNettoWert.Text      = "–";

        lblBruttoLabel.AutoSize = true;
        lblBruttoLabel.Location = new Point(400, 20);
        lblBruttoLabel.Text     = "Brutto:";

        lblBruttoWert.AutoSize  = true;
        lblBruttoWert.Font      = new Font("Segoe UI", 9f, FontStyle.Bold);
        lblBruttoWert.ForeColor = Color.DarkGreen;
        lblBruttoWert.Location  = new Point(444, 20);
        lblBruttoWert.Text      = "–";

        lblStatusLabel.AutoSize = true;
        lblStatusLabel.Location = new Point(600, 20);
        lblStatusLabel.Text     = "Status:";

        lblStatusWert.AutoSize  = true;
        lblStatusWert.Font      = new Font("Segoe UI", 9f, FontStyle.Bold);
        lblStatusWert.Location  = new Point(645, 20);
        lblStatusWert.Text      = "–";

        btnStornieren.Anchor    = AnchorStyles.Top | AnchorStyles.Right;
        btnStornieren.BackColor = Color.FromArgb(180, 30, 30);
        btnStornieren.ForeColor = Color.White;
        btnStornieren.Font      = new Font("Segoe UI", 9f, FontStyle.Bold);
        btnStornieren.Location  = new Point(950, 6);
        btnStornieren.Size      = new Size(130, 44);
        btnStornieren.Text      = "Stornieren";
        btnStornieren.Enabled   = false;
        btnStornieren.UseVisualStyleBackColor = false;

        // ── splitMain ─────────────────────────────────────────────────────────
        splitMain.Dock             = DockStyle.Fill;
        splitMain.Orientation      = Orientation.Horizontal;
        splitMain.SplitterDistance = 360;
        splitMain.SplitterWidth    = 4;

        panelListHeader.Dock      = DockStyle.Top;
        panelListHeader.Height    = 26;
        panelListHeader.BackColor = Color.FromArgb(230, 230, 230);
        panelListHeader.Controls.Add(lblListHeader);

        lblListHeader.AutoSize = true;
        lblListHeader.Font     = new Font("Segoe UI", 8.5f, FontStyle.Bold);
        lblListHeader.Location = new Point(6, 5);
        lblListHeader.Text     = "Rechnungsliste";

        dgwRechnungen.Dock                        = DockStyle.Fill;
        dgwRechnungen.AllowUserToAddRows          = false;
        dgwRechnungen.AllowUserToDeleteRows       = false;
        dgwRechnungen.AllowUserToResizeRows       = false;
        dgwRechnungen.AutoSizeColumnsMode         = DataGridViewAutoSizeColumnsMode.None;
        dgwRechnungen.BackgroundColor             = SystemColors.Window;
        dgwRechnungen.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgwRechnungen.ColumnHeadersHeight         = 28;
        dgwRechnungen.MultiSelect                 = false;
        dgwRechnungen.ReadOnly                    = true;
        dgwRechnungen.RowHeadersVisible           = false;
        dgwRechnungen.SelectionMode               = DataGridViewSelectionMode.FullRowSelect;

        splitMain.Panel1.Controls.Add(dgwRechnungen);
        splitMain.Panel1.Controls.Add(panelListHeader);

        panelZeilHeader.Dock      = DockStyle.Top;
        panelZeilHeader.Height    = 26;
        panelZeilHeader.BackColor = Color.FromArgb(230, 230, 230);
        panelZeilHeader.Controls.Add(lblZeilHeader);

        lblZeilHeader.AutoSize = true;
        lblZeilHeader.Font     = new Font("Segoe UI", 8.5f, FontStyle.Bold);
        lblZeilHeader.Location = new Point(6, 5);
        lblZeilHeader.Text     = "Rechnungszeilen";

        dgwZeilen.Dock                        = DockStyle.Fill;
        dgwZeilen.AllowUserToAddRows          = false;
        dgwZeilen.AllowUserToDeleteRows       = false;
        dgwZeilen.AllowUserToResizeRows       = false;
        dgwZeilen.AutoSizeColumnsMode         = DataGridViewAutoSizeColumnsMode.None;
        dgwZeilen.BackgroundColor             = SystemColors.Window;
        dgwZeilen.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgwZeilen.ColumnHeadersHeight         = 28;
        dgwZeilen.MultiSelect                 = false;
        dgwZeilen.ReadOnly                    = true;
        dgwZeilen.RowHeadersVisible           = false;
        dgwZeilen.SelectionMode               = DataGridViewSelectionMode.FullRowSelect;

        splitMain.Panel2.Controls.Add(dgwZeilen);
        splitMain.Panel2.Controls.Add(panelZeilHeader);

        // ── Form ──────────────────────────────────────────────────────────────
        AutoScaleDimensions = new SizeF(7f, 15f);
        AutoScaleMode       = AutoScaleMode.Font;
        ClientSize          = new Size(1200, 750);
        Controls.Add(splitMain);
        Controls.Add(panelDetail);
        Controls.Add(panelFilter);
        Name        = "FrmRechnungList";
        Text        = "Rechnungsliste";
        WindowState = FormWindowState.Normal;

        panelFilter.ResumeLayout(false);
        panelFilter.PerformLayout();
        panelDetail.ResumeLayout(false);
        panelDetail.PerformLayout();
        splitMain.Panel1.ResumeLayout(false);
        splitMain.Panel2.ResumeLayout(false);
        splitMain.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgwRechnungen).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgwZeilen).EndInit();
        ResumeLayout(false);
        PerformLayout();
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
}