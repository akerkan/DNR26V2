using DNR26V2.Domain.Entities.Etikett;

namespace DNR26V2.Forms.Etikett;

/// <summary>
/// Simple dialog to edit the style properties of a single label field.
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
        nudX.Value      = _field.X;
        nudY.Value      = _field.Y;
        nudWidth.Value  = _field.Width;
        nudHeight.Value = _field.Height;

        txtFontName.Text      = _field.FontName;
        nudFontSize.Value     = (decimal)_field.FontSize;
        chkBold.Checked       = _field.Bold;
        chkItalic.Checked     = _field.Italic;
        chkVisible.Checked    = _field.Visible;

        btnForeColor.BackColor = ParseColor(_field.ForeColorHex, Color.Black);
        btnBackColor.BackColor = ParseColor(_field.BackColorHex, Color.White);

        cmbAlign.SelectedIndex = _field.TextAlignH;
    }

    private void BtnForeColor_Click(object? sender, EventArgs e)
    {
        using var dlg = new ColorDialog { Color = btnForeColor.BackColor };
        if (dlg.ShowDialog() == DialogResult.OK)
            btnForeColor.BackColor = dlg.Color;
    }

    private void BtnBackColor_Click(object? sender, EventArgs e)
    {
        using var dlg = new ColorDialog { Color = btnBackColor.BackColor };
        if (dlg.ShowDialog() == DialogResult.OK)
            btnBackColor.BackColor = dlg.Color;
    }

    private void BtnOK_Click(object? sender, EventArgs e)
    {
        _field.X           = (int)nudX.Value;
        _field.Y           = (int)nudY.Value;
        _field.Width       = (int)nudWidth.Value;
        _field.Height      = (int)nudHeight.Value;
        _field.FontName    = txtFontName.Text.Trim();
        _field.FontSize    = (float)nudFontSize.Value;
        _field.Bold        = chkBold.Checked;
        _field.Italic      = chkItalic.Checked;
        _field.Visible     = chkVisible.Checked;
        _field.ForeColorHex= ColorTranslator.ToHtml(btnForeColor.BackColor);
        _field.BackColorHex= ColorTranslator.ToHtml(btnBackColor.BackColor);
        _field.TextAlignH  = cmbAlign.SelectedIndex;

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
}
