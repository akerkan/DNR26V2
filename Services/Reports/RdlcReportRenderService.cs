using System.Data;
using DNR26V2.Domain.DTOs.Reports;
using DNR26V2.Forms.Reports;
using DNR26V2.Services.System;
using Microsoft.Reporting.WinForms;

namespace DNR26V2.Services.Reports;

public class RdlcReportRenderService : IReportRenderService
{
    private const string RechnungRdlc = "Reports\\Rechnung.rdlc";
    private const string AuftragRdlc = "Reports\\Auftragsbestaetigung.rdlc";
    private const string LieferscheinRdlc = "Reports\\Lieferschein.rdlc";

    private readonly IInvoiceReportDataService _invoiceReportDataService;
    private readonly IOrderReportDataService _orderReportDataService;
    private readonly IDeliveryReportDataService _deliveryReportDataService;
    private readonly IAppSetupService _appSetupService;

    public RdlcReportRenderService(
        IInvoiceReportDataService invoiceReportDataService,
        IOrderReportDataService orderReportDataService,
        IDeliveryReportDataService deliveryReportDataService,
        IAppSetupService appSetupService)
    {
        _invoiceReportDataService = invoiceReportDataService;
        _orderReportDataService = orderReportDataService;
        _deliveryReportDataService = deliveryReportDataService;
        _appSetupService = appSetupService;
    }

    public async Task PreviewInvoiceAsync(int invoiceId)
    {
        var report = await _invoiceReportDataService.GetInvoiceReportDataAsync(invoiceId);
        using var form = new FrmReportViewer(GetRdlcPath(RechnungRdlc), new[] { report });
        form.ShowDialog();
    }

    public async Task PreviewInvoicesAsync(IEnumerable<int> invoiceIds)
    {
        var reports = await _invoiceReportDataService.GetInvoiceReportDataAsync(invoiceIds);
        using var form = new FrmReportViewer(GetRdlcPath(RechnungRdlc), reports);
        form.ShowDialog();
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
        var report = await _invoiceReportDataService.GetInvoiceReportDataAsync(invoiceId);
        return RenderPdf(
            GetRdlcPath(RechnungRdlc),
            ReportDataTableFactory.SingleRowTable(report.Header),
            ReportDataTableFactory.ToDataTable(report.Lines));
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
        var report = await _orderReportDataService.GetOrderReportDataAsync(orderId);
        using var form = new FrmReportViewer(
            GetRdlcPath(AuftragRdlc),
            ReportDataTableFactory.SingleRowTable(report.Header),
            ReportDataTableFactory.ToDataTable(report.Lines),
            $"Auftragsbestätigung - {report.Header.Auftragsnummer}");
        form.ShowDialog();
    }

    public async Task PrintOrderAsync(int orderId, string? printerName = null)
    {
        var pdf = await RenderOrderPdfAsync(orderId);
        await PrintPdfAsync(pdf, $"auftrag_{orderId}.pdf", printerName);
    }

    public async Task<byte[]> RenderOrderPdfAsync(int orderId)
    {
        var report = await _orderReportDataService.GetOrderReportDataAsync(orderId);
        return RenderPdf(
            GetRdlcPath(AuftragRdlc),
            ReportDataTableFactory.SingleRowTable(report.Header),
            ReportDataTableFactory.ToDataTable(report.Lines));
    }

    public async Task PreviewDeliveryAsync(int deliveryId)
    {
        var report = await _deliveryReportDataService.GetDeliveryReportDataAsync(deliveryId);
        using var form = new FrmReportViewer(
            GetRdlcPath(LieferscheinRdlc),
            ReportDataTableFactory.SingleRowTable(report.Header),
            ReportDataTableFactory.ToDataTable(report.Lines),
            $"Lieferschein - {report.Header.Lieferscheinnummer}");
        form.ShowDialog();
    }

    public async Task PreviewDeliveriesAsync(IEnumerable<int> deliveryIds)
    {
        var reports = await _deliveryReportDataService.GetDeliveryReportDataAsync(deliveryIds);
        using var form = new FrmReportViewer(
            GetRdlcPath(LieferscheinRdlc),
            ReportDataTableFactory.ToDataTable(reports.Select(x => x.Header)),
            ReportDataTableFactory.ToDataTable(reports.SelectMany(x => x.Lines)),
            $"Lieferschein-Vorschau - {reports.Count} Dokumente");
        form.ShowDialog();
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
        var report = await _deliveryReportDataService.GetDeliveryReportDataAsync(deliveryId);
        return RenderPdf(
            GetRdlcPath(LieferscheinRdlc),
            ReportDataTableFactory.SingleRowTable(report.Header),
            ReportDataTableFactory.ToDataTable(report.Lines));
    }

    private static byte[] RenderPdf(string rdlcPath, DataTable headerTable, DataTable linesTable)
    {
        using var localReport = new LocalReport();
        localReport.ReportPath = rdlcPath;
        localReport.DataSources.Clear();
        localReport.DataSources.Add(new ReportDataSource(ReportDataTableFactory.DsHeader, headerTable));
        localReport.DataSources.Add(new ReportDataSource(ReportDataTableFactory.DsLines, linesTable));
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

    private static string GetRdlcPath(string relativePath)
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
        if (!File.Exists(path))
            throw new FileNotFoundException($"RDLC nicht gefunden: {path}");
        return path;
    }
}
