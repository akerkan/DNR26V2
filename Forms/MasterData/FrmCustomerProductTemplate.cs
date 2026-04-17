using System.ComponentModel;
using System.Threading;
using DNR26V2.Domain.DTOs;
using DNR26V2.Domain.Entities.MasterData;
using DNR26V2.Domain.Enums;
using DNR26V2.Domain.Exceptions;
using DNR26V2.Forms.Base;
using DNR26V2.Helpers;
using DNR26V2.Services.MasterData;

namespace DNR26V2.Forms.MasterData;

public partial class FrmCustomerProductTemplate : BaseListForm
{
    private readonly ICustomerService _customerService;
    private readonly IProductService _productService;
    private readonly ICustomerProductService _cpService;

    private readonly SemaphoreSlim _lock = new(1, 1);
    private bool _suppressKundenChanged;
    private bool _isDirty;

    private int _selectedKundeId;
    private string _selectedKundename = string.Empty;

    private IReadOnlyList<ProductAttribute> _attribute = [];

    /// <summary>Attribut-IDs deren Werte mindestens ein IstVorlage=true haben → DropDown statt DropDownList.</summary>
    private readonly HashSet<int> _vorlageAttributIds = [];

    private readonly List<KundenproduktZeile> _zeilen = [];
    private readonly System.Windows.Forms.Timer _searchTimer = new() { Interval = 350 };

    // ── Konstruktoren ─────────────────────────────────────────────────────────

    public FrmCustomerProductTemplate()
    {
        _customerService = null!;
        _productService = null!;
        _cpService = null!;
        InitializeComponent();
        WireUpEvents();
    }

    public FrmCustomerProductTemplate(
        ICustomerService customerService,
        IProductService productService,
        ICustomerProductService cpService)
    {
        _customerService = customerService;
        _productService = productService;
        _cpService = cpService;
        InitializeComponent();
        WireUpEvents();
    }

    private static bool IsDesignMode() =>
        LicenseManager.UsageMode == LicenseUsageMode.Designtime;

    // ── Events verdrahten ─────────────────────────────────────────────────────

    private void WireUpEvents()
    {
        Load += FrmCustomerProductTemplate_Load;

        txtSuche.TextChanged += (_, _) => { _searchTimer.Stop(); _searchTimer.Start(); };
        txtSuche.KeyDown += TxtSuche_KeyDown;
        dgwKunden.SelectionChanged += DgwKunden_SelectionChanged;
        btnSpeichern.Click += BtnSpeichern_Click;

        dgwKundenprodukte.CellValueChanged += (_, _) => _isDirty = true;

        // IstVorlage: ComboBox-Spalten editierbar machen
        dgwKundenprodukte.EditingControlShowing += DgwKundenprodukte_EditingControlShowing;
        dgwKundenprodukte.DataError += DgwKundenprodukte_DataError;

        _searchTimer.Tick += async (_, _) =>
        {
            _searchTimer.Stop();
            await LoadKundenAsync();
        };
    }

    // ── Load ──────────────────────────────────────────────────────────────────

    private async void FrmCustomerProductTemplate_Load(object? sender, EventArgs e)
    {
        if (IsDesignMode() || _customerService is null) return;

        WindowState = FormWindowState.Maximized;
        ConfigureGrid(dgwKunden);
        SetupAlleProdukte();

        await LoadAttributeAsync();
        await LoadKundenAsync();
    }

    // ── Attribute einmalig laden ──────────────────────────────────────────────

    private async Task LoadAttributeAsync()
    {
        await _lock.WaitAsync();
        try
        {
            _attribute = await _cpService.GetApplicableAttributesAsync();

            _vorlageAttributIds.Clear();
            foreach (var attr in _attribute)
            {
                // IstVorlage artık Attribute header'ında — wert bazlı değil
                if (attr.Feldtyp == AttributeFieldType.Lookup && attr.IstVorlage)
                    _vorlageAttributIds.Add(attr.Id);
            }
        }
        catch (Exception ex) { ShowError($"Attribut-Ladefehler:\n{ex.Message}"); }
        finally { _lock.Release(); }
    }

    // ── IstVorlage: ComboBox editierbar maken ───────────────────────────────

