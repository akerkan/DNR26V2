namespace DNR26V2.Services.System;

public interface IGridSettingsService
{
    string? Load(string gridKey);
    Task SaveAsync(string gridKey, string json);
    Task DeleteAsync(string gridKey);
}