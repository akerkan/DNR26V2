namespace DNR26V2;

partial class FrmMain
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        // ==== MAIN MENU ====
        var menuStrip = new MenuStrip();

        // --- Stammdaten ---
        var menuStammdaten = new ToolStripMenuItem("&Stammdaten");
        var menuKunden = new ToolStripMenuItem("&Kunden", null, MenuKunden_Click);  // ← KORRIGIERT
        var menuProdukte = new ToolStripMenuItem("&Produkte", null, MenuArtikel_Click);
        var menuAttribute = new ToolStripMenuItem("&Attribute", null, MenuArtikelattribute_Click);
        var menuKundenVorlage = new ToolStripMenuItem("&Kunden-/Artikelvorlage", null, MenuKundenArtikelvorlage_Click);
        var menuRouten = new ToolStripMenuItem("&Routen", null, OnMenuItemNotImplemented);
        var menuFahrer = new ToolStripMenuItem("&Fahrer", null, OnMenuItemNotImplemented);
        var menuStandort = new ToolStripMenuItem("S&tandort", null, OnMenuItemNotImplemented);
        menuStammdaten.DropDownItems.AddRange(new ToolStripItem[]
        {
            menuKunden, menuProdukte, menuAttribute, menuKundenVorlage,new ToolStripSeparator(),
            new ToolStripSeparator(),
            menuRouten, menuFahrer, menuStandort
        });

        // --- Verkauf ---
        var menuVerkauf = new ToolStripMenuItem("&Verkauf");
        var menuAuftraege = new ToolStripMenuItem("&Aufträge", null, OnMenuItemNotImplemented);
        var menuTagesbestellung = new ToolStripMenuItem("&Tagesbestellung", null, OnMenuItemNotImplemented);
        var menuLieferungen = new ToolStripMenuItem("&Lieferungen", null, OnMenuItemNotImplemented);
        var menuTouren = new ToolStripMenuItem("T&ouren", null, OnMenuItemNotImplemented);
        menuVerkauf.DropDownItems.AddRange(new ToolStripItem[]
        {
            menuTagesbestellung, menuAuftraege, new ToolStripSeparator(),
            menuLieferungen, menuTouren
        });

        // --- Finanzen ---
        var menuFinanzen = new ToolStripMenuItem("&Finanzen");
        var menuRechnungen = new ToolStripMenuItem("&Rechnungen", null, OnMenuItemNotImplemented);
        var menuSammelrechnung = new ToolStripMenuItem("&Sammelrechnung", null, OnMenuItemNotImplemented);
        var menuGutschriften = new ToolStripMenuItem("&Gutschriften", null, OnMenuItemNotImplemented);
        var menuZahlungen = new ToolStripMenuItem("&Zahlungen", null, OnMenuItemNotImplemented);
        var menuKundenposten = new ToolStripMenuItem("Kunden&posten", null, OnMenuItemNotImplemented);
        menuFinanzen.DropDownItems.AddRange(new ToolStripItem[]
        {
            menuRechnungen, menuSammelrechnung, new ToolStripSeparator(),
            menuGutschriften, new ToolStripSeparator(),
            menuZahlungen, menuKundenposten
        });

        // --- Berichte ---
        var menuBerichte = new ToolStripMenuItem("&Berichte");
        var menuBerichtKundenverkauf = new ToolStripMenuItem("&Kundenverkauf", null, OnMenuItemNotImplemented);
        var menuBerichtProduktverkauf = new ToolStripMenuItem("&Produktverkauf", null, OnMenuItemNotImplemented);
        var menuBerichtZahlungen = new ToolStripMenuItem("&Zahlungen", null, OnMenuItemNotImplemented);
        menuBerichte.DropDownItems.AddRange(new ToolStripItem[]
        {
            menuBerichtKundenverkauf, menuBerichtProduktverkauf,
            new ToolStripSeparator(), menuBerichtZahlungen
        });

        // --- System ---
        var menuSystem = new ToolStripMenuItem("&System");
        var menuEinstellungen = new ToolStripMenuItem("&Einstellungen", null, MenuSystemEinstellungen_Click);
        var menuVerbindungstest = new ToolStripMenuItem("&Verbindungstest", null, MenuSystemVerbindungstest_Click);
        var menuBeenden = new ToolStripMenuItem("&Beenden", null, MenuSystemBeenden_Click);
        menuSystem.DropDownItems.AddRange(new ToolStripItem[]
        {
            menuEinstellungen, menuVerbindungstest,
            new ToolStripSeparator(), menuBeenden
        });

        menuStrip.Items.AddRange(new ToolStripItem[]
        {
            menuStammdaten, menuVerkauf, menuFinanzen, menuBerichte, menuSystem
        });

        // ==== STATUS BAR ====
        var statusStrip = new StatusStrip();

        _statusLabelDb = new ToolStripStatusLabel
        {
            Text = "",
            Spring = false,
            BorderSides = ToolStripStatusLabelBorderSides.Right
        };

        _statusLabelConnection = new ToolStripStatusLabel
        {
            Text = "● Prüfe...",
            ForeColor = Color.Gray,
            Spring = false,
            BorderSides = ToolStripStatusLabelBorderSides.Right
        };

        var statusSpacer = new ToolStripStatusLabel
        {
            Spring = true,
            Text = ""
        };

        _statusLabelVersion = new ToolStripStatusLabel
        {
            Text = "",
            Alignment = ToolStripItemAlignment.Right
        };

        statusStrip.Items.AddRange(new ToolStripItem[]
        {
            _statusLabelConnection, _statusLabelDb, statusSpacer, _statusLabelVersion
        });

        // ==== FORM SETUP ====
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1280, 720);
        IsMdiContainer = true;
        MainMenuStrip = menuStrip;
        StartPosition = FormStartPosition.CenterScreen;
        WindowState = FormWindowState.Maximized;

        Controls.Add(statusStrip);
        Controls.Add(menuStrip);

        Load += FrmMain_Load;
    }

    #endregion
}