    private void DgwKundenprodukte_EditingControlShowing(object? sender,
        DataGridViewEditingControlShowingEventArgs e)
    {
        // Her seferinde önce unsubscribe — çift subscription önlenir
        if (e.Control is ComboBox prev)
            prev.TextChanged -= OnVorlageComboTextChanged;

        if (e.Control is ComboBox cmb
            && dgwKundenprodukte.CurrentCell?.OwningColumn?.Name is string colName
            && colName.StartsWith("_Attr_")
            && int.TryParse(colName[6..], out var attrId)
            && _vorlageAttributIds.Contains(attrId))
        {
            cmb.DropDownStyle = ComboBoxStyle.DropDown;
            cmb.TextChanged += OnVorlageComboTextChanged; // ← Serbest metin takibi
        }
    }

    // Alan olarak ekle:
    private bool _suppressTagUpdate;

    /// <summary>
    /// Kullanıcı IstVorlage ComboBox'a yazarken Tag'i güncelle.
    /// Listeden seçim yapıldıysa → Tag temizle (normal lookup).
    /// Serbest metin giriliyorsa → Tag'e kaydet (CellFormatting gösterecek).
    /// </summary>
    private void OnVorlageComboTextChanged(object? sender, EventArgs e)
    {
        if (_suppressTagUpdate) return;
        if (sender is not ComboBox cmb) return;
        var cell = dgwKundenprodukte.CurrentCell;
        if (cell?.OwningColumn is not DataGridViewComboBoxColumn col) return;
        if (col.DataSource is not List<ProductAttributeValue> items) return;

        var text = cmb.Text;
        var eslesme = items.FirstOrDefault(i =>
            i.Id > 0 &&
            string.Equals(i.Bezeichnung, text, StringComparison.OrdinalIgnoreCase));

        if (eslesme is not null)
        {
            cell.Tag = null;
        }
        else
        {
            var newTag = string.IsNullOrEmpty(text) ? null : text;
            if ((string?)cell.Tag != newTag)          // ← gerçekten değişti mi?
            {
                cell.Tag = newTag;
                _isDirty = true;                      // ← EKLENDİ
            }
        }
    }

    private void DgwKundenprodukte_DataError(object? sender, DataGridViewDataErrorEventArgs e)
    {
        // IstVorlage sütunlarında serbest metin girişi → hatayı sessizce geç
        if (dgwKundenprodukte.Columns[e.ColumnIndex].Name.StartsWith("_Attr_"))
            e.ThrowException = false;
    }

    // ── CellFormatting: Tag varsa serbest metni göster ────────────────────────

    private void DgwKundenprodukte_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var col = dgwKundenprodukte.Columns[e.ColumnIndex];
        if (!col.Name.StartsWith("_Attr_")) return;
        if (col is not DataGridViewComboBoxColumn) return;

        var cell = dgwKundenprodukte.Rows[e.RowIndex].Cells[e.ColumnIndex];

