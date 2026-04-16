using DNR26V2.Domain.Entities.MasterData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DNR26V2.Data.Configurations;

internal sealed class DriverConfiguration : IEntityTypeConfiguration<Driver>
{
    public void Configure(EntityTypeBuilder<Driver> entity)
    {
        entity.ToTable("Driver");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityColumn();

        entity.Property(e => e.Fahrercode)   .HasMaxLength(20).IsRequired();
        entity.Property(e => e.Vorname)      .HasMaxLength(100).IsRequired();
        entity.Property(e => e.Nachname)     .HasMaxLength(100).IsRequired();
        entity.Property(e => e.Telefonnummer).HasMaxLength(50);
        entity.Property(e => e.Aktiv)        .HasDefaultValue(true);

        entity.HasIndex(e => e.Fahrercode).IsUnique();

        entity.Property(e => e.ErstelltAm) .HasDefaultValueSql("GETDATE()");
        entity.Property(e => e.ErstelltVon).HasMaxLength(100).IsRequired();
        entity.Property(e => e.GeaendertVon).HasMaxLength(100);
    }
}