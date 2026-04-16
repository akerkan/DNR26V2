using System.Data;
using Dapper;
using DNR26V2.Data.Context;
using DNR26V2.Services.System;

namespace DNR26V2.Services.System;

public class NoSeriesService : INoSeriesService
{
    private readonly DapperContext _dapper;

    public NoSeriesService(DapperContext dapper)
    {
        _dapper = dapper;
    }

    public async Task<string> GetNextNumberAsync(string seriencode, DateTime belegdatum)
    {
        using var connection = _dapper.CreateConnection();
        connection.Open();

        // SERIALIZABLE verhindert Race Conditions bei gleichzeitigen Anfragen
        using var transaction = connection.BeginTransaction(IsolationLevel.Serializable);

        try
        {
            var row = await connection.QuerySingleOrDefaultAsync<NoSeriesRow>(
                @"SELECT LetzteVerwendeteNr,
                         LetztesVerwendetesDatum,
                         Praefix,
                         Nummernformat,
                         Trennzeichen
                  FROM   NoSeries WITH (UPDLOCK, ROWLOCK)
                  WHERE  Seriencode = @Seriencode
                  AND    Aktiv      = 1",
                new { Seriencode = seriencode },
                transaction);

            if (row is null)
                throw new InvalidOperationException(
                    $"Nummernserie '{seriencode}' nicht gefunden oder inaktiv.");

            var neueNummer = BerechneNaechsteNummer(row, belegdatum);

            await connection.ExecuteAsync(
                @"UPDATE NoSeries
                  SET    LetzteVerwendeteNr       = @Nr,
                         LetztesVerwendetesDatum  = @Datum,
                         GeaendertAm              = GETDATE(),
                         GeaendertVon             = @Benutzer
                  WHERE  Seriencode = @Seriencode",
                new
                {
                    Nr         = neueNummer,
                    Datum      = belegdatum.Date,
                    Seriencode = seriencode,
                    Benutzer   = Environment.UserName
                },
                transaction);

            transaction.Commit();
            return neueNummer;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    // ── Nummern-Berechnung ────────────────────────────────────────────────────

    private static string BerechneNaechsteNummer(NoSeriesRow row, DateTime datum)
    {
        char   sep    = row.Trennzeichen;
        string format = string.IsNullOrWhiteSpace(row.Nummernformat) ? "000" : row.Nummernformat;
        int    nextSeq = 1;

        // Gleiches Datum → Sequenz inkrementieren
        if (row.LetztesVerwendetesDatum.HasValue
            && row.LetztesVerwendetesDatum.Value.Date == datum.Date
            && !string.IsNullOrWhiteSpace(row.LetzteVerwendeteNr))
        {
            var parts = row.LetzteVerwendeteNr.Split(sep);
            if (parts.Length >= 4 && int.TryParse(parts[^1], out int last))
                nextSeq = last + 1;
        }

        return $"{row.Praefix}{sep}{datum:yy}{sep}{datum:MMdd}{sep}{nextSeq.ToString(format)}";
    }

    // ── Dapper-DTO ────────────────────────────────────────────────────────────

    private sealed record NoSeriesRow(
        string?  LetzteVerwendeteNr,
        DateTime? LetztesVerwendetesDatum,
        string   Praefix,
        string?  Nummernformat,
        char     Trennzeichen);
}