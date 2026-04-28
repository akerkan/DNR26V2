namespace DNR26V2.Forms.Payments;

partial class FrmKundenkonto
{
    private System.ComponentModel.IContainer? components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        panelLeft = new Panel();
        dgwKunden = new DataGridView();
        txtKundeSuche = new TextBox();
        lblKundeSuche = new Label();
        panelFilter = new Panel();
        btnLaden = new Button();
        dtpBis = new DateTimePicker();
        lblBis = new Label();
        dtpVon = new DateTimePicker();
        lblVon = new Label();
        cboKunde = new ComboBox();
        lblKunde = new Label();
        panelSummary = new Panel();
        lblZeitraumValue = new Label();
        lblZeitraum = new Label();
        lblSaldoValue = new Label();
        lblSaldo = new Label();
        lblKundennameValue = new Label();
        lblKundenname = new Label();
        dgwKundenkonto = new DataGridView();
        colDatum = new DataGridViewTextBoxColumn();
        colBelegart = new DataGridViewTextBoxColumn();
        colBelegNr = new DataGridViewTextBoxColumn();
        colBeschreibung = new DataGridViewTextBoxColumn();
        colSoll = new DataGridViewTextBoxColumn();
        colHaben = new DataGridViewTextBoxColumn();
        colSaldo = new DataGridViewTextBoxColumn();
        colNotiz = new DataGridViewTextBoxColumn();
        btnPreview = new Button();
        btnPdf = new Button();
        panelLeft.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgwKunden).BeginInit();
        panelSummary.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgwKundenkonto).BeginInit();
        SuspendLayout();
        // 
        // panelLeft
        // 
        panelLeft.Controls.Add(dgwKunden);
        panelLeft.Controls.Add(txtKundeSuche);
        panelLeft.Controls.Add(lblKundeSuche);
        panelLeft.Dock = DockStyle.Left;
        panelLeft.Location = new Point(0, 0);
        panelLeft.Margin = new Padding(3, 4, 3, 4);
        panelLeft.Name = "panelLeft";
        panelLeft.Size = new Size(491, 753);
        panelLeft.TabIndex = 0;
        // 
        // dgwKunden
        // 
        dgwKunden.AllowUserToAddRows = false;
        dgwKunden.AllowUserToDeleteRows = false;
        dgwKunden.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgwKunden.BackgroundColor = SystemColors.Window;
        dgwKunden.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgwKunden.Dock = DockStyle.Fill;
        dgwKunden.Location = new Point(0, 47);
        dgwKunden.Margin = new Padding(3, 4, 3, 4);
        dgwKunden.MultiSelect = false;
        dgwKunden.Name = "dgwKunden";
        dgwKunden.ReadOnly = true;
        dgwKunden.RowHeadersVisible = false;
        dgwKunden.RowHeadersWidth = 51;
        dgwKunden.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgwKunden.Size = new Size(491, 706);
        dgwKunden.TabIndex = 2;
        // 
        // txtKundeSuche
        // 
        txtKundeSuche.Dock = DockStyle.Top;
        txtKundeSuche.Location = new Point(0, 20);
        txtKundeSuche.Margin = new Padding(3, 4, 3, 4);
        txtKundeSuche.Name = "txtKundeSuche";
        txtKundeSuche.Size = new Size(491, 27);
        txtKundeSuche.TabIndex = 1;
        // 
        // lblKundeSuche
        // 
        lblKundeSuche.AutoSize = true;
        lblKundeSuche.Dock = DockStyle.Top;
        lblKundeSuche.Location = new Point(0, 0);
        lblKundeSuche.Name = "lblKundeSuche";
        lblKundeSuche.Size = new Size(100, 20);
        lblKundeSuche.TabIndex = 0;
        lblKundeSuche.Text = "Kunde suchen";
        // 
        // panelFilter
        // 
        panelFilter.Dock = DockStyle.Top;
        panelFilter.Location = new Point(491, 0);
        panelFilter.Margin = new Padding(3, 4, 3, 4);
        panelFilter.Name = "panelFilter";
        panelFilter.Size = new Size(771, 29);
        panelFilter.TabIndex = 1;
        // 
        // btnLaden
        // 
        btnLaden.Location = new Point(911, 23);
        btnLaden.Name = "btnLaden";
        btnLaden.Size = new Size(96, 27);
        btnLaden.TabIndex = 8;
        btnLaden.Text = "Laden";
        btnLaden.UseVisualStyleBackColor = true;
        // 
        // dtpBis
        // 
        dtpBis.Format = DateTimePickerFormat.Short;
        dtpBis.Location = new Point(786, 25);
        dtpBis.Name = "dtpBis";
        dtpBis.Size = new Size(109, 27);
        dtpBis.TabIndex = 7;
        // 
        // lblBis
        // 
        lblBis.AutoSize = true;
        lblBis.Location = new Point(748, 28);
        lblBis.Name = "lblBis";
        lblBis.Size = new Size(26, 15);
        lblBis.TabIndex = 6;
        lblBis.Text = "Bis:";
        // 
        // dtpVon
        // 
        dtpVon.Format = DateTimePickerFormat.Short;
        dtpVon.Location = new Point(620, 25);
        dtpVon.Name = "dtpVon";
        dtpVon.Size = new Size(109, 27);
        dtpVon.TabIndex = 5;
        // 
        // lblVon
        // 
        lblVon.AutoSize = true;
        lblVon.Location = new Point(583, 28);
        lblVon.Name = "lblVon";
        lblVon.Size = new Size(30, 15);
        lblVon.TabIndex = 4;
        lblVon.Text = "Von:";
        // 
        // cboKunde
        // 
        cboKunde.DropDownStyle = ComboBoxStyle.DropDownList;
        cboKunde.FormattingEnabled = true;
        cboKunde.Location = new Point(251, 25);
        cboKunde.Name = "cboKunde";
        cboKunde.Size = new Size(310, 28);
        cboKunde.TabIndex = 3;
        // 
        // lblKunde
        // 
        lblKunde.AutoSize = true;
        lblKunde.Location = new Point(197, 28);
        lblKunde.Name = "lblKunde";
        lblKunde.Size = new Size(44, 15);
        lblKunde.TabIndex = 2;
        lblKunde.Text = "Kunde:";
        // 
        // panelSummary
        // 
        panelSummary.BackColor = Color.FromArgb(245, 248, 255);
        panelSummary.Controls.Add(lblZeitraumValue);
        panelSummary.Controls.Add(lblZeitraum);
        panelSummary.Controls.Add(lblSaldoValue);
        panelSummary.Controls.Add(lblSaldo);
        panelSummary.Controls.Add(lblKundennameValue);
        panelSummary.Controls.Add(lblKundenname);
        panelSummary.Controls.Add(btnPreview);
        panelSummary.Controls.Add(btnPdf);
        panelSummary.Dock = DockStyle.Top;
        panelSummary.Location = new Point(491, 29);
        panelSummary.Margin = new Padding(3, 4, 3, 4);
        panelSummary.Name = "panelSummary";
        panelSummary.Size = new Size(771, 92);
        panelSummary.TabIndex = 1;
        // 
        // lblZeitraumValue
        // 
        lblZeitraumValue.AutoSize = true;
        lblZeitraumValue.Location = new Point(594, 57);
        lblZeitraumValue.Name = "lblZeitraumValue";
        lblZeitraumValue.Size = new Size(15, 20);
        lblZeitraumValue.TabIndex = 5;
        lblZeitraumValue.Text = "-";
        // 
        // lblZeitraum
        // 
        lblZeitraum.AutoSize = true;
        lblZeitraum.Location = new Point(666, 24);
        lblZeitraum.Name = "lblZeitraum";
        lblZeitraum.Size = new Size(69, 20);
        lblZeitraum.TabIndex = 4;
        lblZeitraum.Text = "Zeitraum";
        // 
        // lblSaldoValue
        // 
        lblSaldoValue.AutoSize = true;
        lblSaldoValue.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblSaldoValue.Location = new Point(441, 39);
        lblSaldoValue.Name = "lblSaldoValue";
        lblSaldoValue.Size = new Size(84, 32);
        lblSaldoValue.TabIndex = 3;
        lblSaldoValue.Text = "0,00 €";
        // 
        // lblSaldo
        // 
        lblSaldo.AutoSize = true;
        lblSaldo.Location = new Point(441, 13);
        lblSaldo.Name = "lblSaldo";
        lblSaldo.Size = new Size(47, 20);
        lblSaldo.TabIndex = 2;
        lblSaldo.Text = "Saldo";
        // 
        // lblKundennameValue
        // 
        lblKundennameValue.AutoSize = true;
        lblKundennameValue.Location = new Point(14, 57);
        lblKundennameValue.Name = "lblKundennameValue";
        lblKundennameValue.Size = new Size(15, 20);
        lblKundennameValue.TabIndex = 1;
        lblKundennameValue.Text = "-";
        // 
        // lblKundenname
        // 
        lblKundenname.AutoSize = true;
        lblKundenname.Location = new Point(14, 24);
        lblKundenname.Name = "lblKundenname";
        lblKundenname.Size = new Size(96, 20);
        lblKundenname.TabIndex = 0;
        lblKundenname.Text = "Kundenname";
        // 
        // dgwKundenkonto
        // 
        dgwKundenkonto.AllowUserToAddRows = false;
        dgwKundenkonto.AllowUserToDeleteRows = false;
        dgwKundenkonto.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgwKundenkonto.BackgroundColor = SystemColors.Window;
        dgwKundenkonto.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgwKundenkonto.Columns.AddRange(new DataGridViewColumn[] { colDatum, colBelegart, colBelegNr, colBeschreibung, colSoll, colHaben, colSaldo, colNotiz });
        dgwKundenkonto.Dock = DockStyle.Fill;
        dgwKundenkonto.Location = new Point(491, 121);
        dgwKundenkonto.Margin = new Padding(3, 4, 3, 4);
        dgwKundenkonto.MultiSelect = false;
        dgwKundenkonto.Name = "dgwKundenkonto";
        dgwKundenkonto.ReadOnly = true;
        dgwKundenkonto.RowHeadersVisible = false;
        dgwKundenkonto.RowHeadersWidth = 51;
        dgwKundenkonto.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgwKundenkonto.Size = new Size(771, 632);
        dgwKundenkonto.TabIndex = 2;
        // 
        // colDatum
        // 
        colDatum.DataPropertyName = "Datum";
        colDatum.FillWeight = 70F;
        colDatum.HeaderText = "Datum";
        colDatum.MinimumWidth = 6;
        colDatum.Name = "colDatum";
        colDatum.ReadOnly = true;
        // 
        // colBelegart
        // 
        colBelegart.DataPropertyName = "Belegart";
        colBelegart.FillWeight = 85F;
        colBelegart.HeaderText = "Belegart";
        colBelegart.MinimumWidth = 6;
        colBelegart.Name = "colBelegart";
        colBelegart.ReadOnly = true;
        // 
        // colBelegNr
        // 
        colBelegNr.DataPropertyName = "BelegNr";
        colBelegNr.FillWeight = 90F;
        colBelegNr.HeaderText = "Beleg-Nr.";
        colBelegNr.MinimumWidth = 6;
        colBelegNr.Name = "colBelegNr";
        colBelegNr.ReadOnly = true;
        // 
        // colBeschreibung
        // 
        colBeschreibung.DataPropertyName = "Beschreibung";
        colBeschreibung.FillWeight = 170F;
        colBeschreibung.HeaderText = "Beschreibung";
        colBeschreibung.MinimumWidth = 6;
        colBeschreibung.Name = "colBeschreibung";
        colBeschreibung.ReadOnly = true;
        // 
        // colSoll
        // 
        colSoll.DataPropertyName = "Soll";
        colSoll.FillWeight = 80F;
        colSoll.HeaderText = "Soll";
        colSoll.MinimumWidth = 6;
        colSoll.Name = "colSoll";
        colSoll.ReadOnly = true;
        // 
        // colHaben
        // 
        colHaben.DataPropertyName = "Haben";
        colHaben.FillWeight = 80F;
        colHaben.HeaderText = "Haben";
        colHaben.MinimumWidth = 6;
        colHaben.Name = "colHaben";
        colHaben.ReadOnly = true;
        // 
        // colSaldo
        // 
        colSaldo.DataPropertyName = "Saldo";
        colSaldo.FillWeight = 90F;
        colSaldo.HeaderText = "Saldo";
        colSaldo.MinimumWidth = 6;
        colSaldo.Name = "colSaldo";
        colSaldo.ReadOnly = true;
        // 
        // colNotiz
        // 
        colNotiz.DataPropertyName = "Notiz";
        colNotiz.FillWeight = 130F;
        colNotiz.HeaderText = "Notiz";
        colNotiz.MinimumWidth = 6;
        colNotiz.Name = "colNotiz";
        colNotiz.ReadOnly = true;
        // 
        // btnPreview
        // 
        btnPreview.Location = new Point(200, 13);
        btnPreview.Name = "btnPreview";
        btnPreview.Size = new Size(120, 32);
        btnPreview.TabIndex = 10;
        btnPreview.Text = "Vorschau / Druck";
        btnPreview.UseVisualStyleBackColor = true;
        // 
        // btnPdf
        // 
        btnPdf.Location = new Point(330, 13);
        btnPdf.Name = "btnPdf";
        btnPdf.Size = new Size(80, 32);
        btnPdf.TabIndex = 11;
        btnPdf.Text = "PDF";
        btnPdf.UseVisualStyleBackColor = true;
        // 
        // FrmKundenkonto
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1262, 753);
        Controls.Add(dgwKundenkonto);
        Controls.Add(panelSummary);
        Controls.Add(panelFilter);
        Controls.Add(panelLeft);
        Margin = new Padding(3, 4, 3, 4);
        Name = "FrmKundenkonto";
        Text = "Kundenkonto";
        panelLeft.ResumeLayout(false);
        panelLeft.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgwKunden).EndInit();
        panelSummary.ResumeLayout(false);
        panelSummary.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgwKundenkonto).EndInit();
        ResumeLayout(false);
    }

    private Panel panelLeft;
    private TextBox txtKundeSuche;
    private Label lblKundeSuche;
    private DataGridView dgwKunden;
    private DataGridViewTextBoxColumn colKundenNr;
    private DataGridViewTextBoxColumn colKundenName;
    private Panel panelFilter;
    private Button btnLaden;
    private DateTimePicker dtpBis;
    private Label lblBis;
    private DateTimePicker dtpVon;
    private Label lblVon;
    private ComboBox cboKunde;
    private Label lblKunde;
    private Panel panelSummary;
    private Label lblZeitraumValue;
    private Label lblZeitraum;
    private Label lblSaldoValue;
    private Label lblSaldo;
    private Label lblKundennameValue;
    private Label lblKundenname;
    private DataGridView dgwKundenkonto;
    private DataGridViewTextBoxColumn colDatum;
    private DataGridViewTextBoxColumn colBelegart;
    private DataGridViewTextBoxColumn colBelegNr;
    private DataGridViewTextBoxColumn colBeschreibung;
    private DataGridViewTextBoxColumn colSoll;
    private DataGridViewTextBoxColumn colHaben;
    private DataGridViewTextBoxColumn colSaldo;
    private DataGridViewTextBoxColumn colNotiz;
    private Button btnPreview;
    private Button btnPdf;
}
