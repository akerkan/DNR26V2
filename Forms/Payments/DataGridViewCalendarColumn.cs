namespace DNR26V2.Forms.Payments;

/// <summary>
/// DataGridView column that shows a DateTimePicker when a cell is being edited.
/// Standard WinForms IDataGridViewEditingControl pattern.
/// </summary>
public class DataGridViewCalendarColumn : DataGridViewColumn
{
    public DataGridViewCalendarColumn()
        : base(new DataGridViewCalendarCell()) { }

    public override DataGridViewCell CellTemplate
    {
        get => base.CellTemplate;
        set
        {
            if (value is not null and not DataGridViewCalendarCell)
                throw new InvalidCastException("CellTemplate must be DataGridViewCalendarCell.");
            base.CellTemplate = value;
        }
    }
}

public class DataGridViewCalendarCell : DataGridViewTextBoxCell
{
    public override void InitializeEditingControl(
        int rowIndex, object? initialFormattedValue,
        DataGridViewCellStyle dataGridViewCellStyle)
    {
        base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);

        if (DataGridView?.EditingControl is DataGridViewCalendarEditingControl ctl)
        {
            if (Value is DateTime dt)
                ctl.Value = dt;
            else
                ctl.Value = DateTime.Today;
        }
    }

    public override Type EditType    => typeof(DataGridViewCalendarEditingControl);
    public override Type ValueType   => typeof(DateTime);
    public override object DefaultNewRowValue => DateTime.Today;
}

public class DataGridViewCalendarEditingControl : DateTimePicker, IDataGridViewEditingControl
{
    private DataGridView? _dataGridView;
    private bool          _valueChanged;
    private int           _rowIndex;

    public DataGridViewCalendarEditingControl()
    {
        Format = DateTimePickerFormat.Short;
    }

    // ?? IDataGridViewEditingControl ???????????????????????????????????????????

    public object EditingControlFormattedValue
    {
        get => Value.ToString("dd.MM.yyyy");
        set
        {
            if (value is string s && DateTime.TryParse(s, out var dt))
                Value = dt;
        }
    }

    public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        => EditingControlFormattedValue;

    public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
    {
        Font      = dataGridViewCellStyle.Font;
        BackColor = dataGridViewCellStyle.BackColor;
        ForeColor = dataGridViewCellStyle.ForeColor;
    }

    public int  EditingControlRowIndex
    {
        get => _rowIndex;
        set => _rowIndex = value;
    }

    public bool EditingControlValueChanged
    {
        get => _valueChanged;
        set => _valueChanged = value;
    }

    public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        => keyData is Keys.Left or Keys.Right or Keys.Home or Keys.End
           || !dataGridViewWantsInputKey;

    public void PrepareEditingControlForEdit(bool selectAll) { }

    public Cursor EditingPanelCursor => Cursors.Default;
    public bool RepositionEditingControlOnValueChange => false;

    public DataGridView? EditingControlDataGridView
    {
        get => _dataGridView;
        set => _dataGridView = value;
    }

    // ?? Value change notification ?????????????????????????????????????????????

    protected override void OnValueChanged(EventArgs e)
    {
        _valueChanged = true;
        _dataGridView?.NotifyCurrentCellDirty(true);
        base.OnValueChanged(e);
    }
}
