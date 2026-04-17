using DNR26V2.Domain.Entities.MasterData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DNR26V2.Data.Configurations;

internal sealed class CustomerProductAttributeMappingConfiguration
    : IEntityTypeConfiguration<CustomerProductAttributeMapping>
{
    public void Configure(EntityTypeBuilder<CustomerProductAttributeMapping> entity)
    {
        entity.ToTable("CustomerProductAttributeMapping");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityColumn();

        entity.Property(e => e.FreierText).HasMaxLength(500);

        entity.HasOne(e => e.CustomerProduct)
              .WithMany(cp => cp.Spezifikationen)
              .HasForeignKey(e => e.CustomerProductId)
              .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(e => e.Attribut)
              .WithMany(a => a.KundenMappings)
              .HasForeignKey(e => e.AttributId)
              .OnDelete(DeleteBehavior.NoAction);

        entity.HasOne(e => e.AttributWert)
              .WithMany()
              .HasForeignKey(e => e.AttributWertId)
              .OnDelete(DeleteBehavior.NoAction);

        entity.Property(e => e.ErstelltAm)  .HasDefaultValueSql("GETDATE()");
        entity.Property(e => e.ErstelltVon) .HasMaxLength(100).IsRequired();
        entity.Property(e => e.GeaendertVon).HasMaxLength(100);
    }
}