# ORDER_FLOW_TRACKING.md
# DNR26V2 — Order ? Delivery ? Invoice ? Credit Memo ? Payment Flow

Technical and business reference for the complete ERP document chain.

> **Quick navigation:** Read only what you need.
> - Entities: §1 | Core Logic: §2 | Formulas: §3 | Rules: §4 | BelegArt: §5
> - Workflows: §6 | Edge Cases: §7 | Payment Module: §8 | Status: §9

---

## 1. ENTITIES

### Order (`Orders` table)
Represents a customer's purchase order for a specific delivery date.

| Field | Type | Purpose |
|---|---|---|
| `Status` | `OrderStatus` | `Offen` ? `Freigegeben` ? `Gebucht` ? `Storniert` / `Geloescht` |
| `LieferDatum` | `DateTime` | Requested delivery date |
| `Auftragsnummer` | `string` | System-generated — prefix `AUF` |

---

### OrderLine (`OrderLines` table)
One product position within an Order.

| Field | Type | Purpose |
|---|---|---|
| `Menge` | `decimal(10,3)` | Ordered quantity |
| `MengeGeliefert` | `decimal(10,3)` | Cumulative delivered quantity |
| `MengeFakturiert` | `decimal(10,3)` | Cumulative invoiced quantity (reduced by credit memo) |
| `Preis` | `decimal` | Unit price |

**Computed (not stored):** `Offen = Menge - MengeGeliefert`

---

### DeliveryHeader (`Deliveries` table)
Represents a physical shipment (Lieferschein) for an Order.

| Field | Type | Purpose |
|---|---|---|
| `Status` | `DeliveryStatus` | `Offen` ? `TeilStorniert` ? `Fakturiert` ? `Storniert` |
| `AuftragId` | `int?` | FK ? Order (nullable for manual deliveries) |
| `Lieferscheinnummer` | `string` | System-generated — prefix `LS` |
| `Gesamtbrutto` | `decimal` | Header total — used as ledger Betrag |

---

### DeliveryLine (`DeliveryLines` table)
One product position within a DeliveryHeader.

| Field | Type | Purpose |
|---|---|---|
| `Menge` | `decimal(10,3)` | Delivered quantity. **Negative for storno lines.** |
| `MengeFakturiert` | `decimal(18,2)` | Cumulative invoiced quantity |
| `AuftragZeileId` | `int?` | FK ? OrderLine — traceability link |
| `AmountInclVat` | `decimal` | Line amount incl. VAT |

**Important:** `MengeGeliefert` on DeliveryLine is always `0`. Use `Menge` for invoicing logic.

---

### InvoiceHeader (`Invoices` table)
Represents a posted invoice or credit memo. Both document types share this table.

| Field | Type | Purpose |
|---|---|---|
| `Status` | `InvoiceStatus` | `Gebucht=1` / `Storniert=2` / `Gutgeschrieben=3` |
| `BelegArt` | `InvoiceDocumentType` | `Rechnung=0` / `Gutschrift=1` |
| `OriginalRechnungId` | `int?` | FK ? original invoice (Gutschrift rows only) |
| `Rechnungsnummer` | `string` | System-generated — prefix `RE` or `GS` |
| `Gesamtbrutto` | `decimal` | Always positive — used as ledger Betrag |

---

### InvoiceLine (`InvoiceLines` table)
One product position within an InvoiceHeader. Shared by both Rechnung and Gutschrift.

| Field | Type | Purpose |
|---|---|---|
| `Menge` | `decimal(10,3)` | Always positive — source of truth for rollback |
| `DeliveryLineId` | `int` | FK ? DeliveryLine — the physical line being invoiced |
| `LineAmount` | `decimal` | Always positive — sign from `BelegArt` |

**Always use `InvoiceLine.Menge` — not `FakturierteMenge` — in booking/rollback logic.**

---

