using System.ComponentModel;
using System.Data;
using DNR26V2.Domain.DTOs.Reports;
using DNR26V2.Services.Reports;
using Microsoft.Reporting.WinForms;

namespace DNR26V2.Forms.Reports;

public partial class FrmReportViewer : Form
{
    private readonly string _rdlcPath;
    private readonly DataTable? _headerTable;
    private readonly DataTable? _linesTable;
    private readonly string? _windowTitle;

    public FrmReportViewer()
    {
        _rdlcPath = string.Empty;
        InitializeComponent();
    }

    public FrmReportViewer(string rdlcPath, IEnumerable<InvoiceReportData> reports)
    {
        _rdlcPath = rdlcPath;
        var reportList = reports.ToList();
        _headerTable = ReportDataTableFactory.ToDataTable(reportList.Select(x => x.Header));
        _linesTable = ReportDataTableFactory.ToDataTable(reportList.SelectMany(x => x.Lines));
        _windowTitle = reportList.Count == 1
            ? $"Berichtsvorschau - {reportList[0].Header.Rechnungsnummer}"
            : $"Berichtsvorschau - {reportList.Count} Rechnungen";
        InitializeComponent();
    }

    public FrmReportViewer(string rdlcPath, DataTable headerTable, DataTable linesTable, string? windowTitle = null)
    {
        _rdlcPath = rdlcPath;
        _headerTable = headerTable;
        _linesTable = linesTable;
        _windowTitle = windowTitle;
        InitializeComponent();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
        if (_headerTable is null || _linesTable is null) return;

        reportViewerMain.ProcessingMode = ProcessingMode.Local;
        reportViewerMain.LocalReport.ReportPath = _rdlcPath;
        reportViewerMain.LocalReport.DataSources.Clear();
        reportViewerMain.LocalReport.DataSources.Add(new ReportDataSource(
            ReportDataTableFactory.DsHeader,
            _headerTable));
        reportViewerMain.LocalReport.DataSources.Add(new ReportDataSource(
            ReportDataTableFactory.DsLines,
            _linesTable));
        reportViewerMain.RefreshReport();

        if (!string.IsNullOrWhiteSpace(_windowTitle))
            Text = _windowTitle;
    }
}

