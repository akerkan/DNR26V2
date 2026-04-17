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
        NewXxx(); // → zeigt Detail-Panel und setzt Fokus
}
```

---

## ⚠️ EF CORE / MIGRATION RULES — NEVER VIOLATE

These mistakes were made and must never happen again:

1. **When multiple FK relations point to the same table** (e.g. `TurId` and `AusnahmeTurId` both → `Route`): ALWAYS use `OnDelete(DeleteBehavior.NoAction)` on ALL of them — SQL Server rejects cascade on multiple paths
2. **Always configure FK relationships explicitly in `AppDbContext.cs`** using `modelBuilder.Entity<>()` — never rely on EF Core convention defaults for cascade behavior
3. **Before writing a migration, check if that migration name already exists** — use a new name if needed
4. **Template for dual-FK to same table** (always use this):

```csharp
modelBuilder.Entity<Customer>()
    .HasOne(c => c.Tur)
    .WithMany()
    .HasForeignKey(c => c.TurId)
    .OnDelete(DeleteBehavior.NoAction);

modelBuilder.Entity<Customer>()
    .HasOne(c => c.AusnahmeTur)
    .WithMany()
    .HasForeignKey(c => c.AusnahmeTurId)
    .OnDelete(DeleteBehavior.NoAction);
```

5. **Every table needs a `IEntityTypeConfiguration<T>` class** in `Data\Configurations\` — never skip this

---

## ARCHITECTURE

- Single-project solution: `C:\Users\eak\Dev\DNR26V2\DNR26V2.sln`
- .NET 8 WinForms + Entity Framework Core + Migrations
- SQL Server + SQL LocalDB support
- MDI WinForms (MdiParent/MdiChild)
- BaseForm for shared behavior
- When MdiChild is maximized → MdiParent scrollbars must still be visible

---

## DI REGISTRATION PATTERN (Program.cs)

All forms and services are registered in `Program.cs → ConfigureServices()`.
- Services → `AddScoped<IXxxService, XxxService>()`
- Forms → `AddTransient<FrmXxx>()`
- FrmMain uses `GetService<T>()` helper + `BaseListForm.GetOrCreateInstance<T>()` for MDI child forms

---

## MDI MENU → FORM CONNECTION PATTERN (FrmMain)

```csharp
// FrmMain.cs
private FrmXxxList? FrmXxxListInstance;

private void MenuXxx_Click(object? sender, EventArgs e)
    => BaseListForm.GetOrCreateInstance<FrmXxxList>(ref FrmXxxListInstance, this, () => GetService<FrmXxxList>());

