using DNR26V2.Domain.Entities.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DNR26V2.Data.Configurations.Payments;

internal sealed class PaymentLineConfiguration : IEntityTypeConfiguration<PaymentLine>
{
    public void Configure(EntityTypeBuilder<PaymentLine> b)
    {
        b.ToTable("PaymentLines");
        b.HasKey(x => x.Id);
        b.Property(x => x.ReferenceType).IsRequired();
        b.Property(x => x.ReferenceId).IsRequired();
        b.Property(x => x.PaymentMethod).IsRequired();
        b.Property(x => x.Amount).HasColumnType("decimal(18,2)").IsRequired();
        b.Property(x => x.Notiz).HasMaxLength(500);
    }
}
