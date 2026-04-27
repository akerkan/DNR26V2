using System.ComponentModel;
using DNR26V2.Domain.DTOs.Reports;
using DNR26V2.Services.Reports;
using Microsoft.Reporting.WinForms;

namespace DNR26V2.Forms.Reports;

public partial class FrmReportViewer : Form
{
    private readonly IReadOnlyList<InvoiceReportData> _reports;
    private readonly string _rdlcPath;

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

        tabReports.TabPages.Clear();

        foreach (var report in _reports)
        {
            var page = new TabPage(report.Header.Rechnungsnummer);
            var viewer = new ReportViewer
            {
                Dock = DockStyle.Fill,
                ProcessingMode = ProcessingMode.Local
            };

            viewer.LocalReport.ReportPath = _rdlcPath;
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(new ReportDataSource(
                ReportDataTableFactory.DsHeader,
                ReportDataTableFactory.SingleRowTable(report.Header)));
            viewer.LocalReport.DataSources.Add(new ReportDataSource(
                ReportDataTableFactory.DsLines,
                ReportDataTableFactory.ToDataTable(report.Lines)));
            viewer.RefreshReport();

            page.Controls.Add(viewer);
            tabReports.TabPages.Add(page);
        }
    }
}
