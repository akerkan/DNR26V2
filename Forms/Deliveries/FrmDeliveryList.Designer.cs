namespace DNR26V2.Forms.Deliveries;

partial class FrmDeliveryList
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        pnlFilter        = new System.Windows.Forms.Panel();
        lblVon           = new System.Windows.Forms.Label();
        dtpVon           = new System.Windows.Forms.DateTimePicker();
        lblBis           = new System.Windows.Forms.Label();
        dtpBis           = new System.Windows.Forms.DateTimePicker();
        lblKunde         = new System.Windows.Forms.Label();
        txtKunde         = new System.Windows.Forms.TextBox();
        lblStatus        = new System.Windows.Forms.Label();
        cmbStatus        = new System.Windows.Forms.ComboBox();
        btnSuchen        = new System.Windows.Forms.Button();
        pnlToolbar       = new System.Windows.Forms.Panel();
        btnAbschliessen  = new System.Windows.Forms.Button();
        btnStornieren    = new System.Windows.Forms.Button();
        btnAlleMarkieren = new System.Windows.Forms.Button();
        dgwLieferscheine = new System.Windows.Forms.DataGridView();
        colLsChecked     = new System.Windows.Forms.DataGridViewCheckBoxColumn();

        pnlFilter.SuspendLayout();
        pnlToolbar.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgwLieferscheine).BeginInit();
        SuspendLayout();

        // pnlFilter
        pnlFilter.Controls.Add(lblVon);
        pnlFilter.Controls.Add(dtpVon);
        pnlFilter.Controls.Add(lblBis);
        pnlFilter.Controls.Add(dtpBis);
        pnlFilter.Controls.Add(lblKunde);
        pnlFilter.Controls.Add(txtKunde);
        pnlFilter.Controls.Add(lblStatus);
        pnlFilter.Controls.Add(cmbStatus);
        pnlFilter.Controls.Add(btnSuchen);
        pnlFilter.Dock     = System.Windows.Forms.DockStyle.Top;
        pnlFilter.Height   = 44;
        pnlFilter.Name     = "pnlFilter";
        pnlFilter.Padding  = new System.Windows.Forms.Padding(4, 0, 4, 0);
        pnlFilter.TabIndex = 0;

        lblVon.AutoSize = true;
        lblVon.Location = new System.Drawing.Point(6, 13);
        lblVon.Name     = "lblVon";
        lblVon.Text     = "Datum Von:";
        lblVon.TabIndex = 0;

        dtpVon.Format   = System.Windows.Forms.DateTimePickerFormat.Short;
        dtpVon.Location = new System.Drawing.Point(76, 10);
        dtpVon.Name     = "dtpVon";
        dtpVon.Size     = new System.Drawing.Size(105, 23);
        dtpVon.TabIndex = 1;

        lblBis.AutoSize = true;
        lblBis.Location = new System.Drawing.Point(188, 13);
        lblBis.Name     = "lblBis";
        lblBis.Text     = "Datum Bis:";
        lblBis.TabIndex = 2;

        dtpBis.Format   = System.Windows.Forms.DateTimePickerFormat.Short;
        dtpBis.Location = new System.Drawing.Point(258, 10);
        dtpBis.Name     = "dtpBis";
        dtpBis.Size     = new System.Drawing.Size(105, 23);
        dtpBis.TabIndex = 3;

        lblKunde.AutoSize = true;
        lblKunde.Location = new System.Drawing.Point(372, 13);
        lblKunde.Name     = "lblKunde";
        lblKunde.Text     = "Kunde:";
        lblKunde.TabIndex = 4;

        txtKunde.Location = new System.Drawing.Point(416, 10);
        txtKunde.Name     = "txtKunde";
        txtKunde.Size     = new System.Drawing.Size(160, 23);
        txtKunde.TabIndex = 5;

        lblStatus.AutoSize = true;
        lblStatus.Location = new System.Drawing.Point(584, 13);
        lblStatus.Name     = "lblStatus";
        lblStatus.Text     = "Status:";
        lblStatus.TabIndex = 6;

        cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        cmbStatus.Items.AddRange(new object[] { "Alle", "Offen", "Abgeschlossen", "Fakturiert", "Storniert" });
        cmbStatus.Location     = new System.Drawing.Point(628, 10);
        cmbStatus.Name         = "cmbStatus";
        cmbStatus.Size         = new System.Drawing.Size(150, 23);
        cmbStatus.TabIndex     = 7;
        cmbStatus.SelectedIndex = 0;

        btnSuchen.Location = new System.Drawing.Point(786, 9);
        btnSuchen.Name     = "btnSuchen";
        btnSuchen.Size     = new System.Drawing.Size(80, 26);
        btnSuchen.TabIndex = 8;
        btnSuchen.Text     = "Suchen";

        // pnlToolbar
        pnlToolbar.Controls.Add(btnAbschliessen);
        pnlToolbar.Controls.Add(btnStornieren);
        pnlToolbar.Controls.Add(btnAlleMarkieren);
        pnlToolbar.Dock     = System.Windows.Forms.DockStyle.Bottom;
        pnlToolbar.Height   = 36;
        pnlToolbar.Name     = "pnlToolbar";
        pnlToolbar.TabIndex = 2;

        btnAbschliessen.Enabled  = false;
        btnAbschliessen.Location = new System.Drawing.Point(6, 6);
        btnAbschliessen.Name     = "btnAbschliessen";
        btnAbschliessen.Size     = new System.Drawing.Size(120, 26);
        btnAbschliessen.TabIndex = 0;
        btnAbschliessen.Text     = "Abschliessen";

        btnStornieren.Enabled  = false;
        btnStornieren.Location = new System.Drawing.Point(132, 6);
        btnStornieren.Name     = "btnStornieren";
        btnStornieren.Size     = new System.Drawing.Size(105, 26);
        btnStornieren.TabIndex = 1;
        btnStornieren.Text     = "Stornieren";

        btnAlleMarkieren.Enabled  = false;
        btnAlleMarkieren.Location = new System.Drawing.Point(243, 6);
        btnAlleMarkieren.Name     = "btnAlleMarkieren";
        btnAlleMarkieren.Size     = new System.Drawing.Size(130, 26);
        btnAlleMarkieren.TabIndex = 2;
        btnAlleMarkieren.Text     = "Alle markieren";

        // dgwLieferscheine
        dgwLieferscheine.AllowUserToAddRows    = false;
        dgwLieferscheine.AllowUserToDeleteRows = false;
        dgwLieferscheine.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { colLsChecked });
        dgwLieferscheine.Dock            = System.Windows.Forms.DockStyle.Fill;
        dgwLieferscheine.MultiSelect     = false;
        dgwLieferscheine.Name            = "dgwLieferscheine";
        dgwLieferscheine.ReadOnly        = true;
        dgwLieferscheine.RowHeadersVisible = false;
        dgwLieferscheine.SelectionMode   = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        dgwLieferscheine.TabIndex        = 1;

        colLsChecked.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
        colLsChecked.HeaderText   = "";
        colLsChecked.Name         = "colLsChecked";
        colLsChecked.ReadOnly     = false;
        colLsChecked.Width        = 30;

        // Form
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize          = new System.Drawing.Size(1100, 650);
        Controls.Add(dgwLieferscheine);
        Controls.Add(pnlToolbar);
        Controls.Add(pnlFilter);
        Name = "FrmDeliveryList";
        Text = "Lieferungen";

        pnlFilter.ResumeLayout(false);
        pnlFilter.PerformLayout();
        pnlToolbar.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgwLieferscheine).EndInit();
        ResumeLayout(false);
    }

    private System.Windows.Forms.Panel                        pnlFilter;
    private System.Windows.Forms.Label                        lblVon;
    private System.Windows.Forms.DateTimePicker               dtpVon;
    private System.Windows.Forms.Label                        lblBis;
    private System.Windows.Forms.DateTimePicker               dtpBis;
    private System.Windows.Forms.Label                        lblKunde;
    private System.Windows.Forms.TextBox                      txtKunde;
    private System.Windows.Forms.Label                        lblStatus;
    private System.Windows.Forms.ComboBox                     cmbStatus;
    private System.Windows.Forms.Button                       btnSuchen;
    private System.Windows.Forms.Panel                        pnlToolbar;
    private System.Windows.Forms.Button                       btnAbschliessen;
    private System.Windows.Forms.Button                       btnStornieren;
    private System.Windows.Forms.Button                       btnAlleMarkieren;
    private System.Windows.Forms.DataGridView                 dgwLieferscheine;
    private System.Windows.Forms.DataGridViewCheckBoxColumn   colLsChecked;
}