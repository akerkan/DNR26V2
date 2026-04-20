# DNR26V2 – GitHub Copilot Instructions

You are acting as a **senior software architect, ERP process analyst, database architect, and lead .NET WinForms engineer** for this project.

---

## CRITICAL NAMING CONVENTION

- Database **TABLE** names → **ENGLISH**
- Database **COLUMN** (field) names → **GERMAN**
- C# class names, methods, services, namespaces → **ENGLISH**
- UI (labels, forms, button texts, menu items) → **GERMAN**

---

## WORKING RULES

- Legacy project **DNR26** → READ-ONLY reference. NEVER touch it.
- ALL new code → only in **DNR26V2**
- Solo developer — keep it practical, no overengineering
- Repository: `https://github.com/akerkan/DNR26V2`

---

## ⚠️ FORM DESIGN RULES — ABSOLUTE, NEVER VIOLATE

These rules were violated multiple times. They are NON-NEGOTIABLE:

1. **ALL controls MUST be placed in the Designer (.Designer.cs)** — NEVER create controls at runtime in code
2. **Forms MUST always open correctly in Visual Studio Designer (Entwurf) mode** — if Designer breaks, the solution is WRONG
3. **NO Dependency Injection in forms** — no `IServiceProvider`, no constructor injection
4. **Use classic `new Form()` instantiation** — nothing else
5. **NEVER change the form layout, tabs, panels, or menus unless explicitly asked** — only add what is requested
6. **When adding new fields to a form:** only tell the developer "add a Label here, a ComboBox there, name it X" — do NOT rewrite the whole form
7. **If Designer stops working after a change → immediately revert that change**
8. **Do NOT remove or reorganize existing working controls** — only ADD what is asked
9. **Detail panel (panelDetail) MUST always be visible on form load** — NEVER call `SetDetailVisible(false)` on load. If the grid is empty → automatically call `NewXxx()` to enter new-record mode. This was violated in FrmProductList and FrmCustomerList.
10. **Do NOT rewrite entire files. Show only the changed/added blocks with minimal context.**

---

## ⚠️ DESIGNER CRASH — ROOT CAUSE & PREVENTION

**The Visual Studio Designer CANNOT open a form if the project has ANY compile error — even if the error is unrelated to that form.**

### Most common causes in this project:
1. **An event hook in `WireUpEvents()` references a Button/Control that does NOT yet exist in `.Designer.cs`**
2. **A new field is declared in `.cs` but not in `.Designer.cs`** (or vice versa).

### RULE — NEVER VIOLATE:
- **NEVER add an event hook in `WireUpEvents()` for a control that does not yet exist in `.Designer.cs`.**
- **Always add the control to the Designer FIRST, then wire up the event in code.**
- **After any code change: build the project (Strg+Shift+B) before opening the Designer.**

---

## ⚠️ DESIGNER CODE RULES — ABSOLUTE, NEVER VIOLATE

**The Designer can ONLY serialize linear, unconditional code. Any logic causes a Designer crash.**

### FORBIDDEN in InitializeComponent():
1. **NO `for`/`foreach` loops**
2. **NO `if`/`else` conditions**
3. **NO `switch` expressions**
4. **NO `SetChildIndex()`**
5. **NO `Controls.Contains()` checks**
6. **NO `var` declarations for new controls** — declare all fields at class level

---

## ⚠️ LIST FORM LOAD PATTERN — ALWAYS USE THIS

```csharp
private async void FrmXxxList_Load(object? sender, EventArgs e)
{
    if (IsDesignMode() || _xxxService is null) return;

    WindowState = FormWindowState.Maximized;
    await LoadListAsync();
}
```

---

## ⚠️ EF CORE / MIGRATION RULES — NEVER VIOLATE

1. **When multiple FK relations point to the same table**: ALWAYS use `OnDelete(DeleteBehavior.NoAction)` on ALL of them
2. **Before writing a migration, check if that migration name already exists**
3. **Filtered index syntax for SQL Server**: use `[Col] <> x AND [Col] <> y` — NEVER `NOT IN (x, y)`
4. **Every table needs a `IEntityTypeConfiguration<T>` class** in `Data\Configurations\`

---

## ⚠️ GRID COLUMN CHOOSER — REQUIRED FOR ALL GRIDS

1. **`EnableColumnChooser(dgwXxx)`** in `WireUpEvents()`
2. **`ApplyColumnChooserSettings(dgwXxx)`** at the end of `StyleGrid()`
3. **`ApplyColumnHeaders(dgwXxx, _columnHeaders)`** at the start of `StyleGrid()`
4. **`_columnHeaders` Dictionary** with ALL DTO fields

---

## ⚠️ CODE COMMENT LANGUAGE RULE

- All code comments → **English or German only**
- Turkish comments are **NOT allowed** anywhere in the codebase

---

## ⚠️ STATUS COLOR HELPER — `Helpers\StatusColorHelper.cs`

All grid row colors and status label colors MUST be routed through `StatusColorHelper`.

```csharp
// Grid row color (CellFormatting):
row.DefaultCellStyle.BackColor = StatusColorHelper.GetOrderStatusBackColor(dto.AuftragStatus);

