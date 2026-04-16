using DNR26V2.Domain.Configuration;
using Microsoft.Extensions.Configuration;

namespace DNR26V2.Data.Context;

/// <summary>
/// Wählt den aktiven ConnectionString basierend auf AppSettings.UseLocalDb.
/// </summary>
public static class ConnectionStringProvider
{
    public static string Resolve(IConfiguration configuration, AppSettings appSettings)
    {
        var key = appSettings.UseLocalDb ? "LocalDb" : "SqlServer";
        var connectionString = configuration.GetConnectionString(key);

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException(
                $"ConnectionString '{key}' ist nicht in appsettings.json konfiguriert.");

        return connectionString;
    }
}