### Customer (`Customer` table) — relevant payment fields
| Field | Type | Purpose |
|---|---|---|
| `ReceivableSource` | `ReceivableSource` | `Invoice=1` (default) or `Delivery=0` — determines ledger source |

---

### PaymentHeader (`PaymentHeaders` table)
Represents a posted payment. Single header per payment action.

| Field | Type | Purpose |
|---|---|---|
| `Id` | `int` | PK |
| `Zahlungsnummer` | `string` | System-generated — NoSeries `ZA` (e.g. ZA-26-0424-001) |
| `KundeId` | `int` | FK ? Customer |
| `Buchungsdatum` | `DateTime` | Booking date — entered per row in `FrmZahlungseingaenge` |
| `Notiz` | `string?` | Optional header note |
| `ErstelltVon` | `string` | `Environment.UserName` at time of posting |

### PaymentLine (`PaymentLines` table)
Represents a single payment row, linked to either an invoice or a delivery.

| Field | Type | Purpose |
|---|---|---|
| `PaymentHeaderId` | `int` | FK ? PaymentHeader |
| `ReferenceType` | `int` | `0` = InvoiceHeader.Id · `1` = DeliveryHeader.Id · `-1` = Storno back-ref |
| `ReferenceId` | `int` | FK value into the referenced header (or original PaymentLine.Id for storno) |
| `PaymentMethod` | `PaymentMethod` | `Bar=0` / `Bank=1` |
| `Amount` | `decimal` | **Positive** for normal payments · **Negative** for storno reversals |
| `Notiz` | `string?` | Optional line note |

**Sign rule:** `Amount < 0` indicates a storno line. `SUM(Amount)` per `ReferenceId` gives net paid amount.

---

## 2. CORE LOGIC

### 2.1 Auftrag buchen ? Lieferschein erstellen (`OrderService.BuchenAsync` + `DeliveryService.CreateFromOrderAsync`)

1. Order status set to `Gebucht`
2. For each `OrderLine` where `Menge - MengeGeliefert > 0` (Offen > 0):
   - A `DeliveryLine` is created with `Menge = offeneMenge = OrderLine.Menge - OrderLine.MengeGeliefert`
   - `DeliveryLine.AuftragZeileId = OrderLine.Id` (traceability)
   - `OrderLine.MengeGeliefert += offeneMenge`
3. Lines where `Offen = 0` are **skipped completely** — no DeliveryLine created

```
DeliveryLine.Menge        = OrderLine.Menge - OrderLine.MengeGeliefert
OrderLine.MengeGeliefert += DeliveryLine.Menge
Lines where Offen = 0 are SKIPPED
```

---

### 2.2 Rechnung buchen (`InvoiceService.BuchenAsync`)

1. For each `DeliveryLine` where `Menge > 0` AND `MengeFakturiert < Menge`:
   - An `InvoiceLine` is created with `Menge = fakturiert = DeliveryLine.Menge - DeliveryLine.MengeFakturiert`
   - `DeliveryLine.MengeFakturiert += InvoiceLine.Menge`
   - `OrderLine.MengeFakturiert += InvoiceLine.Menge` (via `AuftragZeileId`)
2. Storno lines (`DeliveryLine.Menge < 0`) are **skipped**
3. `DeliveryHeader.Status` is set to `Fakturiert`
4. Guard: if `InvoiceLine.Menge > (DeliveryLine.Menge - DeliveryLine.MengeFakturiert)` → `ValidationException`
5. `InvoiceHeader.BelegArt = Rechnung`, `Status = Gebucht`
6. `FrmRechnungErfassung` may open `FrmReportViewer` afterwards via `IReportRenderService.PreviewInvoiceAsync`

```
InvoiceLine.Menge            = DeliveryLine.Menge - DeliveryLine.MengeFakturiert
DeliveryLine.MengeFakturiert += InvoiceLine.Menge
OrderLine.MengeFakturiert    += InvoiceLine.Menge
```
Guard: `InvoiceLine.Menge > (DeliveryLine.Menge - MengeFakturiert)` → `ValidationException`

