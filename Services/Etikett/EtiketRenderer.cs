using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using DNR26V2.Domain.DTOs.Etikett;
using DNR26V2.Domain.Entities.Etikett;
using DNR26V2.Domain.Enums;

namespace DNR26V2.Services.Etikett;

/// <summary>
/// Renders a label onto any Graphics surface (screen preview or printer).
/// Canvas reference size: 378 × 567 px (10 × 15 cm at 96 DPI).
/// Pass a scale factor to adapt to printer resolution.
/// </summary>
public static class EtiketRenderer
{
    /// <summary>Reference canvas width at 96 DPI (10 cm).</summary>
    public const int CanvasW = 378;
    /// <summary>Reference canvas height at 96 DPI (15 cm).</summary>
    public const int CanvasH = 567;

    /// <summary>Renders the label to a new Bitmap at the given scale.</summary>
    public static Bitmap RenderBitmap(
        EtiketDruckData data,
        IReadOnlyList<EtiketLayoutField> layout,
        float scale = 1f)
    {
        int w = (int)(CanvasW * scale);
        int h = (int)(CanvasH * scale);
        var bmp = new Bitmap(w, h, PixelFormat.Format32bppArgb);
        using var g = Graphics.FromImage(bmp);
        g.Clear(Color.White);
        Render(g, data, layout, scale);
        return bmp;
    }

    /// <summary>Renders directly onto the provided Graphics (e.g. printer).</summary>
    public static void Render(
        Graphics g,
        EtiketDruckData data,
        IReadOnlyList<EtiketLayoutField> layout,
        float scale = 1f)
    {
        g.Clear(Color.White);

        foreach (var field in layout.Where(f => f.Visible))
        {
            var rect = ScaleRect(field, scale);
            DrawField(g, field, rect, data, scale);
        }
    }

    // ?? Private helpers ???????????????????????????????????????????????????????

    private static RectangleF ScaleRect(EtiketLayoutField f, float scale)
        => new RectangleF(f.X * scale, f.Y * scale, f.Width * scale, f.Height * scale);

    private static void DrawField(
        Graphics g, EtiketLayoutField field, RectangleF rect,
        EtiketDruckData data, float scale)
    {
        var backColor = ParseColor(field.BackColorHex, Color.White);
        var foreColor = ParseColor(field.ForeColorHex, Color.Black);

        // Special: UrunAdi uses product Printfarbe as background
        if (field.Feld == EtiketFeld.UrunAdi && !string.IsNullOrWhiteSpace(data.Printfarbe))
        {
            try { backColor = ColorTranslator.FromHtml(data.Printfarbe); }
            catch { /* keep default */ }
        }

        // Fill background
        if (backColor != Color.White)
            g.FillRectangle(new SolidBrush(backColor), rect);

        string text = GetFieldText(field.Feld, data);
        if (string.IsNullOrEmpty(text)) return;

        var fontStyle = (field.Bold   ? FontStyle.Bold   : FontStyle.Regular)
                      | (field.Italic ? FontStyle.Italic : FontStyle.Regular);

        using var font   = new Font(field.FontName, field.FontSize * scale, fontStyle, GraphicsUnit.Pixel);
        using var brush  = new SolidBrush(foreColor);
        var sf = BuildStringFormat(field);

        g.DrawString(text, font, brush, rect, sf);
    }

    private static StringFormat BuildStringFormat(EtiketLayoutField field)
    {
        var sf = new StringFormat
        {
            LineAlignment = StringAlignment.Near,
            Trimming      = StringTrimming.None,
            FormatFlags   = StringFormatFlags.LineLimit
        };
        sf.Alignment = field.TextAlignH switch
        {
            1 => StringAlignment.Center,
            2 => StringAlignment.Far,
            _ => StringAlignment.Near
        };
        return sf;
    }

    private static string GetFieldText(EtiketFeld feld, EtiketDruckData d)
    {
        var dayName = d.LieferDatum.DayOfWeek switch
        {
            DayOfWeek.Monday    => "MONTAG",
            DayOfWeek.Tuesday   => "DIENSTAG",
            DayOfWeek.Wednesday => "MITTWOCH",
            DayOfWeek.Thursday  => "DONNERSTAG",
            DayOfWeek.Friday    => "FREITAG",
            DayOfWeek.Saturday  => "SAMSTAG",
            _                   => "SONNTAG",
        };

        return feld switch
        {
            EtiketFeld.BarcodeObenLinks    => d.Barcode ?? string.Empty,
            EtiketFeld.BarcodeObenRechts   => d.Barcode ?? string.Empty,
            EtiketFeld.SchockGefroren      => "Schock gefrostet",
            EtiketFeld.UrunAdi             => d.Produktname,
            EtiketFeld.Untertitel          => d.Untertitel ?? string.Empty,
            EtiketFeld.Zutaten             => StripHtml(d.Zutaten ?? string.Empty),
            EtiketFeld.Hinweis             => StripHtml(d.Hinweis ?? string.Empty),
            EtiketFeld.HaltbarBis          => $"bei -18°C  mindestens haltbar bis\n{d.LieferDatum.AddMonths(6):dd.MM.yyyy}",
            EtiketFeld.Wochentag           => dayName,
            EtiketFeld.ChargeEingefrorenAm => $"Charge/eingefroren am\n{d.LieferDatum:dd.MM.yyyy}",
            EtiketFeld.Kundenname          => d.Kundenname,
            EtiketFeld.MengeGewicht        => BuildMengeText(d),
            EtiketFeld.FirmaFooter         => BuildFooter(d),
            _                              => string.Empty,
        };
    }

    private static string BuildMengeText(EtiketDruckData d)
    {
        if (d.Menge > 0 && d.Gewicht > 0)
            return $"{d.Menge:G29} × {d.Gewicht:G29} Kg";
        if (d.Gewicht > 0)
            return $"{d.Gewicht:G29} Kg";
        if (d.Menge > 0)
            return $"{d.Menge:G29} STK";
        return string.Empty;
    }

    private static string BuildFooter(EtiketDruckData d)
    {
        var parts = new List<string> { d.Firmenname };
        if (!string.IsNullOrEmpty(d.FirmenAdresse)) parts.Add(d.FirmenAdresse);
        if (!string.IsNullOrEmpty(d.FirmenTelefon)) parts.Add($"T: {d.FirmenTelefon}");
        if (!string.IsNullOrEmpty(d.FirmenEmail))   parts.Add($"E: {d.FirmenEmail}");
        return string.Join("  ", parts);
    }

    private static string StripHtml(string html)
        => Regex.Replace(html, "<.*?>", string.Empty).Trim();

    private static Color ParseColor(string hex, Color fallback)
    {
        try { return ColorTranslator.FromHtml(hex); }
        catch { return fallback; }
    }
}
