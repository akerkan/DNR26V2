using DNR26V2.Domain.Entities.MasterData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DNR26V2.Data.Configurations;

internal sealed class ProductAttributeValueConfiguration : IEntityTypeConfiguration<ProductAttributeValue>
{
    public void Configure(EntityTypeBuilder<ProductAttributeValue> entity)
    {
        entity.ToTable("ProductAttributeValue");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityColumn();

        entity.Property(e => e.Bezeichnung).HasMaxLength(200).IsRequired();
        entity.Property(e => e.Sortierung) .HasDefaultValue(0);
        entity.Property(e => e.Aktiv)      .HasDefaultValue(true);

        entity.Property(e => e.ErstelltAm)  .HasDefaultValueSql("GETDATE()");
        entity.Property(e => e.ErstelltVon) .HasMaxLength(100).IsRequired();
        entity.Property(e => e.GeaendertVon).HasMaxLength(100);

        entity.HasOne(e => e.Attribut)
              .WithMany(a => a.Werte)
              .HasForeignKey(e => e.AttributId)
              .OnDelete(DeleteBehavior.Cascade);

        entity.HasIndex(e => e.AttributId);
    }
}