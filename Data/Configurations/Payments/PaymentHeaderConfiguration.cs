using DNR26V2.Domain.Entities.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DNR26V2.Data.Configurations.Payments;

internal sealed class PaymentHeaderConfiguration : IEntityTypeConfiguration<PaymentHeader>
{
    public void Configure(EntityTypeBuilder<PaymentHeader> b)
    {
        b.ToTable("PaymentHeaders");
        b.HasKey(x => x.Id);
        b.Property(x => x.Buchungsdatum).IsRequired();
        b.Property(x => x.Notiz).HasMaxLength(500);
        b.Property(x => x.ErstelltVon).HasMaxLength(100).IsRequired();
        b.Property(x => x.ErstelltAm).HasDefaultValueSql("GETDATE()");
        b.HasMany(x => x.Lines).WithOne(l => l.Header).HasForeignKey(l => l.PaymentHeaderId).OnDelete(DeleteBehavior.Cascade);
    }
}
