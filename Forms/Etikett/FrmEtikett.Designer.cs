namespace DNR26V2.Forms.Etikett;

partial class FrmEtikett
{
    private System.ComponentModel.IContainer components = null!;

    // ── Left panel ────────────────────────────────────────────────────────────
    private TextBox                   txtKundenSearch  = null!;
    private DataGridView              dgwKunden        = null!;
    private DataGridViewTextBoxColumn colKdName        = null!;
    private DataGridViewTextBoxColumn colKdTur         = null!;

    // ── Right top bar ─────────────────────────────────────────────────────────
    private DateTimePicker dtpLieferDatum   = null!;
    private DateTimePicker dtpHerstellDatum = null!;
    private NumericUpDown  nudKopien        = null!;

    // ── Right products grid ───────────────────────────────────────────────────
    private DataGridView              dgwProdukte      = null!;
    private DataGridViewTextBoxColumn colArtikelnummer = null!;
    private DataGridViewTextBoxColumn colProduktname   = null!;
    private DataGridViewTextBoxColumn colMenge         = null!;
    private DataGridViewTextBoxColumn colGewicht       = null!;
    private DataGridViewButtonColumn  colEtikett       = null!;

    // ── Layout containers ─────────────────────────────────────────────────────
    private SplitContainer split     = null!;
    private Panel          leftPanel = null!;
    private Panel          pnlDates  = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        txtKundenSearch = new TextBox();
        dgwKunden = new DataGridView();
        leftPanel = new Panel();
        dtpLieferDatum = new DateTimePicker();
        dtpHerstellDatum = new DateTimePicker();
        nudKopien = new NumericUpDown();
        pnlDates = new Panel();
        lblLiefer = new Label();
        lblHerstell = new Label();
        lblKopien = new Label();
        colArtikelnummer = new DataGridViewTextBoxColumn();
        colProduktname = new DataGridViewTextBoxColumn();
        colMenge = new DataGridViewTextBoxColumn();
        colGewicht = new DataGridViewTextBoxColumn();
        colEtikett = new DataGridViewButtonColumn();
        dgwProdukte = new DataGridView();
        split = new SplitContainer();
        ((System.ComponentModel.ISupportInitialize)dgwKunden).BeginInit();
        leftPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudKopien).BeginInit();
        pnlDates.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgwProdukte).BeginInit();
        ((System.ComponentModel.ISupportInitialize)split).BeginInit();
        split.Panel1.SuspendLayout();
        split.Panel2.SuspendLayout();
        split.SuspendLayout();
        SuspendLayout();
        // 
        // txtKundenSearch
        // 
        txtKundenSearch.Dock = DockStyle.Top;
        txtKundenSearch.Location = new Point(0, 0);
        txtKundenSearch.Name = "txtKundenSearch";
        txtKundenSearch.PlaceholderText = "Suche Kunde …";
        txtKundenSearch.Size = new Size(392, 23);
        txtKundenSearch.TabIndex = 0;
        // 
        // dgwKunden
        // 
        dgwKunden.ColumnHeadersHeight = 29;
        dgwKunden.Dock = DockStyle.Fill;
        dgwKunden.Location = new Point(0, 23);
        dgwKunden.Name = "dgwKunden";
        dgwKunden.RowHeadersWidth = 51;
        dgwKunden.Size = new Size(392, 657);
        dgwKunden.TabIndex = 1;
        // 
        // leftPanel
        // 
        leftPanel.Controls.Add(dgwKunden);
        leftPanel.Controls.Add(txtKundenSearch);
        leftPanel.Dock = DockStyle.Fill;
        leftPanel.Location = new Point(0, 0);
        leftPanel.Name = "leftPanel";
        leftPanel.Size = new Size(392, 680);
        leftPanel.TabIndex = 0;
        // 
        // dtpLieferDatum
        // 
        dtpLieferDatum.Font = new Font("Segoe UI", 13F);
        dtpLieferDatum.Format = DateTimePickerFormat.Short;
        dtpLieferDatum.Location = new Point(8, 28);
        dtpLieferDatum.Name = "dtpLieferDatum";
        dtpLieferDatum.Size = new Size(150, 31);
        dtpLieferDatum.TabIndex = 0;
        dtpLieferDatum.Value = new DateTime(2026, 5, 3, 0, 0, 0, 0);
        // 
        // dtpHerstellDatum
        // 
        dtpHerstellDatum.Font = new Font("Segoe UI", 13F);
        dtpHerstellDatum.Format = DateTimePickerFormat.Short;
        dtpHerstellDatum.Location = new Point(175, 28);
        dtpHerstellDatum.Name = "dtpHerstellDatum";
        dtpHerstellDatum.Size = new Size(150, 31);
        dtpHerstellDatum.TabIndex = 1;
        dtpHerstellDatum.Value = new DateTime(2026, 5, 3, 0, 0, 0, 0);
        // 
        // nudKopien
        // 
        nudKopien.Location = new Point(345, 30);
        nudKopien.Maximum = new decimal(new int[] { 99, 0, 0, 0 });
        nudKopien.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudKopien.Name = "nudKopien";
        nudKopien.Size = new Size(55, 23);
        nudKopien.TabIndex = 2;
        nudKopien.Value = new decimal(new int[] { 1, 0, 0, 0 });
        nudKopien.Visible = false;
        // 
        // pnlDates
        // 
        pnlDates.BackColor = Color.WhiteSmoke;
        pnlDates.Controls.Add(lblLiefer);
        pnlDates.Controls.Add(dtpLieferDatum);
        pnlDates.Controls.Add(lblHerstell);
        pnlDates.Controls.Add(dtpHerstellDatum);
        pnlDates.Controls.Add(lblKopien);
        pnlDates.Controls.Add(nudKopien);
        pnlDates.Dock = DockStyle.Top;
        pnlDates.Location = new Point(0, 0);
        pnlDates.Name = "pnlDates";
        pnlDates.Padding = new Padding(8);
        pnlDates.Size = new Size(998, 84);
        pnlDates.TabIndex = 1;
        // 
        // lblLiefer
        // 
        lblLiefer.Location = new Point(8, 6);
        lblLiefer.Name = "lblLiefer";
        lblLiefer.Size = new Size(100, 19);
        lblLiefer.TabIndex = 0;
        lblLiefer.Text = "Lieferungsdatum";
        // 
        // lblHerstell
        // 
        lblHerstell.Location = new Point(175, 6);
        lblHerstell.Name = "lblHerstell";
        lblHerstell.Size = new Size(125, 17);
        lblHerstell.TabIndex = 1;
        lblHerstell.Text = "Herstellungsdatum";
        // 
        // lblKopien
        // 
        lblKopien.Location = new Point(0, 0);
        lblKopien.Name = "lblKopien";
        lblKopien.Size = new Size(100, 23);
        lblKopien.TabIndex = 2;
        // 
        // colArtikelnummer
        // 
        colArtikelnummer.HeaderText = "Artikelnummer";
        colArtikelnummer.MinimumWidth = 8;
        colArtikelnummer.Name = "colArtikelnummer";
        colArtikelnummer.ReadOnly = true;
        colArtikelnummer.Width = 150;
        // 
        // colProduktname
        // 
        colProduktname.HeaderText = "Produktname";
        colProduktname.MinimumWidth = 8;
        colProduktname.Name = "colProduktname";
        colProduktname.ReadOnly = true;
        colProduktname.Width = 150;
        // 
        // colMenge
        // 
        colMenge.HeaderText = "Menge";
        colMenge.Name = "colMenge";
        colMenge.Width = 80;
        // 
        // colGewicht
        // 
        colGewicht.HeaderText = "Gewicht";
        colGewicht.Name = "colGewicht";
        colGewicht.Width = 80;
        // 
        // colEtikett
        // 
        colEtikett.HeaderText = "Etikett";
        colEtikett.MinimumWidth = 60;
        colEtikett.Name = "colEtikett";
        colEtikett.ReadOnly = true;
        colEtikett.Text = "Drucken";
        colEtikett.UseColumnTextForButtonValue = true;
        colEtikett.Width = 72;
        // 
        // dgwProdukte
        // 
        dgwProdukte.ColumnHeadersHeight = 29;
        dgwProdukte.Columns.AddRange(new DataGridViewColumn[] { colArtikelnummer, colProduktname, colMenge, colGewicht, colEtikett });
        dgwProdukte.Dock = DockStyle.Fill;
        dgwProdukte.Location = new Point(0, 84);
        dgwProdukte.Name = "dgwProdukte";
        dgwProdukte.RowHeadersWidth = 51;
        dgwProdukte.Size = new Size(998, 596);
        dgwProdukte.TabIndex = 0;
        // 
        // split
        // 
        split.Dock = DockStyle.Fill;
        split.Location = new Point(0, 0);
        split.Name = "split";
        // 
        // split.Panel1
        // 
        split.Panel1.Controls.Add(leftPanel);
        split.Panel1MinSize = 200;
        // 
        // split.Panel2
        // 
        split.Panel2.Controls.Add(dgwProdukte);
        split.Panel2.Controls.Add(pnlDates);
        split.Panel2MinSize = 450;
        split.Size = new Size(1394, 680);
        split.SplitterDistance = 392;
        split.TabIndex = 0;
        // 
        // FrmEtikett
        // 
        ClientSize = new Size(1394, 680);
        Controls.Add(split);
        MinimumSize = new Size(900, 560);
        Name = "FrmEtikett";
        Text = "Etikett erstellen";
        ((System.ComponentModel.ISupportInitialize)dgwKunden).EndInit();
        leftPanel.ResumeLayout(false);
        leftPanel.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudKopien).EndInit();
        pnlDates.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgwProdukte).EndInit();
        split.Panel1.ResumeLayout(false);
        split.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)split).EndInit();
        split.ResumeLayout(false);
        ResumeLayout(false);
    }
    private Label lblLiefer;
    private Label lblHerstell;
    private Label lblKopien;
}