// Status label:
lblAuftragStatus.ForeColor = StatusColorHelper.GetOrderStatusLabelColor(_currentStatus);

// Load once in LoadAppSetupAsync():
StatusColorHelper.Configure(setup);
```

---

## ⚠️ LIST FORM — CHECKBOX BATCH-ACTION PATTERN

All list forms with batch actions (`FrmOrderList`, `FrmDeliveryList`, etc.) MUST follow this pattern:

### Designer
- Add `DataGridViewCheckBoxColumn colXxxChecked` as the **first column** of the grid
- `colXxxChecked.ReadOnly = false` — MUST be explicitly set; `ConfigureGrid()` sets grid `ReadOnly=true`
- `btnAlleMarkieren` — initially `Enabled = false` in Designer; code enables it dynamically
- `btnBuchen` / `btnFreigeben` — initially `Enabled = false`

### StyleGrid() — checkbox column handling
```csharp
// After ConfigureGrid() which sets ReadOnly=true on grid level:
dgwAuftraege.ReadOnly = false;  // restore — checkbox needs this
dgwAuftraege.Columns["colXxxChecked"].ReadOnly = false;
foreach (DataGridViewColumn col in dgwAuftraege.Columns)
    if (col.Name != "colXxxChecked") col.ReadOnly = true;
```

### Button enable/disable — use ApplyBtnState()
```csharp
// ALWAYS use this helper — prevents "gray but clickable" rendering bug
private static void ApplyBtnState(Button btn, bool enabled)
{
    btn.Enabled   = enabled;
    btn.ForeColor = SystemColors.ControlText;
}
```

### Alle markieren — toggle logic
```csharp
// Determine active filter status (Offen or Freigegeben)
// Count matching rows; if not all checked → check all; if all checked → uncheck all
// _suppressChecked = true; ApplyCheckmarks(); _suppressChecked = false;
// dgwAuftraege.Refresh(); UpdateButtonStates();
```

### Batch-action buttons — fallback pattern
```csharp
// 1. Collect IDs from _checkedIds that match the required status
// 2. If none checked → fall back to single selected row
// 3. Confirm dialog with count
// 4. Execute in lock, collect errors per ID
// 5. _checkedIds.Remove(id) on success
// 6. await LoadListAsync() at end
```

### Checkbox single-click commit
```csharp
private void DgwXxx_DirtyStateChanged(object? s, EventArgs e)
{
    if (dgwXxx.IsCurrentCellDirty &&
        dgwXxx.CurrentCell?.OwningColumn?.Name == "colXxxChecked")
        dgwXxx.CommitEdit(DataGridViewDataErrorContexts.Commit);
}
```

---

## ⚠️ FRMORDERENTRY — COMPLETED PATTERNS

`FrmOrderEntry` is the daily order entry form. Key patterns:

### Button visibility rules
| Status | Speichern | Hinzufügen | Freigeben | Buchen | Löschen | Öffnen | Nachlieferung |
|---|---|---|---|---|---|---|---|
| null/Offen | ✅ | ✅ | ✅ (if saved) | ✅ | ✅ (if saved) | ❌ | ❌ |
| Freigegeben | ❌ | ❌ | ❌ | ✅ | ❌ | ✅ | ❌ |
| Gebucht | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | ✅ |
| Storniert | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ |

### NavigateToAuftrag — MUST use this pattern
```csharp
public async void NavigateToAuftrag(int kundeId, DateTime lieferdatum)
{
    BringToFront();
    _isLoading = true; dtpLieferdatum.Value = lieferdatum.Date; _isLoading = false;
    // Switch to btnAlle (no day filter) so customer appears regardless of LiefertXx flags
    btnAlle.BackColor = Color.SteelBlue; btnAlle.ForeColor = Color.White;
    _activeDayBtn = btnAlle; _selectedDay = null;
    await ReloadAsync();
    SelectKundeById(kundeId);
    await LoadPositionenAsync(kundeId);  // direct call — avoids race condition
}
```

---

## ⚠️ DOCUMENT STATUS & TERMINOLOGY

### Auftrag lifecycle (CRITICAL — NO EXCEPTIONS)
```
Offen
  → [Freigeben]  → Freigegeben
  → [Löschen]    → Gelöscht        (soft-delete, no document number)

Freigegeben
  → [Buchen]     → Gebucht         → Lieferschein (Offen) created
  → [Öffnen]     → Offen           (NOT Stornieren — Stornieren only for Lieferschein/Rechnung)

Gebucht
  → [Stornieren via Lieferungen]   (never directly on Auftrag level)
```

> **"Stornieren" on Auftrag level does NOT exist.**
> **"Öffnen" = Freigegeben → Offen (reverse of Freigeben).**
> **"Löschen" only when Status = Offen (before any document number is assigned).**

### Lieferschein Status Flow
```
Offen → [Abschliessen] → Abgeschlossen → [Fakturieren] → Fakturiert
     → [Stornieren]    → Storniert     (linked Auftrag → back to Freigegeben)