---

### 2.3 Gutschrift erstellen (`InvoiceService.CreateGutschriftAsync`)

This is the **only** invoice correction path. Direct "Stornieren" is no longer accessible via UI.

1. Validate: original invoice `Status == Gebucht` AND `BelegArt == Rechnung` — blocks double credit
2. Generate credit memo number using `GutschriftPraefix` (e.g. GS20260001)
3. Create new `InvoiceHeader` with:
   - `BelegArt = Gutschrift`
   - `Status = Gebucht`
   - `OriginalRechnungId = original.Id`
   - All header totals copied **positive** from original
4. Create corresponding `InvoiceLine` rows — all amounts **positive**, copied from original lines
5. Roll back tracking:
   - `DeliveryLine.MengeFakturiert = Max(0, MengeFakturiert - InvoiceLine.Menge)`
   - `OrderLine.MengeFakturiert = Max(0, MengeFakturiert - InvoiceLine.Menge)`
6. Mark original invoice: `Status = Gutgeschrieben`
7. Single `SaveChangesAsync`

```
Gutschrift.Gesamtbrutto       = original.Gesamtbrutto          (positive)
Gutschrift.InvoiceLine.Menge  = original.InvoiceLine.Menge    (positive)
DeliveryLine.MengeFakturiert  = Max(0, MengeFakturiert - il.Menge)
OrderLine.MengeFakturiert     = Max(0, MengeFakturiert - il.Menge)
original.Status               = Gutgeschrieben
```

**Delivery not touched:** `DeliveryHeader.Status` is NOT changed. The physical delivery remains intact.

**Re-invoiceability restored:** After credit memo, `DeliveryLine.MengeFakturiert = 0`, so the delivery reappears in `FrmRechnungErfassung` and can be invoiced again.

---

### 2.4 Teil-Storno Lieferung (`DeliveryService.StornierenZeileAsync`)

1. A new `DeliveryLine` with **negative** `Menge` is appended to the same `DeliveryHeader`
2. `OrderLine.MengeGeliefert = Max(0, MengeGeliefert - originalDeliveryLine.Menge)`
3. If any `OrderLine.Menge - MengeGeliefert > 0` ? `Order.Status = Offen` (reopened)
4. `DeliveryHeader.Status` = `TeilStorniert`

```
new DeliveryLine.Menge   = -originalDeliveryLine.Menge
OrderLine.MengeGeliefert = Max(0, MengeGeliefert - originalDeliveryLine.Menge)
```

**Guard:** A line with `MengeFakturiert > 0` cannot be Teil-storniert. Create a credit memo first.

---

### 2.5 Zahlung buchen (`PaymentPostingService.BuchenAsync`)

1. Generate `Zahlungsnummer` using `NoSeries.GetNextNumberAsync("ZA", Buchungsdatum)`
2. Create `PaymentHeader`
3. For each active row in `FrmZahlungseingaenge`:
   - If `Bar` > 0 ? add `PaymentLine(Method=Bar,  Amount=Bar,  ReferenceType, ReferenceId)`
   - If `Bank` > 0 ? add `PaymentLine(Method=Bank, Amount=Bank, ReferenceType, ReferenceId)`

**ReferenceId semantics:**
- `ReferenceType=0` ? `InvoiceHeader.Id` (customers with `ReceivableSource=Invoice`)
- `ReferenceType=1` ? `DeliveryHeader.Id` (customers with `ReceivableSource=Delivery`)

**OffenerBetrag calculation (in `CustomerLedgerService.GetLedgerAsync`):**
```
PaidAmount    = SUM(PaymentLine.Amount WHERE ReferenceType+ReferenceId match AND Header.KundeId=kundeId)
OffenerBetrag = Max(0, Header.Gesamtbrutto - PaidAmount)
```

---

### 2.6 Zahlung stornieren (`PaymentPostingService.StornierenAsync`)

