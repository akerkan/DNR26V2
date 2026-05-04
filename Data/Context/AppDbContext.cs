using DNR26V2.Domain.Entities;
using DNR26V2.Domain.Entities.Etikett;
using DNR26V2.Domain.Entities.MasterData;
using DNR26V2.Domain.Entities.System;
using DNR26V2.Domain.Entities.Orders;
using DNR26V2.Domain.Entities.Deliveries;
using Microsoft.EntityFrameworkCore;
using DNR26V2.Domain.Entities.Invoices;
using DNR26V2.Data.Configurations.Invoices;

namespace DNR26V2.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    // ── System ────────────────────────────────────────────────────────────────
    public DbSet<AppSetup>        AppSetup        { get; set; } = null!;
    public DbSet<NoSeries>        NoSeries        { get; set; } = null!;
    public DbSet<AuditLog>        AuditLog        { get; set; } = null!;
    public DbSet<Location>        Location        { get; set; } = null!;
    public DbSet<UserGridSetting> UserGridSetting { get; set; } = null!;

    // ── Stammdaten ────────────────────────────────────────────────────────────
    public DbSet<Customer>                        Customer                        { get; set; } = null!;
    public DbSet<Route>                           Route                           { get; set; } = null!;
    public DbSet<Driver>                          Driver                          { get; set; } = null!;
    public DbSet<CustomerProduct>                 CustomerProduct                 { get; set; } = null!;
    public DbSet<CustomerProductAttributeMapping> CustomerProductAttributeMapping { get; set; } = null!;
    public DbSet<Product>                         Product                         { get; set; } = null!;
    public DbSet<ProductAttribute>                ProductAttribute                { get; set; } = null!;
    public DbSet<ProductAttributeValue>           ProductAttributeValue           { get; set; } = null!;
    public DbSet<ProductAttributeMapping>         ProductAttributeMapping         { get; set; } = null!;

    // ── Aufträge ──────────────────────────────────────────────────────────────
    public DbSet<Order>     Order     { get; set; } = null!;
    public DbSet<OrderLine> OrderLine { get; set; } = null!;

    // ── Lieferungen ───────────────────────────────────────────────────────────
    public DbSet<DeliveryHeader> DeliveryHeader { get; set; } = null!;
    public DbSet<DeliveryLine>   DeliveryLine   { get; set; } = null!;

    // ── Rechnungen / Zahlungen ────────────────────────────────────────────────
    public DbSet<InvoiceHeader>                              Invoices       { get; set; } = null!;
    public DbSet<InvoiceLine>                                InvoiceLines   { get; set; } = null!;
    public DbSet<DNR26V2.Domain.Entities.Payments.PaymentHeader> PaymentHeaders { get; set; } = null!;
    public DbSet<DNR26V2.Domain.Entities.Payments.PaymentLine>   PaymentLines   { get; set; } = null!;

    // ── Modul 8: Etikett ──────────────────────────────────────────────────────
    public DbSet<EtiketLayoutField> EtiketLayoutFields { get; set; } = null!;
    public DbSet<EtiketPaperConfig> EtiketPaperConfigs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        modelBuilder.ApplyConfiguration(new InvoiceHeaderConfiguration());
        modelBuilder.ApplyConfiguration(new InvoiceLineConfiguration());
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