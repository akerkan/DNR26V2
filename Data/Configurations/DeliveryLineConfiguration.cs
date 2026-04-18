using DNR26V2.Domain.Entities.Deliveries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DNR26V2.Data.Configurations;

internal sealed class DeliveryLineConfiguration : IEntityTypeConfiguration<DeliveryLine>
{
    public void Configure(EntityTypeBuilder<DeliveryLine> entity)
    {
        entity.ToTable("DeliveryLines");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityColumn();

        entity.Property(e => e.Menge).HasPrecision(10, 3).HasDefaultValue(0m);
        entity.Property(e => e.MengeGeliefert).HasPrecision(10, 3).HasDefaultValue(0m);
        entity.Property(e => e.Gewicht).HasPrecision(10, 3).HasDefaultValue(0m);
        entity.Property(e => e.Preis).HasPrecision(10, 2).HasDefaultValue(0m);
        entity.Property(e => e.Notiz).HasMaxLength(500);

        entity.HasOne(e => e.Lieferschein)
              .WithMany(d => d.Zeilen)
              .HasForeignKey(e => e.LieferscheinId)
              .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(e => e.Artikel)
              .WithMany()
              .HasForeignKey(e => e.ArtikelId)
              .OnDelete(DeleteBehavior.NoAction);

        entity.Property(e => e.ErstelltAm).HasDefaultValueSql("GETDATE()");
        entity.Property(e => e.ErstelltVon).HasMaxLength(100).IsRequired();
        entity.Property(e => e.GeaendertVon).HasMaxLength(100);
    }
}