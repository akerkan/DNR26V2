namespace DNR26V2.Forms.MasterData;

partial class FrmCustomerProductTemplate
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
        splitMain = new SplitContainer();
        pnlLinks = new Panel();
        dgwKunden = new DataGridView();
        txtSuche = new TextBox();
        lblKunden = new Label();
        splitRechts = new SplitContainer();
        pnlAlleProdukte = new Panel();
        dgwAlleProdukte = new DataGridView();
        lblAlleProdukte = new Label();
        pnlKundenprodukte = new Panel();
        dgwKundenprodukte = new DataGridView();
        pnlKundenprodukteFuss = new Panel();
        btnSpeichern = new Button();
        lblKundenprodukte = new Label();
        ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
        splitMain.Panel1.SuspendLayout();
        splitMain.Panel2.SuspendLayout();
        splitMain.SuspendLayout();
        pnlLinks.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgwKunden).BeginInit();
        ((System.ComponentModel.ISupportInitialize)splitRechts).BeginInit();
        splitRechts.Panel1.SuspendLayout();
        splitRechts.Panel2.SuspendLayout();
        splitRechts.SuspendLayout();
        pnlAlleProdukte.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgwAlleProdukte).BeginInit();
        pnlKundenprodukte.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgwKundenprodukte).BeginInit();
        pnlKundenprodukteFuss.SuspendLayout();
        SuspendLayout();
        // 
        // splitMain
        // 
        splitMain.Dock = DockStyle.Fill;
        splitMain.FixedPanel = FixedPanel.Panel1;
        splitMain.Location = new Point(0, 0);
        splitMain.Name = "splitMain";
        // 
        // splitMain.Panel1
        // 
        splitMain.Panel1.Controls.Add(pnlLinks);
        // 
        // splitMain.Panel2
        // 
        splitMain.Panel2.Controls.Add(splitRechts);
        splitMain.Size = new Size(1200, 700);
        splitMain.SplitterDistance = 428;
        splitMain.TabIndex = 0;
        // 
        // pnlLinks
        // 
        pnlLinks.Controls.Add(dgwKunden);
        pnlLinks.Controls.Add(txtSuche);
        pnlLinks.Controls.Add(lblKunden);
        pnlLinks.Dock = DockStyle.Fill;
        pnlLinks.Location = new Point(0, 0);
        pnlLinks.Name = "pnlLinks";
        pnlLinks.Padding = new Padding(4);
        pnlLinks.Size = new Size(428, 700);
        pnlLinks.TabIndex = 0;
        // 
        // dgwKunden
        // 
        dgwKunden.AllowUserToAddRows = false;
        dgwKunden.AllowUserToDeleteRows = false;
        dgwKunden.AllowUserToOrderColumns = true;
        dataGridViewCellStyle1.BackColor = Color.FromArgb(245, 245, 250);
        dgwKunden.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
        dgwKunden.BackgroundColor = SystemColors.Window;
        dgwKunden.BorderStyle = BorderStyle.Fixed3D;
        dgwKunden.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgwKunden.Dock = DockStyle.Fill;
        dgwKunden.Location = new Point(4, 49);
        dgwKunden.MultiSelect = false;
        dgwKunden.Name = "dgwKunden";
        dgwKunden.ReadOnly = true;
        dgwKunden.RowHeadersVisible = false;
        dgwKunden.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgwKunden.Size = new Size(420, 647);
        dgwKunden.TabIndex = 1;
        // 
        // txtSuche
        // 
        txtSuche.Dock = DockStyle.Top;
        txtSuche.Font = new Font("Segoe UI", 9F);
        txtSuche.Location = new Point(4, 26);
        txtSuche.Name = "txtSuche";
        txtSuche.PlaceholderText = "Suche...";
        txtSuche.Size = new Size(420, 23);
        txtSuche.TabIndex = 0;
        // 
        // lblKunden
        // 
        lblKunden.Dock = DockStyle.Top;
        lblKunden.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblKunden.Location = new Point(4, 4);
        lblKunden.Name = "lblKunden";
        lblKunden.Size = new Size(420, 22);
        lblKunden.TabIndex = 2;
        lblKunden.Text = "Kunden";
        lblKunden.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // splitRechts
        // 
        splitRechts.Dock = DockStyle.Fill;
        splitRechts.FixedPanel = FixedPanel.Panel1;
        splitRechts.Location = new Point(0, 0);
        splitRechts.Name = "splitRechts";
        splitRechts.Orientation = Orientation.Horizontal;
        // 
        // splitRechts.Panel1
        // 
        splitRechts.Panel1.Controls.Add(pnlAlleProdukte);
        // 
        // splitRechts.Panel2
        // 
        splitRechts.Panel2.Controls.Add(pnlKundenprodukte);
        splitRechts.Size = new Size(768, 700);
        splitRechts.SplitterDistance = 317;
        splitRechts.TabIndex = 0;
        // 
        // pnlAlleProdukte
        // 
        pnlAlleProdukte.Controls.Add(dgwAlleProdukte);
        pnlAlleProdukte.Controls.Add(lblAlleProdukte);
        pnlAlleProdukte.Dock = DockStyle.Fill;
        pnlAlleProdukte.Location = new Point(0, 0);
        pnlAlleProdukte.Name = "pnlAlleProdukte";
        pnlAlleProdukte.Padding = new Padding(4);
        pnlAlleProdukte.Size = new Size(768, 317);
        pnlAlleProdukte.TabIndex = 0;
        // 
        // dgwAlleProdukte
        // 
        dgwAlleProdukte.AllowUserToAddRows = false;
        dgwAlleProdukte.AllowUserToDeleteRows = false;
        dataGridViewCellStyle2.BackColor = Color.FromArgb(245, 245, 250);
        dgwAlleProdukte.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
        dgwAlleProdukte.BackgroundColor = SystemColors.Window;
        dgwAlleProdukte.BorderStyle = BorderStyle.Fixed3D;
        dgwAlleProdukte.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgwAlleProdukte.Dock = DockStyle.Fill;
        dgwAlleProdukte.Location = new Point(4, 26);
        dgwAlleProdukte.MultiSelect = false;
        dgwAlleProdukte.Name = "dgwAlleProdukte";
        dgwAlleProdukte.ReadOnly = true;
        dgwAlleProdukte.RowHeadersVisible = false;
        dgwAlleProdukte.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgwAlleProdukte.Size = new Size(760, 287);
        dgwAlleProdukte.TabIndex = 0;
        // 
        // lblAlleProdukte
        // 
        lblAlleProdukte.Dock = DockStyle.Top;
        lblAlleProdukte.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblAlleProdukte.Location = new Point(4, 4);
        lblAlleProdukte.Name = "lblAlleProdukte";
        lblAlleProdukte.Size = new Size(760, 22);
        lblAlleProdukte.TabIndex = 1;
        lblAlleProdukte.Text = "Alle Produkte";
        lblAlleProdukte.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // pnlKundenprodukte
        // 
        pnlKundenprodukte.Controls.Add(dgwKundenprodukte);
        pnlKundenprodukte.Controls.Add(pnlKundenprodukteFuss);
        pnlKundenprodukte.Controls.Add(lblKundenprodukte);
        pnlKundenprodukte.Dock = DockStyle.Fill;
        pnlKundenprodukte.Location = new Point(0, 0);
        pnlKundenprodukte.Name = "pnlKundenprodukte";
        pnlKundenprodukte.Padding = new Padding(4);
        pnlKundenprodukte.Size = new Size(768, 379);
        pnlKundenprodukte.TabIndex = 0;
        // 
        // dgwKundenprodukte
        // 
        dgwKundenprodukte.AllowUserToAddRows = false;
        dgwKundenprodukte.AllowUserToDeleteRows = false;
        dgwKundenprodukte.AllowUserToOrderColumns = true;
        dataGridViewCellStyle3.BackColor = Color.FromArgb(245, 245, 250);
        dgwKundenprodukte.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
        dgwKundenprodukte.BackgroundColor = SystemColors.Window;
        dgwKundenprodukte.BorderStyle = BorderStyle.Fixed3D;
        dgwKundenprodukte.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgwKundenprodukte.Dock = DockStyle.Fill;
        dgwKundenprodukte.Location = new Point(4, 26);
        dgwKundenprodukte.MultiSelect = false;
        dgwKundenprodukte.Name = "dgwKundenprodukte";
        dgwKundenprodukte.RowHeadersVisible = false;
        dgwKundenprodukte.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgwKundenprodukte.Size = new Size(760, 311);
        dgwKundenprodukte.TabIndex = 0;
        // 
        // pnlKundenprodukteFuss
        // 
        pnlKundenprodukteFuss.Controls.Add(btnSpeichern);
        pnlKundenprodukteFuss.Dock = DockStyle.Bottom;
        pnlKundenprodukteFuss.Location = new Point(4, 337);
        pnlKundenprodukteFuss.Name = "pnlKundenprodukteFuss";
        pnlKundenprodukteFuss.Padding = new Padding(4);
        pnlKundenprodukteFuss.Size = new Size(760, 38);
        pnlKundenprodukteFuss.TabIndex = 1;
        // 
        // btnSpeichern
        // 
        btnSpeichern.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnSpeichern.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnSpeichern.Location = new Point(4, 4);
        btnSpeichern.Name = "btnSpeichern";
        btnSpeichern.Size = new Size(120, 28);
        btnSpeichern.TabIndex = 0;
        btnSpeichern.Text = "Speichern";
        btnSpeichern.UseVisualStyleBackColor = true;
        // 
        // lblKundenprodukte
        // 
        lblKundenprodukte.Dock = DockStyle.Top;
        lblKundenprodukte.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblKundenprodukte.Location = new Point(4, 4);
        lblKundenprodukte.Name = "lblKundenprodukte";
        lblKundenprodukte.Size = new Size(760, 22);
        lblKundenprodukte.TabIndex = 2;
        lblKundenprodukte.Text = "Kundenprodukte";
        lblKundenprodukte.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // FrmCustomerProductTemplate
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1200, 700);
        Controls.Add(splitMain);
        Font = new Font("Segoe UI", 9F);
        Name = "FrmCustomerProductTemplate";
        Text = "Kunden- / Artikelvorlage";
        splitMain.Panel1.ResumeLayout(false);
        splitMain.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
        splitMain.ResumeLayout(false);
        pnlLinks.ResumeLayout(false);
        pnlLinks.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgwKunden).EndInit();
        splitRechts.Panel1.ResumeLayout(false);
        splitRechts.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitRechts).EndInit();
        splitRechts.ResumeLayout(false);
        pnlAlleProdukte.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgwAlleProdukte).EndInit();
        pnlKundenprodukte.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgwKundenprodukte).EndInit();
        pnlKundenprodukteFuss.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private SplitContainer        splitMain;
    private Panel                  pnlLinks;
    private Label                  lblKunden;
    private TextBox                txtSuche;
    private DataGridView           dgwKunden;
    private SplitContainer        splitRechts;
    private Panel                  pnlAlleProdukte;
    private Label                  lblAlleProdukte;
    private DataGridView           dgwAlleProdukte;
    private Panel                  pnlKundenprodukte;
    private Panel                  pnlKundenprodukteFuss;
    private Label                  lblKundenprodukte;
    private DataGridView           dgwKundenprodukte;
    private Button                 btnSpeichern;
}