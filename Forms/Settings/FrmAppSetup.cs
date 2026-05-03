using DNR26V2.Data.Context;
using DNR26V2.Domain.Entities.System;
using DNR26V2.Domain.Enums;
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
            cmbDrucker1.Text                    = _setup.DruckerWeissesPapier ?? string.Empty;
            cmbDrucker2.Text                    = _setup.DruckerMitLogo ?? string.Empty;
            cmbDrucker3.Text                    = _setup.DruckerEtikett ?? string.Empty;
            chkEtiketInAuftrag.Checked          = _setup.EtiketButtonInAuftrag;
            chkEtiketInLieferschein.Checked     = _setup.EtiketButtonInLieferschein;

            // Wire up printer loading on DropDown (lazy, avoids slow startup)
            cmbDrucker1.DropDown -= CmbDrucker_DropDown;
            cmbDrucker2.DropDown -= CmbDrucker_DropDown;
            cmbDrucker3.DropDown -= CmbDrucker_DropDown;
            cmbDrucker1.DropDown += CmbDrucker_DropDown;
            cmbDrucker2.DropDown += CmbDrucker_DropDown;
            cmbDrucker3.DropDown += CmbDrucker_DropDown;

            // Sonstige
            nudMwst.Value               = (decimal)_setup.StandardMwstProzent;
            nudSeitengroesse.Value      = _setup.SeitenGroesse;

            // New: TurKontrolle
            chkTurKontrolle.Checked     = _setup.TurKontrolle;

            // NoSeries in DataGridView laden
            await LoadNoSeriesAsync();

            // In LoadDataAsync() — nach bestehendem Binding:
            cmbPreisFormel.SelectedIndex = (int)_setup.PreisFormel;
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
            _setup.DruckerWeissesPapier        = cmbDrucker1.Text.NullIfEmpty();
            _setup.DruckerMitLogo              = cmbDrucker2.Text.NullIfEmpty();
            _setup.DruckerEtikett              = cmbDrucker3.Text.NullIfEmpty();
            _setup.EtiketButtonInAuftrag       = chkEtiketInAuftrag.Checked;
            _setup.EtiketButtonInLieferschein  = chkEtiketInLieferschein.Checked;
            _setup.StandardMwstProzent = nudMwst.Value;
            _setup.SeitenGroesse       = (int)nudSeitengroesse.Value;

            // New: TurKontrolle speichern
            _setup.TurKontrolle        = chkTurKontrolle.Checked;

            // In BtnSpeichern_Click() — vor SaveAsync():
            _setup.PreisFormel          = (PreisFormel)cmbPreisFormel.SelectedIndex;

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

    /// <summary>
    /// Populates installed printers into the combo on first open.
    /// Preserves any manually typed value.
    /// </summary>
    private void CmbDrucker_DropDown(object? sender, EventArgs e)
    {
        if (sender is not ComboBox cmb) return;
        var current = cmb.Text;
        cmb.Items.Clear();
        foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            cmb.Items.Add(printer);
        cmb.Text = current;
    }
}

// Kleine Hilfs-Extension – nur für diesen Kontext
internal static class StringExtensions
{
    public static string? NullIfEmpty(this string? s)
        => string.IsNullOrWhiteSpace(s) ? null : s.Trim();
}