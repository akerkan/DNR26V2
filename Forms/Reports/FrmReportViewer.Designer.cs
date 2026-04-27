namespace DNR26V2.Forms.Reports;

partial class FrmReportViewer
{
    private System.ComponentModel.IContainer components = null!;
    private Microsoft.Reporting.WinForms.ReportViewer reportViewerMain = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        reportViewerMain = new Microsoft.Reporting.WinForms.ReportViewer();
        SuspendLayout();
        // 
        // reportViewerMain
        // 
        reportViewerMain.Dock = DockStyle.Fill;
        reportViewerMain.Location = new Point(0, 0);
        reportViewerMain.Name = "reportViewerMain";
        reportViewerMain.ServerReport.BearerToken = null;
        reportViewerMain.Size = new Size(1200, 800);
        reportViewerMain.TabIndex = 0;
        // 
        // FrmReportViewer
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1200, 800);
        Controls.Add(reportViewerMain);
        Name = "FrmReportViewer";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Berichtsvorschau";
        ResumeLayout(false);
    }
}