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
10. **Do NOT rewrite entire files. Show only the changed/added blocks with minimal context.

---

## ⚠️ DESIGNER CRASH — ROOT CAUSE & PREVENTION

**The Visual Studio Designer CANNOT open a form if the project has ANY compile error — even if the error is unrelated to that form.**

The error message is:
> *"Die Basisklasse 'DNR26V2.Forms.Base.BaseListForm' konnte nicht geladen werden."*

### Why this happens:
- The Designer needs to instantiate the base class at design time.
- If the project does not compile cleanly, the Designer cannot load any assembly → fails on ALL forms.

### Most common causes in this project:
1. **An event hook in `WireUpEvents()` references a Button/Control that does NOT yet exist in `.Designer.cs`**
   Example: `btnAlleAuswaehlenGespeichert.Click += ...` — if `btnAlleAuswaehlenGespeichert` is not declared in the Designer, the project won't compile → Designer breaks.
2. **A new field is declared in `.cs` but not in `.Designer.cs`** (or vice versa).

### RULE — NEVER VIOLATE:
- **NEVER add an event hook in `WireUpEvents()` for a control that does not yet exist in `.Designer.cs`.**
- **Always add the control to the Designer FIRST, then wire up the event in code.**
- **After any code change: build the project (Strg+Shift+B) before opening the Designer.**
- **If the Designer breaks → immediately revert the last code change.**

---

## ⚠️ DESIGNER CODE RULES — ABSOLUTE, NEVER VIOLATE

**The Designer can ONLY serialize linear, unconditional code. Any logic causes a Designer crash.**

### FORBIDDEN in InitializeComponent() — causes Designer errors:

1. **NO `for`/`foreach` loops** — always write explicit individual lines
2. **NO `if`/`else` conditions** — the Designer cannot serialize conditional logic
3. **NO `switch` expressions** — same reason
4. **NO `SetChildIndex()`** — tab order is determined by the order of `Controls.Add()` calls
5. **NO `Controls.Contains()` checks** — every control is added exactly once, unconditionally
6. **NO `Controls.IndexOf()`** — no dynamic positioning
7. **NO `var` declarations for new controls** — declare all fields at class level, initialize in the new-block at the top of InitializeComponent()

### CORRECT pattern — always:

```csharp
// ✅ Tab order: determined by order of Controls.Add() calls
tabDetail.Controls.Add(tabStamm);
tabDetail.Controls.Add(tabAdresse);
tabDetail.Controls.Add(tabEinstellungen);
tabDetail.Controls.Add(tabLiefertage);
tabDetail.Controls.Add(tabLeihgeraete);

// ✅ RowStyles: explicit individual lines, never in a loop
tlpXxx.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
tlpXxx.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
tlpXxx.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
```

### FORBIDDEN examples:

```csharp
// ❌ Loop
for (int i = 0; i < 5; i++)
    tlpXxx.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));

// ❌ Condition
if (!tabDetail.Controls.Contains(tabLiefertage))
    tabDetail.Controls.Add(tabLiefertage);

// ❌ SetChildIndex
tabDetail.Controls.SetChildIndex(tabLiefertage, idxEinstellungen);

// ❌ switch in InitializeComponent
TextBox tb = i switch { 0 => txtGeraete1, ... };
```

---

## ⚠️ LIST FORM LOAD PATTERN — ALWAYS USE THIS

Every list form (`FrmXxxList`) must follow this exact load pattern:

```csharp
private async void FrmXxxList_Load(object? sender, EventArgs e)
{
    if (IsDesignMode() || _xxxService is null) return;

    WindowState = FormWindowState.Maximized;
    // ❌ NEVER: SetDetailVisible(false);
    await LoadListAsync();

    if (dgwXxx.Rows.Count == 0)
        NewXxx(); // → shows detail panel and sets focus
}
```

---

## ⚠️ EF CORE / MIGRATION RULES — NEVER VIOLATE

