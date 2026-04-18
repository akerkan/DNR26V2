using DNR26V2.Data.Context;
using DNR26V2.Domain.Entities.System;
using Microsoft.EntityFrameworkCore;

namespace DNR26V2.Data.Seed;

public class DatabaseSeeder
{
    private readonly AppDbContext _db;

    public DatabaseSeeder(AppDbContext db)
    {
        _db = db;
    }

    public async Task SeedAsync()
    {
        // Migrationen anwenden – DB wird automatisch angelegt falls nicht vorhanden
        await _db.Database.MigrateAsync();

        await SeedLocationAsync();
        await SeedAppSetupAsync();
        await SeedNoSeriesAsync();
    }

    private async Task SeedLocationAsync()
    {
        if (await _db.Location.AnyAsync()) return;

        _db.Location.Add(new Location
        {
            Standortcode = "HAUPT",
            Bezeichnung  = "Hauptstandort",
            Land         = "Deutschland",
            IstStandard  = true,
            Aktiv        = true,
            ErstelltVon  = "SYSTEM"
        });

        await _db.SaveChangesAsync();
    }

    private async Task SeedAppSetupAsync()
    {
        if (await _db.AppSetup.AnyAsync()) return;

        _db.AppSetup.Add(new AppSetup
        {
            Id                   = 1,
            Firmenname           = "Mein Unternehmen",
            FirmenLand           = "Deutschland",
            StandardMwstProzent  = 7.00m,
            StandardStandortCode = "HAUPT",
            RechnungPraefix      = "RE",
            LieferscheinPraefix  = "LS",
            GutschriftPraefix    = "GS",
            ZahlungPraefix       = "ZA",
            SeitenGroesse        = 20,
            ErstelltVon          = "SYSTEM"
        });

        await _db.SaveChangesAsync();
    }

    private async Task SeedNoSeriesAsync()
    {
        if (await _db.NoSeries.AnyAsync()) return;

        _db.NoSeries.AddRange(
            new NoSeries { Seriencode = "RE",  Beschreibung = "Rechnungsnummer",    Praefix = "RE",  ErstelltVon = "SYSTEM" },
            new NoSeries { Seriencode = "LS",  Beschreibung = "Lieferscheinnummer", Praefix = "LS",  ErstelltVon = "SYSTEM" },
            new NoSeries { Seriencode = "GS",  Beschreibung = "Gutschriftsnummer",  Praefix = "GS",  ErstelltVon = "SYSTEM" },
            new NoSeries { Seriencode = "ZA",  Beschreibung = "Zahlungsnummer",     Praefix = "ZA",  ErstelltVon = "SYSTEM" },
            new NoSeries { Seriencode = "AUF", Beschreibung = "Auftragsnummer",     Praefix = "AUF", ErstelltVon = "SYSTEM" }
        );

        await _db.SaveChangesAsync();
    }
}