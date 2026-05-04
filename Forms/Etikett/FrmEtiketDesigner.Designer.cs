using DNR26V2.Services.Etikett;

using System.ComponentModel;

namespace DNR26V2.Forms.Etikett;

partial class FrmEtiketDesigner
{
    private System.ComponentModel.IContainer components = null!;

    // ── Toolbar ───────────────────────────────────────────────────────────────
    private ToolStrip          toolStrip        = null!;
    private ToolStripButton    tsBtnSpeichern   = null!;
    private ToolStripButton    tsBtnReset       = null!;
    private ToolStripSeparator tsSep1           = null!;
    private ToolStripLabel     tsLblPapier      = null!;
    private ToolStripTextBox   tsTxtBreiteCm    = null!;
    private ToolStripLabel     tsLblX           = null!;
    private ToolStripTextBox   tsTxtHoeheCm     = null!;
    private ToolStripLabel     tsLblCm          = null!;
    private ToolStripButton    tsBtnPapierOk    = null!;
    private ToolStripSeparator tsSep2           = null!;
    private ToolStripButton    tsBtnSchliessen  = null!;

    // ── Main split ────────────────────────────────────────────────────────────
    private SplitContainer     mainSplit        = null!;

    // ── Left: Properties ──────────────────────────────────────────────────────
    private Label              lblSelectedFeld  = null!;
    private Panel              pnlPropContent   = null!;
    private TableLayoutPanel   tbl              = null!;
    private NumericUpDown      nudPropX         = null!;
    private NumericUpDown      nudPropY         = null!;
    private NumericUpDown      nudPropW         = null!;
    private NumericUpDown      nudPropH         = null!;
    private NumericUpDown      nudPropSize      = null!;
    private ComboBox           cmbPropFont      = null!;
    private CheckBox           chkPropBold      = null!;
    private CheckBox           chkPropItalic    = null!;
    private CheckBox           chkPropVisible   = null!;
    private ComboBox           cmbPropAlign     = null!;
    private Button             btnPropForeColor = null!;
    private Button             btnPropBackColor = null!;

