using DNR26V2.Domain.Entities.Deliveries;
using DNR26V2.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DNR26V2.Data.Configurations;

internal sealed class DeliveryHeaderConfiguration : IEntityTypeConfiguration<DeliveryHeader>
{
    public void Configure(EntityTypeBuilder<DeliveryHeader> entity)
    {
        entity.ToTable("Deliveries");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityColumn();

        entity.Property(e => e.Lieferscheinnummer).HasMaxLength(20).IsRequired();
        entity.Property(e => e.Status).HasDefaultValue(DeliveryStatus.Offen);
        entity.Property(e => e.Notiz).HasMaxLength(500);

        entity.HasIndex(e => e.Lieferscheinnummer).IsUnique();

        entity.HasOne(e => e.Kunde)
              .WithMany()
              .HasForeignKey(e => e.KundeId)
              .OnDelete(DeleteBehavior.NoAction);

        entity.HasOne(e => e.Auftrag)
              .WithMany()
              .HasForeignKey(e => e.AuftragId)
              .OnDelete(DeleteBehavior.NoAction);

        entity.Property(e => e.ErstelltAm).HasDefaultValueSql("GETDATE()");
        entity.Property(e => e.ErstelltVon).HasMaxLength(100).IsRequired();
        entity.Property(e => e.GeaendertVon).HasMaxLength(100);
    }
}