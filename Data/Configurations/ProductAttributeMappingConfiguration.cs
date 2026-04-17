using DNR26V2.Domain.Entities.MasterData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DNR26V2.Data.Configurations;

internal sealed class ProductAttributeMappingConfiguration : IEntityTypeConfiguration<ProductAttributeMapping>
{
    public void Configure(EntityTypeBuilder<ProductAttributeMapping> entity)
    {
        entity.ToTable("ProductAttributeMapping");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityColumn();

        entity.Property(e => e.FreierText).HasMaxLength(500).IsRequired(false);

        // FK → Product
        entity.HasOne(e => e.Artikel)
              .WithMany()
              .HasForeignKey(e => e.ArtikelId)
              .OnDelete(DeleteBehavior.Cascade);

        // FK → ProductAttribute
        entity.HasOne(e => e.Attribut)
              .WithMany(a => a.Mappings)
              .HasForeignKey(e => e.AttributId)
              .OnDelete(DeleteBehavior.Restrict);

        // FK → ProductAttributeValue (nullable)
        entity.HasOne(e => e.AttributWert)
              .WithMany(v => v.Mappings)
              .HasForeignKey(e => e.AttributWertId)
              .IsRequired(false)
              .OnDelete(DeleteBehavior.Restrict);

        // Ein Artikel kann jedes Attribut nur einmal haben
        entity.HasIndex(e => new { e.ArtikelId, e.AttributId }).IsUnique();
    }
}