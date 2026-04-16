using DNR26V2.Domain.Entities.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DNR26V2.Data.Configurations;

internal sealed class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> entity)
    {
        entity.ToTable("AuditLog");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityColumn();

        entity.Property(e => e.Tabellenname).HasMaxLength(100).IsRequired();
        entity.Property(e => e.Belegnummer) .HasMaxLength(50);
        entity.Property(e => e.Aktion)      .HasMaxLength(50).IsRequired();
        entity.Property(e => e.AlterWert)   .HasColumnType("nvarchar(max)");
        entity.Property(e => e.NeuerWert)   .HasColumnType("nvarchar(max)");
        entity.Property(e => e.Grund)       .HasMaxLength(500);
        entity.Property(e => e.Benutzer)    .HasMaxLength(100).IsRequired();
        entity.Property(e => e.Zeitstempel) .HasDefaultValueSql("GETDATE()");
        entity.Property(e => e.IPAdresse)   .HasMaxLength(50);

        // Schnelle Suche nach Tabelle + Datensatz + Benutzer
        entity.HasIndex(e => e.Tabellenname);
        entity.HasIndex(e => e.Benutzer);
        entity.HasIndex(e => e.Zeitstempel);
    }
}