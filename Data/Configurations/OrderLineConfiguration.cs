using DNR26V2.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DNR26V2.Data.Configurations;

internal sealed class OrderLineConfiguration : IEntityTypeConfiguration<OrderLine>
{
    public void Configure(EntityTypeBuilder<OrderLine> entity)
    {
        entity.ToTable("OrderLines");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityColumn();

        entity.Property(e => e.Menge).HasPrecision(10, 3).HasDefaultValue(0m);
        entity.Property(e => e.Gewicht).HasPrecision(10, 3).HasDefaultValue(0m);
        entity.Property(e => e.Preis).HasPrecision(10, 2).HasDefaultValue(0m);
        entity.Property(e => e.Notiz).HasMaxLength(500);

        entity.HasOne(e => e.Auftrag)
              .WithMany(o => o.Zeilen)
              .HasForeignKey(e => e.AuftragId)
              .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(e => e.Artikel)
              .WithMany()
              .HasForeignKey(e => e.ArtikelId)
              .OnDelete(DeleteBehavior.NoAction);

        entity.Property(e => e.ErstelltAm).HasDefaultValueSql("GETDATE()");
        entity.Property(e => e.ErstelltVon).HasMaxLength(100).IsRequired();
        entity.Property(e => e.GeaendertVon).HasMaxLength(100);

        entity.Property(e => e.GrossAmount).HasColumnType("decimal(18,4)").HasDefaultValue(0m);
        entity.Property(e => e.DiscountProzent).HasColumnType("decimal(5,2)").HasDefaultValue(0m);
        entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0m);
        entity.Property(e => e.MwstProzent).HasColumnType("decimal(5,2)").HasDefaultValue(7.00m);
        entity.Property(e => e.LineAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0m);
        entity.Property(e => e.VatAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0m);
        entity.Property(e => e.AmountInclVat).HasColumnType("decimal(18,2)").HasDefaultValue(0m);
    }
}