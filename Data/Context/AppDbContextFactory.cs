using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DNR26V2.Data.Context;

/// <summary>
/// Wird von EF Core Tools (dotnet ef) zur Migration-Erstellung verwendet.
/// Benötigt keine laufende Anwendung.
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\MSSQLLocalDB;" +
            "Database=DNR26V2;" +
            "Trusted_Connection=True;" +
            "TrustServerCertificate=True;");

        return new AppDbContext(optionsBuilder.Options);
    }
}