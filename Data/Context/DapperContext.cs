using Microsoft.Data.SqlClient;
using System.Data;

namespace DNR26V2.Data.Context;

/// <summary>
/// Stellt IDbConnection-Instanzen für Dapper-Abfragen bereit.
/// Keine gepoolten Langzeit-Verbindungen – jeder Aufruf erstellt eine frische Connection.
/// </summary>
public class DapperContext
{
    private readonly string _connectionString;

    public DapperContext(string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
        => new SqlConnection(_connectionString);

    /// <summary>
    /// Schneller Verbindungstest. Gibt true zurück wenn die DB erreichbar ist.
    /// </summary>
    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}