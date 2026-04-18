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
tabDetail.Controls.Add(tabLiefertage);   // position = order here
tabDetail.Controls.Add(tabLeihgeraete);  // NOT SetChildIndex!

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

### 🔲 MODULE 4 – Auftragserfassung
- Daily order per customer: which products, which quantities, which driver/tour
- `CustomerProduct` template used as default for new orders
- Planned entities: `Order` (table: `Orders`), `OrderLine` (table: `OrderLines`)
- Planned services: `IOrderService`, `OrderService`
- Planned forms: `FrmOrderEntry`
- Key rule: posting an order (`btnBuchen`) immediately creates a Lieferschein via `DeliveryService.CreateFromOrderAsync`

### 🔲 MODULE 5 – Lieferungen
- Planned entities: `DeliveryHeader` (table: `Deliveries`), `DeliveryLine` (table: `DeliveryLines`), `DeliveryLineChange` (table: `DeliveryLineChanges`)
- `DeliveryLineChanges` → audit log for every line modification (no silent edits)
- Planned services: `IDeliveryService`, `DeliveryService`
- Planned forms: `FrmDeliveryList`

### 🔲 MODULE 6 – Rechnungen
- Planned entities: `InvoiceHeader` (table: `Invoices`), `InvoiceLine` (table: `InvoiceLines`), `InvoiceDelivery` (table: `InvoiceDeliveries`)
- Three invoice creation modes:
  1. Single Lieferschein → Rechnung
  2. Customer + date range → Rechnung (all open deliveries)
  3. **Bulk**: all customers, date range → one Rechnung per customer, single transaction
- Planned services: `IInvoiceService`, `InvoiceService`
- Planned forms: `FrmInvoiceList`, `FrmBulkRechnungserstellung`
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
| `Order` | `Orders` | Module4_Orders *(planned)* | 🔲 |
| `OrderLine` | `OrderLines` | Module4_Orders *(planned)* | 🔲 |
| `DeliveryHeader` | `Deliveries` | Module5_Deliveries *(planned)* | 🔲 |
| `DeliveryLine` | `DeliveryLines` | Module5_Deliveries *(planned)* | 🔲 |
| `DeliveryLineChange` | `DeliveryLineChanges` | Module5_Deliveries *(planned)* | 🔲 |
| `InvoiceHeader` | `Invoices` | Module6_Invoices *(planned)* | 🔲 |
| `InvoiceLine` | `InvoiceLines` | Module6_Invoices *(planned)* | 🔲 |
| `InvoiceDelivery` | `InvoiceDeliveries` | Module6_Invoices *(planned)* | 🔲 |

---

## DOCUMENT FLOW

```
Order (Offen)
  → btnBuchen → Status: Bestaetigt
  → DeliveryService.CreateFromOrderAsync()
      → Delivery (Offen)
          → lines editable while Offen
          → every edit logged to DeliveryLineChanges
          → btnAbschliessen → Status: Abgeschlossen
          → btnRechnung (single) OR included in Bulk
              → Invoice (Offen)
                  → Status: Gebucht
                  → linked Delivery: Status → Fakturiert
                  → Storno: Status → Storniert
                             linked Deliveries → back to Abgeschlossen
```

---

## DOCUMENT NUMBER GENERATION

All document numbers (`Auftragsnummer`, `Lieferscheinnummer`, `Rechnungsnummer`) are generated via `NoSeries` table using a stored procedure or service method with `transaction + UPDLOCK` to prevent race conditions.

---

## SOFT-DELETE & STORNO RULES

| Document | Rule |
|---|---|
| `Order` | `Status = Storniert` — no physical delete |
| `DeliveryLine` | Edit logged to `DeliveryLineChanges` — original row preserved |
| `Delivery` | `Status = Storniert` — no physical delete |
| `Invoice` | `Status = Storniert` + `StornoRechnungId` + `StornoDatum` + `StornoGrund` — linked deliveries reset to `Abgeschlossen` |
| `CustomerProduct` | `Aktiv = false` — no physical delete |
