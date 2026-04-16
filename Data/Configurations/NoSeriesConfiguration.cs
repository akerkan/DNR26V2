using DNR26V2.Domain.Entities.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DNR26V2.Data.Configurations;

internal sealed class NoSeriesConfiguration : IEntityTypeConfiguration<NoSeries>
{
    public void Configure(EntityTypeBuilder<NoSeries> entity)
    {
        entity.ToTable("NoSeries");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityColumn();

        entity.Property(e => e.Seriencode)               .HasMaxLength(20).IsRequired();
        entity.Property(e => e.Beschreibung)             .HasMaxLength(200);
        entity.Property(e => e.Praefix)                  .HasMaxLength(10).IsRequired();
        entity.Property(e => e.LetzteVerwendeteNr)       .HasMaxLength(50);
        entity.Property(e => e.LetztesVerwendetesDatum)  .HasColumnType("date");
        entity.Property(e => e.Nummernformat)            .HasMaxLength(10).HasDefaultValue("000");
        entity.Property(e => e.Trennzeichen)             .HasColumnType("nchar(1)").HasDefaultValue('-');
        entity.Property(e => e.Aktiv)                    .HasDefaultValue(true);

        entity.Property(e => e.ErstelltAm)  .HasDefaultValueSql("GETDATE()");
        entity.Property(e => e.ErstelltVon) .HasMaxLength(100).IsRequired();
        entity.Property(e => e.GeaendertVon).HasMaxLength(100);

        entity.HasIndex(e => e.Seriencode).IsUnique();
    }
}