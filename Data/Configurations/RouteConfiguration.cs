using DNR26V2.Domain.Entities.MasterData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DNR26V2.Data.Configurations;

internal sealed class RouteConfiguration : IEntityTypeConfiguration<Route>
{
    public void Configure(EntityTypeBuilder<Route> entity)
    {
        entity.ToTable("Route");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityColumn();

        entity.Property(e => e.Routencode) .HasMaxLength(20).IsRequired();
        entity.Property(e => e.Bezeichnung).HasMaxLength(200).IsRequired();
        entity.Property(e => e.IstWochenendtour).HasDefaultValue(false);
        entity.Property(e => e.Aktiv)           .HasDefaultValue(true);

        entity.HasIndex(e => e.Routencode).IsUnique();

        entity.Property(e => e.ErstelltAm) .HasDefaultValueSql("GETDATE()");
        entity.Property(e => e.ErstelltVon).HasMaxLength(100).IsRequired();
        entity.Property(e => e.GeaendertVon).HasMaxLength(100);
    }
}