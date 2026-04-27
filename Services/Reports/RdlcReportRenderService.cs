using DNR26V2.Domain.DTOs.Reports;
using DNR26V2.Forms.Reports;
using DNR26V2.Services.System;
using Microsoft.Reporting.WinForms;

namespace DNR26V2.Services.Reports;

public class RdlcReportRenderService : IReportRenderService
{
    private const string RechnungRdlc = "Reports\\Rechnung.rdlc";

    private readonly IInvoiceReportDataService _reportDataService;
    private readonly IAppSetupService _appSetupService;

    public RdlcReportRenderService(
        IInvoiceReportDataService reportDataService,
        IAppSetupService appSetupService)
    {
        _reportDataService = reportDataService;
        _appSetupService = appSetupService;
    }

    public async Task PreviewInvoiceAsync(int invoiceId)
    {
        var report = await _reportDataService.GetInvoiceReportDataAsync(invoiceId);
        using var form = new FrmReportViewer(GetRdlcPath(), new[] { report });
        form.ShowDialog();
    }

    public async Task PreviewInvoicesAsync(IEnumerable<int> invoiceIds)
    {
        var reports = await _reportDataService.GetInvoiceReportDataAsync(invoiceIds);
        using var form = new FrmReportViewer(GetRdlcPath(), reports);
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
        var report = await _reportDataService.GetInvoiceReportDataAsync(invoiceId);
        return RenderSinglePdf(report);
    }

    public async Task<byte[]> RenderInvoicesPdfAsync(IEnumerable<int> invoiceIds)
    {
        var ids = invoiceIds.Distinct().ToList();
        if (ids.Count == 1)
            return await RenderInvoicePdfAsync(ids[0]);

        throw new NotSupportedException("Bulk PDF merge ist noch nicht implementiert. Vorschau/Druck über Mehrfachvorschau verwenden.");
    }

    private byte[] RenderSinglePdf(InvoiceReportData report)
    {
        using var localReport = new LocalReport();
        localReport.ReportPath = GetRdlcPath();
        localReport.DataSources.Clear();
        localReport.DataSources.Add(new ReportDataSource(
            ReportDataTableFactory.DsHeader,
            ReportDataTableFactory.SingleRowTable(report.Header)));
        localReport.DataSources.Add(new ReportDataSource(
            ReportDataTableFactory.DsLines,
            ReportDataTableFactory.ToDataTable(report.Lines)));
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

    private static string GetRdlcPath()
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, RechnungRdlc);
        if (!File.Exists(path))
            throw new FileNotFoundException($"RDLC nicht gefunden: {path}");
        return path;
    }
}