1. Generate `stornoNummer` using `NoSeries.GetNextNumberAsync("ZA", Today)`
2. Create new `PaymentHeader` with:
   - `Zahlungsnummer = stornoNummer`
   - `Notiz = "STORNO: {orig.Zahlungsnummer} — {note}"`
3. Add new `PaymentLine(ReferenceType=-1, ReferenceId=originalLine.Id, Amount=-originalLine.Amount)`

Guards: already-negative lines cannot be storniert; double-storno blocked via `ReferenceType=-1` check.

---

## 3. IMPORTANT FORMULAS

```
Offen (OrderLine)           = Menge - MengeGeliefert
Fakturierbar (DeliveryLine) = Menge - MengeFakturiert
OffenerBetrag (Ledger)      = Max(0, Header.Gesamtbrutto - SUM(PaymentLine.Amount))
Saldo (Customer)            = SUM(OffenerBetrag of Rechnung/Lieferschein rows) - SUM(Gutschrift.Betrag)
```

---

## 4. RULES

| Rule | Detail |
|---|---|
| Never invoice negative lines | `DeliveryLine.Menge < 0` ? storno line ? skipped |
| Never exceed Menge | Guard in `BuchenAsync` |
| Delivery is physical truth | `DeliveryHeader.Status` NOT changed by invoice/payment ops |
| DB amounts always positive | Invoice/Payment amounts stored positive — sign from `BelegArt` or `Amount < 0` |
| No double credit | `CreateGutschriftAsync` checks `Status == Gebucht && BelegArt == Rechnung` |
| Original invoice preserved | After Gutschrift: original `Status = Gutgeschrieben`, stays in DB |
| Teil-Storno blocked if invoiced | `MengeFakturiert > 0` ? ValidationException |
| Payment cross-customer guard | `PaymentLines` always joined through `Header.KundeId` |
| Ledger = header-level | One row per `InvoiceHeader` / `DeliveryHeader` — NOT per line |
| Storno = negative PaymentLine | `Amount < 0` is the storno signal; `ReferenceType=-1` marks storno back-ref |
| Gutschrift not payable | `OffenerBetrag = 0` on Gutschrift rows — `BuchenAsync` blocks if `OffenerBetrag <= 0` |

---

## 5. DOCUMENT TYPE SEMANTICS (`BelegArt`)

| BelegArt / Type | DB amounts | Financial effect | Number prefix |
|---|---|---|---|
| `Rechnung (0)` | Positive | Customer owes | RE |
| `Gutschrift (1)` | Positive | Credit to customer | GS |
| `Zahlung (normal)` | Positive | Reduces open amount | ZA |
| `Zahlung (Storno)` | Negative | Restores open amount | ZA |

---

## 6. USER WORKFLOWS

### Normal flow
```
Auftrag ? buchen ? Lieferschein (Offen)
Lieferschein ? FrmRechnungErfassung ? BuchenAsync ? Rechnung (Gebucht)
Rechnung ? FrmZahlungseingaenge ? BuchenAsync ? PaymentHeader (ZA-...)
```

### Reporting / preview flow
```
FrmOrderList → Auftragsbestätigung drucken → PreviewOrderAsync → FrmReportViewer
FrmDeliveryList → markierte Lieferscheine → PreviewDeliveriesAsync → FrmReportViewer
FrmRechnungList → markierte Rechnungen → PreviewInvoicesAsync → FrmReportViewer
FrmSammelRechnung → SammelBuchenAsync → RechnungIds → PreviewInvoicesAsync → FrmReportViewer
```

### Invoice correction
```
Rechnung (Gebucht) ? Gutschrift erstellen ? CreateGutschriftAsync
  ? original: Status = Gutgeschrieben
  ? new Gutschrift: BelegArt = Gutschrift, Status = Gebucht
  ? MengeFakturiert rolled back ? delivery reappears in FrmRechnungErfassung
  ? OffenerBetrag restored in FrmZahlungseingaenge
```

