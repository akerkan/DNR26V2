using DNR26V2.Domain.Entities.Invoices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DNR26V2.Data.Configurations.Invoices;

public class InvoiceLineConfiguration : IEntityTypeConfiguration<InvoiceLine>
{
    public void Configure(EntityTypeBuilder<InvoiceLine> b)
    {
        b.ToTable("InvoiceLines");

        b.HasKey(x => x.Id);

        b.Property(x => x.Menge)
            .HasPrecision(10, 3)
            .HasDefaultValue(0m);

        b.Property(x => x.FakturierteMenge)
            .HasPrecision(10, 3)
            .HasDefaultValue(0m);

        b.Property(x => x.Gewicht)
            .HasPrecision(10, 3)
            .HasDefaultValue(0m);

        b.Property(x => x.Preis)
            .HasPrecision(10, 2)
            .HasDefaultValue(0m);

        b.Property(x => x.MwstProzent)
            .HasPrecision(5, 2)
            .HasDefaultValue(7.00m);

        b.Property(x => x.Gesamtpreis)
            .HasPrecision(10, 2)
            .HasDefaultValue(0m);

        b.Property(x => x.Notiz)
            .HasMaxLength(500);

        b.Property(x => x.ErstelltAm)
            .HasDefaultValueSql("GETDATE()");

        b.Property(x => x.ErstelltVon)
            .IsRequired()
            .HasMaxLength(100);

        b.Property(x => x.GeaendertVon)
            .HasMaxLength(100);

        // Alle FK → NoAction (kein Cascade-Konflikt)
        b.HasOne(x => x.Lieferschein)
            .WithMany()
            .HasForeignKey(x => x.LieferscheinId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        b.HasOne(x => x.DeliveryLine)
            .WithMany()
            .HasForeignKey(x => x.DeliveryLineId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        b.HasOne(x => x.Artikel)
            .WithMany()
            .HasForeignKey(x => x.ArtikelId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
    }
}