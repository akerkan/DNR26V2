using DNR26V2.Data.Context;
using DNR26V2.Domain.Entities.System;
using DNR26V2.Forms.Base;
using Microsoft.EntityFrameworkCore;

namespace DNR26V2.Forms.Settings;

public partial class FrmLocationSetup : BaseCardForm
{
    private readonly AppDbContext _db;
    private Location? _location;

    public FrmLocationSetup(AppDbContext db)
    {
        _db = db;
        InitializeComponent();
    }

    private async void FrmLocationSetup_Load(object sender, EventArgs e)
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        try
        {
            _location = await _db.Location.FirstOrDefaultAsync(l => l.IstStandard)
                        ?? await _db.Location.FirstOrDefaultAsync();

            if (_location is null)
            {
                ShowError("Kein Standort gefunden.");
                return;
            }

            txtStandortcode.Text = _location.Standortcode;
            txtBezeichnung.Text  = _location.Bezeichnung;
            txtAdresse.Text      = _location.Adresse ?? string.Empty;
            txtPLZ.Text          = _location.PLZ ?? string.Empty;
            txtOrt.Text          = _location.Ort ?? string.Empty;
            txtLand.Text         = _location.Land;
            chkAktiv.Checked     = _location.Aktiv;
        }
        catch (Exception ex)
        {
            ShowError($"Fehler beim Laden:\n\n{ex.Message}");
        }
    }

    private async void BtnSpeichern_Click(object sender, EventArgs e)
    {
        if (_location is null) return;

        if (string.IsNullOrWhiteSpace(txtStandortcode.Text) ||
            string.IsNullOrWhiteSpace(txtBezeichnung.Text))
        {
            ShowError("Standortcode und Bezeichnung sind Pflichtfelder.");
            return;
        }

        try
        {
            _location.Standortcode = txtStandortcode.Text.Trim().ToUpper();
            _location.Bezeichnung  = txtBezeichnung.Text.Trim();
            _location.Adresse      = txtAdresse.Text.NullIfEmpty();
            _location.PLZ          = txtPLZ.Text.NullIfEmpty();
            _location.Ort          = txtOrt.Text.NullIfEmpty();
            _location.Land         = txtLand.Text.Trim();
            _location.Aktiv        = chkAktiv.Checked;

            _db.Location.Update(_location);
            await _db.SaveChangesAsync();

            ShowSuccess("Standort erfolgreich gespeichert.");
        }
        catch (Exception ex)
        {
            ShowError($"Fehler beim Speichern:\n\n{ex.Message}");
        }
    }

    private void BtnSchliessen_Click(object sender, EventArgs e) => Close();
}