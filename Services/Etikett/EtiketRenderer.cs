using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using global::System.Net;
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

        // Draw background image if present (before text)
        if (field.ImageData is { Length: > 0 })
        {
            try
            {
                using var ms  = new MemoryStream(field.ImageData);
                using var img = Image.FromStream(ms);
                DrawImage(g, img, rect, field.ImageSizeMode);
            }
            catch { /* ignore broken image data */ }
        }

        string text = GetFieldText(field.Feld, data);
        if (string.IsNullOrWhiteSpace(text)) return;

        var baseStyle = (field.Bold   ? FontStyle.Bold   : FontStyle.Regular)
                      | (field.Italic ? FontStyle.Italic : FontStyle.Regular);

        DrawRichText(g, text, field.FontName, field.FontSize * scale, baseStyle, foreColor, rect, field.TextAlignH);
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
            EtiketFeld.Zutaten             => d.Zutaten ?? string.Empty,
            EtiketFeld.Hinweis             => d.Hinweis ?? string.Empty,
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

    private sealed record TextRun(string Text, FontStyle Style, bool NewLine = false);

    private static void DrawRichText(
        Graphics g,
        string raw,
        string fontName,
        float fontSize,
        FontStyle baseStyle,
        Color color,
        RectangleF rect,
        int textAlignH)
    {
        var runs = ParseHtmlRuns(raw, baseStyle);
        if (runs.Count == 0) return;

        using var brush = new SolidBrush(color);
        using var measureFormat = (StringFormat)StringFormat.GenericTypographic.Clone();
        measureFormat.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;

        var line = new List<(string Text, Font Font, float Width)>();
        float y = rect.Top;

        void FlushLine()
        {
            if (line.Count == 0)
            {
                using var lf = new Font(fontName, fontSize, baseStyle, GraphicsUnit.Pixel);
                y += lf.GetHeight(g);
                return;
            }

            float lineWidth  = line.Sum(s => s.Width);
            float lineHeight = line.Max(s => s.Font.GetHeight(g));
            if (y + lineHeight > rect.Bottom) return;

            float x = textAlignH switch
            {
                1 => rect.Left + (rect.Width - lineWidth) / 2f,
                2 => rect.Right - lineWidth,
                _ => rect.Left
            };

            foreach (var s in line)
            {
                g.DrawString(s.Text, s.Font, brush, x, y, measureFormat);
                x += s.Width;
                s.Font.Dispose();
            }

            line.Clear();
            y += lineHeight;
        }

        foreach (var run in runs)
        {
            if (run.NewLine)
            {
                FlushLine();
                continue;
            }

            if (string.IsNullOrEmpty(run.Text))
                continue;

            var chunks = Regex.Matches(run.Text, @"\S+|\s+")
                              .Select(m => m.Value)
                              .ToList();

            foreach (var chunk in chunks)
            {
                if (line.Count == 0 && string.IsNullOrWhiteSpace(chunk))
                    continue;

                var font = new Font(fontName, fontSize, run.Style, GraphicsUnit.Pixel);
                float w = g.MeasureString(chunk, font, PointF.Empty, measureFormat).Width;
                float currentLineWidth = line.Sum(s => s.Width);

                if (currentLineWidth + w <= rect.Width || line.Count == 0)
                {
                    if (w > rect.Width && line.Count == 0 && chunk.Length > 1)
                    {
                        var parts = SplitChunkToFit(g, chunk, font, rect.Width, measureFormat);
                        font.Dispose();
                        foreach (var part in parts)
                        {
                            var pf = new Font(fontName, fontSize, run.Style, GraphicsUnit.Pixel);
                            float pw = g.MeasureString(part, pf, PointF.Empty, measureFormat).Width;
                            float cur = line.Sum(s => s.Width);
                            if (cur + pw > rect.Width && line.Count > 0)
                                FlushLine();
                            line.Add((part, pf, pw));
                            if (part != parts.Last())
                                FlushLine();
                        }
                    }
                    else
                    {
                        line.Add((chunk, font, w));
                    }
                }
                else
                {
                    font.Dispose();
                    FlushLine();
                    if (y >= rect.Bottom) return;

                    if (!string.IsNullOrWhiteSpace(chunk))
                    {
                        var nf = new Font(fontName, fontSize, run.Style, GraphicsUnit.Pixel);
                        float nw = g.MeasureString(chunk, nf, PointF.Empty, measureFormat).Width;
                        line.Add((chunk, nf, nw));
                    }
                }

                if (y >= rect.Bottom) return;
            }
        }

        FlushLine();
    }

    private static List<TextRun> ParseHtmlRuns(string input, FontStyle baseStyle)
    {
        var runs = new List<TextRun>();

        int b = 0, i = 0, u = 0;
        var parts = Regex.Split(input.Replace("\r\n", "\n"), "(<[^>]+>)");

        FontStyle CurrentStyle()
        {
            var s = baseStyle;
            if (b > 0) s |= FontStyle.Bold;
            if (i > 0) s |= FontStyle.Italic;
            if (u > 0) s |= FontStyle.Underline;
            return s;
        }

        foreach (var part in parts)
        {
            if (string.IsNullOrEmpty(part)) continue;

            if (part.StartsWith("<") && part.EndsWith(">"))
            {
                var t = part.Trim().ToLowerInvariant();
                switch (t)
                {
                    case "<b>":
                    case "<strong>": b++; break;
                    case "</b>":
                    case "</strong>": if (b > 0) b--; break;
                    case "<i>":
                    case "<em>": i++; break;
                    case "</i>":
                    case "</em>": if (i > 0) i--; break;
                    case "<u>": u++; break;
                    case "</u>": if (u > 0) u--; break;
                    case "<br>":
                    case "<br/>":
                    case "<br />": runs.Add(new TextRun(string.Empty, CurrentStyle(), NewLine: true)); break;
                    case "<p>":
                    case "</p>": runs.Add(new TextRun(string.Empty, CurrentStyle(), NewLine: true)); break;
                }

                continue;
            }

            var decoded = WebUtility.HtmlDecode(part);
            var splitByNewline = decoded.Split(new[] { '\n' }, StringSplitOptions.None);
            for (int idx = 0; idx < splitByNewline.Length; idx++)
            {
                if (!string.IsNullOrEmpty(splitByNewline[idx]))
                    runs.Add(new TextRun(splitByNewline[idx], CurrentStyle()));

                if (idx < splitByNewline.Length - 1)
                    runs.Add(new TextRun(string.Empty, CurrentStyle(), NewLine: true));
            }
        }

        return runs;
    }

    private static List<string> SplitChunkToFit(
        Graphics g,
        string chunk,
        Font font,
        float maxWidth,
        StringFormat measureFormat)
    {
        var result = new List<string>();
        var remaining = chunk;

        while (!string.IsNullOrEmpty(remaining))
        {
            int len = remaining.Length;
            while (len > 1)
            {
                var candidate = remaining[..len];
                var width = g.MeasureString(candidate, font, PointF.Empty, measureFormat).Width;
                if (width <= maxWidth) break;
                len--;
            }

            if (len <= 0) len = 1;
            result.Add(remaining[..len]);
            remaining = remaining[len..];
        }

        return result;
    }

    private static void DrawImage(Graphics g, Image img, RectangleF dest, int sizeMode)
    {
        switch (sizeMode)
        {
            case 0: // Strecken
                g.DrawImage(img, dest);
                break;
            case 1: // Anpassen (Zoom)
                double ratio = Math.Min(dest.Width / img.Width, dest.Height / img.Height);
                float iw = (float)(img.Width  * ratio);
                float ih = (float)(img.Height * ratio);
                g.DrawImage(img,
                    dest.X + (dest.Width  - iw) / 2f,
                    dest.Y + (dest.Height - ih) / 2f, iw, ih);
                break;
            case 2: // Zentrieren
                g.DrawImage(img,
                    dest.X + (dest.Width  - img.Width)  / 2f,
                    dest.Y + (dest.Height - img.Height) / 2f);
                break;
        }
    }

    private static Color ParseColor(string hex, Color fallback)
    {
        try { return ColorTranslator.FromHtml(hex); }
        catch { return fallback; }
    }
}
