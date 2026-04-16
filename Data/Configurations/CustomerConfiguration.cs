using DNR26V2.Domain.Entities.MasterData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DNR26V2.Data.Configurations;

internal sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> entity)
    {
        entity.ToTable("Customer");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityColumn();

        entity.Property(e => e.Kundennummer).HasMaxLength(20).IsRequired();
        entity.Property(e => e.Kundenname)  .HasMaxLength(200).IsRequired();
        entity.Property(e => e.Name2)       .HasMaxLength(200);
        entity.Property(e => e.Inhaber)     .HasMaxLength(200);

        entity.Property(e => e.Adresse) .HasMaxLength(300);
        entity.Property(e => e.Adresse2).HasMaxLength(300);
        entity.Property(e => e.PLZ)     .HasMaxLength(10);
        entity.Property(e => e.Ort)     .HasMaxLength(100);
        entity.Property(e => e.Land)    .HasMaxLength(100).HasDefaultValue("Deutschland");

        entity.Property(e => e.Telefonnummer).HasMaxLength(50);
        entity.Property(e => e.Handynummer)  .HasMaxLength(50);
        entity.Property(e => e.EMail)        .HasMaxLength(200);

        entity.Property(e => e.ALName2)   .HasMaxLength(200);
        entity.Property(e => e.ALInhaber) .HasMaxLength(200);
        entity.Property(e => e.ALAdresse) .HasMaxLength(300);
        entity.Property(e => e.ALAdresse2).HasMaxLength(300);
        entity.Property(e => e.ALPLZ)     .HasMaxLength(10);
        entity.Property(e => e.ALOrt)     .HasMaxLength(100);
        entity.Property(e => e.ALLand)    .HasMaxLength(100);

        entity.Property(e => e.Routenfolge).HasDefaultValue(0);
        entity.Property(e => e.Limit)      .HasPrecision(10, 2).HasDefaultValue(0m);

        entity.Property(e => e.Geraete1).HasMaxLength(200);
        entity.Property(e => e.Geraete2).HasMaxLength(200);
        entity.Property(e => e.Geraete3).HasMaxLength(200);
        entity.Property(e => e.Geraete4).HasMaxLength(200);
        entity.Property(e => e.Geraete5).HasMaxLength(200);

        entity.Property(e => e.Notizen).HasColumnType("nvarchar(max)");
        entity.Property(e => e.Aktiv)  .HasDefaultValue(true);
        entity.Property(e => e.Offen)  .HasDefaultValue(true);

        entity.HasIndex(e => e.Kundennummer).IsUnique();

        // StandardTur → Route (DB: NO ACTION, App logic handles set-null)
        entity.HasOne(e => e.Tur)
              .WithMany()
              .HasForeignKey(e => e.TurId)
              .OnDelete(DeleteBehavior.NoAction);

        // AusnahmeTur → Route (DB: NO ACTION)
        entity.HasOne(e => e.AusnahmeTur)
              .WithMany()
              .HasForeignKey(e => e.AusnahmeTurId)
              .OnDelete(DeleteBehavior.NoAction);

        // Kundenfilter FK — ebenfalls NO ACTION falls SetNull zuvor gesetzt
        entity.HasOne(e => e.KundenfilterNavigation)
              .WithMany()
              .HasForeignKey(e => e.KundenfilterId)
              .OnDelete(DeleteBehavior.NoAction);

        entity.Property(e => e.ErstelltAm)  .HasDefaultValueSql("GETDATE()");
        entity.Property(e => e.ErstelltVon) .HasMaxLength(100).IsRequired();
        entity.Property(e => e.GeaendertVon).HasMaxLength(100);
    }
}