### Payment storno
```
FrmZahlungseingaenge ? Gebuchte Zahlungen grid ? right-click ? Zeile stornieren
  ? input dialog for Stornogrund
  ? StornierenAsync ? new PaymentHeader (ZA) + negative PaymentLine
  ? Saldo and OffenerBetrag updated on reload
```

### Delivery-based customer
```
ReceivableSource = Delivery
Ledger source = DeliveryHeader rows (not InvoiceHeader)
Rechnung rows excluded from ledger (avoid double-counting)
ReferenceType = 1 in PaymentLines
```

---

## 7. KNOWN EDGE CASES

### 7.1 Teil-Storno (partial delivery cancellation)
- Storno line (`Menge < 0`) appended — original not deleted
- `OrderLine.MengeGeliefert` reduced ? Order becomes re-bookable
- **Blocked if `MengeFakturiert > 0`** — Gutschrift must be created first

### 7.2 Re-invoicing after Gutschrift
- `DeliveryLine.MengeFakturiert = 0` after rollback
- Delivery reappears in `FrmRechnungErfassung` (filter: `MengeFakturiert < Menge`, NOT header status)

### 7.3 Re-booking after Teil-Storno
- `CreateFromOrderAsync` skips lines where `Menge - MengeGeliefert = 0`

### 7.4 Payment storno re-opens balance
- After storno: `SUM(PaymentLine.Amount)` decreases ? `OffenerBetrag` increases automatically
- No separate "reopen" action needed — formula-driven

### 7.5 Delivery-source customers — double-booking prevention
- For `ReceivableSource=Delivery`: show Lieferschein rows; exclude all Rechnung rows
- Gutschrift rows shown as informational (`OffenerBetrag = 0`)
- Payment keyed on `InvoiceHeader.Id` vs `DeliveryHeader.Id` — different ID spaces

---

## 8. MODULE 7 — PAYMENT MODULE REFERENCE

### Services
| Service | Interface | Purpose |
|---|---|---|
| `PaymentPostingService` | `IPaymentPostingService` | Post and reverse payments |
| `CustomerLedgerService` | `ICustomerLedgerService` | Open items, saldo, payment history |

### IPaymentPostingService methods
| Method | Returns | Description |
|---|---|---|
| `BuchenAsync(request)` | `int` (PaymentHeader.Id) | Validate + post rows ? creates PaymentHeader + Lines |
| `StornierenAsync(lineId, note)` | `int` (new PaymentHeader.Id) | Reverse a single PaymentLine |

### ICustomerLedgerService methods
| Method | Returns | Description |
|---|---|---|
| `GetLedgerAsync(kundeId, von, bis)` | `IReadOnlyList<CustomerLedgerEntryDto>` | Open items per Beleg |
| `GetSaldoAsync(kundeId, von, bis)` | `decimal` | Net open balance |
| `GetPaymentHistoryAsync(kundeId, von, bis)` | `IReadOnlyList<PaymentHistoryDto>` | Posted payment lines |

### DTOs
| DTO | Key fields |
|---|---|
| `CustomerLedgerEntryDto` | `ReferenzId` (Header.Id), `BelegArt`, `Betrag`, `OffenerBetrag`, `Richtung` |
| `PaymentPostingRequest` | `KundeId`, `Buchungsdatum`, `Rows[]` |
| `PaymentPostingRowItem` | `ReferenceId` (Header.Id), `ReferenceType`, `BelegArt`, `OffenerBetrag`, `Bar`, `Bank` |
| `PaymentHistoryDto` | `PaymentLineId`, `Zahlungsnummer`, `IsStorno`, `Amount`, `BelegNr` |

### Form: `FrmZahlungseingaenge`
| Area | Description |
|---|---|
| Left panel | Customer list + text filter |
| Right top | Von/Bis date filter + Laden + Buchen + Saldo label |
| `dgwLedger` | Open items per Beleg — editable `Buchungsdatum` (CalendarColumn), `Bar`, `Bank`, `Notiz` |
| `dgwHistory` | Posted payments — right-click ? **Zeile stornieren** |

