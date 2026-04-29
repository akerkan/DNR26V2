# DNR26V2 — CONTRIBUTING & ARCHITECTURE INDEX

> **Wie nutzen:** Sag dem Copilot genau welchen Abschnitt er lesen soll.
> Beispiel: *„Lies RULE_FORMS und MODULE_07"* statt *„lies alles"*.

---

## QUICK REFERENCE TABLE

| Was du brauchst | Lies |
|---|---|
| Formular bauen / Designer-Regeln | [RULE_FORMS](#rule_forms) |
| Berichte / RDLC / Preview / Print | [RULE_REPORTING](#rule_reporting) + passendes Modul |
| Datenbankregeln / Migrations | [RULE_DB](#rule_db) |
| Preis-/Rabatt-Berechnung | [RULE_PRICING](#rule_pricing) |
| Allgemeine Code-Regeln | [RULE_CODE](#rule_code) |
| Stammdaten (Kunde, Artikel, Attribute) | [MODULE_02](#module_02) + [MODULE_03](#module_03) |
| Aufträge | [MODULE_04_ORDERS](#module_04_orders) |
| Lieferungen + Teil-Storno | [MODULE_05_DELIVERIES](#module_05_deliveries) |
| Rechnungen + Gutschriften | [MODULE_06_INVOICES](#module_06_invoices) |
| Zahlungen + Ledger + Storno | [MODULE_07_PAYMENTS](#module_07_payments) |
| Vollständige Dokumentenkette | `ORDER_FLOW_TRACKING.md §1–§7` |
| Aktueller Implementierungsstand | `ORDER_FLOW_TRACKING.md §9` |

---

# RULE_FORMS

**Controls ONLY in `.Designer.cs` — NEVER at runtime.**

1. Controls → only in `.Designer.cs`
2. NEVER create controls in `.cs` code-behind
3. Forms MUST open in WinForms Designer without errors
4. Designer fails on ANY compile error → fix errors first, then open Designer
5. `panelDetail` MUST be visible on Load
6. If grid empty on load → auto `NewCustomer()` / `New()` mode
7. NEVER modify layout unless explicitly requested
8. After adding controls: ALWAYS build before opening Designer
9. Field declared in `.cs` MUST also be declared in `.Designer.cs` (partial class)
10. If Designer breaks → `git checkout HEAD -- *.Designer.cs` immediately

**DI in Forms:**
- Forms receive services via constructor
- Designer-only ctor takes `null!` for all services
- `IsDesignMode()` guard on all Load/async methods

---

# RULE_REPORTING

1. RDLC files are infrastructure assets — never redesign visually unless explicitly requested
2. Placeholder RDLC is allowed, but it MUST open in RDLC Designer
3. Report binding uses `DataTable`, never `ObjectDataSource`
4. Standard dataset names: `dsHeader` and `dsLines`
5. `ReportDataTableFactory` column names MUST match DTO property names exactly
6. `FrmReportViewer` is the shared preview host for order / delivery / invoice reports
7. `FrmReportViewer` uses a single `ReportViewer`; bulk preview is grouped in RDLC, not via tabs
8. Bulk preview pages are separated in RDLC via grouping / page break on document number
9. Report controls must remain in `.Designer.cs` like other WinForms controls
10. Printer selection rule: if `BriefpapierVerwenden = true` prefer `DruckerMitLogo`, else `DruckerWeissesPapier`, else default printer

---

# RDLC Reporting Rules (2024)

- All RDLC reports must use SQL View → DataTable → ReportDataSource (with correct dataset name).
- No business logic or DTO mapping in C# for reporting.
- All report logic must be centralized in RdlcReportRenderService.
- Forms must not contain report loading logic.
- Dataset names in RDLC must match the DataTable binding name in code.
- SQL Views must provide all required fields for reporting.
- XSD datasets are only for design-time support and must match the SQL View.
- Do not edit RDLC or XSD files in this repository unless you are updating design-time fields.
- Remove obsolete DTO/report data services only if they are definitely unused after migration to DataTable-based reporting.

---

# RULE_DB

1. TABLE names → **English** (`PaymentHeaders`, `Invoices`)
2. COLUMN names → **German** (`KundeId`, `Buchungsdatum`, `Zahlungsnummer`)
3. C# class/property names → **English**
4. UI labels/buttons → **German**
5. Multiple FK on same table → `OnDelete(DeleteBehavior.NoAction)` ALWAYS
6. Every entity → MUST have an EF `IEntityTypeConfiguration<T>` class
7. `NOT IN` → FORBIDDEN in SQL; use `<> x AND <> y`
8. Migrations: run `dotnet ef migrations add <Name>` + `dotnet ef database update`
9. Migration Designer.cs MUST exist — manually created migrations without Designer → EF won't find them
10. All `decimal` money fields → store precision explicitly (`decimal(18,2)`)

---

# RULE_CODE

1. Comments → English or German only — **Turkish comments FORBIDDEN**
2. NEVER use `float` or `double` → always `decimal`
3. NEVER rewrite entire files — show only changed blocks
4. Build after EVERY change (`Ctrl+Shift+B`)
5. Async all the way — no `.Result` or `.Wait()`
6. `ValidationException` (domain) → user-visible errors from services
7. `Environment.UserName` → `ErstelltVon` / audit fields
8. Ledger/payment amounts: always query with `Header.KundeId` filter — never trust ReferenceId alone

---

# RULE_PRICING

```
Calculation order (MANDATORY):
1. GrossAmount   = Menge × Preis
2. DiscountAmt   = GrossAmount × DiscountProzent / 100
3. LineAmount     = GrossAmount - DiscountAmt
4. VatAmount      = LineAmount × MwstProzent / 100
5. AmountInclVat  = LineAmount + VatAmount
```

- All stored amounts → **always positive** (sign from `BelegArt`)
- Gutschrift display → UI/print layer adds minus sign
- `InvoiceCalculator` helper in `Domain/Enums/InvoiceCalculator.cs`

---

# MODULE_02

**Stammdaten — Kunden, Routen, Fahrer, Attribute**

| Entity | Table | Key fields |
|---|---|---|
| `Customer` | `Customer` | `Kundennummer`, `ReceivableSource`, `TurWertId`, `KundenGruppeWertId` |
| `Route` | `Routes` | `Bezeichnung` |
| `ProductAttribute` | `ProductAttributes` | `EntityType` (Tour/KundenGruppe) |
| `ProductAttributeValue` | `ProductAttributeValues` | `Bezeichnung`, `IstVorlage` |

**Forms:** `FrmCustomerList` · `FrmProductAttributeList` · `FrmCustomerProductTemplate`

**`Customer.ReceivableSource`:**
- `Invoice=1` (default) → ledger from `InvoiceHeader`
- `Delivery=0` → ledger from `DeliveryHeader`
- Editable in `FrmCustomerList → Tab: Einstellungen → Forderungsart`

---

# MODULE_03

**Produkte / Artikel**

| Entity | Table | Key fields |
|---|---|---|
| `Product` | `Products` | `Artikelnummer`, `Bezeichnung`, `Preis`, `MwstProzent`, `PreisFormel` |
| `CustomerProduct` | `CustomerProducts` | `KundeId`, `ArtikelId`, `Preis` |

**Forms:** `FrmProductList` · `FrmCustomerProductTemplate`

---

# MODULE_04_ORDERS

**Aufträge (Orders)**

| Entity | Table | Key fields |
|---|---|---|
| `Order` | `Orders` | `Status` (Offen→Gebucht), `Auftragsnummer` (AUF) |
| `OrderLine` | `OrderLines` | `Menge`, `MengeGeliefert`, `MengeFakturiert`, `Preis` |

**Service:** `OrderService` / `IOrderService`

**Key rules:**
- `BuchenAsync` → sets Status=Gebucht, calls `DeliveryService.CreateFromOrderAsync`
- `SaveAuftragAsync` → preserves `MengeGeliefert` + `MengeFakturiert` when replacing lines
- Offen = `Menge - MengeGeliefert` (never stored)

**Forms:** `FrmOrderEntry` (Tagesbestellung) · `FrmOrderList`

**Reporting:**
- RDLC: `Reports/Auftragsbestaetigung.rdlc`
- DTO/service: `OrderPrintHeaderDto`, `OrderPrintLineDto`, `OrderReportData`, `IOrderReportDataService`
- Preview/print entrypoint: `IReportRenderService.PreviewOrderAsync` / `PrintOrderAsync` / `RenderOrderPdfAsync`
- `FrmOrderList` opens preview via button `Auftragsbestätigung drucken`

---

# MODULE_05_DELIVERIES

**Lieferungen (Deliveries)**

| Entity | Table | Key fields |
|---|---|---|
| `DeliveryHeader` | `Deliveries` | `Status`, `Lieferscheinnummer` (LS), `Gesamtbrutto` |
| `DeliveryLine` | `DeliveryLines` | `Menge` (negative=storno), `MengeFakturiert`, `AuftragZeileId` |

**Service:** `DeliveryService` / `IDeliveryService`

**Key rules:**
- `CreateFromOrderAsync` → skips lines where `Menge - MengeGeliefert = 0`
- `StornierenZeileAsync` → appends negative line; blocked if `MengeFakturiert > 0`
- `DeliveryHeader.Status` NEVER changed by invoice or payment operations
- Filter for open deliveries: `MengeFakturiert < Menge` (line-level), NOT header status

**Forms:** `FrmDeliveryList`

**Reporting:**
- RDLC: `Reports/Lieferschein.rdlc`
- DTO/service: `DeliveryPrintHeaderDto`, `DeliveryPrintLineDto`, `DeliveryReportData`, `IDeliveryReportDataService`
- Preview/print entrypoint: `IReportRenderService.PreviewDeliveryAsync` / `PreviewDeliveriesAsync` / `PrintDeliveryAsync`
- `FrmDeliveryList` uses existing checkbox selection for bulk preview via `Ausgewählte Lieferscheine drucken`

---

# MODULE_06_INVOICES

**Rechnungen + Gutschriften**

| Entity | Table | Key fields |
|---|---|---|
| `InvoiceHeader` | `Invoices` | `BelegArt` (Rechnung/Gutschrift), `Status`, `Rechnungsnummer` (RE/GS), `Gesamtbrutto` |
| `InvoiceLine` | `InvoiceLines` | `Menge` (always positive), `DeliveryLineId`, `AmountInclVat` |

**Service:** `InvoiceService` / `IInvoiceService`

**Key rules:**
- `BuchenAsync` → creates InvoiceLines; updates `MengeFakturiert` via explicit EF load (NOT navigation)
- `CreateGutschriftAsync` → only valid path for correction; positive amounts; rollback `MengeFakturiert`
- Original invoice stays in DB with `Status=Gutgeschrieben`
- NoSeries: `RE` for Rechnung · `GS` for Gutschrift
- `BelegArt=Gutschrift` → `OffenerBetrag=0` in ledger (not a receivable)

**Forms:** `FrmRechnungErfassung` · `FrmSammelRechnung` · `FrmRechnungList`

**Reporting:**
- RDLC: `Reports/Rechnung.rdlc`
- Shared preview host: `FrmReportViewer`
- `FrmRechnungList` supports single preview and bulk preview from selected invoices
- `FrmRechnungErfassung` asks after booking whether preview should be opened
- `FrmSammelRechnung` collects generated invoice ids and can open bulk preview after booking