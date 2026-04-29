using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Reporting.WinForms;
using DNR26V2.Forms.Reports;
using DNR26V2.Services.System;

namespace DNR26V2.Services.Reports;

public class RdlcReportRenderService : IReportRenderService
{
    private const string RechnungRdlc = "Reports\\Rechnung.rdlc";
    private const string AuftragRdlc = "Reports\\Auftragsbestaetigung.rdlc";
    private const string LieferscheinRdlc = "Reports\\Lieferschein.rdlc";
    private const string KundenkontoRdlc = "Reports\\Kundenkonto.rdlc";

    private readonly IAppSetupService _appSetupService;
    private readonly IConfiguration _configuration;

    public RdlcReportRenderService(
        IAppSetupService appSetupService,
        IConfiguration configuration)
    {
        _appSetupService = appSetupService;
        _configuration = configuration;
    }

    public async Task PreviewInvoiceAsync(int invoiceId)
    {
        var dt = await LoadInvoiceReportDataAsync(invoiceId);
        ShowInvoicePreview(dt);
    }

    private async Task<DataTable> LoadInvoiceReportDataAsync(int invoiceId)
    {
        var dt = new DataTable();
        var sql = @"SELECT * FROM dbo.vwInvoiceReport WHERE RechnungId = @RechnungId ORDER BY Lieferdatum, Lieferscheinnummer, Artikelnummer;";
        using var con = new SqlConnection(GetConnectionString());
        using var cmd = new SqlCommand(sql, con);
        cmd.Parameters.AddWithValue("@RechnungId", invoiceId);
        using var da = new SqlDataAdapter(cmd);
        await Task.Run(() => da.Fill(dt));
        return dt;
    }

