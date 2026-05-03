using DNR26V2.Domain.Entities.MasterData;
using DNR26V2.Forms.Base;
using DNR26V2.Services.MasterData;
using DNR26V2.Services.Reports;

public partial class FrmTourList : BaseListForm
{
    private readonly IReportRenderService _reportService;
    private readonly IProductAttributeService _attributeService;

    public FrmTourList(IReportRenderService reportService, IProductAttributeService attributeService)
    {
        _reportService = reportService;
        _attributeService = attributeService;
        InitializeComponent();
    }

    private async void FrmTourList_Load(object sender, EventArgs e)
    {
        var tourValues = await _attributeService.GetValuesByEntityTypeAsync(DNR26V2.Domain.Enums.AttributeEntityType.Tour);
        cmbTour.DataSource = tourValues.Where(x => x.Aktiv).ToList();
        cmbTour.DisplayMember = "Bezeichnung";
        cmbTour.ValueMember = "Id";
        if (cmbTour.Items.Count > 0) cmbTour.SelectedIndex = 0;
        monthCalendar.SelectionStart = DateTime.Today;
    }

    private async void btnVorschau_Click(object sender, EventArgs e)
    {
        if (cmbTour.SelectedItem is ProductAttributeValue tour && monthCalendar.SelectionStart != null)
        {
            await _reportService.PreviewTourListAsync(tour.Id, monthCalendar.SelectionStart.Date);
        }
    }
}