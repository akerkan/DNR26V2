using DNR26V2.Domain.Entities.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DNR26V2.Data.Configurations;

internal sealed class AppSetupConfiguration : IEntityTypeConfiguration<AppSetup>
{
    public void Configure(EntityTypeBuilder<AppSetup> entity)
    {
        entity.ToTable("AppSetup");
        entity.HasKey(e => e.Id);

        // Singleton: Id wird nie automatisch generiert, CHECK sperrt Id ≠ 1
        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.HasCheckConstraint("CK_AppSetup_Singleton", "[Id] = 1");

        entity.Property(e => e.Firmenname)         .HasMaxLength(200).IsRequired();
        entity.Property(e => e.Firmenadresse)       .HasMaxLength(500);
        entity.Property(e => e.FirmenPLZ)           .HasMaxLength(10);
        entity.Property(e => e.FirmenOrt)           .HasMaxLength(100);
        entity.Property(e => e.FirmenLand)          .HasMaxLength(100).HasDefaultValue("Deutschland");
        entity.Property(e => e.FirmenTelefon)       .HasMaxLength(50);
        entity.Property(e => e.FirmenEmail)         .HasMaxLength(200);
        entity.Property(e => e.FirmenSteuernummer)  .HasMaxLength(50);
        entity.Property(e => e.FirmenUStIdNr)       .HasMaxLength(50);
        entity.Property(e => e.StandardMwstProzent) .HasPrecision(5, 2).HasDefaultValue(7.00m);
        entity.Property(e => e.StandardStandortCode).HasMaxLength(20);
        entity.Property(e => e.DruckerWeissesPapier).HasMaxLength(200);
        entity.Property(e => e.DruckerMitLogo)      .HasMaxLength(200);
        entity.Property(e => e.RechnungPraefix)     .HasMaxLength(10).HasDefaultValue("RE");
        entity.Property(e => e.LieferscheinPraefix) .HasMaxLength(10).HasDefaultValue("LS");
        entity.Property(e => e.GutschriftPraefix)   .HasMaxLength(10).HasDefaultValue("GS");
        entity.Property(e => e.ZahlungPraefix)      .HasMaxLength(10).HasDefaultValue("ZA");
        entity.Property(e => e.SeitenGroesse)        .HasDefaultValue(20);

        entity.Property(e => e.ErstelltAm)  .HasDefaultValueSql("GETDATE()");
        entity.Property(e => e.ErstelltVon) .HasMaxLength(100).IsRequired();
        entity.Property(e => e.GeaendertVon).HasMaxLength(100);
    }
}