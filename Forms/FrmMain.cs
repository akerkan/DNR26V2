using DNR26V2.Data.Context;
using DNR26V2.Domain.Configuration;
using DNR26V2.Forms.Base;
using DNR26V2.Forms.Deliveries;
using DNR26V2.Forms.Invoices;
using DNR26V2.Forms.MasterData;
using DNR26V2.Forms.Orders;
using DNR26V2.Forms.Settings;
using DNR26V2.Forms.Payments;
using DNR26V2.Helpers;
using DNR26V2.Services.System;
using Microsoft.Extensions.DependencyInjection;

namespace DNR26V2;

public partial class FrmMain : Form
{
    private readonly IServiceProvider  _serviceProvider;
    private readonly AppSettings       _appSettings;
    private readonly DapperContext     _dapperContext;
    private readonly IAppSetupService  _appSetupService;

    private ToolStripStatusLabel _statusLabelDb         = null!;
    private ToolStripStatusLabel _statusLabelVersion    = null!;
    private ToolStripStatusLabel _statusLabelConnection = null!;

    public FrmMain(
        IServiceProvider  serviceProvider,
        AppSettings       appSettings,
        DapperContext     dapperContext,
        IAppSetupService  appSetupService)
    {
        _serviceProvider  = serviceProvider;
        _appSettings      = appSettings;
        _dapperContext    = dapperContext;
        _appSetupService  = appSetupService;

        InitializeComponent();

        Text = $"{_appSettings.ApplicationName} v{_appSettings.Version}";
        _statusLabelDb.Text      = _appSettings.UseLocalDb ? "LocalDB" : "SQL Server";
        _statusLabelVersion.Text = $"v{_appSettings.Version}";
    }

    private async void FrmMain_Load(object sender, EventArgs e)
        => await TestDatabaseConnectionAsync();

    // ── Verbindungstest ───────────────────────────────────────────────────────

