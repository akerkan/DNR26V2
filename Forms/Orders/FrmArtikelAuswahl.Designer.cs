namespace DNR26V2.Forms.Orders;

partial class FrmArtikelAuswahl
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        pnlTop    = new Panel();
        lblSuche  = new Label();
        txtSuche  = new TextBox();
        pnlBottom = new Panel();
        btnOk     = new Button();
        btnAbbrechen = new Button();
        dgwArtikel   = new DataGridView();
        colArtId     = new DataGridViewTextBoxColumn();
        colArtNr     = new DataGridViewTextBoxColumn();
        colProduktname = new DataGridViewTextBoxColumn();
        colVKPreis   = new DataGridViewTextBoxColumn();
        pnlTop.SuspendLayout();
        pnlBottom.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgwArtikel).BeginInit();
        SuspendLayout();
        //
        // pnlTop
        //
        pnlTop.Controls.Add(txtSuche);
        pnlTop.Controls.Add(lblSuche);
        pnlTop.Dock = DockStyle.Top;
        pnlTop.Location = new Point(0, 0);
        pnlTop.Name = "pnlTop";
        pnlTop.Padding = new Padding(8, 8, 8, 4);
        pnlTop.Size = new Size(600, 44);
        pnlTop.TabIndex = 0;
        //
        // lblSuche
        //
        lblSuche.AutoSize = true;
        lblSuche.Location = new Point(8, 14);
        lblSuche.Name = "lblSuche";
        lblSuche.Size = new Size(44, 15);
        lblSuche.TabIndex = 0;
        lblSuche.Text = "Suche:";
        //
        // txtSuche
        //
        txtSuche.Location = new Point(60, 10);
        txtSuche.Name = "txtSuche";
        txtSuche.PlaceholderText = "Artikel-Nr. oder Bezeichnung...";
        txtSuche.Size = new Size(530, 23);
        txtSuche.TabIndex = 1;
        //
        // pnlBottom
        //
        pnlBottom.Controls.Add(btnAbbrechen);
        pnlBottom.Controls.Add(btnOk);
        pnlBottom.Dock = DockStyle.Bottom;
        pnlBottom.Location = new Point(0, 406);
        pnlBottom.Name = "pnlBottom";
        pnlBottom.Padding = new Padding(6, 8, 6, 8);
        pnlBottom.Size = new Size(600, 44);
        pnlBottom.TabIndex = 1;
        //
        // btnOk
        //
        btnOk.BackColor = Color.SteelBlue;
        btnOk.DialogResult = DialogResult.OK;
        btnOk.FlatStyle = FlatStyle.Flat;
        btnOk.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnOk.ForeColor = Color.White;
        btnOk.Location = new Point(6, 8);
        btnOk.Name = "btnOk";
        btnOk.Size = new Size(120, 28);
        btnOk.TabIndex = 0;
        btnOk.Text = "? Hinzufügen";
        btnOk.UseVisualStyleBackColor = false;
        //
        // btnAbbrechen
        //
        btnAbbrechen.DialogResult = DialogResult.Cancel;
        btnAbbrechen.Location = new Point(132, 8);
        btnAbbrechen.Name = "btnAbbrechen";
        btnAbbrechen.Size = new Size(100, 28);
        btnAbbrechen.TabIndex = 1;
        btnAbbrechen.Text = "Abbrechen";
        //
        // dgwArtikel
        //
        dgwArtikel.AllowUserToAddRows = false;
        dgwArtikel.AllowUserToDeleteRows = false;
        dgwArtikel.AutoGenerateColumns = false;
        dgwArtikel.BackgroundColor = SystemColors.Window;
        dgwArtikel.BorderStyle = BorderStyle.Fixed3D;
        dgwArtikel.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgwArtikel.Columns.AddRange(colArtId, colArtNr, colProduktname, colVKPreis);
        dgwArtikel.Dock = DockStyle.Fill;
        dgwArtikel.Location = new Point(0, 44);
        dgwArtikel.MultiSelect = false;
        dgwArtikel.Name = "dgwArtikel";
        dgwArtikel.ReadOnly = true;
        dgwArtikel.RowHeadersVisible = false;
        dgwArtikel.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgwArtikel.Size = new Size(600, 362);
        dgwArtikel.TabIndex = 2;
        //
        // colArtId
        //
        colArtId.Name = "_ArtikelId";
        colArtId.Visible = false;
        //
        // colArtNr
        //
        colArtNr.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colArtNr.HeaderText = "Art.-Nr.";
        colArtNr.Name = "Artikelnummer";
        colArtNr.Width = 100;
        //
        // colProduktname
        //
        colProduktname.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colProduktname.HeaderText = "Bezeichnung";
        colProduktname.Name = "Produktname";
        //
        // colVKPreis
        //
        colVKPreis.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colVKPreis.DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight };
        colVKPreis.HeaderText = "VK-Preis";
        colVKPreis.Name = "VKPreis";
        colVKPreis.Width = 90;
        //
        // FrmArtikelAuswahl
        //
        AcceptButton = btnOk;
        CancelButton = btnAbbrechen;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(600, 450);
        Controls.Add(dgwArtikel);
        Controls.Add(pnlBottom);
        Controls.Add(pnlTop);
        Font = new Font("Segoe UI", 9F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FrmArtikelAuswahl";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Artikel hinzufügen";
        pnlTop.ResumeLayout(false);
        pnlTop.PerformLayout();
        pnlBottom.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgwArtikel).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlTop;
    private Label lblSuche;
    private TextBox txtSuche;
    private Panel pnlBottom;
    private Button btnOk;
    private Button btnAbbrechen;
    private DataGridView dgwArtikel;
    private DataGridViewTextBoxColumn colArtId;
    private DataGridViewTextBoxColumn colArtNr;
    private DataGridViewTextBoxColumn colProduktname;
    private DataGridViewTextBoxColumn colVKPreis;
}