using DNR26V2.Data.Context;
using DNR26V2.Domain.Entities.System;
using DNR26V2.Forms.Base;
using Microsoft.EntityFrameworkCore;

namespace DNR26V2.Forms.Settings;

public partial class FrmAppSetup : BaseCardForm
{
    private readonly AppDbContext _db;
    private AppSetup? _setup;

    public FrmAppSetup(AppDbContext db)
    {
        _db = db;
        InitializeComponent();
    }

    private async void FrmAppSetup_Load(object sender, EventArgs e)
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        try
        {
            _setup = await _db.AppSetup.FirstOrDefaultAsync();

            if (_setup is null)
            {
                ShowError("Keine Einstellungen gefunden. Bitte Datenbank initialisieren.");
                return;
            }

            // Firmendaten
            txtFirmenname.Text          = _setup.Firmenname;
            txtFirmenadresse.Text       = _setup.Firmenadresse ?? string.Empty;
            txtFirmenPLZ.Text           = _setup.FirmenPLZ ?? string.Empty;
            txtFirmenOrt.Text           = _setup.FirmenOrt ?? string.Empty;
            txtFirmenLand.Text          = _setup.FirmenLand;
            txtFirmenTelefon.Text       = _setup.FirmenTelefon ?? string.Empty;
            txtFirmenEmail.Text         = _setup.FirmenEmail ?? string.Empty;
            txtSteuernummer.Text        = _setup.FirmenSteuernummer ?? string.Empty;
            txtUStIdNr.Text             = _setup.FirmenUStIdNr ?? string.Empty;

            // Drucker
            txtDrucker1.Text            = _setup.DruckerWeissesPapier ?? string.Empty;
            txtDrucker2.Text            = _setup.DruckerMitLogo ?? string.Empty;

            // Sonstige
            nudMwst.Value               = (decimal)_setup.StandardMwstProzent;
            nudSeitengroesse.Value      = _setup.SeitenGroesse;

            // NoSeries in DataGridView laden
            await LoadNoSeriesAsync();
        }
        catch (Exception ex)
        {
            ShowError($"Fehler beim Laden der Einstellungen:\n\n{ex.Message}");
        }
    }

    private async Task LoadNoSeriesAsync()
    {
        var liste = await _db.NoSeries.OrderBy(n => n.Seriencode).ToListAsync();
        dgwNoSeries.DataSource = liste;
        ConfigureNoSeriesGrid();
    }

    private void ConfigureNoSeriesGrid()
    {
        if (dgwNoSeries.Columns.Count == 0) return;

        var hidden = new[] { "Id", "ErstelltAm", "ErstelltVon", "GeaendertAm", "GeaendertVon" };
        foreach (var col in hidden)
            if (dgwNoSeries.Columns.Contains(col))
                dgwNoSeries.Columns[col].Visible = false;

        dgwNoSeries.Columns["Seriencode"].HeaderText           = "Code";
        dgwNoSeries.Columns["Beschreibung"].HeaderText         = "Bezeichnung";
        dgwNoSeries.Columns["LetzteVerwendeteNr"].HeaderText   = "Letzte Nummer";
        dgwNoSeries.Columns["LetztesVerwendetesDatum"].HeaderText = "Zuletzt verwendet";
        dgwNoSeries.Columns["Aktiv"].HeaderText                = "Aktiv";
    }

    private async void BtnSpeichern_Click(object sender, EventArgs e)
    {
        if (_setup is null) return;

        try
        {
            _setup.Firmenname          = txtFirmenname.Text.Trim();
            _setup.Firmenadresse       = txtFirmenadresse.Text.NullIfEmpty();
            _setup.FirmenPLZ           = txtFirmenPLZ.Text.NullIfEmpty();
            _setup.FirmenOrt           = txtFirmenOrt.Text.NullIfEmpty();
            _setup.FirmenLand          = txtFirmenLand.Text.Trim();
            _setup.FirmenTelefon       = txtFirmenTelefon.Text.NullIfEmpty();
            _setup.FirmenEmail         = txtFirmenEmail.Text.NullIfEmpty();
            _setup.FirmenSteuernummer  = txtSteuernummer.Text.NullIfEmpty();
            _setup.FirmenUStIdNr       = txtUStIdNr.Text.NullIfEmpty();
            _setup.DruckerWeissesPapier = txtDrucker1.Text.NullIfEmpty();
            _setup.DruckerMitLogo      = txtDrucker2.Text.NullIfEmpty();
            _setup.StandardMwstProzent = nudMwst.Value;
            _setup.SeitenGroesse       = (int)nudSeitengroesse.Value;

            _db.AppSetup.Update(_setup);
            await _db.SaveChangesAsync();

            ShowSuccess("Einstellungen erfolgreich gespeichert.");
        }
        catch (Exception ex)
        {
            ShowError($"Fehler beim Speichern:\n\n{ex.Message}");
        }
    }

    private void BtnSchliessen_Click(object sender, EventArgs e) => Close();
}

// Kleine Hilfs-Extension – nur für diesen Kontext
internal static class StringExtensions
{
    public static string? NullIfEmpty(this string? s)
        => string.IsNullOrWhiteSpace(s) ? null : s.Trim();
}