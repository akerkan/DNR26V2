using System;
using System.Drawing;
using System.Globalization;
using DNR26V2.Domain.Entities.System;
using DNR26V2.Domain.Enums;

namespace DNR26V2.Helpers;

public static class StatusColorHelper
{
    // Konfiguration (gesetzt via Configure)
    private static Color _offenBack = Color.FromArgb(255, 255, 200);
    private static Color _freigegebenBack = Color.FromArgb(200, 230, 255);
    private static Color _gebuchtBack = Color.FromArgb(200, 255, 200);
    private static Color _storniertBack = Color.FromArgb(240, 240, 240);

    private static Color? _offenLabel = null;
    private static Color? _freigegebenLabel = null;
    private static Color? _gebuchtLabel = null;
    private static Color? _storniertLabel = null;

    public static void Configure(AppSetup? setup)
    {
        if (setup is null) return;

        _offenBack = TryParseHtmlColor(setup.ColorOrderOffen, _offenBack);
        _freigegebenBack = TryParseHtmlColor(setup.ColorOrderFreigegeben, _freigegebenBack);
        _gebuchtBack = TryParseHtmlColor(setup.ColorOrderGebucht, _gebuchtBack);
        _storniertBack = TryParseHtmlColor(setup.ColorOrderStorniert, _storniertBack);

        _offenLabel = TryParseHtmlColorNullable(setup.ColorOrderLabelOffen);
        _freigegebenLabel = TryParseHtmlColorNullable(setup.ColorOrderLabelFreigegeben);
        _gebuchtLabel = TryParseHtmlColorNullable(setup.ColorOrderLabelGebucht);
        _storniertLabel = TryParseHtmlColorNullable(setup.ColorOrderLabelStorniert);
    }

    public static Color GetOrderStatusBackColor(OrderStatus? status) => status switch
    {
        OrderStatus.Gebucht => _gebuchtBack,
        OrderStatus.Freigegeben => _freigegebenBack,
        OrderStatus.Offen => _offenBack,
        OrderStatus.Storniert => _storniertBack,
        _ => SystemColors.Window
    };

    public static Color GetOrderStatusLabelColor(OrderStatus? status)
    {
        return status switch
        {
            OrderStatus.Gebucht => _gebuchtLabel ?? Color.DarkGreen,
            OrderStatus.Freigegeben => _freigegebenLabel ?? Color.SteelBlue,
            OrderStatus.Offen => _offenLabel ?? Color.DarkOrange,
            OrderStatus.Storniert => _storniertLabel ?? Color.Gray,
            _ => SystemColors.ControlText
        };
    }

    // ── Delivery Status Colors ────────────────────────────────────────────────
    // Mapped to same visual palette as Order for consistency

    public static Color GetDeliveryStatusBackColor(DeliveryStatus status) => status switch
    {
        DeliveryStatus.Offen => _offenBack,
        DeliveryStatus.TeilStorniert => Color.FromArgb(255, 235, 200),   // orange-ish
        DeliveryStatus.Fakturiert => _gebuchtBack,
        DeliveryStatus.Storniert => _storniertBack,
        _ => SystemColors.Window
    };

    public static Color GetDeliveryStatusLabelColor(DeliveryStatus status) => status switch
    {
        DeliveryStatus.Offen => _offenLabel ?? Color.DarkOrange,
        DeliveryStatus.TeilStorniert => Color.DarkOrange,
        DeliveryStatus.Fakturiert => _gebuchtLabel ?? Color.DarkGreen,
        DeliveryStatus.Storniert => _storniertLabel ?? Color.Gray,
        _ => SystemColors.ControlText
    };

    private static Color TryParseHtmlColor(string? html, Color fallback)
    {
        if (string.IsNullOrWhiteSpace(html)) return fallback;
        try
        {
            return ColorTranslator.FromHtml(html);
        }
        catch { return fallback; }
    }

    private static Color? TryParseHtmlColorNullable(string? html)
    {
        if (string.IsNullOrWhiteSpace(html)) return null;
        try { return ColorTranslator.FromHtml(html); }
        catch { return null; }
    }
}