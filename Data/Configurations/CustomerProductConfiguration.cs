using DNR26V2.Domain.Entities.MasterData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DNR26V2.Data.Configurations;

internal sealed class CustomerProductConfiguration : IEntityTypeConfiguration<CustomerProduct>
{
    public void Configure(EntityTypeBuilder<CustomerProduct> entity)
    {
        entity.ToTable("CustomerProduct");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityColumn();

        entity.Property(e => e.Preis)  .HasPrecision(10, 2).HasDefaultValue(0m);
        entity.Property(e => e.Menge)  .HasPrecision(10, 3).HasDefaultValue(0m);
        entity.Property(e => e.Gewicht).HasPrecision(10, 3).HasDefaultValue(0m);
        entity.Property(e => e.Aktiv)  .HasDefaultValue(true);

        entity.HasIndex(e => new { e.KundeId, e.ArtikelId }).IsUnique();

        entity.HasOne(e => e.Customer)
              .WithMany()
              .HasForeignKey(e => e.KundeId)
              .OnDelete(DeleteBehavior.NoAction);

        entity.HasOne(e => e.Product)
              .WithMany()
              .HasForeignKey(e => e.ArtikelId)
              .OnDelete(DeleteBehavior.NoAction);

        entity.Property(e => e.ErstelltAm)  .HasDefaultValueSql("GETDATE()");
        entity.Property(e => e.ErstelltVon) .HasMaxLength(100).IsRequired();
        entity.Property(e => e.GeaendertVon).HasMaxLength(100);
    }
}