namespace DNR26V2.Forms.Reports;
partial class FrmReportViewer
{
    private System.ComponentModel.IContainer components = null!; private Panel panelViewerHost = null!;
    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        panelViewerHost = new Panel();
        SuspendLayout();
        // 
        // panelViewerHost
        // 
        panelViewerHost.Dock = DockStyle.Fill;
        panelViewerHost.Location = new Point(0, 0);
        panelViewerHost.Name = "panelViewerHost";
        panelViewerHost.Size = new Size(1200, 800);
        panelViewerHost.TabIndex = 0;
        // 
        // FrmReportViewer
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1200, 800);
        Controls.Add(panelViewerHost);
        Name = "FrmReportViewer";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Berichtsvorschau";
        ResumeLayout(false);
    }
}