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
// ✅ Tab-Reihenfolge: durch Reihenfolge der Controls.Add()-Aufrufe festlegen
tabDetail.Controls.Add(tabStamm);
tabDetail.Controls.Add(tabAdresse);
tabDetail.Controls.Add(tabEinstellungen);
tabDetail.Controls.Add(tabLiefertage);   // ← Position = Reihenfolge hier
tabDetail.Controls.Add(tabLeihgeraete);  // ← nicht SetChildIndex!

// ✅ RowStyles: explizit einzeln, nie in einer Schleife
tlpXxx.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
tlpXxx.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
tlpXxx.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
```

### FORBIDDEN examples:
```csharp
// ❌ Schleife
for (int i = 0; i < 5; i++)
    tlpXxx.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));

// ❌ Bedingung
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
        NewXxx(); // → zeigt Detail-Panel und setzt Fokus
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

## ⚠️ GRID COLUMN CHOOSER — PFLICHTREGELN FÜR ALLE GRIDS

1. **`EnableColumnChooser(dgwXxx)`** in `WireUpEvents()`
2. **`ApplyColumnChooserSettings(dgwXxx)`** am Ende von `StyleGrid()`
3. **`ApplyColumnHeaders(dgwXxx, _columnHeaders)`** am Anfang von `StyleGrid()`
4. **`_columnHeaders` Dictionary** mit ALLEN DTO-Feldern
5. Einstellungen in `UserGridSetting` Tabelle (DB), nicht JSON

---

## CUSTOMER FIELD TERMINOLOGY

| Feldname (C#/DB) | Deutsche UI-Bezeichnung | Bedeutung |
|---|---|---|
| `Geraete1`..`Geraete5` | Leihgerät 1..5 | Beim Kunden hinterlegte Leihgeräte/Ausstattung (z.B. Kühlbox, Spender) |
| `LiefertMo`..`LiefertSo` | Mo..So | Liefertage der Woche |

**FrmCustomerList Tabs (in dieser Reihenfolge):**
- `tabStamm` → Stammdaten
- `tabAdresse` → Adresse
- `tabAltAdresse` → Alt. Lieferadresse
- `tabEinstellungen` → Einstellungen (Tour, Gruppe, Limit, Aktiv, Preis ausblenden)
- `tabLiefertage` → Liefertage (Mo–So Checkboxen)
- `tabLeihgeraete` → Leihgeräte (5 Freitextfelder für verliehene Geräte)

---

## ARCHITECTURE

- Single-project solution: `C:\Users\eak\Dev\DNR26V2\DNR26V2.sln`
- .NET 8 WinForms + Entity Framework Core + Migrations
- SQL Server + SQL LocalDB support
- MDI WinForms (MdiParent/MdiChild)

---

## DI REGISTRATION PATTERN (Program.cs)

- Services → `AddScoped<IXxxService, XxxService>()`
- Forms → `AddTransient<FrmXxx>()`
- `IGridSettingsService` → `AddSingleton<IGridSettingsService, GridSettingsService>()`
- After `BuildServiceProvider()`: `GridColumnChooser.SetService(...)`

---

## MDI MENU → FORM CONNECTION PATTERN (FrmMain)

```csharp
private FrmXxxList? FrmXxxListInstance;
private void MenuXxx_Click(object? sender, EventArgs e)
    => BaseListForm.GetOrCreateInstance<FrmXxxList>(ref FrmXxxListInstance, this, () => GetService<FrmXxxList>());
```

---

## MODULE STATUS

### ✅ MODULE 0 – Project Setup & Infrastructure
### ✅ MODULE 1 – System Tables (AppSetup, NoSeries, AuditLog, Location, UserGridSetting)
### ✅ MODULE 2 – Kundenstammdaten
- `FrmCustomerList` — working, 6 Tabs (Stamm/Adresse/AltAdresse/Einstellungen/Liefertage/Leihgeräte)
- Migration: `Module2_Fix_TurAusnahmeTur_v2`

### ✅ MODULE 3 – Produktstammdaten
- Product, ProductAttribute, ProductAttributeValue, ProductAttributeMapping
- `FrmProductList`, `FrmProductAttributeList` — working
- Migrations: `Module3_Product_Felder_Printfarbe`, `Module3_ProductAttributes`, `System_UserGridSettings`

### 🔲 MODULE 4 – Auftragserfassung
### 🔲 MODULE 5 – Lieferungen
### 🔲 MODULE 6 – Rechnungen
### 🔲 MODULE 7 – Zahlungen
### 🔲 MODULE 8 – Etiketten / Labels

---

## PRODUCT FIELDS

| Field | Purpose |
|---|---|
| Feld1–4 | Etikett-Positionen |
| Printfarbe | Hintergrundfarbe auf dem Etikett |

---

## DEVELOPER PREFERENCES

- `MessageBox.Show` für Fehler
- `SemaphoreSlim` für async DbContext-Schutz
- Search timer (350ms debounce)
- `ComboItem(int Id, string Text)` für alle ComboBox-Bindings
- `NullIfEmpty()` für optionale Strings
- **KEINE Schleifen, KEINE Bedingungen, KEINE SetChildIndex in `InitializeComponent()`**