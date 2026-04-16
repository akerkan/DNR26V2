using DNR26V2.Domain.Entities;
using DNR26V2.Domain.Entities.MasterData;
using DNR26V2.Domain.Entities.System;
using Microsoft.EntityFrameworkCore;

namespace DNR26V2.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    // ── System ────────────────────────────────────────────────────────────────
    public DbSet<AppSetup>  AppSetup  { get; set; } = null!;
    public DbSet<NoSeries>  NoSeries  { get; set; } = null!;
    public DbSet<AuditLog>  AuditLog  { get; set; } = null!;
    public DbSet<Location>  Location  { get; set; } = null!;

    // ── Stammdaten (Modul 2) ──────────────────────────────────────────────────
    public DbSet<Customer>       Customer       { get; set; } = null!;
    public DbSet<CustomerFilter> CustomerFilter { get; set; } = null!;
    public DbSet<Route>          Route          { get; set; } = null!;
    public DbSet<Driver>         Driver         { get; set; } = null!;

    // ── Modul 3+ DbSets werden hier ergänzt ──────────────────────────────────

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        ApplyAuditFields();
        return base.SaveChanges();
    }

    private void ApplyAuditFields()
    {
        var user = Environment.UserName;
        var now  = DateTime.Now;

        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.ErstelltAm  = now;
                    entry.Entity.ErstelltVon = user;
                    break;
                case EntityState.Modified:
                    entry.Entity.GeaendertAm  = now;
                    entry.Entity.GeaendertVon = user;
                    entry.Property(e => e.ErstelltAm).IsModified  = false;
                    entry.Property(e => e.ErstelltVon).IsModified = false;
                    break;
            }
        }
    }
}