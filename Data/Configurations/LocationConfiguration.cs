using DNR26V2.Domain.Entities.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DNR26V2.Data.Configurations;

internal sealed class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> entity)
    {
        entity.ToTable("Location");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityColumn();

        entity.Property(e => e.Standortcode).HasMaxLength(20).IsRequired();
        entity.Property(e => e.Bezeichnung) .HasMaxLength(200).IsRequired();
        entity.Property(e => e.Adresse)     .HasMaxLength(200);
        entity.Property(e => e.PLZ)         .HasMaxLength(10);
        entity.Property(e => e.Ort)         .HasMaxLength(100);
        entity.Property(e => e.Land)        .HasMaxLength(100).HasDefaultValue("Deutschland");
        entity.Property(e => e.IstStandard) .HasDefaultValue(false);
        entity.Property(e => e.Aktiv)       .HasDefaultValue(true);

        entity.Property(e => e.ErstelltAm)  .HasDefaultValueSql("GETDATE()");
        entity.Property(e => e.ErstelltVon) .HasMaxLength(100).IsRequired();
        entity.Property(e => e.GeaendertVon).HasMaxLength(100);

        entity.HasIndex(e => e.Standortcode).IsUnique();
    }
}