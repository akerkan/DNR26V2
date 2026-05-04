using DNR26V2.Services.Etikett;

namespace DNR26V2.Forms.Etikett;

partial class FrmEtiketDesigner
{
    private System.ComponentModel.IContainer components = null!;

    // ── Properties panel controls ─────────────────────────────────────────────
    private Label         lblSelectedFeld  = null!;
    private Panel         pnlPropContent   = null!;
    private NumericUpDown nudPropX         = null!;
    private NumericUpDown nudPropY         = null!;
    private NumericUpDown nudPropW         = null!;
    private NumericUpDown nudPropH         = null!;
    private ComboBox      cmbPropFont      = null!;
    private NumericUpDown nudPropSize      = null!;
    private CheckBox      chkPropBold      = null!;
    private CheckBox      chkPropItalic    = null!;
    private CheckBox      chkPropVisible   = null!;
    private ComboBox      cmbPropAlign     = null!;
    private Button        btnPropForeColor = null!;
    private Button        btnPropBackColor = null!;

    // ── Canvas ────────────────────────────────────────────────────────────────
    internal Panel canvas = null!;

    // ── Layout ───────────────────────────────────────────────────────────────
    private SplitContainer designSplit  = null!;
    private Button          btnSpeichern = null!;
    private Button          btnReset     = null!;
    private Button          btnSchliessen= null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        btnSpeichern = new Button();
        btnReset = new Button();
        btnSchliessen = new Button();
        pnlButtons = new FlowLayoutPanel();
        lblSelectedFeld = new Label();
        tbl = new TableLayoutPanel();
        cmbPropFont = new ComboBox();
        chkPropBold = new CheckBox();
        chkPropItalic = new CheckBox();
        chkPropVisible = new CheckBox();
        cmbPropAlign = new ComboBox();
        btnPropForeColor = new Button();
        btnPropBackColor = new Button();
        pnlPropContent = new Panel();
        leftPanel = new Panel();
        canvas = new Panel();
        canvasContainer = new Panel();
        designSplit = new SplitContainer();
        pnlButtons.SuspendLayout();
        leftPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)designSplit).BeginInit();
        designSplit.Panel2.SuspendLayout();
        designSplit.SuspendLayout();
        SuspendLayout();
        // 
        // btnSpeichern
        // 
        btnSpeichern.Location = new Point(3, 3);
        btnSpeichern.Name = "btnSpeichern";
        btnSpeichern.Size = new Size(75, 23);
        btnSpeichern.TabIndex = 0;
        btnSpeichern.Click += BtnSpeichern_Click;
        // 
        // btnReset
        // 
        btnReset.Location = new Point(84, 3);
        btnReset.Name = "btnReset";
        btnReset.Size = new Size(75, 23);
        btnReset.TabIndex = 1;
        btnReset.Click += BtnReset_Click;
        // 
        // btnSchliessen
        // 
        btnSchliessen.Location = new Point(165, 3);
        btnSchliessen.Name = "btnSchliessen";
        btnSchliessen.Size = new Size(75, 23);
        btnSchliessen.TabIndex = 2;
        btnSchliessen.Click += BtnSchliessen_Click;
        // 
        // pnlButtons
        // 
        pnlButtons.Controls.Add(btnSpeichern);
        pnlButtons.Controls.Add(btnReset);
        pnlButtons.Controls.Add(btnSchliessen);
        pnlButtons.Controls.Add(canvas);
        pnlButtons.Controls.Add(tbl);
        pnlButtons.Controls.Add(pnlPropContent);
        pnlButtons.Controls.Add(leftPanel);
        pnlButtons.Controls.Add(canvasContainer);
        pnlButtons.Location = new Point(13, 12);
        pnlButtons.Name = "pnlButtons";
        pnlButtons.Size = new Size(486, 334);
        pnlButtons.TabIndex = 1;
        // 
        // lblSelectedFeld
        // 
        lblSelectedFeld.Location = new Point(0, 0);
        lblSelectedFeld.Name = "lblSelectedFeld";
        lblSelectedFeld.Size = new Size(100, 23);
        lblSelectedFeld.TabIndex = 1;
        // 
        // tbl
        // 
        tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
        tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tbl.Location = new Point(3, 109);
        tbl.Name = "tbl";
        tbl.Size = new Size(200, 100);
        tbl.TabIndex = 0;
        // 
        // cmbPropFont
        // 
        cmbPropFont.Location = new Point(0, 0);
        cmbPropFont.Name = "cmbPropFont";
        cmbPropFont.Size = new Size(121, 23);
        cmbPropFont.TabIndex = 0;
        // 
        // chkPropBold
        // 
        chkPropBold.Location = new Point(0, 0);
        chkPropBold.Name = "chkPropBold";
        chkPropBold.Size = new Size(104, 24);
        chkPropBold.TabIndex = 0;
        // 
        // chkPropItalic
        // 
        chkPropItalic.Location = new Point(0, 0);
        chkPropItalic.Name = "chkPropItalic";
        chkPropItalic.Size = new Size(104, 24);
        chkPropItalic.TabIndex = 0;
        // 
        // chkPropVisible
        // 
        chkPropVisible.Location = new Point(0, 0);
        chkPropVisible.Name = "chkPropVisible";
        chkPropVisible.Size = new Size(104, 24);
        chkPropVisible.TabIndex = 0;
        // 
        // cmbPropAlign
        // 
        cmbPropAlign.Items.AddRange(new object[] { "Links", "Mitte", "Rechts" });
        cmbPropAlign.Location = new Point(0, 0);
        cmbPropAlign.Name = "cmbPropAlign";
        cmbPropAlign.Size = new Size(121, 23);
        cmbPropAlign.TabIndex = 0;
        // 
        // btnPropForeColor
        // 
        btnPropForeColor.Location = new Point(0, 0);
        btnPropForeColor.Name = "btnPropForeColor";
        btnPropForeColor.Size = new Size(75, 23);
        btnPropForeColor.TabIndex = 0;
        btnPropForeColor.Click += BtnPropForeColor_Click;
        // 
        // btnPropBackColor
        // 
        btnPropBackColor.Location = new Point(0, 0);
        btnPropBackColor.Name = "btnPropBackColor";
        btnPropBackColor.Size = new Size(75, 23);
        btnPropBackColor.TabIndex = 0;
        btnPropBackColor.Click += BtnPropBackColor_Click;
        // 
        // pnlPropContent
        // 
        pnlPropContent.Enabled = false;
        pnlPropContent.Location = new Point(209, 109);
        pnlPropContent.Name = "pnlPropContent";
        pnlPropContent.Size = new Size(200, 100);
        pnlPropContent.TabIndex = 0;
        // 
        // leftPanel
        // 
        leftPanel.Controls.Add(lblSelectedFeld);
        leftPanel.Location = new Point(3, 215);
        leftPanel.Name = "leftPanel";
        leftPanel.Size = new Size(200, 100);
        leftPanel.TabIndex = 0;
        // 
        // canvas
        // 
        canvas.Location = new Point(246, 3);
        canvas.Name = "canvas";
        canvas.Size = new Size(200, 100);
        canvas.TabIndex = 0;
        // 
        // canvasContainer
        // 
        canvasContainer.AutoScroll = true;
        canvasContainer.AutoScrollMinSize = new Size(378, 567);
        canvasContainer.Location = new Point(209, 215);
        canvasContainer.Name = "canvasContainer";
        canvasContainer.Size = new Size(200, 100);
        canvasContainer.TabIndex = 0;
        // 
        // designSplit
        // 
        designSplit.Location = new Point(0, 0);
        designSplit.Name = "designSplit";
        // 
        // designSplit.Panel2
        // 
        designSplit.Panel2.Controls.Add(pnlButtons);
        designSplit.Size = new Size(958, 635);
        designSplit.SplitterDistance = 319;
        designSplit.TabIndex = 0;
        // 
        // FrmEtiketDesigner
        // 
        ClientSize = new Size(968, 643);
        Controls.Add(designSplit);
        MinimizeBox = false;
        MinimumSize = new Size(720, 680);
        Name = "FrmEtiketDesigner";
        Text = "Label-Designer  (10 × 15 cm)";
        pnlButtons.ResumeLayout(false);
        leftPanel.ResumeLayout(false);
        designSplit.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)designSplit).EndInit();
        designSplit.ResumeLayout(false);
        ResumeLayout(false);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static NumericUpDown Nud(int min, int max) => new()
    {
        Dock    = DockStyle.Fill,
        Minimum = min,
        Maximum = max,
    };

    private static void Row(TableLayoutPanel tbl, string label, Control ctrl)
    {
        tbl.Controls.Add(new Label
        {
            Text      = label,
            Dock      = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin    = new Padding(0, 3, 4, 3),
        });
        ctrl.Margin = new Padding(0, 3, 0, 3);
        tbl.Controls.Add(ctrl);
    }
    private FlowLayoutPanel pnlButtons;
    private TableLayoutPanel tbl;
    private Panel leftPanel;
    private Panel canvasContainer;

    private void BuildPropertiesPanel()
    {
        tbl.RowCount = 0;
        tbl.RowStyles.Clear();

        nudPropX    = Nud(0, 9999);
        nudPropY    = Nud(0, 9999);
        nudPropW    = Nud(20, 9999);
        nudPropH    = Nud(20, 9999);
        nudPropSize = Nud(4, 200);

        Row(tbl, "X",        nudPropX);
        Row(tbl, "Y",        nudPropY);
        Row(tbl, "Breite",   nudPropW);
        Row(tbl, "Höhe",     nudPropH);
        Row(tbl, "Schriftgröße", nudPropSize);

        // Die anderen Controls (cmbPropFont, chkPropBold etc.)
        // wurden in InitializeComponent() bereits erzeugt,
        // müssen aber noch zu pnlPropContent hinzugefügt werden, falls nicht bereits geschehen.
        pnlPropContent.Controls.Add(tbl);
    }
}
