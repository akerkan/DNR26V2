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

        // Pflichtfelder
        entity.Property(e => e.Kundennummer).HasMaxLength(20).IsRequired();
        entity.Property(e => e.Kundenname)  .HasMaxLength(200).IsRequired();
        entity.HasIndex(e => e.Kundennummer).IsUnique();

        // Adressfelder
        entity.Property(e => e.Name2)   .HasMaxLength(200);
        entity.Property(e => e.Inhaber) .HasMaxLength(200);
        entity.Property(e => e.Adresse) .HasMaxLength(200);
        entity.Property(e => e.Adresse2).HasMaxLength(200);
        entity.Property(e => e.PLZ)     .HasMaxLength(10);
        entity.Property(e => e.Ort)     .HasMaxLength(100);
        entity.Property(e => e.Land)    .HasMaxLength(100).HasDefaultValue("Deutschland");

        // Kontakt
        entity.Property(e => e.Telefonnummer).HasMaxLength(50);
        entity.Property(e => e.Handynummer)  .HasMaxLength(50);
        entity.Property(e => e.EMail)        .HasMaxLength(200);

        // Alternative Lieferadresse
        entity.Property(e => e.ALName2)   .HasMaxLength(200);
        entity.Property(e => e.ALInhaber) .HasMaxLength(200);
        entity.Property(e => e.ALAdresse) .HasMaxLength(200);
        entity.Property(e => e.ALAdresse2).HasMaxLength(200);
        entity.Property(e => e.ALPLZ)     .HasMaxLength(10);
        entity.Property(e => e.ALOrt)     .HasMaxLength(100);
        entity.Property(e => e.ALLand)    .HasMaxLength(100);

        // Finanzen
        entity.Property(e => e.Limit).HasPrecision(18, 2).HasDefaultValue(0m);

        // Geräte
        entity.Property(e => e.Geraete1).HasMaxLength(100);
        entity.Property(e => e.Geraete2).HasMaxLength(100);
        entity.Property(e => e.Geraete3).HasMaxLength(100);
        entity.Property(e => e.Geraete4).HasMaxLength(100);
        entity.Property(e => e.Geraete5).HasMaxLength(100);

        // Sonstiges
        entity.Property(e => e.Notizen).HasMaxLength(500);
        entity.Property(e => e.Aktiv)  .HasDefaultValue(true);
        entity.Property(e => e.Offen)  .HasDefaultValue(true);

        // Fremdschlüssel
        entity.HasOne(e => e.Route)
              .WithMany(r => r.Kunden)
              .HasForeignKey(e => e.RouteId)
              .OnDelete(DeleteBehavior.SetNull);

        entity.HasOne(e => e.KundenfilterNavigation)
              .WithMany(f => f.Kunden)
              .HasForeignKey(e => e.KundenfilterId)
              .OnDelete(DeleteBehavior.SetNull);

        // Audit
        entity.Property(e => e.ErstelltAm) .HasDefaultValueSql("GETDATE()");
        entity.Property(e => e.ErstelltVon).HasMaxLength(100).IsRequired();
        entity.Property(e => e.GeaendertVon).HasMaxLength(100);
    }
}