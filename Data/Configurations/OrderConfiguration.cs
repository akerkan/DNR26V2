using DNR26V2.Domain.Entities.Orders;
using DNR26V2.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DNR26V2.Data.Configurations;

internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> entity)
    {
        entity.ToTable("Orders");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityColumn();

        entity.Property(e => e.Auftragsnummer).HasMaxLength(20).IsRequired();
        entity.Property(e => e.Status).HasDefaultValue(OrderStatus.Offen);
        entity.Property(e => e.Notiz).HasMaxLength(500);

        entity.HasIndex(e => e.Auftragsnummer).IsUnique();
        entity.HasIndex(e => new { e.KundeId, e.LieferDatum })
              .IsUnique()
              .HasFilter("[Status] <> 2");

        entity.HasOne(e => e.Kunde)
              .WithMany()
              .HasForeignKey(e => e.KundeId)
              .OnDelete(DeleteBehavior.NoAction);

        entity.Property(e => e.ErstelltAm).HasDefaultValueSql("GETDATE()");
        entity.Property(e => e.ErstelltVon).HasMaxLength(100).IsRequired();
        entity.Property(e => e.GeaendertVon).HasMaxLength(100);
    }
}