using DNR26V2.Domain.Entities.MasterData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DNR26V2.Data.Configurations;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> entity)
    {
        entity.ToTable("Product");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityColumn();

        entity.Property(e => e.Artikelnummer).HasMaxLength(50).IsRequired();
        entity.Property(e => e.Bezeichnung)  .HasMaxLength(200).IsRequired();
        entity.Property(e => e.Bezeichnung2) .HasMaxLength(200);
        entity.Property(e => e.Einheit)      .HasMaxLength(20).HasDefaultValue("STK");
        entity.Property(e => e.VKPreis)      .HasPrecision(18, 4).HasDefaultValue(0m);
        entity.Property(e => e.EKPreis)      .HasPrecision(18, 4).HasDefaultValue(0m);
        entity.Property(e => e.MwstProzent)  .HasPrecision(5, 2).HasDefaultValue(7m);

        entity.Property(e => e.Feld1)        .HasMaxLength(2000);
        entity.Property(e => e.Feld2)        .HasMaxLength(2000);
        entity.Property(e => e.Feld3)        .HasMaxLength(2000);
        entity.Property(e => e.Feld4)        .HasMaxLength(2000);
        entity.Property(e => e.Printfarbe)   .HasMaxLength(50);

        entity.Property(e => e.Barcode)      .HasMaxLength(100);
        entity.Property(e => e.Notizen)      .HasMaxLength(4000);
        entity.Property(e => e.Aktiv)        .HasDefaultValue(true);

        entity.Property(e => e.ErstelltAm)  .HasDefaultValueSql("GETDATE()");
        entity.Property(e => e.ErstelltVon) .HasMaxLength(100).IsRequired();
        entity.Property(e => e.GeaendertVon).HasMaxLength(100);

        entity.HasIndex(e => e.Artikelnummer).IsUnique();
    }
}