        // Tag varsa → serbest metin göster (hücre değeri ne olursa olsun)
        if (cell.Tag is string freitext && !string.IsNullOrEmpty(freitext))
        {
            e.Value = freitext;
            e.FormattingApplied = true;
        }
    }

    // ── Kundenliste ───────────────────────────────────────────────────────────

    private async Task LoadKundenAsync()
    {
        IReadOnlyList<CustomerListDto> liste;

        await _lock.WaitAsync();
        try
        {
            Cursor = Cursors.WaitCursor;
            liste = await _customerService.GetListAsync(new CustomerListFilter
            {
                Suche = txtSuche.Text.NullIfEmpty(),
                NurAktiv = true
            });
        }
        catch (Exception ex) { ShowError($"Ladefehler:\n{ex.Message}"); return; }
        finally
        {
            Cursor = Cursors.Default;
            _lock.Release();
        }

        // ── UI-Arbeit: ÖNCE suppress = true, DataSource değişikliği SONRA ──────
        _suppressKundenChanged = true;
        try
        {
            var prevId = SelectedKundeId();
            dgwKunden.DataSource = null;
            dgwKunden.DataSource = liste.ToList();
            StyleKundenGrid();

            if (prevId > 0) SelectKundeById(prevId);
            else if (dgwKunden.Rows.Count > 0)
                dgwKunden.CurrentCell = GetFirstVisibleCell(dgwKunden.Rows[0]);
        }
        finally
        {
            _suppressKundenChanged = false; // ← DataSource bittikten SONRA sıfırla
        }

        var id = SelectedKundeId();
        if (id > 0) await LoadRechtsAsync(id);
    }

    private void StyleKundenGrid()
    {
        if (dgwKunden.Columns.Count == 0) return;
        foreach (DataGridViewColumn c in dgwKunden.Columns) c.Visible = false;
        ShowKol(dgwKunden, "Kundennummer", "Nr.", 70);
        ShowKol(dgwKunden, "Kundenname", "Name", 0, fill: true);
        ShowKol(dgwKunden, "Tur", "Tour", 55);
        ShowKol(dgwKunden, "Offen", "Offen", 45);
        ShowKol(dgwKunden, "Aktiv", "Aktiv", 45);
    }

    // ── Rechte Seite laden ────────────────────────────────────────────────────

    private async Task LoadRechtsAsync(int kundeId)
    {
        IReadOnlyList<ProductListDto> alleProdukte;
        IReadOnlyList<CustomerProductDto> kundenProdukte;

        await _lock.WaitAsync();
        try
        {
            Cursor = Cursors.WaitCursor;
            alleProdukte = await _productService.GetListAsync(new ProductListFilter { NurAktiv = true });
            kundenProdukte = await _cpService.GetByCustomerAsync(kundeId);
        }
        catch (Exception ex) { ShowError($"Ladefehler:\n{ex.Message}"); return; }
        finally
        {
            Cursor = Cursors.Default;
            _lock.Release();
        }

        var kundeRow = dgwKunden.CurrentRow?.DataBoundItem as CustomerListDto;
        _selectedKundeId = kundeId;
        _selectedKundename = kundeRow?.Kundenname ?? string.Empty;
        lblAlleProdukte.Text = $"Alle Produkte  |  Kunde: {_selectedKundename}";

        var zugeordneteIds = kundenProdukte.Select(kp => kp.ArtikelId).ToHashSet();
        var nichtZugeordnet = alleProdukte
            .Where(p => !zugeordneteIds.Contains(p.Id))
            .ToList();

        dgwAlleProdukte.DataSource = null;
        dgwAlleProdukte.DataSource = nichtZugeordnet;
        StyleAlleProduktGrid();

        await BuildKundenproduktGridAsync(kundenProdukte);
        _isDirty = false;
    }

    // ── Oberes Grid: Alle Produkte ────────────────────────────────────────────

    private void SetupAlleProdukte()
    {
        dgwAlleProdukte.AllowUserToAddRows = false;
        dgwAlleProdukte.AllowUserToDeleteRows = false;
        dgwAlleProdukte.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgwAlleProdukte.MultiSelect = false;
        dgwAlleProdukte.RowHeadersVisible = false;
        dgwAlleProdukte.BackgroundColor = SystemColors.Window;
        dgwAlleProdukte.ReadOnly = false;
        dgwAlleProdukte.CellClick += DgwAlleProdukte_HinzufuegenClick;
    }

    private void StyleAlleProduktGrid()
    {
        if (dgwAlleProdukte.Columns.Count == 0) return;

        foreach (DataGridViewColumn c in dgwAlleProdukte.Columns)
        {
            c.Visible = false;
            c.ReadOnly = true;
        }

        ShowKol(dgwAlleProdukte, "Artikelnummer", "Art.-Nr.", 90);
        ShowKol(dgwAlleProdukte, "Bezeichnung", "Produkt", 0, fill: true);
        ShowKol(dgwAlleProdukte, "VKPreis", "VK-Preis", 80, format: "N2", rightAlign: true);

        // Her seferinde sil ve sona yeniden ekle → sütun sırası her zaman doğru
        if (dgwAlleProdukte.Columns.Contains("_KundenPreis"))
            dgwAlleProdukte.Columns.Remove("_KundenPreis");
        if (dgwAlleProdukte.Columns.Contains("_Hinzufuegen"))
            dgwAlleProdukte.Columns.Remove("_Hinzufuegen");

        dgwAlleProdukte.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "_KundenPreis",
            HeaderText = "Kundenpreis",
            Width = 90,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
            ReadOnly = false,
            DefaultCellStyle = new DataGridViewCellStyle
            {
                Format = "N2",
                Alignment = DataGridViewContentAlignment.MiddleRight
            }
        });

        dgwAlleProdukte.Columns.Add(new DataGridViewButtonColumn
        {
            Name = "_Hinzufuegen",
            HeaderText = "",
            Text = "Hinzufügen",
            UseColumnTextForButtonValue = true,
            Width = 90,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        });

        foreach (DataGridViewRow row in dgwAlleProdukte.Rows)
        {
            if (row.IsNewRow) continue;
            if (row.Cells["_KundenPreis"].Value is null or "")
                row.Cells["_KundenPreis"].Value = row.Cells["VKPreis"].Value;
        }
    }

    private async void DgwAlleProdukte_HinzufuegenClick(object? s, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        if (dgwAlleProdukte.Columns[e.ColumnIndex].Name != "_Hinzufuegen") return;
        if (_selectedKundeId <= 0) return;
        if (dgwAlleProdukte.Rows[e.RowIndex].DataBoundItem is not ProductListDto produkt) return;

        dgwAlleProdukte.EndEdit();

        var preisVal = dgwAlleProdukte.Rows[e.RowIndex].Cells["_KundenPreis"].Value;
        var preis = ParseDecimal(preisVal);

        try
        {
            await _lock.WaitAsync();
            try { await _cpService.AddProductAsync(_selectedKundeId, produkt.Id, preis); }
            finally { _lock.Release(); }

            await LoadRechtsAsync(_selectedKundeId);
            ShowSuccess($"'{produkt.Bezeichnung}' hinzugefügt.");
        }
        catch (ValidationException ex) { ShowError(ex.Message); }
        catch (Exception ex) { ShowError($"Fehler:\n{ex.Message}"); }
    }

    // ── Unteres Grid: Kundenprodukte (dynamische Attribut-Spalten) ────────────


    private async Task BuildKundenproduktGridAsync(IReadOnlyList<CustomerProductDto> kundenProdukte)
    {
        dgwKundenprodukte.CellValueChanged -= KpCellChanged;
        dgwKundenprodukte.CellClick -= DgwKundenprodukte_EntfernenClick;
        dgwKundenprodukte.CellFormatting -= DgwKundenprodukte_CellFormatting;

        _zeilen.Clear();
        dgwKundenprodukte.Rows.Clear();
        dgwKundenprodukte.Columns.Clear();
        dgwKundenprodukte.DataSource = null;

        AddKpCol("_Id", "", 40, readOnly: true, hidden: true);
        AddKpCol("_ArtikelId", "", 40, readOnly: true, hidden: true);
        AddKpCol("Artikelnummer", "Art.-Nr.", 90, readOnly: true);
        AddKpCol("Produktname", "Produkt", 0, readOnly: true, fill: true);
        AddKpCol("Preis", "Preis", 85, format: "N2", rightAlign: true);
        AddKpCol("Menge", "Menge", 70, format: "N3", rightAlign: true);
        AddKpCol("Gewicht", "Gewicht", 70, format: "N3", rightAlign: true);

        foreach (var attr in _attribute)
        {
            if (attr.Feldtyp == AttributeFieldType.Lookup)
            {
                // Boş seçenek için Bezeichnung = "" (Id=0) — ComboBox boş görünür
                var items = new List<ProductAttributeValue>
                {
                    new() { Id = 0, Bezeichnung = "" }
                };
                items.AddRange(attr.Werte);

                dgwKundenprodukte.Columns.Add(new DataGridViewComboBoxColumn
                {
                    Name = $"_Attr_{attr.Id}",
                    HeaderText = attr.Bezeichnung,
                    Width = 110,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                    DisplayMember = "Bezeichnung",
                    ValueMember = "Id",
                    DataSource = items,
                    DataPropertyName = string.Empty
                });
            }
            else
            {
                AddKpCol($"_Attr_{attr.Id}", attr.Bezeichnung, 110);
            }
        }

        dgwKundenprodukte.Columns.Add(new DataGridViewButtonColumn
        {
            Name = "_Entfernen",
            HeaderText = "",
            Text = "Entfernen",
            UseColumnTextForButtonValue = true,
            Width = 80,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        });

        foreach (var kp in kundenProdukte)
        {
            IReadOnlyList<CustomerProductAttributeDto> attrs;
            await _lock.WaitAsync();
            try { attrs = await _cpService.GetAttributesAsync(kp.Id); }
            finally { _lock.Release(); }

            _zeilen.Add(new KundenproduktZeile(kp, attrs));

            var rowIdx = dgwKundenprodukte.Rows.Add();
            var row = dgwKundenprodukte.Rows[rowIdx];

            row.Cells["_Id"].Value = kp.Id;
            row.Cells["_ArtikelId"].Value = kp.ArtikelId;
            row.Cells["Artikelnummer"].Value = kp.Artikelnummer;
            row.Cells["Produktname"].Value = kp.Produktname;
            row.Cells["Preis"].Value = kp.Preis;
            row.Cells["Menge"].Value = kp.Menge;
            row.Cells["Gewicht"].Value = kp.Gewicht;

            foreach (var attr in _attribute)
            {
                var colName = $"_Attr_{attr.Id}";
                if (!dgwKundenprodukte.Columns.Contains(colName)) continue;

                var wert = attrs.FirstOrDefault(a => a.AttributId == attr.Id);

                if (attr.Feldtyp == AttributeFieldType.Lookup)
                {
                    if (_vorlageAttributIds.Contains(attr.Id)
                        && wert?.AttributWertId is null or 0
                        && !string.IsNullOrEmpty(wert?.FreierText))
                    {
                        row.Cells[colName].Value = 0;
                        row.Cells[colName].Tag = wert.FreierText;
                    }
                    else
                    {
                        // 0 = boş seçenek (Bezeichnung="") → ComboBox boş görünür
                        row.Cells[colName].Value = wert?.AttributWertId ?? 0;
                    }
                }
                else
                {
                    row.Cells[colName].Value = wert?.FreierText ?? string.Empty;
                }
            }
        }

        dgwKundenprodukte.CellClick += DgwKundenprodukte_EntfernenClick;
        dgwKundenprodukte.CellValueChanged += KpCellChanged;
        dgwKundenprodukte.CellFormatting += DgwKundenprodukte_CellFormatting;
    }

    private void KpCellChanged(object? s, DataGridViewCellEventArgs e) => _isDirty = true;

    private void AddKpCol(string name, string header, int width,
        bool readOnly = false, bool hidden = false, bool fill = false,
        string? format = null, bool rightAlign = false)
    {
        var col = new DataGridViewTextBoxColumn
        {
            Name = name,
            HeaderText = header,
            ReadOnly = readOnly,
            Visible = !hidden,
            AutoSizeMode = fill
                ? DataGridViewAutoSizeColumnMode.Fill
                : DataGridViewAutoSizeColumnMode.None
        };
        if (!fill) col.Width = width;
        if (format is not null)
            col.DefaultCellStyle.Format = format;
        if (rightAlign)
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        dgwKundenprodukte.Columns.Add(col);
    }

    // ── Speichern ─────────────────────────────────────────────────────────────

    private async Task SaveKundenProdukteAsync()
    {
        if (!_isDirty) return;

        _suppressTagUpdate = true;                                         // ← EKLENDİ
        dgwKundenprodukte.EndEdit();
        _suppressTagUpdate = false;                                        // ← EKLENDİ

        try
        {
            foreach (DataGridViewRow row in dgwKundenprodukte.Rows)
            {
                if (row.IsNewRow) continue;
                if (row.Cells["_Id"].Value is not int cpId || cpId <= 0) continue;

                var entity = await _cpService.GetByIdAsync(cpId);
                if (entity is null) continue;

                entity.Preis   = ParseDecimal(row.Cells["Preis"].Value);
                entity.Menge   = ParseDecimal(row.Cells["Menge"].Value);
                entity.Gewicht = ParseDecimal(row.Cells["Gewicht"].Value);

                await _lock.WaitAsync();
                try { await _cpService.SaveAsync(entity); }
                finally { _lock.Release(); }

                foreach (var attr in _attribute)
                {
                    var colName = $"_Attr_{attr.Id}";
                    if (!dgwKundenprodukte.Columns.Contains(colName)) continue;

                    var cell    = row.Cells[colName];
                    var cellVal = cell.Value;
                    var mapping = new CustomerProductAttributeMapping
                    {
                        CustomerProductId = cpId,
                        AttributId        = attr.Id
                    };

                    if (attr.Feldtyp == AttributeFieldType.Lookup)
                    {
                        if (_vorlageAttributIds.Contains(attr.Id)
                            && cell.Tag is string tagText
                            && !string.IsNullOrEmpty(tagText))
                        {
                            mapping.AttributWertId = null;
                            mapping.FreierText     = tagText;
                        }
                        else if (cellVal is int i && i > 0)
                        {
                            mapping.AttributWertId = i;
                            mapping.FreierText     = null;
                        }
                        else
                        {
                            mapping.AttributWertId = null;
                            mapping.FreierText     = null;
                        }
                    }
                    else
                    {
                        mapping.FreierText = cellVal?.ToString().NullIfEmpty();
                    }

                    await _lock.WaitAsync();
                    try { await _cpService.SaveAttributeAsync(mapping); }
                    finally { _lock.Release(); }
                }
            }

            _isDirty = false;
            ShowSuccess("Kundenprodukte gespeichert.");
        }
        catch (Exception ex) { ShowError($"Fehler beim Speichern:\n{ex.Message}"); }
    }

    private async void DgwKundenprodukte_EntfernenClick(object? s, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        if (dgwKundenprodukte.Columns[e.ColumnIndex].Name != "_Entfernen") return;

        var row = dgwKundenprodukte.Rows[e.RowIndex];
        var name = row.Cells["Produktname"].Value?.ToString() ?? "?";
        if (!Confirm($"'{name}' aus Kundenschablone entfernen?")) return;

        if (row.Cells["_Id"].Value is not int cpId || cpId <= 0) return;

        try
        {
            await _lock.WaitAsync();
            try { await _cpService.RemoveAsync(cpId); }
            finally { _lock.Release(); }

            await LoadRechtsAsync(_selectedKundeId);
        }
        catch (Exception ex) { ShowError($"Fehler:\n{ex.Message}"); }
    }

    // ── Hilfsmethoden ─────────────────────────────────────────────────────────

    private int SelectedKundeId() =>
        dgwKunden.CurrentRow?.DataBoundItem is CustomerListDto d ? d.Id : 0;

    private static decimal ParseDecimal(object? val)
    {
        if (val is decimal d) return d;
        if (val is string s && decimal.TryParse(
                s, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.CurrentCulture, out var r)) return r;
        return 0;
    }

    private void SelectKundeById(int id)
    {
        foreach (DataGridViewRow row in dgwKunden.Rows)
        {
            if (row.DataBoundItem is CustomerListDto d && d.Id == id)
            {
                row.Selected = true;
                var cell = GetFirstVisibleCell(row);
                if (cell is not null) dgwKunden.CurrentCell = cell;
                break;
            }
        }
    }

    private static void ShowKol(DataGridView grid, string name, string header, int width,
        bool fill = false, string? format = null, bool rightAlign = false)
    {
        if (!grid.Columns.Contains(name)) return;
        var col = grid.Columns[name];
        col.Visible = true;
        col.HeaderText = header;
        if (fill) col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        else { col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; col.Width = width; }
        if (format is not null)
            col.DefaultCellStyle.Format = format;
        if (rightAlign)
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
    }

    private static DataGridViewCell? GetFirstVisibleCell(DataGridViewRow row)
    {
        foreach (DataGridViewCell c in row.Cells)
            if (c.OwningColumn?.Visible == true) return c;
        return null;
    }

    // ── Events ────────────────────────────────────────────────────────────────

    private async void DgwKunden_SelectionChanged(object? s, EventArgs e)
    {
        if (IsDesignMode() || _suppressKundenChanged) return;
        if (_isDirty && !Confirm("Ungespeicherte Änderungen verwerfen?")) return;
        var id = SelectedKundeId();
        if (id > 0) await LoadRechtsAsync(id);
    }

    private async void TxtSuche_KeyDown(object? s, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;
        _searchTimer.Stop();
        await LoadKundenAsync();
        e.Handled = true;
    }

    private async void BtnSpeichern_Click(object? s, EventArgs e) =>
        await SaveKundenProdukteAsync();

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == (Keys.Control | Keys.S)) { _ = SaveKundenProdukteAsync(); return true; }
        return base.ProcessCmdKey(ref msg, keyData);
    }

    // ── Hilfsklasse ───────────────────────────────────────────────────────────

    private sealed class KundenproduktZeile
    {
        public CustomerProductDto Dto { get; }
        public IReadOnlyList<CustomerProductAttributeDto> Attrs { get; }
        public KundenproduktZeile(CustomerProductDto dto,
            IReadOnlyList<CustomerProductAttributeDto> attrs)
        { Dto = dto; Attrs = attrs; }
    }
}