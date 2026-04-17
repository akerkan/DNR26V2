using DNR26V2.Data.Context;
using DNR26V2.Domain.Configuration;
using DNR26V2.Forms.Base;
using DNR26V2.Forms.MasterData;
using DNR26V2.Forms.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace DNR26V2;

public partial class FrmMain : Form
{
    private readonly IServiceProvider _serviceProvider;
    private readonly AppSettings      _appSettings;
    private readonly DapperContext    _dapperContext;

    private ToolStripStatusLabel _statusLabelDb         = null!;
    private ToolStripStatusLabel _statusLabelVersion    = null!;
    private ToolStripStatusLabel _statusLabelConnection = null!;

    public FrmMain(
        IServiceProvider serviceProvider,
        AppSettings      appSettings,
        DapperContext    dapperContext)
    {
        _serviceProvider = serviceProvider;
        _appSettings     = appSettings;
        _dapperContext   = dapperContext;

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
            MessageBox.Show(
                "Die Datenbankverbindung konnte nicht hergestellt werden.\n\n" +
                "Bitte prüfen Sie appsettings.json.",
                "Verbindungsfehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    // ── DI-Helfer ─────────────────────────────────────────────────────────────

    private T GetService<T>() where T : notnull
        => _serviceProvider.GetRequiredService<T>();

    // ── Stammdaten-Menü ───────────────────────────────────────────────────────

    private FrmCustomerList?          FrmCustomerListInstance;
    private FrmProductList?           FrmProductListInstance;
    private FrmProductAttributeList?  FrmProductAttributeListInstance;
    private FrmCustomerProductTemplate? FrmCustomerProductTemplateInstance;

    private void MenuKunden_Click(object? sender, EventArgs e)
        => BaseListForm.GetOrCreateInstance<FrmCustomerList>(ref FrmCustomerListInstance, this, () => GetService<FrmCustomerList>());

    private void MenuArtikel_Click(object? sender, EventArgs e)
        => BaseListForm.GetOrCreateInstance<FrmProductList>(ref FrmProductListInstance, this, () => GetService<FrmProductList>());

    private void MenuArtikelattribute_Click(object? sender, EventArgs e)
        => BaseListForm.GetOrCreateInstance<FrmProductAttributeList>(ref FrmProductAttributeListInstance, this, () => GetService<FrmProductAttributeList>());

    private void MenuKundenArtikelvorlage_Click(object? sender, EventArgs e)
    => BaseListForm.GetOrCreateInstance<FrmCustomerProductTemplate>(ref FrmCustomerProductTemplateInstance, this,() => GetService<FrmCustomerProductTemplate>());

    private void OnMenuItemNotImplemented(object? sender, EventArgs e)
    {
        if (sender is ToolStripMenuItem item)
            MessageBox.Show($"'{item.Text}' ist noch nicht implementiert.",
                "In Entwicklung", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

    private void MenuSystemBeenden_Click(object? sender, EventArgs e)
        => Close();
}
