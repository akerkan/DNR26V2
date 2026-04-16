using Dapper;
using DNR26V2.Data.Context;

namespace DNR26V2.Services.System;

public class AuditLogService : IAuditLogService
{
    private readonly DapperContext _dapper;

    public AuditLogService(DapperContext dapper)
    {
        _dapper = dapper;
    }

    public async Task LogAsync(
        string  tabellenname,
        int     datensatzId,
        string  aktion,
        string? belegnummer = null,
        string? grund       = null,
        string? alterWert   = null,
        string? neuerWert   = null)
    {
        try
        {
            using var connection = _dapper.CreateConnection();

            await connection.ExecuteAsync(
                @"INSERT INTO AuditLog
                      (Tabellenname, DatensatzId, Belegnummer, Aktion,
                       AlterWert, NeuerWert, Grund, Benutzer, Zeitstempel)
                  VALUES
                      (@Tabellenname, @DatensatzId, @Belegnummer, @Aktion,
                       @AlterWert, @NeuerWert, @Grund, @Benutzer, GETDATE())",
                new
                {
                    Tabellenname = tabellenname,
                    DatensatzId  = datensatzId,
                    Belegnummer  = belegnummer,
                    Aktion       = aktion,
                    AlterWert    = alterWert,
                    NeuerWert    = neuerWert,
                    Grund        = grund,
                    Benutzer     = Environment.UserName
                });
        }
        catch
        {
            // AuditLog darf nie die Hauptoperation zum Scheitern bringen
        }
    }
}