    // ── Right: Canvas ─────────────────────────────────────────────────────────
    private Panel              canvasContainer  = null!;
    internal Panel             canvas           = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        toolStrip = new ToolStrip();
        tsBtnSpeichern = new ToolStripButton();
        tsBtnReset = new ToolStripButton();
        tsSep1 = new ToolStripSeparator();
        tsLblPapier = new ToolStripLabel();
        tsTxtBreiteCm = new ToolStripTextBox();
        tsLblX = new ToolStripLabel();
        tsTxtHoeheCm = new ToolStripTextBox();
        tsLblCm = new ToolStripLabel();
        tsBtnPapierOk = new ToolStripButton();
        tsSep2 = new ToolStripSeparator();
        tsBtnSchliessen = new ToolStripButton();
        mainSplit = new SplitContainer();
        pnlPropContent = new Panel();
        tbl = new TableLayoutPanel();
        lblSelectedFeld = new Label();
        canvasContainer = new Panel();
        canvas = new Panel();
        nudPropX = Nud(0, 2000);
        nudPropY = Nud(0, 2000);
        nudPropW = Nud(20, 2000);
        nudPropH = Nud(20, 2000);
        nudPropSize = Nud(4, 200);
        cmbPropFont = new ComboBox();
        chkPropBold = new CheckBox();
        chkPropItalic = new CheckBox();
        chkPropVisible = new CheckBox();
        cmbPropAlign = new ComboBox();
        btnPropForeColor = new Button();
        btnPropBackColor = new Button();
        toolStrip.SuspendLayout();
        ((ISupportInitialize)mainSplit).BeginInit();
        mainSplit.Panel1.SuspendLayout();
        mainSplit.Panel2.SuspendLayout();
        mainSplit.SuspendLayout();
        pnlPropContent.SuspendLayout();
        canvasContainer.SuspendLayout();
        SuspendLayout();
        // 
        // toolStrip
        // 
        toolStrip.Items.AddRange(new ToolStripItem[] { tsBtnSpeichern, tsBtnReset, tsSep1, tsLblPapier, tsTxtBreiteCm, tsLblX, tsTxtHoeheCm, tsLblCm, tsBtnPapierOk, tsSep2, tsBtnSchliessen });
        toolStrip.Location = new Point(0, 0);
        toolStrip.Name = "toolStrip";
        toolStrip.Size = new Size(1020, 25);
        toolStrip.TabIndex = 1;
        // 
        // tsBtnSpeichern
        // 
        tsBtnSpeichern.DisplayStyle = ToolStripItemDisplayStyle.Text;
        tsBtnSpeichern.Name = "tsBtnSpeichern";
        tsBtnSpeichern.Size = new Size(63, 22);
        tsBtnSpeichern.Text = "Speichern";
        tsBtnSpeichern.Click += BtnSpeichern_Click;
        // 
        // tsBtnReset
        // 
        tsBtnReset.DisplayStyle = ToolStripItemDisplayStyle.Text;
        tsBtnReset.Name = "tsBtnReset";
        tsBtnReset.Size = new Size(39, 22);
        tsBtnReset.Text = "Reset";
        tsBtnReset.Click += BtnReset_Click;
        // 
        // tsSep1
        // 
        tsSep1.Name = "tsSep1";
        tsSep1.Size = new Size(6, 25);
        // 
        // tsLblPapier
        // 
        tsLblPapier.Name = "tsLblPapier";
        tsLblPapier.Size = new Size(52, 22);
        tsLblPapier.Text = "   Papier:";
        // 
        // tsTxtBreiteCm
        // 
        tsTxtBreiteCm.Name = "tsTxtBreiteCm";
        tsTxtBreiteCm.Size = new Size(100, 25);
        tsTxtBreiteCm.Text = "10";
        tsTxtBreiteCm.ToolTipText = "Breite in cm";
        // 
        // tsLblX
        // 
        tsLblX.Name = "tsLblX";
        tsLblX.Size = new Size(21, 22);
        tsLblX.Text = " × ";
        // 
        // tsTxtHoeheCm
        // 
        tsTxtHoeheCm.Name = "tsTxtHoeheCm";
        tsTxtHoeheCm.Size = new Size(100, 25);
        tsTxtHoeheCm.Text = "15";
        tsTxtHoeheCm.ToolTipText = "Höhe in cm";
        // 
        // tsLblCm
        // 
        tsLblCm.Name = "tsLblCm";
        tsLblCm.Size = new Size(30, 22);
        tsLblCm.Text = " cm ";
        // 
        // tsBtnPapierOk
        // 
        tsBtnPapierOk.DisplayStyle = ToolStripItemDisplayStyle.Text;
        tsBtnPapierOk.Name = "tsBtnPapierOk";
        tsBtnPapierOk.Size = new Size(23, 22);
        tsBtnPapierOk.Text = "✓";
        tsBtnPapierOk.ToolTipText = "Papierformat übernehmen & speichern";
        tsBtnPapierOk.Click += TsBtnPapierOk_Click;
        // 
        // tsSep2
        // 
        tsSep2.Name = "tsSep2";
        tsSep2.Size = new Size(6, 25);
        // 
        // tsBtnSchliessen
        // 
        tsBtnSchliessen.Alignment = ToolStripItemAlignment.Right;
        tsBtnSchliessen.DisplayStyle = ToolStripItemDisplayStyle.Text;
        tsBtnSchliessen.Name = "tsBtnSchliessen";
        tsBtnSchliessen.Size = new Size(62, 22);
        tsBtnSchliessen.Text = "Schließen";
        tsBtnSchliessen.Click += BtnSchliessen_Click;
        // 
        // mainSplit
        // 
        mainSplit.Dock = DockStyle.Fill;
        mainSplit.FixedPanel = FixedPanel.Panel1;
        mainSplit.Location = new Point(0, 25);
        mainSplit.Name = "mainSplit";
        // 
        // mainSplit.Panel1
        // 
        mainSplit.Panel1.Controls.Add(pnlPropContent);
        mainSplit.Panel1.Controls.Add(lblSelectedFeld);
        // 
        // mainSplit.Panel2
        // 
        mainSplit.Panel2.Controls.Add(canvasContainer);
        mainSplit.Size = new Size(1020, 675);
        mainSplit.SplitterDistance = 286;
        mainSplit.TabIndex = 0;
        // 
        // pnlPropContent
        // 
        pnlPropContent.AutoScroll = true;
        pnlPropContent.Controls.Add(tbl);
        pnlPropContent.Dock = DockStyle.Fill;
        pnlPropContent.Enabled = false;
        pnlPropContent.Location = new Point(0, 36);
        pnlPropContent.Name = "pnlPropContent";
        pnlPropContent.Size = new Size(286, 639);
        pnlPropContent.TabIndex = 0;
        // 
        // tbl
        // 
        tbl.AutoSize = true;
        tbl.ColumnCount = 2;
        tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 85F));
        tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tbl.Dock = DockStyle.Top;
        tbl.Location = new Point(0, 0);
        tbl.Name = "tbl";
        tbl.Padding = new Padding(6, 8, 6, 8);
        tbl.Size = new Size(286, 16);
        tbl.TabIndex = 0;
        Row(tbl, "X:",      nudPropX);
        Row(tbl, "Y:",      nudPropY);
        Row(tbl, "Breite:", nudPropW);
        Row(tbl, "Höhe:",   nudPropH);
        Row(tbl, "Größe:",  nudPropSize);
        Row(tbl, "Schrift:", cmbPropFont);
        Row(tbl, "Fett:",   chkPropBold);
        Row(tbl, "Kursiv:", chkPropItalic);
        Row(tbl, "Sichtbar:", chkPropVisible);
        Row(tbl, "Ausricht.:", cmbPropAlign);
        Row(tbl, "Vorderf.:", btnPropForeColor);
        Row(tbl, "Hinterf.:", btnPropBackColor);
        // 
        // lblSelectedFeld
        // 
        lblSelectedFeld.BackColor = Color.FromArgb(240, 240, 240);
        lblSelectedFeld.Dock = DockStyle.Top;
        lblSelectedFeld.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblSelectedFeld.ForeColor = Color.Gray;
        lblSelectedFeld.Location = new Point(0, 0);
        lblSelectedFeld.Name = "lblSelectedFeld";
        lblSelectedFeld.Padding = new Padding(4);
        lblSelectedFeld.Size = new Size(286, 36);
        lblSelectedFeld.TabIndex = 1;
        lblSelectedFeld.Text = "— kein Feld ausgewählt —";
        lblSelectedFeld.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // canvasContainer
        // 
        canvasContainer.AutoScroll = true;
        canvasContainer.BackColor = Color.FromArgb(200, 200, 200);
        canvasContainer.Controls.Add(canvas);
        canvasContainer.Dock = DockStyle.Fill;
        canvasContainer.Location = new Point(0, 0);
        canvasContainer.Name = "canvasContainer";
        canvasContainer.Padding = new Padding(14);
        canvasContainer.Size = new Size(730, 675);
        canvasContainer.TabIndex = 0;
        // 
        // canvas
        // 
        canvas.BackColor = Color.White;
        canvas.Location = new Point(14, 14);
        canvas.Name = "canvas";
        canvas.Size = new Size(378, 567);
        canvas.TabIndex = 0;
        // 
        // cmbPropFont
        // 
        cmbPropFont.Dock = DockStyle.Fill;
        cmbPropFont.Location = new Point(0, 0);
        cmbPropFont.Name = "cmbPropFont";
        cmbPropFont.Size = new Size(121, 23);
        cmbPropFont.TabIndex = 0;
        // 
        // chkPropBold
        // 
        chkPropBold.Dock = DockStyle.Fill;
        chkPropBold.Location = new Point(0, 0);
        chkPropBold.Name = "chkPropBold";
        chkPropBold.Size = new Size(104, 24);
        chkPropBold.TabIndex = 0;
        chkPropBold.Text = "Fett";
        // 
        // chkPropItalic
        // 
        chkPropItalic.Dock = DockStyle.Fill;
        chkPropItalic.Location = new Point(0, 0);
        chkPropItalic.Name = "chkPropItalic";
        chkPropItalic.Size = new Size(104, 24);
        chkPropItalic.TabIndex = 0;
        chkPropItalic.Text = "Kursiv";
        // 
        // chkPropVisible
        // 
        chkPropVisible.Dock = DockStyle.Fill;
        chkPropVisible.Location = new Point(0, 0);
        chkPropVisible.Name = "chkPropVisible";
        chkPropVisible.Size = new Size(104, 24);
        chkPropVisible.TabIndex = 0;
        chkPropVisible.Text = "Sichtbar";
        // 
        // cmbPropAlign
        // 
        cmbPropAlign.Dock = DockStyle.Fill;
        cmbPropAlign.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbPropAlign.Items.AddRange(new object[] { "Links", "Mitte", "Rechts" });
        cmbPropAlign.Location = new Point(0, 0);
        cmbPropAlign.Name = "cmbPropAlign";
        cmbPropAlign.Size = new Size(121, 23);
        cmbPropAlign.TabIndex = 0;
        // 
        // btnPropForeColor
        // 
        btnPropForeColor.Dock = DockStyle.Fill;
        btnPropForeColor.Location = new Point(0, 0);
        btnPropForeColor.Name = "btnPropForeColor";
        btnPropForeColor.Size = new Size(75, 26);
        btnPropForeColor.TabIndex = 0;
        btnPropForeColor.Text = "Vorderfarbe";
        btnPropForeColor.Click += BtnPropForeColor_Click;
        // 
        // btnPropBackColor
        // 
        btnPropBackColor.Dock = DockStyle.Fill;
        btnPropBackColor.Location = new Point(0, 0);
        btnPropBackColor.Name = "btnPropBackColor";
        btnPropBackColor.Size = new Size(75, 26);
        btnPropBackColor.TabIndex = 0;
        btnPropBackColor.Text = "Hinterfarbe";
        btnPropBackColor.Click += BtnPropBackColor_Click;
        // 
        // FrmEtiketDesigner
        // 
        ClientSize = new Size(1020, 700);
        Controls.Add(mainSplit);
        Controls.Add(toolStrip);
        MinimizeBox = false;
        MinimumSize = new Size(820, 500);
        Name = "FrmEtiketDesigner";
        Text = "Label-Designer";
        toolStrip.ResumeLayout(false);
        toolStrip.PerformLayout();
        mainSplit.Panel1.ResumeLayout(false);
        mainSplit.Panel2.ResumeLayout(false);
        ((ISupportInitialize)mainSplit).EndInit();
        mainSplit.ResumeLayout(false);
        pnlPropContent.ResumeLayout(false);
        pnlPropContent.PerformLayout();
        canvasContainer.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    // ── Designer helpers (called only from InitializeComponent) ──────────────

    private static NumericUpDown Nud(int min, int max) => new()
    {
        Dock    = DockStyle.Fill,
        Minimum = min,
        Maximum = max,
        Margin  = new Padding(0, 2, 0, 2),
    };

    private static void Row(TableLayoutPanel tbl, string labelText, Control ctrl)
    {
        tbl.Controls.Add(new Label
        {
            Text      = labelText,
            Dock      = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleRight,
            Margin    = new Padding(0, 3, 6, 3),
        });
        ctrl.Margin = new Padding(0, 3, 0, 3);
        tbl.Controls.Add(ctrl);
    }
}
