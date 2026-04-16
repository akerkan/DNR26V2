namespace DNR26V2.Domain.Configuration;

public sealed class AppSettings
{
    public const string SectionName = "AppSettings";

    public bool UseLocalDb { get; set; } = true;
    public string ApplicationName { get; set; } = "DNR26V2";
    public string Version { get; set; } = "2.0.0";
}