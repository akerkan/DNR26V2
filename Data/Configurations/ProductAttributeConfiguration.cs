using DNR26V2.Domain.Entities.MasterData;
using DNR26V2.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DNR26V2.Data.Configurations;

internal sealed class ProductAttributeConfiguration : IEntityTypeConfiguration<ProductAttribute>
{
    public void Configure(EntityTypeBuilder<ProductAttribute> entity)
    {
        entity.ToTable("ProductAttribute");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityColumn();

        entity.Property(e => e.Bezeichnung).HasMaxLength(100).IsRequired();
        entity.Property(e => e.Feldtyp)    .HasConversion<int>()
                                            .HasDefaultValue(AttributeFieldType.Lookup);
        entity.Property(e => e.MaxLaenge)  .IsRequired(false);
        entity.Property(e => e.Aktiv)      .HasDefaultValue(true);

        entity.Property(e => e.ErstelltAm)  .HasDefaultValueSql("GETDATE()");
        entity.Property(e => e.ErstelltVon) .HasMaxLength(100).IsRequired();
        entity.Property(e => e.GeaendertVon).HasMaxLength(100);

        entity.HasIndex(e => e.Bezeichnung).IsUnique();
    }
}