---

## 9. CURRENT STATUS

### Working
| Feature | Service / Form |
|---|---|
| Order → Delivery | `OrderService.BuchenAsync` + `DeliveryService.CreateFromOrderAsync` |
| Delivery → Invoice | `InvoiceService.BuchenAsync` |
| Invoice → Gutschrift | `InvoiceService.CreateGutschriftAsync` |
| Teil-Storno | `DeliveryService.StornierenZeileAsync` |
| Open items ledger | `CustomerLedgerService.GetLedgerAsync` |
| Payment posting | `PaymentPostingService.BuchenAsync` + `FrmZahlungseingaenge` |
| Payment storno | `PaymentPostingService.StornierenAsync` (right-click in history grid) |
| Saldo display | `CustomerLedgerService.GetSaldoAsync` |
| Payment history | `CustomerLedgerService.GetPaymentHistoryAsync` |
| Zahlungsnummer | NoSeries `ZA` → generated in `BuchenAsync` + `StornierenAsync` |
| ReceivableSource | `Customer.ReceivableSource` → editable in `FrmCustomerList → Einstellungen` |
| Invoice preview / PDF / print infra | `IInvoiceReportDataService` + `IReportRenderService` + `FrmReportViewer` |
| Order preview / PDF / print infra | `IOrderReportDataService` + `PreviewOrderAsync` |
| Delivery preview / PDF / print infra | `IDeliveryReportDataService` + `PreviewDeliveriesAsync` |
| Invoice bulk preview | `FrmRechnungList` checkbox selection + `PreviewInvoicesAsync` |
| Delivery bulk preview | `FrmDeliveryList` checkbox selection + `PreviewDeliveriesAsync` |
| After-booking preview prompt | `FrmRechnungErfassung` + `FrmSammelRechnung` |

### Still Open / Not Yet Implemented
- Final visual RDLC layout for `Rechnung`, `Auftragsbestaetigung`, `Lieferschein`
- Presentation-layer minus sign rendering for Gutschrift amounts (grid, print)
- Partial credit memo (per-line selection)
- Audit log for invoice/payment booking events
- Bulk PDF merge for multiple invoices in one output file
- Payment reconciliation / full settlement engine (Phase 2)
- `ZahlungszielTage` (payment terms) enforcement
- Berichte → Kundenverkauf / Produktverkauf
- Berichte → Zahlungen (currently `OnMenuItemNotImplemented`)

### Deprecated
| Method | Reason | Replacement |
|---|---|---|
| `InvoiceService.StornierenAsync` | No Gegendokument | `CreateGutschriftAsync` |

---

# RDLC Reporting Flow (2024)

## Architecture
- All reporting data comes from SQL Views (e.g. vwInvoiceReport, vwOrderReport, vwDeliveryReport, vwKundenkontoReport).
- At runtime, data is loaded via ADO.NET (SqlConnection, SqlCommand, SqlDataAdapter) into a DataTable.
- DataTable is bound to the RDLC ReportViewer using ReportDataSource with the exact dataset name (e.g. dsInvoice, dsOrder, dsDelivery, dsKundenkonto).
- All report logic is centralized in RdlcReportRenderService.
- Forms must call only the centralized report service (never load or bind reports directly).
- No business logic or DTO mapping in C# for reporting.
- XSD datasets are only for RDLC design-time support and must match the SQL View.

## Migration Steps
- Remove old report DTO/service code only if it is no longer referenced after migration.
- Do not remove business/domain DTOs used outside reporting.
- Do not edit RDLC or XSD files unless updating design-time fields.

## Manual Steps for Developers
- Ensure each RDLC file has a dataset matching the DataTable name in code.
- Ensure each XSD dataset matches the SQL View columns for design-time support.
- Create/refresh the XSD and RDLC datasets as needed.
