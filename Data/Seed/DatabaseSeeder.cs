using DNR26V2.Data.Context;
using DNR26V2.Domain.Entities.System;
using Microsoft.EntityFrameworkCore;

namespace DNR26V2.Data.Seed;

/// <summary>
/// Erstellt Startwerte wenn die Tabellen leer sind.
/// Wird beim Anwendungsstart ausgeführt (idempotent).
/// </summary>
public class DatabaseSeeder
{
    private readonly AppDbContext _db;

    public DatabaseSeeder(AppDbContext db)
    {
        _db = db;
    }

    public async Task SeedAsync()
    {
        // Migrationen anwenden (lokale DB wird ggf. angelegt)
        await _db.Database.MigrateAsync();

        await SeedLocationAsync();
        await SeedAppSetupAsync();
        await SeedNoSeriesAsync();
    }

    // ── Standort ─────────────────────────────────────────────────────────────
    private async Task SeedLocationAsync()
    {
        if (await _db.Location.AnyAsync())
            return;

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

    // ── AppSetup ──────────────────────────────────────────────────────────────
    private async Task SeedAppSetupAsync()
    {
        if (await _db.AppSetup.AnyAsync())
            return;

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

    // ── NoSeries ──────────────────────────────────────────────────────────────
    private async Task SeedNoSeriesAsync()
    {
        if (await _db.NoSeries.AnyAsync())
            return;

        var serien = new[]
        {
            new NoSeries { Seriencode = "RE", Beschreibung = "Rechnungsnummer",      Praefix = "RE",  ErstelltVon = "SYSTEM" },
            new NoSeries { Seriencode = "LS", Beschreibung = "Lieferscheinnummer",   Praefix = "LS",  ErstelltVon = "SYSTEM" },
            new NoSeries { Seriencode = "GS", Beschreibung = "Gutschriftsnummer",    Praefix = "GS",  ErstelltVon = "SYSTEM" },
            new NoSeries { Seriencode = "ZA", Beschreibung = "Zahlungsnummer",       Praefix = "ZA",  ErstelltVon = "SYSTEM" },
            new NoSeries { Seriencode = "AU", Beschreibung = "Auftragsnummer",       Praefix = "AU",  ErstelltVon = "SYSTEM" }
        };

        _db.NoSeries.AddRange(serien);
        await _db.SaveChangesAsync();
    }
}