// FrmMain.Designer.cs
var menuXxx = new ToolStripMenuItem("&Xxx", null, MenuXxx_Click);
```

---

## MODULE STATUS

### ✅ MODULE 0 – Project Setup & Infrastructure
- Single project structure, EF Core, Migrations, BaseForm, LocalDB+SQL Server

### ✅ MODULE 1 – System Tables
- AppSetup, NoSeries, AuditLog, Location (single warehouse)

### ✅ MODULE 2 – Kundenstammdaten (Customer Master Data)
- `FrmCustomerList` — working, Designer OK, MDI menu connected (Stammdaten → Kunden)
- Customer fields: Kundennummer, Kundenname, Telefonnummer, Handynummer, EMail, Routenfolge (int), KreditLimit, Aktiv, Offen, PreisAusblenden
- Address section: Name2, Inhaber, Adresse, Adresse2, PLZ, Ort, Land + AbweichendeLieferadresse checkbox
- **Tour** (int, FK → Route, `OnDelete(DeleteBehavior.NoAction)`): StandardTour
- **AusnahmeTur** (int, FK → Route, `OnDelete(DeleteBehavior.NoAction)`): Exception tour. AusnahmeTur ≠ Tur (validated)
- Kundenfilter: ComboBox + lookup, working
- Liefertage: MO/DI/MI/DO/FR/SA/SO checkboxes, working
- Ausstattung: Geräte 1–5 text fields, working
- Migration applied: `Module2_Fix_TurAusnahmeTur_v2`

### 🔲 MODULE 3 – Produktstammdaten (Product Master Data)
#### ✅ Done so far:
- `Product` entity with fields: Artikelnummer, Bezeichnung, Bezeichnung2, Einheit, VKPreis, EKPreis, MwstProzent, **Feld1–Feld4**, **Printfarbe**, Barcode, Notizen, Aktiv
- `ProductListDto` — includes Feld1–4 and Printfarbe for grid display
- `ProductConfiguration` (`Data\Configurations\ProductConfiguration.cs`) — fully configured
- `IProductService` / `ProductService` — Dapper list query + EF Core detail/save/delete
- `FrmProductList` — working, Designer OK, MDI menu connected (Stammdaten → Produkte)
  - Grid shows: Art.-Nr., Bezeichnung, Feld1–4, Printfarbe (cell background colored), Aktiv
  - Detail panel always visible (never hidden on load)
  - Empty grid → auto NewProduct() mode
  - Printfarbe: `pnlPrintfarbe` (Panel, color preview) + `btnPrintfarbe` (Button → ColorDialog)
  - Grid Printfarbe column: cell background matches stored color, auto contrast text color
  - Feld1–4: MultiLine TextBoxes in detail panel
  - Keyboard: F2 = Neu, Ctrl+S = Speichern
- Migration pending: `Module3_Product_Felder_Printfarbe` → **run `Update-Database` before testing**

#### 🔲 Still needed for Module 3:
- DonerSekil lookup table + form
- DonerBoruTipi lookup table + form

### 🔲 MODULE 4 – Auftragserfassung (Sales Order Entry)
- Fast daily order entry
- SalesOrderHeader / SalesOrderLine

### 🔲 MODULE 5 – Lieferungen (Deliveries)
- DeliveryHeader / DeliveryLine
- AusnahmeTur / Normaltour assignment buttons
- Driver list print (sorted by Routenfolge)
- No destructive edit after finalization

### 🔲 MODULE 6 – Rechnungen (Invoices)
- SalesInvoiceHeader / Line, twice-per-month, Storno logic

### 🔲 MODULE 7 – Zahlungen (Payments)
- PaymentEntry / CustomerLedgerEntry, no deletion

### 🔲 MODULE 8 – Etiketten / Labels
- Uses Printfarbe and Feld1–4 from Product for label positioning and color

---

## DATABASE TABLES (summary)

**Master Data:** Customer, Product, CustomerProduct, CustomerPrice, CustomerDeliveryDay, Driver, Route, Location

**Operations:** SalesOrderHeader/Line, DeliveryHeader/Line

**Finance:** SalesInvoiceHeader/Line, SalesCreditMemoHeader/Line, PaymentEntry, CustomerLedgerEntry

**System:** NoSeries, AppSetup, AuditLog

**Audit fields on ALL tables:** ErstelltAm, ErstelltVon, GeaendertAm, GeaendertVon

**Document status lifecycle:** OPEN → FINALIZED → POSTED → STORNIERT

---

## PRODUCT FIELDS — PURPOSE REFERENCE

| Field | Purpose |
|---|---|
| Feld1 | Etikett-Position 1 (z.B. Zutaten-Kurztext) |
| Feld2 | Etikett-Position 2 (z.B. Zutaten-Langtext) |
| Feld3 | Etikett-Position 3 (z.B. Hinweise) |
| Feld4 | Etikett-Position 4 (reserve) |
| Printfarbe | Hintergrundfarbe auf dem Etikett (Name oder Hex z.B. `Red`, `#FF0080`) |

---

## DEVELOPER PREFERENCES

- Errors shown with `MessageBox.Show`
- DataGridView for lists
- EF Core preferred over stored procedures in V2
- German UI / English code+table names / German column names
- `SemaphoreSlim` for parallel DbContext protection in async form methods
- Search timer (350ms debounce) for live search in list forms
- `ComboItem(int Id, string Text)` helper for all ComboBox bindings
- `NullIfEmpty()` extension for trimming optional string fields before save