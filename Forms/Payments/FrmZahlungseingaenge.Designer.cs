namespace DNR26V2.Forms.Payments;

partial class FrmZahlungseingaenge
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        splitMain          = new SplitContainer();
        panelLeft          = new Panel();
        lblKundeSearch     = new Label();
        txtKundeSearch     = new TextBox();
        lstKunden          = new ListBox();
        panelRight         = new Panel();
        panelRightTop      = new Panel();
        lblVon             = new Label();
        dtpVon             = new DateTimePicker();
        lblBis             = new Label();
        dtpBis             = new DateTimePicker();
        btnLaden           = new Button();
        btnBuchen          = new Button();
        lblSaldo           = new Label();
        splitRightVertical = new SplitContainer();
        dgwLedger          = new DataGridView();
        panelHistory       = new Panel();
        lblHistoryHeader   = new Label();
        dgwHistory         = new DataGridView();

        ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
        splitMain.Panel1.SuspendLayout();
        splitMain.Panel2.SuspendLayout();
        splitMain.SuspendLayout();
        panelLeft.SuspendLayout();
        panelRight.SuspendLayout();
        panelRightTop.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitRightVertical).BeginInit();
        splitRightVertical.Panel1.SuspendLayout();
        splitRightVertical.Panel2.SuspendLayout();
        splitRightVertical.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgwLedger).BeginInit();
        panelHistory.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgwHistory).BeginInit();
        SuspendLayout();

        // splitMain
        splitMain.Dock             = DockStyle.Fill;
        splitMain.FixedPanel       = FixedPanel.Panel1;
        splitMain.SplitterDistance = 240;
        splitMain.Panel1.Controls.Add(panelLeft);
        splitMain.Panel2.Controls.Add(panelRight);

        // panelLeft
        panelLeft.Dock    = DockStyle.Fill;
        panelLeft.Padding = new Padding(4);
        panelLeft.Controls.Add(lstKunden);
        panelLeft.Controls.Add(txtKundeSearch);
        panelLeft.Controls.Add(lblKundeSearch);

        // lblKundeSearch
        lblKundeSearch.AutoSize = true;
        lblKundeSearch.Location = new Point(4, 6);
        lblKundeSearch.Text     = "Kundensuche:";

        // txtKundeSearch
        txtKundeSearch.Location = new Point(4, 24);
        txtKundeSearch.Size     = new Size(224, 23);
        txtKundeSearch.Anchor   = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        // lstKunden
        lstKunden.Location      = new Point(4, 54);
        lstKunden.Size          = new Size(224, 500);
        lstKunden.Anchor        = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lstKunden.DisplayMember = "Kundenname";

        // panelRight
        panelRight.Dock    = DockStyle.Fill;
        panelRight.Padding = new Padding(4);
        panelRight.Controls.Add(splitRightVertical);
        panelRight.Controls.Add(panelRightTop);

        // panelRightTop
        panelRightTop.Dock   = DockStyle.Top;
        panelRightTop.Height = 40;
        panelRightTop.Controls.Add(lblVon);
        panelRightTop.Controls.Add(dtpVon);
        panelRightTop.Controls.Add(lblBis);
        panelRightTop.Controls.Add(dtpBis);
        panelRightTop.Controls.Add(btnLaden);
        panelRightTop.Controls.Add(btnBuchen);
        panelRightTop.Controls.Add(lblSaldo);

        // lblVon
        lblVon.AutoSize = true;
        lblVon.Location = new Point(4, 11);
        lblVon.Text     = "Von:";

        // dtpVon
        dtpVon.Location = new Point(36, 8);
        dtpVon.Size     = new Size(110, 23);
        dtpVon.Format   = DateTimePickerFormat.Short;

        // lblBis
        lblBis.AutoSize = true;
        lblBis.Location = new Point(154, 11);
        lblBis.Text     = "Bis:";

        // dtpBis
        dtpBis.Location = new Point(182, 8);
        dtpBis.Size     = new Size(110, 23);
        dtpBis.Format   = DateTimePickerFormat.Short;

        // btnLaden
        btnLaden.Location = new Point(432, 7);
        btnLaden.Size     = new Size(80, 26);
        btnLaden.Text     = "Laden";

        // btnBuchen
        btnBuchen.Location  = new Point(522, 7);
        btnBuchen.Size      = new Size(90, 26);
        btnBuchen.Text      = "Buchen";
        btnBuchen.BackColor = Color.FromArgb(0, 120, 215);
        btnBuchen.ForeColor = Color.White;
        btnBuchen.FlatStyle = FlatStyle.Flat;
        btnBuchen.Enabled   = false;

        // lblSaldo
        lblSaldo.AutoSize  = true;
        lblSaldo.Location  = new Point(624, 11);
        lblSaldo.Font      = new Font(SystemFonts.DefaultFont.FontFamily, 10f, FontStyle.Bold);
        lblSaldo.Text      = "Saldo: –";
        lblSaldo.ForeColor = Color.DarkBlue;

        // splitRightVertical — open items (top) / payment history (bottom)
        splitRightVertical.Dock             = DockStyle.Fill;
        splitRightVertical.Orientation      = Orientation.Horizontal;
        splitRightVertical.SplitterDistance = 320;
        splitRightVertical.Panel1.Controls.Add(dgwLedger);
        splitRightVertical.Panel2.Controls.Add(panelHistory);

        // dgwLedger
        dgwLedger.Dock                  = DockStyle.Fill;
        dgwLedger.AllowUserToAddRows    = false;
        dgwLedger.AllowUserToDeleteRows = false;
        dgwLedger.SelectionMode         = DataGridViewSelectionMode.FullRowSelect;
        dgwLedger.MultiSelect           = false;
        dgwLedger.RowHeadersVisible     = false;
        dgwLedger.BackgroundColor       = SystemColors.Window;
        dgwLedger.BorderStyle           = BorderStyle.Fixed3D;
        dgwLedger.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 250);
        dgwLedger.EditMode              = DataGridViewEditMode.EditOnEnter;

        // panelHistory
        panelHistory.Dock = DockStyle.Fill;
        panelHistory.Controls.Add(dgwHistory);
        panelHistory.Controls.Add(lblHistoryHeader);

        // lblHistoryHeader
        lblHistoryHeader.Dock      = DockStyle.Top;
        lblHistoryHeader.Height    = 22;
        lblHistoryHeader.Text      = " Gebuchte Zahlungen";
        lblHistoryHeader.Font      = new Font(SystemFonts.DefaultFont.FontFamily, 9f, FontStyle.Bold);
        lblHistoryHeader.BackColor = Color.FromArgb(230, 230, 240);

        // dgwHistory
        dgwHistory.Dock                  = DockStyle.Fill;
        dgwHistory.AllowUserToAddRows    = false;
        dgwHistory.AllowUserToDeleteRows = false;
        dgwHistory.ReadOnly              = true;
        dgwHistory.SelectionMode         = DataGridViewSelectionMode.FullRowSelect;
        dgwHistory.MultiSelect           = false;
        dgwHistory.RowHeadersVisible     = false;
        dgwHistory.BackgroundColor       = SystemColors.Window;
        dgwHistory.BorderStyle           = BorderStyle.Fixed3D;
        dgwHistory.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 250, 245);

        // Form
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode       = AutoScaleMode.Font;
        ClientSize          = new Size(1200, 700);
        Controls.Add(splitMain);
        Name        = nameof(FrmZahlungseingaenge);
        Text        = "Zahlungseingänge";
        WindowState = FormWindowState.Maximized;

        ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
        splitMain.Panel1.ResumeLayout(false);
        splitMain.Panel2.ResumeLayout(false);
        splitMain.ResumeLayout(false);
        panelLeft.ResumeLayout(false);
        panelLeft.PerformLayout();
        panelRight.ResumeLayout(false);
        panelRightTop.ResumeLayout(false);
        panelRightTop.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)splitRightVertical).EndInit();
        splitRightVertical.Panel1.ResumeLayout(false);
        splitRightVertical.Panel2.ResumeLayout(false);
        splitRightVertical.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgwLedger).EndInit();
        panelHistory.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgwHistory).EndInit();
        ResumeLayout(false);
    }

    private SplitContainer   splitMain;
    private Panel            panelLeft;
    private Label            lblKundeSearch;
    private TextBox          txtKundeSearch;
    private ListBox          lstKunden;
    private Panel            panelRight;
    private Panel            panelRightTop;
    private Label            lblVon;
    private DateTimePicker   dtpVon;
    private Label            lblBis;
    private DateTimePicker   dtpBis;
    private Button           btnLaden;
    private Button           btnBuchen;
    private Label            lblSaldo;
    private SplitContainer   splitRightVertical;
    private DataGridView     dgwLedger;
    private Panel            panelHistory;
    private Label            lblHistoryHeader;
    private DataGridView     dgwHistory;
}
