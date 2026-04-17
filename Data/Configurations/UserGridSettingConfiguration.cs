using DNR26V2.Domain.Entities.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DNR26V2.Data.Configurations;

internal sealed class UserGridSettingConfiguration : IEntityTypeConfiguration<UserGridSetting>
{
    public void Configure(EntityTypeBuilder<UserGridSetting> entity)
    {
        entity.ToTable("UserGridSetting");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityColumn();

        entity.Property(e => e.Benutzername) .HasMaxLength(100).IsRequired();
        entity.Property(e => e.GridKey)      .HasMaxLength(200).IsRequired();
        entity.Property(e => e.Einstellungen).HasColumnType("nvarchar(max)").IsRequired();
        entity.Property(e => e.GeaendertAm)  .HasDefaultValueSql("GETDATE()");

        entity.HasIndex(e => new { e.Benutzername, e.GridKey }).IsUnique();
    }
}