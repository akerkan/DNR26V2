namespace DNR26V2.Forms.MasterData;

partial class FrmProductList
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();

        // ── Top-Panel ─────────────────────────────────────────────────────────
        pnlTop          = new Panel();
        lblSuche        = new Label();
        txtSuche        = new TextBox();
        chkNurAktiv     = new CheckBox();

        // ── Liste ─────────────────────────────────────────────────────────────
        dgwArtikel      = new DataGridView();

        // ── Detail-Panel ──────────────────────────────────────────────────────
        panelDetail     = new Panel();

        lblArtikelnummer = new Label();
        txtArtikelnummer = new TextBox();

        lblBezeichnung   = new Label();
        txtBezeichnung   = new TextBox();

        lblBezeichnung2  = new Label();
        txtBezeichnung2  = new TextBox();

        lblEinheit       = new Label();
        txtEinheit       = new TextBox();

        lblVKPreis       = new Label();
        nudVKPreis       = new NumericUpDown();

        lblEKPreis       = new Label();
        nudEKPreis       = new NumericUpDown();

        lblMwstProzent   = new Label();
        nudMwstProzent   = new NumericUpDown();

        lblBarcode       = new Label();
        txtBarcode       = new TextBox();

        lblNotizen       = new Label();
        txtNotizen       = new TextBox();

        chkAktiv         = new CheckBox();

        btnNeu           = new Button();
        btnSpeichern     = new Button();
        btnDeaktivieren  = new Button();

        pnlTop.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgwArtikel).BeginInit();
        panelDetail.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudVKPreis).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudEKPreis).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudMwstProzent).BeginInit();
        SuspendLayout();

        // ── pnlTop ────────────────────────────────────────────────────────────
        pnlTop.Controls.Add(lblSuche);
        pnlTop.Controls.Add(txtSuche);
        pnlTop.Controls.Add(chkNurAktiv);
        pnlTop.Dock    = DockStyle.Top;
        pnlTop.Height  = 40;
        pnlTop.Padding = new Padding(6, 6, 6, 0);

        lblSuche.Text     = "Suche:";
        lblSuche.Location = new Point(6, 10);
        lblSuche.AutoSize = true;

        txtSuche.Location = new Point(55, 7);
        txtSuche.Width    = 260;
        txtSuche.Font     = new Font("Segoe UI", 10F);

        chkNurAktiv.Text     = "Nur aktive";
        chkNurAktiv.Location = new Point(330, 9);
        chkNurAktiv.Checked  = true;
        chkNurAktiv.AutoSize = true;

        // ── dgwArtikel ────────────────────────────────────────────────────────
        dgwArtikel.Dock                  = DockStyle.Left;
        dgwArtikel.Width                 = 560;
        dgwArtikel.AllowUserToAddRows    = false;
        dgwArtikel.AllowUserToDeleteRows = false;
        dgwArtikel.ReadOnly              = true;
        dgwArtikel.SelectionMode         = DataGridViewSelectionMode.FullRowSelect;
        dgwArtikel.MultiSelect           = false;
        dgwArtikel.AutoSizeColumnsMode   = DataGridViewAutoSizeColumnsMode.None;
        dgwArtikel.RowHeadersVisible     = false;
        dgwArtikel.BackgroundColor       = SystemColors.Window;
        dgwArtikel.BorderStyle           = BorderStyle.None;
        dgwArtikel.Font                  = new Font("Segoe UI", 9.5F);

        // ── panelDetail ───────────────────────────────────────────────────────
        panelDetail.Dock    = DockStyle.Fill;
        panelDetail.Padding = new Padding(16, 8, 16, 8);
        panelDetail.Visible = false;

        // Zeile 1 – Artikelnummer
        lblArtikelnummer.Text     = "Artikelnummer *";
        lblArtikelnummer.Location = new Point(16, 14);
        lblArtikelnummer.AutoSize = true;

        txtArtikelnummer.Location   = new Point(16, 32);
        txtArtikelnummer.Width      = 160;
        txtArtikelnummer.CharacterCasing = CharacterCasing.Upper;
        txtArtikelnummer.Font       = new Font("Segoe UI", 10F);

        // Zeile 2 – Bezeichnung
        lblBezeichnung.Text     = "Bezeichnung *";
        lblBezeichnung.Location = new Point(16, 64);
        lblBezeichnung.AutoSize = true;

        txtBezeichnung.Location = new Point(16, 82);
        txtBezeichnung.Width    = 380;
        txtBezeichnung.Font     = new Font("Segoe UI", 10F);

        // Zeile 3 – Bezeichnung2
        lblBezeichnung2.Text     = "Bezeichnung 2";
        lblBezeichnung2.Location = new Point(16, 114);
        lblBezeichnung2.AutoSize = true;

        txtBezeichnung2.Location = new Point(16, 132);
        txtBezeichnung2.Width    = 380;
        txtBezeichnung2.Font     = new Font("Segoe UI", 10F);

        // Zeile 4 – Einheit
        lblEinheit.Text     = "Einheit";
        lblEinheit.Location = new Point(16, 164);
        lblEinheit.AutoSize = true;

        txtEinheit.Location = new Point(16, 182);
        txtEinheit.Width    = 80;
        txtEinheit.CharacterCasing = CharacterCasing.Upper;
        txtEinheit.Font     = new Font("Segoe UI", 10F);

        // Zeile 5 – Preise
        lblVKPreis.Text     = "VK-Preis";
        lblVKPreis.Location = new Point(16, 216);
        lblVKPreis.AutoSize = true;

        nudVKPreis.Location      = new Point(16, 234);
        nudVKPreis.Width         = 110;
        nudVKPreis.DecimalPlaces = 4;
        nudVKPreis.Maximum       = 99999.9999m;
        nudVKPreis.Font          = new Font("Segoe UI", 10F);

        lblEKPreis.Text     = "EK-Preis";
        lblEKPreis.Location = new Point(140, 216);
        lblEKPreis.AutoSize = true;

        nudEKPreis.Location      = new Point(140, 234);
        nudEKPreis.Width         = 110;
        nudEKPreis.DecimalPlaces = 4;
        nudEKPreis.Maximum       = 99999.9999m;
        nudEKPreis.Font          = new Font("Segoe UI", 10F);

        lblMwstProzent.Text     = "MwSt %";
        lblMwstProzent.Location = new Point(264, 216);
        lblMwstProzent.AutoSize = true;

        nudMwstProzent.Location      = new Point(264, 234);
        nudMwstProzent.Width         = 80;
        nudMwstProzent.DecimalPlaces = 2;
        nudMwstProzent.Maximum       = 100m;
        nudMwstProzent.Font          = new Font("Segoe UI", 10F);

        // Zeile 6 – Barcode
        lblBarcode.Text     = "Barcode";
        lblBarcode.Location = new Point(16, 270);
        lblBarcode.AutoSize = true;

        txtBarcode.Location = new Point(16, 288);
        txtBarcode.Width    = 200;
        txtBarcode.Font     = new Font("Segoe UI", 10F);

        // Zeile 7 – Notizen
        lblNotizen.Text     = "Notizen";
        lblNotizen.Location = new Point(16, 320);
        lblNotizen.AutoSize = true;

        txtNotizen.Location   = new Point(16, 338);
        txtNotizen.Width      = 380;
        txtNotizen.Height     = 80;
        txtNotizen.Multiline  = true;
        txtNotizen.ScrollBars = ScrollBars.Vertical;
        txtNotizen.Font       = new Font("Segoe UI", 9.5F);

        // Zeile 8 – Status
        chkAktiv.Text     = "Aktiv";
        chkAktiv.Location = new Point(16, 430);
        chkAktiv.AutoSize = true;
        chkAktiv.Checked  = true;

        // ── Buttons ───────────────────────────────────────────────────────────
        btnNeu.Text     = "Neu (F2)";
        btnNeu.Location = new Point(16, 470);
        btnNeu.Width    = 100;
        btnNeu.Height   = 30;

        btnSpeichern.Text     = "Speichern";
        btnSpeichern.Location = new Point(124, 470);
        btnSpeichern.Width    = 100;
        btnSpeichern.Height   = 30;

        btnDeaktivieren.Text     = "Deaktivieren";
        btnDeaktivieren.Location = new Point(232, 470);
        btnDeaktivieren.Width    = 110;
        btnDeaktivieren.Height   = 30;

        // ── panelDetail Controls ──────────────────────────────────────────────
        panelDetail.Controls.Add(lblArtikelnummer);
        panelDetail.Controls.Add(txtArtikelnummer);
        panelDetail.Controls.Add(lblBezeichnung);
        panelDetail.Controls.Add(txtBezeichnung);
        panelDetail.Controls.Add(lblBezeichnung2);
        panelDetail.Controls.Add(txtBezeichnung2);
        panelDetail.Controls.Add(lblEinheit);
        panelDetail.Controls.Add(txtEinheit);
        panelDetail.Controls.Add(lblVKPreis);
        panelDetail.Controls.Add(nudVKPreis);
        panelDetail.Controls.Add(lblEKPreis);
        panelDetail.Controls.Add(nudEKPreis);
        panelDetail.Controls.Add(lblMwstProzent);
        panelDetail.Controls.Add(nudMwstProzent);
        panelDetail.Controls.Add(lblBarcode);
        panelDetail.Controls.Add(txtBarcode);
        panelDetail.Controls.Add(lblNotizen);
        panelDetail.Controls.Add(txtNotizen);
        panelDetail.Controls.Add(chkAktiv);
        panelDetail.Controls.Add(btnNeu);
        panelDetail.Controls.Add(btnSpeichern);
        panelDetail.Controls.Add(btnDeaktivieren);

        // ── Form ──────────────────────────────────────────────────────────────
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode       = AutoScaleMode.Font;
        ClientSize          = new Size(1200, 700);
        Text                = "Artikelstammdaten";
        Controls.Add(panelDetail);
        Controls.Add(dgwArtikel);
        Controls.Add(pnlTop);

        pnlTop.ResumeLayout(false);
        pnlTop.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgwArtikel).EndInit();
        panelDetail.ResumeLayout(false);
        panelDetail.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudVKPreis).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudEKPreis).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudMwstProzent).EndInit();
        ResumeLayout(false);
    }

    #endregion

    // ── Control-Deklarationen ─────────────────────────────────────────────────
    private Panel          pnlTop;
    private Label          lblSuche;
    private TextBox        txtSuche;
    private CheckBox       chkNurAktiv;
    private DataGridView   dgwArtikel;
    private Panel          panelDetail;

    private Label          lblArtikelnummer;
    private TextBox        txtArtikelnummer;
    private Label          lblBezeichnung;
    private TextBox        txtBezeichnung;
    private Label          lblBezeichnung2;
    private TextBox        txtBezeichnung2;
    private Label          lblEinheit;
    private TextBox        txtEinheit;
    private Label          lblVKPreis;
    private NumericUpDown  nudVKPreis;
    private Label          lblEKPreis;
    private NumericUpDown  nudEKPreis;
    private Label          lblMwstProzent;
    private NumericUpDown  nudMwstProzent;
    private Label          lblBarcode;
    private TextBox        txtBarcode;
    private Label          lblNotizen;
    private TextBox        txtNotizen;
    private CheckBox       chkAktiv;
    private Button         btnNeu;
    private Button         btnSpeichern;
    private Button         btnDeaktivieren;
}