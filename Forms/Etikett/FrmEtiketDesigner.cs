using DNR26V2.Domain.Entities.Etikett;
using DNR26V2.Domain.Enums;
using DNR26V2.Forms.Base;
using DNR26V2.Services.Etikett;

namespace DNR26V2.Forms.Etikett;

/// <summary>
/// Split-panel label designer.
/// Left  = live properties (X/Y/W/H + font/color).
/// Right = 10×15 cm canvas with draggable, resizable field panels.
/// </summary>
public partial class FrmEtiketDesigner : BaseForm
{
    private readonly IEtiketService          _service;
    private readonly List<EtiketLayoutField> _fields;

    // ── Selection ─────────────────────────────────────────────────────────────
    private Panel?             _selectedPanel;
    private EtiketLayoutField? _selectedField;

    // ── Drag state ────────────────────────────────────────────────────────────
    private Panel?             _dragging;
    private Point              _dragOffset;

    // ── Resize state ──────────────────────────────────────────────────────────
    private Panel?             _resizing;
    private EtiketLayoutField? _resizingField;
    private ResizeMode         _resizeMode    = ResizeMode.None;
    private Point              _resizeStart;
    private Rectangle          _resizeBounds;

    // ── Guard: prevents feedback loop when setting NUD values in code ─────────
    private bool _updatingProps;

    private const int HitZone = 7;
    private const int MinSize = 20;

    private enum ResizeMode { None, Move, N, S, W, E, NW, NE, SW, SE }

    // ── Constructor ───────────────────────────────────────────────────────────

    public FrmEtiketDesigner(IEtiketService service, IReadOnlyList<EtiketLayoutField> layout)
    {
        _service = service;
        _fields = layout.ToList();
        InitializeComponent();
        BuildPropertiesPanel(); // ← neu, vor WirePropertyEvents
        WirePropertyEvents();
        Load += (_, _) => { PopulateFontCombo(); BuildFieldPanels(); };
    }

    // ── Init ─────────────────────────────────────────────────────────────────

    private void PopulateFontCombo()
    {
        cmbPropFont.Items.Clear();
        foreach (var ff in FontFamily.Families)
            cmbPropFont.Items.Add(ff.Name);
    }

    private void WirePropertyEvents()
    {
        nudPropX.ValueChanged    += PropChanged;
        nudPropY.ValueChanged    += PropChanged;
        nudPropW.ValueChanged    += PropChanged;
        nudPropH.ValueChanged    += PropChanged;
        nudPropSize.ValueChanged += PropChanged;
        cmbPropFont.TextUpdate   += PropChanged;
        cmbPropFont.SelectedIndexChanged += PropChanged;
        chkPropBold.CheckedChanged    += PropChanged;
        chkPropItalic.CheckedChanged  += PropChanged;
        chkPropVisible.CheckedChanged += PropChanged;
        cmbPropAlign.SelectedIndexChanged += PropChanged;
    }

    // ── Field panels ──────────────────────────────────────────────────────────

    private void BuildFieldPanels()
    {
        canvas.Controls.Clear();
        _selectedPanel = null;
        _selectedField = null;
        UpdatePropertiesPanel();

        foreach (var field in _fields)
            canvas.Controls.Add(CreateFieldPanel(field));
    }

    private Panel CreateFieldPanel(EtiketLayoutField field)
    {
        var pnl = new Panel
        {
            Location = new Point(field.X, field.Y),
            Size     = new Size(field.Width, field.Height),
            Tag      = field,
            BackColor= field.Visible
                ? ParseColor(field.BackColorHex, Color.LightYellow)
                : Color.LightGray,
        };

        pnl.Paint += (_, pe) => PaintField(pe.Graphics, pnl, field);
        pnl.MouseDown   += Panel_MouseDown;
        pnl.MouseMove   += Panel_MouseMove;
        pnl.MouseUp     += Panel_MouseUp;
        pnl.MouseClick  += (_, e) => { if (e.Button == MouseButtons.Left) SelectField(pnl, field); };
        pnl.ContextMenuStrip = BuildContextMenu(field, pnl);

        return pnl;
    }

