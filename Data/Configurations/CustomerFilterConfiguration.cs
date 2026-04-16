using DNR26V2.Domain.Entities.MasterData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DNR26V2.Data.Configurations;

internal sealed class CustomerFilterConfiguration : IEntityTypeConfiguration<CustomerFilter>
{
    public void Configure(EntityTypeBuilder<CustomerFilter> entity)
    {
        entity.ToTable("CustomerFilter");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityColumn();

        entity.Property(e => e.Kundenfilter).HasMaxLength(100).IsRequired();
        entity.HasIndex(e => e.Kundenfilter).IsUnique();

        entity.Property(e => e.ErstelltAm) .HasDefaultValueSql("GETDATE()");
        entity.Property(e => e.ErstelltVon).HasMaxLength(100).IsRequired();
        entity.Property(e => e.GeaendertVon).HasMaxLength(100);
    }
}