    private void ShowInvoicePreview(DataTable table)
    {
        var rdlcPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "Invoice.rdlc");
        using var form = new FrmReportViewer();
        form.ReportViewer.LocalReport.DataSources.Clear();
        form.ReportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsInvoice", table));
        form.ReportViewer.LocalReport.ReportPath = rdlcPath;
        form.ReportViewer.RefreshReport();
        form.ShowDialog();
    }

    public async Task PreviewInvoicesAsync(IEnumerable<int> invoiceIds)
    {
        var dt = await LoadBulkInvoiceReportDataAsync(invoiceIds);
        ShowInvoicePreview(dt);
    }

    private async Task<DataTable> LoadBulkInvoiceReportDataAsync(IEnumerable<int> invoiceIds)
    {
        var dt = new DataTable();
        var ids = invoiceIds.ToList();
        if (ids.Count == 0) return dt;
        var idList = string.Join(",", ids);
        var sql = $"SELECT * FROM dbo.vwInvoiceReport WHERE RechnungId IN ({idList}) ORDER BY Lieferdatum, Lieferscheinnummer, Artikelnummer;";
        using var con = new SqlConnection(GetConnectionString());
        using var cmd = new SqlCommand(sql, con);
        using var da = new SqlDataAdapter(cmd);
        await Task.Run(() => da.Fill(dt));
        return dt;
    }

    public async Task PrintInvoiceAsync(int invoiceId, string? printerName = null)
    {
        var pdf = await RenderInvoicePdfAsync(invoiceId);
        await PrintPdfAsync(pdf, $"invoice_{invoiceId}.pdf", printerName);
    }

    public async Task PrintInvoicesAsync(IEnumerable<int> invoiceIds, string? printerName = null)
    {
        foreach (var invoiceId in invoiceIds.Distinct())
            await PrintInvoiceAsync(invoiceId, printerName);
    }

    public async Task<byte[]> RenderInvoicePdfAsync(int invoiceId)
    {
        var dt = await LoadInvoiceReportDataAsync(invoiceId);
        return RenderPdf(
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "Invoice.rdlc"),
            dt);
    }

    public async Task<byte[]> RenderInvoicesPdfAsync(IEnumerable<int> invoiceIds)
    {
        var ids = invoiceIds.Distinct().ToList();
        if (ids.Count == 1)
            return await RenderInvoicePdfAsync(ids[0]);

        throw new NotSupportedException("Bulk PDF merge ist noch nicht implementiert. Vorschau/Druck über Mehrfachvorschau verwenden.");
    }

    public async Task PreviewOrderAsync(int orderId)
    {
        var dt = await LoadOrderReportDataAsync(orderId);
        ShowOrderPreview(dt);
    }

    private async Task<DataTable> LoadOrderReportDataAsync(int orderId)
    {
        var dt = new DataTable();
        var sql = @"SELECT * FROM dbo.vwOrderReport WHERE OrderId = @OrderId ORDER BY Lieferdatum, Lieferscheinnummer, Artikelnummer;";
        using var con = new SqlConnection(GetConnectionString());
        using var cmd = new SqlCommand(sql, con);
        cmd.Parameters.AddWithValue("@OrderId", orderId);
        using var da = new SqlDataAdapter(cmd);
        await Task.Run(() => da.Fill(dt));
        return dt;
    }

    private void ShowOrderPreview(DataTable table)
    {
        var rdlcPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "Order.rdlc");
        using var form = new FrmReportViewer();
        form.ReportViewer.LocalReport.DataSources.Clear();
        form.ReportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsOrder", table));
        form.ReportViewer.LocalReport.ReportPath = rdlcPath;
        form.ReportViewer.RefreshReport();
        form.ShowDialog();
    }

    public async Task PrintOrderAsync(int orderId, string? printerName = null)
    {
        var pdf = await RenderOrderPdfAsync(orderId);
        await PrintPdfAsync(pdf, $"auftrag_{orderId}.pdf", printerName);
    }

    public async Task<byte[]> RenderOrderPdfAsync(int orderId)
    {
        var dt = await LoadOrderReportDataAsync(orderId);
        return RenderPdf(
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "Order.rdlc"),
            dt);
    }

    public async Task PreviewDeliveryAsync(int deliveryId)
    {
        var dt = await LoadDeliveryReportDataAsync(deliveryId);
        ShowDeliveryPreview(dt);
    }

    private async Task<DataTable> LoadDeliveryReportDataAsync(int deliveryId)
    {
        var dt = new DataTable();
        var sql = @"SELECT * FROM dbo.vwDeliveryReport WHERE DeliveryId = @DeliveryId ORDER BY Lieferdatum, Lieferscheinnummer, Artikelnummer;";
        using var con = new SqlConnection(GetConnectionString());
        using var cmd = new SqlCommand(sql, con);
        cmd.Parameters.AddWithValue("@DeliveryId", deliveryId);
        using var da = new SqlDataAdapter(cmd);
        await Task.Run(() => da.Fill(dt));
        return dt;
    }

    private void ShowDeliveryPreview(DataTable table)
    {
        var rdlcPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "Delivery.rdlc");
        using var form = new FrmReportViewer();
        form.ReportViewer.LocalReport.DataSources.Clear();
        form.ReportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsDelivery", table));
        form.ReportViewer.LocalReport.ReportPath = rdlcPath;
        form.ReportViewer.RefreshReport();
        form.ShowDialog();
    }

    public async Task PreviewDeliveriesAsync(IEnumerable<int> deliveryIds)
    {
        var dt = await LoadBulkDeliveryReportDataAsync(deliveryIds);
        ShowDeliveryPreview(dt);
    }

    private async Task<DataTable> LoadBulkDeliveryReportDataAsync(IEnumerable<int> deliveryIds)
    {
        var dt = new DataTable();
        var ids = deliveryIds.ToList();
        if (ids.Count == 0) return dt;
        var idList = string.Join(",", ids);
        var sql = $"SELECT * FROM dbo.vwDeliveryReport WHERE DeliveryId IN ({idList}) ORDER BY Lieferdatum, Lieferscheinnummer, Artikelnummer;";
        using var con = new SqlConnection(GetConnectionString());
        using var cmd = new SqlCommand(sql, con);
        using var da = new SqlDataAdapter(cmd);
        await Task.Run(() => da.Fill(dt));
        return dt;
    }

    public async Task PrintDeliveryAsync(int deliveryId, string? printerName = null)
    {
        var pdf = await RenderDeliveryPdfAsync(deliveryId);
        await PrintPdfAsync(pdf, $"lieferschein_{deliveryId}.pdf", printerName);
    }

    public async Task PrintDeliveriesAsync(IEnumerable<int> deliveryIds, string? printerName = null)
    {
        foreach (var deliveryId in deliveryIds.Distinct())
            await PrintDeliveryAsync(deliveryId, printerName);
    }

    public async Task<byte[]> RenderDeliveryPdfAsync(int deliveryId)
    {
        var dt = await LoadDeliveryReportDataAsync(deliveryId);
        return RenderPdf(
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "Delivery.rdlc"),
            dt);
    }

    public async Task PreviewKundenkontoAsync(int kundeId, DateTime von, DateTime bis)
    {
        var dt = await LoadKundenkontoReportDataAsync(kundeId, von, bis);
        ShowKundenkontoPreview(dt);
    }

    private async Task<DataTable> LoadKundenkontoReportDataAsync(int kundeId, DateTime von, DateTime bis)
    {
        var dt = new DataTable();
        var sql = @"SELECT * FROM dbo.vwKundenkontoReport WHERE KundeId = @KundeId AND Datum BETWEEN @Von AND @Bis ORDER BY Datum, BelegNr;";
        using var con = new SqlConnection(GetConnectionString());
        using var cmd = new SqlCommand(sql, con);
        cmd.Parameters.AddWithValue("@KundeId", kundeId);
        cmd.Parameters.AddWithValue("@Von", von);
        cmd.Parameters.AddWithValue("@Bis", bis);
        using var da = new SqlDataAdapter(cmd);
        await Task.Run(() => da.Fill(dt));
        return dt;
    }

    private void ShowKundenkontoPreview(DataTable table)
    {
        var rdlcPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "Kundenkonto.rdlc");
        using var form = new FrmReportViewer();
        form.ReportViewer.LocalReport.DataSources.Clear();
        form.ReportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsKundenkonto", table));
        form.ReportViewer.LocalReport.ReportPath = rdlcPath;
        form.ReportViewer.RefreshReport();
        form.ShowDialog();
    }

    public async Task<byte[]> RenderKundenkontoPdfAsync(int kundeId, DateTime von, DateTime bis)
    {
        var dt = await LoadKundenkontoReportDataAsync(kundeId, von, bis);
        return RenderPdf(
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "Kundenkonto.rdlc"),
            dt);
    }

    public async Task PrintKundenkontoAsync(int kundeId, DateTime von, DateTime bis, string? printerName = null)
    {
        var pdf = await RenderKundenkontoPdfAsync(kundeId, von, bis);
        await PrintPdfAsync(pdf, $"kundenkonto_{kundeId}.pdf", printerName);
    }

    private static byte[] RenderPdf(string rdlcPath, DataTable table)
    {
        using var localReport = new LocalReport();
        localReport.ReportPath = rdlcPath;
        localReport.DataSources.Clear();
        // The dataset name must match the RDLC file (e.g. dsDelivery)
        var dsName = Path.GetFileNameWithoutExtension(rdlcPath).ToLower() switch
        {
            "delivery" => "dsDelivery",
            "order" => "dsOrder",
            "kundenkonto" => "dsKundenkonto",
            "invoice" => "dsInvoice",
            _ => "dsLines"
        };
        localReport.DataSources.Add(new ReportDataSource(dsName, table));
        return localReport.Render("PDF");
    }

    private async Task PrintPdfAsync(byte[] pdf, string fileName, string? printerName)
    {
        var setup = await _appSetupService.GetAsync();
        var targetPrinter = string.IsNullOrWhiteSpace(printerName)
            ? ResolveConfiguredPrinter(setup)
            : printerName;
        var tempFile = Path.Combine(Path.GetTempPath(), fileName);
        await File.WriteAllBytesAsync(tempFile, pdf);
        var psi = new global::System.Diagnostics.ProcessStartInfo
        {
            FileName = tempFile,
            Verb = "print",
            UseShellExecute = true
        };
        if (!string.IsNullOrWhiteSpace(targetPrinter))
            psi.Arguments = $"\"{targetPrinter}\"";
        global::System.Diagnostics.Process.Start(psi);
    }

    private static string ResolveConfiguredPrinter(Domain.Entities.System.AppSetup setup)
        => setup.BriefpapierVerwenden
            ? (setup.DruckerMitLogo ?? setup.DruckerWeissesPapier ?? string.Empty)
            : (setup.DruckerWeissesPapier ?? setup.DruckerMitLogo ?? string.Empty);

    private string GetConnectionString()
    {
        var useLocalDb = _configuration.GetValue<bool>("AppSettings:UseLocalDb");
        return useLocalDb
            ? _configuration.GetConnectionString("LocalDb")
            : _configuration.GetConnectionString("SqlServer");
    }
}
