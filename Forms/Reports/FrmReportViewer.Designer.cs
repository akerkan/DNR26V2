namespace DNR26V2.Forms.Reports;

partial class FrmReportViewer
{
    private System.ComponentModel.IContainer components = null!;
    private TabControl tabReports = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        tabReports = new TabControl();
        SuspendLayout();
        // 
        // tabReports
        // 
        tabReports.Dock = DockStyle.Fill;
        tabReports.Location = new Point(0, 0);
        tabReports.Name = "tabReports";
        tabReports.Size = new Size(1200, 800);
        tabReports.TabIndex = 0;
        // 
        // FrmReportViewer
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1200, 800);
        Controls.Add(tabReports);
        Name = "FrmReportViewer";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Berichtsvorschau";
        ResumeLayout(false);
    }
}