1. **When multiple FK relations point to the same table**: ALWAYS use `OnDelete(DeleteBehavior.NoAction)` on ALL of them
2. **Always configure FK relationships explicitly in `AppDbContext.cs`**
3. **Before writing a migration, check if that migration name already exists**
4. **Template for dual-FK to same table:**

```csharp
modelBuilder.Entity<Customer>()
    .HasOne(c => c.Tur).WithMany()
    .HasForeignKey(c => c.TurId)
    .OnDelete(DeleteBehavior.NoAction);

modelBuilder.Entity<Customer>()
    .HasOne(c => c.AusnahmeTur).WithMany()
    .HasForeignKey(c => c.AusnahmeTurId)
    .OnDelete(DeleteBehavior.NoAction);
```

5. **Every table needs a `IEntityTypeConfiguration<T>` class** in `Data\Configurations\`
6. **Filtered index syntax for SQL Server**: use `[Col] <> x AND [Col] <> y` — NEVER `NOT IN (x, y)` (causes SQL Server syntax error)

---

## ⚠️ GRID COLUMN CHOOSER — REQUIRED FOR ALL GRIDS

1. **`EnableColumnChooser(dgwXxx)`** in `WireUpEvents()`
2. **`ApplyColumnChooserSettings(dgwXxx)`** at the end of `StyleGrid()`
3. **`ApplyColumnHeaders(dgwXxx, _columnHeaders)`** at the start of `StyleGrid()`
4. **`_columnHeaders` Dictionary** with ALL DTO fields
5. Settings stored in `UserGridSetting` table (DB), not JSON

---

## ⚠️ CODE COMMENT LANGUAGE RULE

- All code comments → **English or German only**
- Turkish comments are **NOT allowed** anywhere in the codebase

---

## ⚠️ STATUS COLOR HELPER — `Helpers\StatusColorHelper.cs`

All grid row colors and status label colors for `OrderStatus`, `DeliveryStatus`, `InvoiceStatus` etc. MUST be routed through `StatusColorHelper`.

- Colors are configured once at form load via `StatusColorHelper.Configure(AppSetup setup)`
- Defaults are hardcoded as fallback (no DB = still works)
- `AppSetup` stores hex color strings: `ColorOrderOffen`, `ColorOrderFreigegeben`, `ColorOrderGebucht`, `ColorOrderStorniert`
- Optional label ForeColors: `ColorOrderLabelOffen`, `ColorOrderLabelFreigegeben`, `ColorOrderLabelGebucht`, `ColorOrderLabelStorniert`

**Standard color mapping:**

| Status       | Grid BackColor (hex) | Label ForeColor |
|---|---|---|
| kein Auftrag | `SystemColors.Window` | `SystemColors.ControlText` |
| Offen        | `#FFFFC8` (gelb)     | `DarkOrange` |
| Freigegeben  | `#C8E6FF` (hellblau) | `SteelBlue` |
| Gebucht      | `#C8FFC8` (grün)     | `DarkGreen` |
| Storniert    | `#F0F0F0` (grau)     | `Gray` |

**Usage in forms:**
```csharp
// Grid row color (CellFormatting):
row.DefaultCellStyle.BackColor = StatusColorHelper.GetOrderStatusBackColor(dto.AuftragStatus);

// Status label:
lblAuftragStatus.ForeColor = StatusColorHelper.GetOrderStatusLabelColor(_currentStatus);

// Load once in LoadAppSetupAsync():
StatusColorHelper.Configure(setup);
```

---

## ⚠️ FRMORDERENTRY — COMPLETED PATTERNS

`FrmOrderEntry` is the daily order entry form. Key patterns to follow for all future order-related forms:

### Tagesauswahl (Wochentag-Buttons)
- `btnMo`..`btnSo` + `btnAlle` — highlighted with `SteelBlue` on selection
- Active button tracked in `_activeDayBtn`
- `_selectedDay` filters the customer list

