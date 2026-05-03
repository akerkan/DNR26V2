using DNR26V2.Domain.Entities.Etikett;
using DNR26V2.Domain.Enums;
using DNR26V2.Forms.Base;
using DNR26V2.Services.Etikett;

namespace DNR26V2.Forms.Etikett;

/// <summary>
/// Runtime label designer: drag fields on the 10×15 cm canvas,
/// right-click to edit properties, Save to persist.
/// </summary>
public partial class FrmEtikettDesigner : BaseForm
{
    private readonly IEtiketService                   _service;
    private readonly List<EtiketLayoutField>          _fields;
    private readonly Dictionary<EtiketFeld, Panel>    _panels = new();

    // Drag state
    private Panel?  _dragging;
    private Point   _dragOffset;

    public FrmEtikettDesigner(IEtiketService service, IReadOnlyList<EtiketLayoutField> layout)
    {
        _service = service;
        _fields  = layout.ToList();
        InitializeComponent();
        Load += (_, _) => BuildFieldPanels();
    }

    // ?? Build draggable panels ????????????????????????????????????????????????

    private void BuildFieldPanels()
    {
        canvas.Controls.Clear();
        _panels.Clear();

        foreach (var field in _fields)
        {
            var pnl = CreateFieldPanel(field);
            canvas.Controls.Add(pnl);
            _panels[field.Feld] = pnl;
        }
    }

    private Panel CreateFieldPanel(EtiketLayoutField field)
    {
        var pnl = new Panel
        {
            Location    = new Point(field.X, field.Y),
            Size        = new Size(field.Width, field.Height),
            BorderStyle = BorderStyle.FixedSingle,
            BackColor   = ParseColor(field.BackColorHex, Color.LightYellow),
            Cursor      = Cursors.SizeAll,
            Tag         = field,
        };

        var lbl = new Label
        {
            Text      = FieldDisplayName(field.Feld),
            Dock      = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = ParseColor(field.ForeColorHex, Color.Black),
            Font      = new Font(field.FontName,
                                 Math.Max(6f, field.FontSize * 0.75f),
                                 field.Bold ? FontStyle.Bold : FontStyle.Regular,
                                 GraphicsUnit.Point),
        };
        lbl.DoubleClick += (_, _) => OpenProperties(field, pnl);

        pnl.Controls.Add(lbl);

        // Drag via mouse on the panel itself
        pnl.MouseDown += Panel_MouseDown;
        pnl.MouseMove += Panel_MouseMove;
        pnl.MouseUp   += Panel_MouseUp;
        pnl.DoubleClick+= (_, _) => OpenProperties(field, pnl);
        pnl.ContextMenuStrip = BuildContextMenu(field, pnl);

        return pnl;
    }

    // ?? Drag ?????????????????????????????????????????????????????????????????

    private void Panel_MouseDown(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left) return;
        _dragging   = (Panel)sender!;
        _dragOffset = e.Location;
        _dragging.BringToFront();
    }

    private void Panel_MouseMove(object? sender, MouseEventArgs e)
    {
        if (_dragging is null) return;
        var newX = Math.Max(0, _dragging.Left + e.X - _dragOffset.X);
        var newY = Math.Max(0, _dragging.Top  + e.Y - _dragOffset.Y);
        newX     = Math.Min(newX, canvas.Width  - _dragging.Width);
        newY     = Math.Min(newY, canvas.Height - _dragging.Height);
        _dragging.Location = new Point(newX, newY);

        // Live-update the field
        if (_dragging.Tag is EtiketLayoutField f)
        { f.X = newX; f.Y = newY; }
    }

    private void Panel_MouseUp(object? sender, MouseEventArgs e)
        => _dragging = null;

    // ?? Context menu ?????????????????????????????????????????????????????????

    private ContextMenuStrip BuildContextMenu(EtiketLayoutField field, Panel pnl)
    {
        var cms = new ContextMenuStrip();
        cms.Items.Add("Eigenschaften …", null, (_, _) => OpenProperties(field, pnl));
        cms.Items.Add(new ToolStripSeparator());
        cms.Items.Add(field.Visible ? "Ausblenden" : "Einblenden", null, (_, _) =>
        {
            field.Visible   = !field.Visible;
            pnl.BackColor   = field.Visible
                ? ParseColor(field.BackColorHex, Color.LightYellow)
                : Color.LightGray;
        });
        return cms;
    }

    // ?? Properties dialog ????????????????????????????????????????????????????

    private void OpenProperties(EtiketLayoutField field, Panel pnl)
    {
        using var dlg = new FrmEtiketFieldProperties(field);
        if (dlg.ShowDialog(this) != DialogResult.OK) return;

        // Refresh panel appearance
        pnl.BackColor = ParseColor(field.BackColorHex, Color.LightYellow);
        if (pnl.Controls.Count > 0 && pnl.Controls[0] is Label lbl)
        {
            lbl.ForeColor = ParseColor(field.ForeColorHex, Color.Black);
            lbl.Font      = new Font(field.FontName,
                                     Math.Max(6f, field.FontSize * 0.75f),
                                     field.Bold ? FontStyle.Bold : FontStyle.Regular,
                                     GraphicsUnit.Point);
        }
        pnl.Size      = new Size(field.Width, field.Height);
        pnl.ContextMenuStrip = BuildContextMenu(field, pnl);
    }

    // ?? Save / Reset ?????????????????????????????????????????????????????????

    private async void BtnSpeichern_Click(object? sender, EventArgs e)
    {
        try
        {
            btnSpeichern.Enabled = false;
            await _service.SaveLayoutAsync(_fields);
            MessageBox.Show("Layout gespeichert.", "Erfolg",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Fehler: " + ex.Message, "Fehler",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally { btnSpeichern.Enabled = true; }
    }

    private async void BtnReset_Click(object? sender, EventArgs e)
    {
        var r = MessageBox.Show(
            "Layout auf Standard zurücksetzen?", "Bestätigen",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (r != DialogResult.Yes) return;

        await _service.ResetLayoutAsync();
        var layout = await _service.GetLayoutAsync();
        _fields.Clear();
        _fields.AddRange(layout);
        BuildFieldPanels();
    }

    private void BtnSchliessen_Click(object? sender, EventArgs e) => Close();

    // ?? Helpers ???????????????????????????????????????????????????????????????

    private static Color ParseColor(string hex, Color fallback)
    {
        try { return ColorTranslator.FromHtml(hex); }
        catch { return fallback; }
    }

    private static string FieldDisplayName(EtiketFeld feld) => feld switch
    {
        EtiketFeld.Logo              => "Logo",
        EtiketFeld.BarcodeObenLinks  => "Barcode links",
        EtiketFeld.BarcodeObenRechts => "Barcode rechts",
        EtiketFeld.SchockGefroren    => "Schock gefrostet",
        EtiketFeld.UrunAdi           => "Produktname",
        EtiketFeld.Untertitel        => "Untertitel (Feld1)",
        EtiketFeld.Zutaten           => "Zutaten (Feld2)",
        EtiketFeld.Hinweis           => "Hinweis (Feld3)",
        EtiketFeld.HaltbarBis        => "Haltbar bis",
        EtiketFeld.Wochentag         => "Wochentag",
        EtiketFeld.ChargeEingefrorenAm => "Charge/eingefroren am",
        EtiketFeld.Kundenname        => "Kundenname",
        EtiketFeld.MengeGewicht      => "Menge / Gewicht",
        EtiketFeld.FirmaFooter       => "Firma Footer",
        _                            => feld.ToString(),
    };
}