    private void PaintField(Graphics g, Panel pnl, EtiketLayoutField field)
    {
        // Selection highlight
        if (ReferenceEquals(pnl, _selectedPanel))
        {
            using var pen = new Pen(Color.DodgerBlue, 2);
            g.DrawRectangle(pen, 1, 1, pnl.Width - 3, pnl.Height - 3);
        }
        else
        {
            using var pen = new Pen(Color.DimGray, 1);
            g.DrawRectangle(pen, 0, 0, pnl.Width - 1, pnl.Height - 1);
        }

        // Field name text
        float fs = Math.Max(6f, Math.Min(9f, field.FontSize * 0.72f));
        using var font  = new Font(field.FontName, fs,
                                   field.Bold ? FontStyle.Bold : FontStyle.Regular,
                                   GraphicsUnit.Point);
        using var brush = new SolidBrush(ParseColor(field.ForeColorHex, Color.Black));
        var sf = new StringFormat
        {
            Alignment     = StringAlignment.Center,
            LineAlignment = StringAlignment.Center,
            Trimming      = StringTrimming.EllipsisCharacter,
            FormatFlags   = StringFormatFlags.NoWrap,
        };
        g.DrawString(FieldDisplayName(field.Feld), font, brush,
                     new RectangleF(2, 2, pnl.Width - 4, pnl.Height - 4), sf);
    }

    // ── Selection ─────────────────────────────────────────────────────────────

    private void SelectField(Panel pnl, EtiketLayoutField field)
    {
        var prev = _selectedPanel;
        _selectedPanel = pnl;
        _selectedField = field;

        prev?.Invalidate();
        pnl.Invalidate();
        UpdatePropertiesPanel();
    }

    // ── Properties panel ──────────────────────────────────────────────────────

    private void UpdatePropertiesPanel()
    {
        _updatingProps = true;
        try
        {
            if (_selectedField is null)
            {
                pnlPropContent.Enabled = false;
                lblSelectedFeld.Text   = "— kein Feld ausgewählt —";
                return;
            }

            pnlPropContent.Enabled = true;
            lblSelectedFeld.Text   = FieldDisplayName(_selectedField.Feld);

            nudPropX.Value    = Math.Max(0,   _selectedField.X);
            nudPropY.Value    = Math.Max(0,   _selectedField.Y);
            nudPropW.Value    = Math.Max(MinSize, _selectedField.Width);
            nudPropH.Value    = Math.Max(MinSize, _selectedField.Height);
            nudPropSize.Value = (decimal)Math.Max(4f, _selectedField.FontSize);

            cmbPropFont.Text           = _selectedField.FontName;
            chkPropBold.Checked        = _selectedField.Bold;
            chkPropItalic.Checked      = _selectedField.Italic;
            chkPropVisible.Checked     = _selectedField.Visible;
            cmbPropAlign.SelectedIndex = Math.Clamp(_selectedField.TextAlignH, 0, 2);

            SetColorBtn(btnPropForeColor, _selectedField.ForeColorHex, Color.Black);
            SetColorBtn(btnPropBackColor, _selectedField.BackColorHex, Color.White);
        }
        finally
        {
            _updatingProps = false;
        }
    }

    private void PropChanged(object? sender, EventArgs e)
    {
        if (_updatingProps || _selectedField is null || _selectedPanel is null) return;

        _selectedField.X         = (int)nudPropX.Value;
        _selectedField.Y         = (int)nudPropY.Value;
        _selectedField.Width     = (int)nudPropW.Value;
        _selectedField.Height    = (int)nudPropH.Value;
        _selectedField.FontSize  = (float)nudPropSize.Value;
        _selectedField.FontName  = cmbPropFont.Text;
        _selectedField.Bold      = chkPropBold.Checked;
        _selectedField.Italic    = chkPropItalic.Checked;
        _selectedField.Visible   = chkPropVisible.Checked;
        _selectedField.TextAlignH= cmbPropAlign.SelectedIndex;

        ApplyFieldToPanel(_selectedPanel, _selectedField);
    }

    private void ApplyFieldToPanel(Panel pnl, EtiketLayoutField field)
    {
        pnl.Location  = new Point(Math.Max(0, field.X), Math.Max(0, field.Y));
        pnl.Size      = new Size(Math.Max(MinSize, field.Width), Math.Max(MinSize, field.Height));
        pnl.BackColor = field.Visible
            ? ParseColor(field.BackColorHex, Color.LightYellow)
            : Color.LightGray;
        pnl.Invalidate();
    }

    // ── Color buttons ─────────────────────────────────────────────────────────