### Kunden-Grid (left panel)
- `dgwKunden` — DataSource = `List<OrderKundeListDto>` (in-memory filter, no re-query)
- Checkbox column `colKundeChecked` — only rows with `AuftragId > 0` can be checked
- Unchecking or checking rows without saved Auftrag → silently prevented (MessageBox)
- `_checkedKundeIds: HashSet<int>` — tracks selections across filter changes
- `BtnAlleAuswaehlenGespeichert` — selects all Offen+saved rows at once
- `_suppressKundenChanged` flag prevents cascading SelectionChanged calls
- `_suppressFilter` flag prevents Tour-ComboBox rebuild from triggering filter

### Positions-Grid (right panel)
- `dgwPositionen` — unbound, manual row management
- Column names set programmatically in `FixPositionenColumnNames()` (not in Designer)
- Enter key navigates: Menge → Gewicht → Preis → Notiz → next row Menge
- Editable only when `Status = Offen` (or null)

### Async locking
- `SemaphoreSlim _lock = new(1,1)` — protects ALL `_orderService` + `_db` calls
- Pattern: `await _lock.WaitAsync(); try { ... } finally { _lock.Release(); }`

### FactBox (right side panel)
- `pnlFactBox` — shows Saldo, LetzterAuftrag, OffeneAufträge, Tour
- Hidden if `PreisAusblenden = true`

### Button visibility rules (already implemented)
| Status | Speichern | Hinzufügen | Freigeben | Buchen | Löschen | Stornieren | Nachlieferung |
|---|---|---|---|---|---|---|---|
| null/Offen | ✅ | ✅ | ✅ (if saved) | ✅ | ✅ (if saved) | ❌ | ❌ |
| Freigegeben | ❌ | ❌ | ❌ | ✅ | ❌ | ✅ | ❌ |
| Gebucht | ❌ | ❌ | ❌ | ❌ | ❌ | ✅ | ✅ |
| Storniert | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ |

### TurKontrolle
- `AppSetup.TurKontrolle` — if `true`, customer must have a Tour assigned before Speichern/Freigeben
- Loaded once in `LoadAppSetupAsync()` into `_turKontrolleEnabled`

### AdvanceToNextUnbooked
- After F5 Buchen: automatically moves selection to next customer without an Auftrag (wrap-around)

---

## ⚠️ DOCUMENT STATUS & TERMINOLOGY

### Core Rule
> **"Löschen"** only when **no document number has been assigned yet** (Status = Offen, never posted).
> **"Stornieren"** for all documents that already have a number or have been posted.

### Auftrag Status Flow

```
Offen
  → [Freigeben]   → Freigegeben
  → [Löschen]     → Gelöscht        (soft-delete, Status = Gelöscht, no document number yet)

Freigegeben
  → [Buchen & Liefern (F5)] → Gebucht
                              → DeliveryService.CreateFromOrderAsync() → Lieferschein (Offen)
  → [Stornieren]  → Storniert       (no Lieferschein exists yet)

Gebucht
  → [Stornieren]  → Storniert       (linked Lieferschein → also Storniert)
                                     (Auftrag zurück → Freigegeben if re-opened)
```

### Lieferschein Status Flow

```
Offen
  → lines editable while Offen
  → every line edit logged to DeliveryLineChanges (audit trail)
  → [+ Nachlieferung] → New Auftrag (same Kunde, same Tag, prefilled)
                         → F5 Buchen → new Lieferscheinnummer
                         → both Lieferscheine combinable in one Rechnung
  → [Stornieren]  → Storniert       (linked Auftrag zurück → Freigegeben)
  → [Abschliessen]→ Abgeschlossen

Abgeschlossen
  → [Fakturieren] → Fakturiert      (via FrmDeliveryList or FrmBulkRechnung)
```

