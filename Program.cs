using DNR26V2.Data.Context;
using DNR26V2.Data.Seed;
using DNR26V2.Domain.Configuration;
using DNR26V2.Forms.MasterData;
using DNR26V2.Forms.Settings;
using DNR26V2.Helpers;
using DNR26V2.Services.MasterData;
using DNR26V2.Services.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DNR26V2;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        var configuration = BuildConfiguration();
        var services      = ConfigureServices(configuration);

        using var serviceProvider = services.BuildServiceProvider();

        // DB migrieren + Seed-Daten anlegen
        InitializeDatabaseAsync(serviceProvider).GetAwaiter().GetResult();

        // GridColumnChooser bekommt den DB-Service einmalig gesetzt
        GridColumnChooser.SetService(serviceProvider.GetRequiredService<IGridSettingsService>());

        Application.Run(serviceProvider.GetRequiredService<FrmMain>());
    }

    private static IConfiguration BuildConfiguration()
        => new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

    private static ServiceCollection ConfigureServices(IConfiguration configuration)
    {
        var services = new ServiceCollection();

        // --- Configuration ---
        var appSettings = new AppSettings();
        configuration.GetSection(AppSettings.SectionName).Bind(appSettings);
        services.AddSingleton(appSettings);
        services.AddSingleton<IConfiguration>(configuration);

        var connectionString = ConnectionStringProvider.Resolve(configuration, appSettings);

        // --- EF Core ---
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        // --- Dapper ---
        services.AddSingleton(new DapperContext(connectionString));

        // --- System-Services ---
        services.AddScoped<INoSeriesService,  NoSeriesService>();
        services.AddScoped<IAuditLogService,  AuditLogService>();
        services.AddScoped<DatabaseSeeder>();
        services.AddSingleton<IGridSettingsService, GridSettingsService>();

        // --- Modul 2: Stammdaten-Services ---
        services.AddScoped<IRouteService,          RouteService>();
        services.AddScoped<IDriverService,         DriverService>();

        // --- Modul 3: Produktstammdaten-Services ---
        services.AddScoped<IProductService,          ProductService>();
        services.AddScoped<IProductAttributeService, ProductAttributeService>();

        // --- Modul 4: Kunden-Bearbeitungs-Services ---
        services.AddScoped<ICustomerService,        CustomerService>();          // ← NEU
        services.AddScoped<ICustomerProductService, CustomerProductService>();

        // --- Forms ---
        services.AddTransient<FrmMain>();
        services.AddTransient<FrmAppSetup>();
        services.AddTransient<FrmLocationSetup>();
        services.AddTransient<FrmCustomerList>(sp =>
            new FrmCustomerList(
                sp.GetRequiredService<ICustomerService>(),
                sp.GetRequiredService<IProductAttributeService>()));
        services.AddTransient<FrmProductList>();
        services.AddTransient<FrmProductAttributeList>();
        services.AddTransient<FrmCustomerProductTemplate>(sp =>           // ← Factory ergänzt
            new FrmCustomerProductTemplate(
                sp.GetRequiredService<ICustomerService>(),
                sp.GetRequiredService<IProductService>(),
                sp.GetRequiredService<ICustomerProductService>()));

        return services;
    }

    private static async Task InitializeDatabaseAsync(IServiceProvider sp)
    {
        try
        {
            using var scope  = sp.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
            await seeder.SeedAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Datenbankinitialisierung fehlgeschlagen:\n\n{ex.Message}\n\n" +
                "Bitte prüfen Sie die Verbindungseinstellungen.",
                "Startfehler",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}