    private void BtnPropForeColor_Click(object? sender, EventArgs e)
    {
        if (_selectedField is null) return;
        using var dlg = new ColorDialog { Color = btnPropForeColor.BackColor, FullOpen = true };
        if (dlg.ShowDialog() != DialogResult.OK) return;
        _selectedField.ForeColorHex = ColorTranslator.ToHtml(dlg.Color);
        SetColorBtn(btnPropForeColor, _selectedField.ForeColorHex, Color.Black);
        _selectedPanel?.Invalidate();
    }

    private void BtnPropBackColor_Click(object? sender, EventArgs e)
    {
        if (_selectedField is null) return;
        using var dlg = new ColorDialog { Color = btnPropBackColor.BackColor, FullOpen = true };
        if (dlg.ShowDialog() != DialogResult.OK) return;
        _selectedField.BackColorHex = ColorTranslator.ToHtml(dlg.Color);
        SetColorBtn(btnPropBackColor, _selectedField.BackColorHex, Color.White);
        if (_selectedPanel != null) ApplyFieldToPanel(_selectedPanel, _selectedField);
    }

    private static void SetColorBtn(Button btn, string hex, Color fallback)
    {
        btn.BackColor = ParseColor(hex, fallback);
        btn.ForeColor = ContrastColor(btn.BackColor);
    }

    // ── Resize-mode detection ─────────────────────────────────────────────────

    private static ResizeMode DetectMode(Panel pnl, Point pt)
    {
        bool l = pt.X <= HitZone;
        bool r = pt.X >= pnl.Width  - HitZone;
        bool t = pt.Y <= HitZone;
        bool b = pt.Y >= pnl.Height - HitZone;

        if (l && t) return ResizeMode.NW;
        if (r && t) return ResizeMode.NE;
        if (l && b) return ResizeMode.SW;
        if (r && b) return ResizeMode.SE;
        if (l)      return ResizeMode.W;
        if (r)      return ResizeMode.E;
        if (t)      return ResizeMode.N;
        if (b)      return ResizeMode.S;
        return ResizeMode.Move;
    }

    private static Cursor CursorFor(ResizeMode m) => m switch
    {
        ResizeMode.NW or ResizeMode.SE => Cursors.SizeNWSE,
        ResizeMode.NE or ResizeMode.SW => Cursors.SizeNESW,
        ResizeMode.N  or ResizeMode.S  => Cursors.SizeNS,
        ResizeMode.W  or ResizeMode.E  => Cursors.SizeWE,
        _                              => Cursors.SizeAll,
    };

    // ── Mouse handlers ────────────────────────────────────────────────────────

    private void Panel_MouseDown(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left) return;
        var pnl   = (Panel)sender!;
        var field = (EtiketLayoutField)pnl.Tag!;
        SelectField(pnl, field);

