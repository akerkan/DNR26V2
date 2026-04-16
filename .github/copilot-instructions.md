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

---

## ARCHITECTURE

- Single-project solution: `C:\Users\eak\Dev\DNR26V2\DNR26V2.sln`
- .NET 8 WinForms + Entity Framework Core + Migrations
- SQL Server + SQL LocalDB support
- MDI WinForms (MdiParent/MdiChild)
- BaseForm for shared behavior
- When MdiChild is maximized → MdiParent scrollbars must still be visible

---

## MODULE STATUS

### ✅ MODULE 0 – Project Setup & Infrastructure
- Single project structure, EF Core, Migrations, BaseForm, LocalDB+SQL Server

### ✅ MODULE 1 – System Tables
- AppSetup, NoSeries, AuditLog, Location (single warehouse)

### ✅ MODULE 2 – Kundenstammdaten (Customer Master Data)
- `FrmCustomerList` (list) + `FrmCustomerEdit` (edit) — both working, Designer OK
- Customer fields: Kundennummer, Kundenname, Telefonnummer, Handynummer, EMail, Routenfolge (int), KreditLimit, Aktiv, Offen, PreisAusblenden
- Address section: Name2, Inhaber, Adresse, Adresse2, PLZ, Ort, Land + AbweichendeLieferadresse checkbox
- **Tour** (int, FK → Route, `OnDelete(DeleteBehavior.NoAction)`): StandardTour — assigned to daily orders automatically
- **AusnahmeTur** (int, FK → Route, `OnDelete(DeleteBehavior.NoAction)`): Exception tour — used when e.g. driver is sick. Assigned in Lieferungen via special function buttons. AusnahmeTur ≠ Tur (validated, cannot be same value)
- **Rheinfolge: REMOVED** — not needed
- **Wochentour/Wochenendtour: REMOVED** — replaced by AusnahmeTur
- **Kundenfilter**: ComboBox + button (lookup), working
- **Liefertage**: MO/DI/MI/DO/FR/SA/SO checkboxes, working
- **Ausstattung**: Geräte 1–5 text fields, working
- Tour assignment logic for Lieferungen:
  - Orders get StandardTour by default
  - In Lieferungen form: button "AusnahmeTur zuweisen" and "Normaltour zurücksetzen"
- Routenfolge: integer, manually entered, used for driver list print order (proximity-based)
- Migration: `Module2_Fix_TurAusnahmeTur_v2` applied successfully

### 🔲 MODULE 3 – Produktstammdaten (Product Master Data)
- Product table: No, Bezeichnung, Alan1–4, Printfarbe
- DonerSekil and DonerBoruTipi as separate lookup tables
- FrmProductList + FrmProductEdit

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

### 🔲 MODULE 8 – Etiketten / Labels (future)

---

## DATABASE TABLES (summary)

**Master Data:** Customer, Product, CustomerProduct, CustomerPrice, CustomerDeliveryDay, Driver, Route, Location

**Operations:** SalesOrderHeader/Line, DeliveryHeader/Line

**Finance:** SalesInvoiceHeader/Line, SalesCreditMemoHeader/Line, PaymentEntry, CustomerLedgerEntry

**System:** NoSeries, AppSetup, AuditLog

**Audit fields on ALL tables:** ErstelltAm, ErstelltVon, GeaendertAm, GeaendertVon

**Document status lifecycle:** OPEN → FINALIZED → POSTED → STORNIERT

---

## DEVELOPER PREFERENCES

- Errors shown with `MessageBox.Show`
- DataGridView for lists
- EF Core preferred over stored procedures in V2
- German UI / English code+table names / German column names
