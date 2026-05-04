using DNR26V2.Domain.Entities.Etikett;

namespace DNR26V2.Forms.Etikett;

/// <summary>
/// Simple dialog to edit font and color properties of a label field.
/// Position and size are changed interactively via drag/resize in the designer.
/// </summary>
public partial class FrmEtiketFieldProperties : Form
{
    private readonly EtiketLayoutField _field;

    public FrmEtiketFieldProperties(EtiketLayoutField field)
    {
        _field = field;
        InitializeComponent();
        Load += (_, _) => PopulateFields();
    }

    private void PopulateFields()
    {
        // Populate font combo with system fonts
        cmbFontName.Items.Clear();
        foreach (var ff in FontFamily.Families)
            cmbFontName.Items.Add(ff.Name);
        cmbFontName.Text = _field.FontName;

        nudFontSize.Value    = (decimal)Math.Max(4f, _field.FontSize);
        chkBold.Checked      = _field.Bold;
        chkItalic.Checked    = _field.Italic;
        chkVisible.Checked   = _field.Visible;
        cmbAlign.SelectedIndex = Math.Clamp(_field.TextAlignH, 0, 2);

        btnForeColor.BackColor = ParseColor(_field.ForeColorHex, Color.Black);
        btnForeColor.ForeColor = ContrastColor(btnForeColor.BackColor);
        btnBackColor.BackColor = ParseColor(_field.BackColorHex, Color.White);
        btnBackColor.ForeColor = ContrastColor(btnBackColor.BackColor);
    }

    private void BtnForeColor_Click(object? sender, EventArgs e)
    {
        using var dlg = new ColorDialog { Color = btnForeColor.BackColor, FullOpen = true };
        if (dlg.ShowDialog() != DialogResult.OK) return;
        btnForeColor.BackColor = dlg.Color;
        btnForeColor.ForeColor = ContrastColor(dlg.Color);
    }

    private void BtnBackColor_Click(object? sender, EventArgs e)
    {
        using var dlg = new ColorDialog { Color = btnBackColor.BackColor, FullOpen = true };
        if (dlg.ShowDialog() != DialogResult.OK) return;
        btnBackColor.BackColor = dlg.Color;
        btnBackColor.ForeColor = ContrastColor(dlg.Color);
    }

    private void BtnOK_Click(object? sender, EventArgs e)
    {
        _field.FontName    = cmbFontName.Text.Trim();
        _field.FontSize    = (float)nudFontSize.Value;
        _field.Bold        = chkBold.Checked;
        _field.Italic      = chkItalic.Checked;
        _field.Visible     = chkVisible.Checked;
        _field.TextAlignH  = cmbAlign.SelectedIndex;
        _field.ForeColorHex= ColorTranslator.ToHtml(btnForeColor.BackColor);
        _field.BackColorHex= ColorTranslator.ToHtml(btnBackColor.BackColor);

        DialogResult = DialogResult.OK;
        Close();
    }

    private void BtnAbbrechen_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private static Color ParseColor(string hex, Color fallback)
    {
        try { return ColorTranslator.FromHtml(hex); }
        catch { return fallback; }
    }

    // Choose black or white text so it's readable on any background
    private static Color ContrastColor(Color bg)
        => (bg.R * 0.299 + bg.G * 0.587 + bg.B * 0.114) > 128
            ? Color.Black : Color.White;
}