        var mode = DetectMode(pnl, e.Location);
        if (mode == ResizeMode.Move)
        {
            _dragging   = pnl;
            _dragOffset = e.Location;
        }
        else
        {
            _resizing      = pnl;
            _resizingField = field;
            _resizeMode    = mode;
            _resizeStart   = pnl.Parent!.PointToClient(Cursor.Position);
            _resizeBounds  = pnl.Bounds;
        }
        pnl.BringToFront();
    }

    private void Panel_MouseMove(object? sender, MouseEventArgs e)
    {
        var pnl = (Panel)sender!;

        if (_dragging is null && _resizing is null)
        {
            pnl.Cursor = CursorFor(DetectMode(pnl, e.Location));
            return;
        }

        if (_dragging != null && _dragging.Tag is EtiketLayoutField df)
        {
            int nx = Math.Clamp(_dragging.Left + e.X - _dragOffset.X,
                                0, canvas.Width  - _dragging.Width);
            int ny = Math.Clamp(_dragging.Top  + e.Y - _dragOffset.Y,
                                0, canvas.Height - _dragging.Height);
            _dragging.Location = new Point(nx, ny);
            df.X = nx; df.Y = ny;
            _updatingProps = true;
            nudPropX.Value = nx; nudPropY.Value = ny;
            _updatingProps = false;
            return;
        }

        if (_resizing != null && _resizingField != null)
        {
            var cur = _resizing.Parent!.PointToClient(Cursor.Position);
            int dx  = cur.X - _resizeStart.X;
            int dy  = cur.Y - _resizeStart.Y;
            var b   = _resizeBounds;

            switch (_resizeMode)
            {
                case ResizeMode.E:  b.Width  = Math.Max(MinSize, b.Width  + dx); break;
                case ResizeMode.S:  b.Height = Math.Max(MinSize, b.Height + dy); break;
                case ResizeMode.SE: b.Width  = Math.Max(MinSize, b.Width  + dx);
                                    b.Height = Math.Max(MinSize, b.Height + dy); break;
                case ResizeMode.W:  b.Width  = Math.Max(MinSize, b.Width  - dx); b.X = b.Right  - b.Width;  break;
                case ResizeMode.N:  b.Height = Math.Max(MinSize, b.Height - dy); b.Y = b.Bottom - b.Height; break;
                case ResizeMode.NW: b.Width  = Math.Max(MinSize, b.Width  - dx); b.X = b.Right  - b.Width;
                                    b.Height = Math.Max(MinSize, b.Height - dy); b.Y = b.Bottom - b.Height; break;
                case ResizeMode.NE: b.Width  = Math.Max(MinSize, b.Width  + dx);
                                    b.Height = Math.Max(MinSize, b.Height - dy); b.Y = b.Bottom - b.Height; break;
                case ResizeMode.SW: b.Width  = Math.Max(MinSize, b.Width  - dx); b.X = b.Right  - b.Width;
                                    b.Height = Math.Max(MinSize, b.Height + dy); break;
            }

            _resizing.Bounds       = b;
            _resizingField.X       = b.X;   _resizingField.Y      = b.Y;
            _resizingField.Width   = b.Width; _resizingField.Height = b.Height;
            _resizing.Invalidate();

            // Live-update property NUDs
            _updatingProps = true;
            nudPropX.Value = b.X; nudPropY.Value = b.Y;
            nudPropW.Value = b.Width; nudPropH.Value = b.Height;
            _updatingProps = false;
        }
    }

    private void Panel_MouseUp(object? sender, MouseEventArgs e)
    {
        _dragging      = null;
        _resizing      = null;
        _resizingField = null;
        _resizeMode    = ResizeMode.None;
    }

    // ── Context menu ─────────────────────────────────────────────────────────

    private ContextMenuStrip BuildContextMenu(EtiketLayoutField field, Panel pnl)
    {
        var cms = new ContextMenuStrip();
        cms.Items.Add(field.Visible ? "Ausblenden" : "Einblenden", null, (_, _) =>
        {
            field.Visible = !field.Visible;
            ApplyFieldToPanel(pnl, field);
            if (ReferenceEquals(pnl, _selectedPanel))
            {
                _updatingProps = true;
                chkPropVisible.Checked = field.Visible;
                _updatingProps = false;
            }
        });
        return cms;
    }

    // ── Save / Reset ─────────────────────────────────────────────────────────

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
        if (MessageBox.Show("Layout auf Standard zurücksetzen?", "Bestätigen",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

        await _service.ResetLayoutAsync();
        var layout = await _service.GetLayoutAsync();
        _fields.Clear();
        _fields.AddRange(layout);
        BuildFieldPanels();
    }

    private void BtnSchliessen_Click(object? sender, EventArgs e) => Close();

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static Color ParseColor(string hex, Color fallback)
    {
        try { return ColorTranslator.FromHtml(hex); }
        catch { return fallback; }
    }

    private static Color ContrastColor(Color bg)
        => (bg.R * 0.299 + bg.G * 0.587 + bg.B * 0.114) > 128
            ? Color.Black : Color.White;

    private static string FieldDisplayName(EtiketFeld feld) => feld switch
    {
        EtiketFeld.Logo               => "Logo",
        EtiketFeld.BarcodeObenLinks   => "Barcode links",
        EtiketFeld.BarcodeObenRechts  => "Barcode rechts",
        EtiketFeld.SchockGefroren     => "Schock gefrostet",
        EtiketFeld.UrunAdi            => "Produktname",
        EtiketFeld.Untertitel         => "Untertitel (Feld1)",
        EtiketFeld.Zutaten            => "Zutaten (Feld2)",
        EtiketFeld.Hinweis            => "Hinweis (Feld3)",
        EtiketFeld.HaltbarBis         => "Haltbar bis",
        EtiketFeld.Wochentag          => "Wochentag",
        EtiketFeld.ChargeEingefrorenAm=> "Charge/eingefroren am",
        EtiketFeld.Kundenname         => "Kundenname",
        EtiketFeld.MengeGewicht       => "Menge / Gewicht",
        EtiketFeld.FirmaFooter        => "Firma Footer",
        _                             => feld.ToString(),
    };
}
