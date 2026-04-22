using DNR26V2.Domain.Entities.Orders;
using DNR26V2.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DNR26V2.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).UseIdentityColumn();

        builder.Property(o => o.Auftragsnummer).IsRequired().HasMaxLength(20);
        builder.Property(o => o.Status).HasDefaultValue(OrderStatus.Offen);
        builder.Property(o => o.Notiz).HasMaxLength(500);

        builder.HasIndex(o => o.Auftragsnummer).IsUnique();
        builder.HasIndex(o => new { o.KundeId, o.LieferDatum })
               .IsUnique()
               .HasFilter("[Status] <> 3 AND [Status] <> 4");

        builder.HasOne(o => o.Kunde)
               .WithMany()
               .HasForeignKey(o => o.KundeId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.Property(o => o.ErstelltAm).HasDefaultValueSql("GETDATE()");
        builder.Property(o => o.ErstelltVon).HasMaxLength(100).IsRequired();
        builder.Property(o => o.GeaendertVon).HasMaxLength(100);

        builder.Property(e => e.Gesamtnetto).HasColumnType("decimal(18,2)").HasDefaultValue(0m);
        builder.Property(e => e.Gesamtmwst).HasColumnType("decimal(18,2)").HasDefaultValue(0m);
        builder.Property(e => e.Gesamtbrutto).HasColumnType("decimal(18,2)").HasDefaultValue(0m);
    }
}