### Rechnung — 2 Erstellungswege

- **Weg 1 (Einzeln):** `FrmDeliveryList` → Lieferschein auswählen → "Fakturieren" → Rechnung für diesen Lieferschein
- **Weg 2 (Stapel):** `FrmBulkRechnung` → Kunde(n) + Datumsbereich → "Fakturieren" → eine Rechnung pro Kunde, alle offenen Lieferscheine, single transaction

### Rechnung Status Flow

```
Offen
  → [Buchen] → Gebucht              (linked Lieferscheine → Fakturiert)

Gebucht
  → [Stornieren] → Storniert        (linked Lieferscheine zurück → Abgeschlossen)
                   + StornoRechnungId + StornoDatum + StornoGrund
```

### Document Status Summary

| Beleg        | Stati                                                        |
|--------------|--------------------------------------------------------------|
| Auftrag      | Offen → Freigegeben → Gebucht → Storniert / Gelöscht        |
| Lieferschein | Offen → Abgeschlossen → Fakturiert → Storniert               |
| Rechnung     | Offen → Gebucht → Storniert                                  |

### Button Visibility Rules

| Beleg        | Status       | Löschen | Stornieren | Bearbeiten |
|--------------|--------------|---------|------------|------------|
| Auftrag      | Offen        | ✅      | ❌         | ✅         |
| Auftrag      | Freigegeben  | ❌      | ✅         | ❌         |
| Auftrag      | Gebucht      | ❌      | ✅         | ❌         |
| Lieferschein | Offen        | ❌      | ✅         | ✅ (Zeilen)|
| Lieferschein | Abgeschlossen| ❌      | ❌         | ❌         |
| Rechnung     | Offen        | ❌      | ❌         | ✅         |
| Rechnung     | Gebucht      | ❌      | ✅         | ❌         |

---

## ⚠️ ARCHIVIERUNG RULES

- **Aufträge** are internal planning documents — not legally required for tax audit (GoBD)
- **Lieferscheine** must be retained as long as the linked Rechnung exists
- **Rechnungen** must be retained for **10 years** (German tax law — §147 AO)
- **Archivierung feature**: planned, not yet implemented
  - `AppSetup.AuftraegeArchivieren` column exists in DB — leave it, do not implement yet
  - Future tables: `OrderArchives`, `OrderLineArchives`

---

## ⚠️ NACHLIEFERUNG RULE

Same customer, same delivery day, additional delivery needed:
1. User clicks "+ Nachlieferung" on open Lieferschein in `FrmDeliveryList`
2. New `Auftrag` is created (Kunde + LieferDatum prefilled, Status = Freigegeben)
3. User adds lines → F5 Buchen → new `Lieferscheinnummer` assigned
4. Both Lieferscheine for that day/customer can be:
   - Printed separately
   - Combined into one Rechnung (Bulk or manual selection)

---

## ARCHITECTURE

- Single-project solution: `C:\Users\eak\Dev\DNR26V2\DNR26V2.sln`
- .NET 8 WinForms + Entity Framework Core + Migrations
- SQL Server + SQL LocalDB support
- MDI WinForms (MdiParent / MdiChild)
- **Dapper** for read/list queries (DTOs)
- **EF Core** for write operations (insert, update, delete)

---

## DI REGISTRATION PATTERN (Program.cs)

```csharp
services.AddScoped<IXxxService, XxxService>();
services.AddTransient<FrmXxx>();
services.AddSingleton<IGridSettingsService, GridSettingsService>();
// After BuildServiceProvider():
GridColumnChooser.SetService(...);
```

---

## MDI MENU → FORM CONNECTION PATTERN (FrmMain)

```csharp
private FrmXxxList? FrmXxxListInstance;
private void MenuXxx_Click(object? sender, EventArgs e)
    => BaseListForm.GetOrCreateInstance<FrmXxxList>(ref FrmXxxListInstance, this, () => GetService<FrmXxxList>());
```

