using DNR26V2.Domain.Entities.Invoices;
using DNR26V2.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DNR26V2.Data.Configurations.Invoices;

public class InvoiceHeaderConfiguration : IEntityTypeConfiguration<InvoiceHeader>
{
    public void Configure(EntityTypeBuilder<InvoiceHeader> b)
    {
        b.ToTable("Invoices");

        b.HasKey(x => x.Id);

        b.Property(x => x.Rechnungsnummer)
            .IsRequired()
            .HasMaxLength(20);

        b.HasIndex(x => x.Rechnungsnummer)
            .IsUnique();

        b.Property(x => x.Rechnungsdatum)
            .IsRequired();

        b.Property(x => x.Von)
            .IsRequired();

        b.Property(x => x.Bis)
            .IsRequired();

        b.Property(x => x.Status)
            .HasDefaultValue(InvoiceStatus.Offen);

        b.Property(x => x.Notiz)
            .HasMaxLength(500);

        b.Property(x => x.IstSammelrechnung)
            .HasDefaultValue(false);

        b.Property(x => x.ErstelltAm)
            .HasDefaultValueSql("GETDATE()");

        b.Property(x => x.ErstelltVon)
            .IsRequired()
            .HasMaxLength(100);

        b.Property(x => x.GeaendertVon)
            .HasMaxLength(100);

        b.Property(x => x.Gesamtnetto).HasColumnType("decimal(18,2)").HasDefaultValue(0m);
        b.Property(x => x.Gesamtmwst).HasColumnType("decimal(18,2)").HasDefaultValue(0m);
        b.Property(x => x.Gesamtbrutto).HasColumnType("decimal(18,2)").HasDefaultValue(0m);

        b.HasOne(x => x.Kunde)
            .WithMany()
            .HasForeignKey(x => x.KundeId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        b.HasMany(x => x.Zeilen)
            .WithOne(z => z.Rechnung)
            .HasForeignKey(z => z.RechnungId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}