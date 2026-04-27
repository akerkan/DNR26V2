using System.ComponentModel;
using DNR26V2.Domain.DTOs.Reports;
using DNR26V2.Services.Reports;
using Microsoft.Reporting.WinForms;

namespace DNR26V2.Forms.Reports;
public partial class FrmReportViewer : Form
{
    private readonly IReadOnlyList<InvoiceReportData> _reports; private readonly string _rdlcPath; private ReportViewer? _reportViewer;
    public FrmReportViewer()
    {
        _reports = [];
        _rdlcPath = string.Empty;
        InitializeComponent();
    }

    public FrmReportViewer(string rdlcPath, IEnumerable<InvoiceReportData> reports)
    {
        _rdlcPath = rdlcPath;
        _reports = reports.ToList();
        InitializeComponent();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
        if (_reports.Count == 0) return;

        panelViewerHost.Controls.Clear();

        _reportViewer = new ReportViewer
        {
            Dock = DockStyle.Fill,
            ProcessingMode = ProcessingMode.Local
        };

        var headers = _reports.Select(x => x.Header).ToList();
        var lines = _reports.SelectMany(x => x.Lines).ToList();

        _reportViewer.LocalReport.ReportPath = _rdlcPath;
        _reportViewer.LocalReport.DataSources.Clear();
        _reportViewer.LocalReport.DataSources.Add(new ReportDataSource(
            ReportDataTableFactory.DsHeader,
            ReportDataTableFactory.ToDataTable(headers)));
        _reportViewer.LocalReport.DataSources.Add(new ReportDataSource(
            ReportDataTableFactory.DsLines,
            ReportDataTableFactory.ToDataTable(lines)));
        _reportViewer.RefreshReport();

        panelViewerHost.Controls.Add(_reportViewer);

        Text = _reports.Count == 1
            ? $"Berichtsvorschau - {_reports[0].Header.Rechnungsnummer}"
            : $"Berichtsvorschau - {_reports.Count} Rechnungen";
    }
}