---

## DEVELOPER PREFERENCES

- `MessageBox.Show` for errors
- `SemaphoreSlim` for async DbContext protection
- Search timer (350 ms debounce)
- `ComboItem(int Id, string Text)` for all ComboBox bindings
- `NullIfEmpty()` for optional strings
- **NO loops, NO conditions, NO SetChildIndex in `InitializeComponent()`**
- **Soft-Delete** for CustomerProduct (`Aktiv = false`), no physical deletion
- **No physical deletion** on any document record — use Status + Storno reference
- **`_isLoading` flag pattern** in form Load handlers to suppress `ValueChanged` events during initialization
- **`_suppressKundenChanged` flag pattern** in grid forms with left/right panels to prevent cascading SelectionChanged
- **`_suppressFilter` flag pattern** when rebuilding ComboBox items that have SelectedIndexChanged wired

---

## CUSTOMER FIELD TERMINOLOGY

| Field (C# / DB) | German UI Label | Meaning |
|---|---|---|
| `Geraete1`..`Geraete5` | Leihgerät 1..5 | Equipment on loan at customer site (e.g. cooler box, dispenser) |
| `LiefertMo`..`LiefertSo` | Mo..So | Weekly delivery days |

### FrmCustomerList Tabs (in this order):

- `tabStamm` → Stammdaten
- `tabAdresse` → Adresse
- `tabAltAdresse` → Alt. Lieferadresse
- `tabEinstellungen` → Einstellungen (Tour, Gruppe, Limit, Aktiv, Preis ausblenden)
- `tabLiefertage` → Liefertage (Mo–So checkboxes)
- `tabLeihgeraete` → Leihgeräte (5 free-text fields)

---

## PRODUCT FIELDS

| Field | Purpose |
|---|---|
| `Feld1`–`Feld4` | Label print positions |
| `Printfarbe` | Background color on the label |

---

## MODULE STATUS

### ✅ MODULE 0 – Project Setup & Infrastructure
- .NET 8 WinForms, EF Core, Dapper, SQL Server
- `AppDbContext`, `DapperContext`, `AuditableEntity`
- `GridColumnChooser`, `UserGridSetting`, `BaseListForm`
- `Program.cs` DI container setup

### ✅ MODULE 1 – System Tables
- Entities: `AppSetup`, `NoSeries`, `AuditLog`, `Location`, `UserGridSetting`
- Forms: `FrmAppSetup`, `FrmLocationSetup`

### ✅ MODULE 2 – Kundenstammdaten
- Entities: `Customer`, `Route`, `Driver`
- Services: `ICustomerService`, `IRouteService`, `IDriverService`
- Forms: `FrmCustomerList` — 6 tabs (Stamm / Adresse / Alt.Adresse / Einstellungen / Liefertage / Leihgeräte)
- Migrations: `Module2_Fix_TurAusnahmeTur_v2`

### ✅ MODULE 3 – Produktstammdaten + Kundenprodukt-Schablone
- Entities: `Product`, `ProductAttribute`, `ProductAttributeValue`, `ProductAttributeMapping`
- Entities: `CustomerProduct`, `CustomerProductAttributeMapping`
- Services: `IProductService`, `IProductAttributeService`, `ICustomerProductService`
- Forms:
  - `FrmProductList` — create/edit products, attribute assignments
  - `FrmProductAttributeList` — attribute definitions, values, Vorlage (free-text), delete
  - `FrmCustomerProductTemplate` — customer product template with dynamic attribute columns
    - Lookup attributes → DropDownList
    - Vorlage attributes (`IstVorlage = true`) → DropDown with free-text entry
    - Soft-delete on removal (`Aktiv = false`)
- Migrations: `Module3_Product_Felder_Printfarbe`, `Module3_ProductAttributes`,
  `System_UserGridSettings`, `AddIstVorlageToProductAttribute`

### ✅ MODULE 4 – Auftragserfassung
- Entities: `Order`, `OrderLine`
- Services: `IOrderService`, `OrderService`
- DTOs: `OrderKundeListDto`, `OrderLineDto`, `ArtikelSuchDto`, `KundenFactBoxDto`
- Helpers: `StatusColorHelper` (`Helpers\StatusColorHelper.cs`) — centralized status colors
- AppSetup extensions: `TurKontrolle`, `ColorOrderOffen/Freigegeben/Gebucht/Storniert`, label colors
- Forms:
  - `FrmOrderEntry` — daily order entry (SplitContainer left/right, day buttons, tour filter)
    - Left: customer grid with checkbox column, `btnAlleFreigeben`, `btnAlleAuswaehlenGespeichert`
    - Right: positions grid, FactBox panel, action buttons
    - Keyboard: F5=Buchen, Ctrl+S=Speichern, Ins=Hinzufügen, Del=Zeile löschen, Space=Checkbox toggle
    - Enter navigation: Menge→Gewicht→Preis→Notiz→next row
    - Auto-advance after Buchen to next customer without Auftrag
- Migrations: `Module4_Orders`, `Module4_AppSetup_StatusColors`
- Key implementation rules:
  - Checkbox in Kunden-Grid: only allowed when `AuftragId > 0`
  - `_checkedKundeIds` HashSet survives filter changes
  - `_lock` SemaphoreSlim protects all async service calls
  - `StatusColorHelper.Configure(setup)` called once in `LoadAppSetupAsync()`

### 🔲 MODULE 5 – Lieferungen
- Entities planned: `DeliveryHeader` (table: `Deliveries`), `DeliveryLine` (table: `DeliveryLines`), `DeliveryLineChange` (table: `DeliveryLineChanges`)
- `DeliveryLineChanges` → audit log for every line modification (no silent edits)
- Services planned: `IDeliveryService`, `DeliveryService`
- Forms planned: `FrmDeliveryList`
- Key features:
  - Stapeldruck (bulk print) of Lieferscheine
  - Einzeldruck (single print)
  - Nachlieferung button → creates new Auftrag (same Kunde/Tag, Status = Freigegeben)
  - Stornieren → Delivery.Status = Storniert, linked Auftrag zurück → Freigegeben
  - Abschliessen → Delivery.Status = Abgeschlossen

### 🔲 MODULE 6 – Rechnungen
- Entities planned: `InvoiceHeader` (table: `Invoices`), `InvoiceLine` (table: `InvoiceLines`), `InvoiceDelivery` (table: `InvoiceDeliveries`)
- Three invoice creation modes:
  1. `FrmDeliveryList` → single Lieferschein → "Fakturieren" → Rechnung
  2. `FrmBulkRechnung` → Kunde + Datumsbereich → Rechnung (alle offenen Lieferscheine)
  3. **Bulk**: all customers, date range → one Rechnung per customer, single transaction
- Services planned: `IInvoiceService`, `InvoiceService`
- Forms planned: `FrmInvoiceList`, `FrmBulkRechnungserstellung`
- Storno: `Status = Storniert`, linked deliveries reset to `Abgeschlossen`, no physical deletion

### 🔲 MODULE 7 – Zahlungen
### 🔲 MODULE 8 – Etiketten / Labels

---

## ENTITY & TABLE OVERVIEW

| Entity (C#) | Table (DB) | Key Migration | Status |
|---|---|---|---|
| `AppSetup` | `AppSetups` | Module1 | ✅ |
| `NoSeries` | `NoSeries` | Module1 | ✅ |
| `AuditLog` | `AuditLogs` | Module1 | ✅ |
| `Location` | `Locations` | Module1 | ✅ |
| `UserGridSetting` | `UserGridSettings` | System_UserGridSettings | ✅ |
| `Customer` | `Customers` | Module2_Fix_TurAusnahmeTur_v2 | ✅ |
| `Route` | `Routes` | Module2 | ✅ |
| `Driver` | `Drivers` | Module2 | ✅ |
| `Product` | `Products` | Module3_Product_Felder_Printfarbe | ✅ |
| `ProductAttribute` | `ProductAttributes` | Module3_ProductAttributes | ✅ |
| `ProductAttributeValue` | `ProductAttributeValues` | Module3_ProductAttributes | ✅ |
| `ProductAttributeMapping` | `ProductAttributeMappings` | Module3_ProductAttributes | ✅ |
| `CustomerProduct` | `CustomerProducts` | Module3_ProductAttributes | ✅ |
| `CustomerProductAttributeMapping` | `CustomerProductAttributeMappings` | Module3_ProductAttributes | ✅ |
| `Order` | `Orders` | Module4_Orders | ✅ |
| `OrderLine` | `OrderLines` | Module4_Orders | ✅ |
| `OrderArchive` | `OrderArchives` | Module4_Archive *(future)* | 🔲 |
| `OrderLineArchive` | `OrderLineArchives` | Module4_Archive *(future)* | 🔲 |
| `DeliveryHeader` | `Deliveries` | Module5_Deliveries *(planned)* | 🔲 |
| `DeliveryLine` | `DeliveryLines` | Module5_Deliveries *(planned)* | 🔲 |
| `DeliveryLineChange` | `DeliveryLineChanges` | Module5_Deliveries *(planned)* | 🔲 |
| `InvoiceHeader` | `Invoices` | Module6_Invoices *(planned)* | 🔲 |
| `InvoiceLine` | `InvoiceLines` | Module6_Invoices *(planned)* | 🔲 |
| `InvoiceDelivery` | `InvoiceDeliveries` | Module6_Invoices *(planned)* | 🔲 |

---

## DOCUMENT FLOW

```
Auftrag (Offen)
  → [Löschen]      → Gelöscht        (soft-delete, no number assigned)
  → [Freigeben]    → Freigegeben

Auftrag (Freigegeben)
  → [Stornieren]   → Storniert
  → [Buchen F5]    → Gebucht
                   → DeliveryService.CreateFromOrderAsync()
                       → Lieferschein (Offen)
                           → lines editable while Offen
                           → every edit → DeliveryLineChanges (audit)
                           → [+ Nachlieferung] → New Auftrag (Freigegeben, same Kunde/Tag)
                           → [Stornieren]      → Storniert
                                                  linked Auftrag → Freigegeben
                           → [Abschliessen]    → Abgeschlossen
                               → [Fakturieren] → Invoice (Offen)
                                   → [Buchen]  → Gebucht
                                               → linked Delivery → Fakturiert
                                   → [Stornieren] → Storniert
                                                    linked Deliveries → Abgeschlossen

Auftrag (Gebucht)
  → [Stornieren]   → Storniert
                   → linked Lieferschein → Storniert
```

---

## DOCUMENT NUMBER GENERATION

All document numbers (`Auftragsnummer`, `Lieferscheinnummer`, `Rechnungsnummer`) are generated via `NoSeries` table using a stored procedure or service method with `transaction + UPDLOCK` to prevent race conditions.

---

## SOFT-DELETE & STORNO RULES

| Document | Rule |
|---|---|
| `Order` (Offen) | `Status = Gelöscht` — soft-delete, no document number |
| `Order` (Freigegeben/Gebucht) | `Status = Storniert` — no physical delete |
| `DeliveryLine` | Edit logged to `DeliveryLineChanges` — original row preserved |
| `Delivery` | `Status = Storniert` — no physical delete |
| `Invoice` | `Status = Storniert` + `StornoRechnungId` + `StornoDatum` + `StornoGrund` — linked deliveries reset to `Abgeschlossen` |
| `CustomerProduct` | `Aktiv = false` — no physical delete |