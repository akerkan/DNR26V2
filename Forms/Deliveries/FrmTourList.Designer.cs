partial class FrmTourList
{
    private ComboBox cmbTour;
    private MonthCalendar monthCalendar;
    private Button btnVorschau;
    private Button btnDrucken;

    private void InitializeComponent()
    {
        cmbTour = new ComboBox();
        monthCalendar = new MonthCalendar();
        btnVorschau = new Button();
        btnDrucken = new Button();
        SuspendLayout();
        // 
        // cmbTour
        // 
        cmbTour.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbTour.Location = new Point(20, 20);
        cmbTour.Name = "cmbTour";
        cmbTour.Size = new Size(200, 28);
        cmbTour.TabIndex = 0;
        // 
        // monthCalendar
        // 
        monthCalendar.Location = new Point(20, 60);
        monthCalendar.Name = "monthCalendar";
        monthCalendar.TabIndex = 1;
        // 
        // btnVorschau
        // 
        btnVorschau.Location = new Point(20, 286);
        btnVorschau.Name = "btnVorschau";
        btnVorschau.Size = new Size(97, 31);
        btnVorschau.TabIndex = 2;
        btnVorschau.Text = "Vorschau";
        btnVorschau.Click += btnVorschau_Click;
        // 
        // btnDrucken
        // 
        btnDrucken.Enabled = false;
        btnDrucken.Location = new Point(132, 286);
        btnDrucken.Name = "btnDrucken";
        btnDrucken.Size = new Size(94, 31);
        btnDrucken.TabIndex = 3;
        btnDrucken.Text = "Drucken";
        // 
        // FrmTourList
        // 
        ClientSize = new Size(248, 334);
        Controls.Add(cmbTour);
        Controls.Add(monthCalendar);
        Controls.Add(btnVorschau);
        Controls.Add(btnDrucken);
        Name = "FrmTourList";
        Text = "Tourenliste";
        Load += FrmTourList_Load;
        ResumeLayout(false);
    }
}