    private async Task TestDatabaseConnectionAsync()
    {
        _statusLabelConnection.Text      = "● Prüfe...";
        _statusLabelConnection.ForeColor = Color.Gray;

        bool ok = await _dapperContext.TestConnectionAsync();
        _statusLabelConnection.Text      = ok ? "● Verbunden"      : "● Nicht verbunden";
        _statusLabelConnection.ForeColor = ok ? Color.Green        : Color.Red;

        if (!ok)
        {
            MessageBox.Show(
                "Die Datenbankverbindung konnte nicht hergestellt werden.\n\n" +
                "Bitte prüfen Sie appsettings.json.",
                "Verbindungsfehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // ✅ Jetzt korrekt:
        var setup = await _appSetupService.GetAsync();
        StatusColorHelper.Configure(setup);
    }

    // ── DI-Helfer ─────────────────────────────────────────────────────────────

    private T GetService<T>() where T : notnull
        => _serviceProvider.GetRequiredService<T>();

    // ── Stammdaten-Menü ───────────────────────────────────────────────────────

    private FrmCustomerList?             FrmCustomerListInstance;
    private FrmProductList?              FrmProductListInstance;
    private FrmProductAttributeList?     FrmProductAttributeListInstance;
    private FrmCustomerProductTemplate?  FrmCustomerProductTemplateInstance;


    private void MenuKunden_Click(object? sender, EventArgs e)
        => BaseListForm.GetOrCreateInstance<FrmCustomerList>(ref FrmCustomerListInstance, this, () => GetService<FrmCustomerList>());

    private void MenuArtikel_Click(object? sender, EventArgs e)
        => BaseListForm.GetOrCreateInstance<FrmProductList>(ref FrmProductListInstance, this, () => GetService<FrmProductList>());

    private void MenuArtikelattribute_Click(object? sender, EventArgs e)
        => BaseListForm.GetOrCreateInstance<FrmProductAttributeList>(ref FrmProductAttributeListInstance, this, () => GetService<FrmProductAttributeList>());

    private void MenuKundenArtikelvorlage_Click(object? sender, EventArgs e)
        => BaseListForm.GetOrCreateInstance<FrmCustomerProductTemplate>(ref FrmCustomerProductTemplateInstance, this, () => GetService<FrmCustomerProductTemplate>());

    // ── Verkauf-Menü ──────────────────────────────────────────────────────────

    private FrmOrderEntry? FrmOrderEntryInstance;

    private void MenuAuftragserfassung_Click(object? sender, EventArgs e)
        => BaseListForm.GetOrCreateInstance<FrmOrderEntry>(ref FrmOrderEntryInstance, this, () => GetService<FrmOrderEntry>());

    // ── Verkauf-Menü (Erweiterung) ────────────────────────────────────────────

    private FrmOrderList? FrmOrderListInstance;

    private void MenuAuftraege_Click(object? sender, EventArgs e)
        => BaseListForm.GetOrCreateInstance<FrmOrderList>(ref FrmOrderListInstance, this, () => GetService<FrmOrderList>());
    public void OpenAuftragserfassung(int kundeId, DateTime lieferdatum)
    {
        var frm = BaseListForm.GetOrCreateInstance<FrmOrderEntry>(ref FrmOrderEntryInstance, this, () => GetService<FrmOrderEntry>());
        frm.NavigateToAuftrag(kundeId, lieferdatum);
    }

   


    // ── System-Menü ───────────────────────────────────────────────────────────

    private void MenuSystemEinstellungen_Click(object? sender, EventArgs e)
    {
        using var frm = GetService<FrmAppSetup>();
        frm.ShowDialog(this);
    }

    private void MenuSystemStandort_Click(object? sender, EventArgs e)
    {
        using var frm = GetService<FrmLocationSetup>();
        frm.ShowDialog(this);
    }

    private void MenuSystemVerbindungstest_Click(object? sender, EventArgs e)
        => _ = TestDatabaseConnectionAsync();

    private void OnMenuItemNotImplemented(object? sender, EventArgs e)
        => MessageBox.Show(
            "Diese Funktion ist noch nicht implementiert.",
            "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);

    private void MenuSystemBeenden_Click(object? sender, EventArgs e)
        => Close();

    // ── Lieferung-Menü (Erweiterung) ─────────────────────────────────────────

    private FrmDeliveryList? FrmDeliveryListInstance;

    private void MenuLieferungen_Click(object? sender, EventArgs e)
        => BaseListForm.GetOrCreateInstance<FrmDeliveryList>(ref FrmDeliveryListInstance, this, () => GetService<FrmDeliveryList>());

    /// <summary>Öffnet FrmDeliveryList und selektiert den Lieferschein zum gegebenen Auftrag.</summary>
    public void OpenDeliveryListForLieferschein(string lieferscheinNr)
    {
        var frm = BaseListForm.GetOrCreateInstance<FrmDeliveryList>(ref FrmDeliveryListInstance, this, () => GetService<FrmDeliveryList>());
        frm.NavigateToLieferschein(lieferscheinNr);
    }

    private FrmTourList? FrmTourListInstance;

    private void MenuTouren_Click(object? sender, EventArgs e)
    {
        BaseListForm.GetOrCreateInstance<FrmTourList>(ref FrmTourListInstance, this, () => GetService<FrmTourList>());
    }


    // ── Rechnung-Menü (Erweiterung) ──────────────────────────────────────────

    private FrmRechnungErfassung? FrmRechnungErfassungInstance;
    private FrmSammelRechnung?    FrmSammelrechnungInstance;
    private FrmRechnungList?      FrmRechnungListInstance;

    private void MenuRechnungErfassung_Click(object? sender, EventArgs e)
        => BaseListForm.GetOrCreateInstance<FrmRechnungErfassung>(ref FrmRechnungErfassungInstance, this, () => GetService<FrmRechnungErfassung>());

    private void MenuSammelrechnung_Click(object? sender, EventArgs e)
        => BaseListForm.GetOrCreateInstance<FrmSammelRechnung>(ref FrmSammelrechnungInstance, this, () => GetService<FrmSammelRechnung>());

    private void MenuRechnungListe_Click(object? sender, EventArgs e)
        => BaseListForm.GetOrCreateInstance<FrmRechnungList>(ref FrmRechnungListInstance, this, () => GetService<FrmRechnungList>());

    // ── Zahlungsmanagement ────────────────────────────────────────────────────

    private FrmZahlungseingaenge? FrmZahlungseingaengeInstance;
    private FrmKundenkonto? FrmKundenkontoInstance;

    private void MenuZahlungseingaenge_Click(object? sender, EventArgs e)
        => BaseListForm.GetOrCreateInstance<FrmZahlungseingaenge>(ref FrmZahlungseingaengeInstance, this, () => GetService<FrmZahlungseingaenge>());

    private void MenuKundenkonto_Click(object? sender, EventArgs e)
        => BaseListForm.GetOrCreateInstance<FrmKundenkonto>(ref FrmKundenkontoInstance, this, () => GetService<FrmKundenkonto>());
}
