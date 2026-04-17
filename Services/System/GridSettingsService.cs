using Dapper;
using DNR26V2.Data.Context;

namespace DNR26V2.Services.System;

public class GridSettingsService : IGridSettingsService
{
    private readonly DapperContext           _dapper;
    private readonly string                  _user = Environment.UserName;
    private readonly Dictionary<string,string> _cache = new();

    public GridSettingsService(DapperContext dapper) => _dapper = dapper;

    public string? Load(string gridKey)
    {
        if (_cache.TryGetValue(gridKey, out var cached)) return cached;
        try
        {
            using var conn = _dapper.CreateConnection();
            var json = conn.QuerySingleOrDefault<string>(
                "SELECT Einstellungen FROM UserGridSetting WHERE Benutzername=@b AND GridKey=@k",
                new { b = _user, k = gridKey });
            if (json is not null) _cache[gridKey] = json;
            return json;
        }
        catch { return null; }
    }

    public async Task SaveAsync(string gridKey, string json)
    {
        _cache[gridKey] = json;
        try
        {
            using var conn = _dapper.CreateConnection();
            await conn.ExecuteAsync(
                @"MERGE UserGridSetting WITH (HOLDLOCK) AS t
                  USING (SELECT @b AS Benutzername, @k AS GridKey) AS s
                  ON t.Benutzername = s.Benutzername AND t.GridKey = s.GridKey
                  WHEN MATCHED THEN
                      UPDATE SET Einstellungen = @j, GeaendertAm = GETDATE()
                  WHEN NOT MATCHED THEN
                      INSERT (Benutzername, GridKey, Einstellungen, GeaendertAm)
                      VALUES (@b, @k, @j, GETDATE());",
                new { b = _user, k = gridKey, j = json });
        }
        catch { }
    }

    public async Task DeleteAsync(string gridKey)
    {
        _cache.Remove(gridKey);
        try
        {
            using var conn = _dapper.CreateConnection();
            await conn.ExecuteAsync(
                "DELETE FROM UserGridSetting WHERE Benutzername=@b AND GridKey=@k",
                new { b = _user, k = gridKey });
        }
        catch { }
    }
}