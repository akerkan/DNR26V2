using DNR26V2.Domain.Entities.Etikett;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DNR26V2.Data.Configurations;

public class EtiketLayoutFieldConfiguration : IEntityTypeConfiguration<EtiketLayoutField>
{
    public void Configure(EntityTypeBuilder<EtiketLayoutField> builder)
    {
        builder.ToTable("EtiketLayoutFields");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.LayoutName).HasMaxLength(100).IsRequired();
        builder.Property(e => e.FontName).HasMaxLength(100);
        builder.Property(e => e.ForeColorHex).HasMaxLength(10);
        builder.Property(e => e.BackColorHex).HasMaxLength(10);
        builder.Property(e => e.FontSize).HasColumnType("real");
    }
}
