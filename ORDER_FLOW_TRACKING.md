# ORDER_FLOW_TRACKING.md
# DNR26V2 — Order ? Delivery ? Invoice ? Credit Memo Flow

Technical and business reference for the complete ERP document chain.

---

## 1. ENTITIES

### Order (`Orders` table)
Represents a customer's purchase order for a specific delivery date.

| Field | Type | Purpose |
|---|---|---|
| `Status` | `OrderStatus` | `Offen` ? `Freigegeben` ? `Gebucht` ? `Storniert` / `Geloescht` |
| `LieferDatum` | `DateTime` | Requested delivery date |
| `Auftragsnummer` | `string` | System-generated document number |

---

### OrderLine (`OrderLines` table)
One product position within an Order.

| Field | Type | Purpose |
|---|---|---|
| `Menge` | `decimal(10,3)` | Ordered quantity |
| `MengeGeliefert` | `decimal(10,3)` | Cumulative delivered quantity |
| `MengeFakturiert` | `decimal(10,3)` | Cumulative invoiced quantity (reduced by credit memo) |
| `Preis` | `decimal` | Unit price |
| `ArtikelId` | `int` | FK ? Product |
| `AuftragId` | `int` | FK ? Order |

**Computed display value (not stored):**
```
Offen = Menge - MengeGeliefert
```

---

### DeliveryHeader (`Deliveries` table)
Represents a physical shipment (Lieferschein) for an Order.

| Field | Type | Purpose |
|---|---|---|
| `Status` | `DeliveryStatus` | `Offen` ? `TeilStorniert` ? `Fakturiert` ? `Storniert` |
| `AuftragId` | `int?` | FK ? Order (nullable for manual deliveries) |
| `LieferDatum` | `DateTime` | Actual delivery date |
| `Lieferscheinnummer` | `string` | System-generated document number |

---

### DeliveryLine (`DeliveryLines` table)
One product position within a DeliveryHeader.

| Field | Type | Purpose |
|---|---|---|
| `Menge` | `decimal(10,3)` | Delivered quantity. **Negative for storno lines.** |
| `MengeGeliefert` | `decimal(10,3)` | Always `0` — unused on DeliveryLine, exists for schema symmetry |
| `MengeFakturiert` | `decimal(18,2)` | Cumulative invoiced quantity. Reduced by credit memo creation. |
| `AuftragZeileId` | `int?` | FK ? OrderLine — traceability link set during delivery creation |
| `LieferscheinId` | `int` | FK ? DeliveryHeader |
| `ArtikelId` | `int` | FK ? Product |

**Important:** `MengeGeliefert` on `DeliveryLine` is always `0`. Do NOT use it for invoicing logic. Use `Menge` instead.

---

### InvoiceHeader (`Invoices` table)
Represents a posted invoice or credit memo. Both document types share this table.

| Field | Type | Purpose |
|---|---|---|
| `Status` | `InvoiceStatus` | `Gebucht` ? `Gutgeschrieben` (see below) |
| `BelegArt` | `InvoiceDocumentType` | `Rechnung` (0) or `Gutschrift` (1) |
| `OriginalRechnungId` | `int?` | FK ? original invoice (set on Gutschrift rows only) |
| `KundeId` | `int` | FK ? Customer |
| `Von` / `Bis` | `DateTime` | Billing period |
| `Rechnungsnummer` | `string` | System-generated document number (prefix RE or GS) |
| `IstSammelrechnung` | `bool` | True when multiple deliveries batched into one invoice |
| `Gesamtnetto` | `decimal` | Net total — **always positive** for both Rechnung and Gutschrift |
| `Gesamtmwst` | `decimal` | VAT total — **always positive** |
| `Gesamtbrutto` | `decimal` | Gross total — **always positive** |

**InvoiceStatus values:**

| Value | Meaning |
|---|---|
| `Gebucht = 1` | Active document — invoice or credit memo in effect |
| `Gutgeschrieben = 3` | Original invoice for which a credit memo has been created — read-only, stays in DB |
| `Storniert = 2` | Legacy direct cancellation — no longer used via UI |

**Amount sign rule:**
All amounts stored in DB are **positive** for both `Rechnung` and `Gutschrift`.
`BelegArt = Gutschrift` is the single source of truth for financial sign.
Presentation layer (UI, print, reports) must render Gutschrift amounts with a leading minus sign.

---

### InvoiceLine (`InvoiceLines` table)
One product position within an InvoiceHeader. Shared by both Rechnung and Gutschrift.

| Field | Type | Purpose |
|---|---|---|
| `Menge` | `decimal(10,3)` | Invoiced quantity — **always positive**, source of truth for rollback |
| `FakturierteMenge` | `decimal(10,3)` | Mirror of `Menge` — kept for display/legacy |
| `DeliveryLineId` | `int` | FK ? DeliveryLine — the physical line being invoiced |
| `LieferscheinId` | `int` | FK ? DeliveryHeader |
| `ArtikelId` | `int` | FK ? Product |
| `LineAmount` | `decimal` | **Always positive** — sign derived from `InvoiceHeader.BelegArt` |

**Always use `InvoiceLine.Menge` — not `FakturierteMenge` — as the source of truth in booking and rollback logic.**

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
DeliveryLine.Menge       = OrderLine.Menge - OrderLine.MengeGeliefert
OrderLine.MengeGeliefert += DeliveryLine.Menge
```

---

### 2.2 Rechnung buchen (`InvoiceService.BuchenAsync`)

1. For each `DeliveryLine` where `Menge > 0` AND `MengeFakturiert < Menge`:
   - An `InvoiceLine` is created with `Menge = fakturiert = DeliveryLine.Menge - DeliveryLine.MengeFakturiert`
   - `DeliveryLine.MengeFakturiert += InvoiceLine.Menge`
   - `OrderLine.MengeFakturiert += InvoiceLine.Menge` (via `AuftragZeileId`)
2. Storno lines (`DeliveryLine.Menge < 0`) are **skipped**
3. `DeliveryHeader.Status` is set to `Fakturiert`
4. Guard: if `InvoiceLine.Menge > (DeliveryLine.Menge - DeliveryLine.MengeFakturiert)` ? `ValidationException`
5. `InvoiceHeader.BelegArt = Rechnung`, `Status = Gebucht`

```
InvoiceLine.Menge            = DeliveryLine.Menge - DeliveryLine.MengeFakturiert
DeliveryLine.MengeFakturiert += InvoiceLine.Menge
OrderLine.MengeFakturiert    += InvoiceLine.Menge
```

**Implementation note:** Tracking updates use explicit `_db.DeliveryLine.Where(...)` / `_db.OrderLine.Where(...)` loads — NOT the Include-loaded navigation collections. This ensures EF generates correct UPDATE statements.

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
Gutschrift.Gesamtnetto       = original.Gesamtnetto          (positive)
Gutschrift.InvoiceLine.Menge = original.InvoiceLine.Menge    (positive)
DeliveryLine.MengeFakturiert = Max(0, MengeFakturiert - il.Menge)
OrderLine.MengeFakturiert    = Max(0, MengeFakturiert - il.Menge)
original.Status              = Gutgeschrieben
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

## 3. IMPORTANT FORMULAS

```
Offen (OrderLine)           = Menge - MengeGeliefert
Fakturierbar (DeliveryLine) = Menge - MengeFakturiert
```

- `Offen` is a **display-only computed value** — never stored in DB
- `Fakturierbar` determines whether a delivery line appears in the invoice capture list
- Both formulas work identically after a credit memo (rollback resets `MengeFakturiert`)

---

## 4. RULES

| Rule | Detail |
|---|---|
| Never invoice negative lines | `DeliveryLine.Menge < 0` ? storno line ? skipped in `BuchenAsync` |
| Never exceed `Menge` | Guard: `InvoiceLine.Menge > (DeliveryLine.Menge - MengeFakturiert)` ? throws |
| Delivery is physical truth | `DeliveryHeader.Status` is NOT changed by invoice or credit memo operations |
| Invoice is financial layer | Only `MengeFakturiert` is affected — physical delivery records are untouched |
| Re-invoiceability by quantity | A delivery is invoiceable again when `DeliveryLine.MengeFakturiert < DeliveryLine.Menge` |
| Teil-Storno blocked if invoiced | `DeliveryLine.MengeFakturiert > 0` ? `ValidationException` on Teil-Storno attempt |
| Tracking values survive save | `SaveAuftragAsync` preserves `MengeGeliefert` and `MengeFakturiert` per `ArtikelId` |
| DB amounts always positive | All `InvoiceLine` and `InvoiceHeader` amount fields are stored positive — sign comes from `BelegArt` |
| No double credit | `CreateGutschriftAsync` checks `Status == Gebucht && BelegArt == Rechnung` before proceeding |
| Original invoice preserved | After credit memo: original `Status = Gutgeschrieben`, stays in DB for audit trail |

---

## 5. DOCUMENT TYPE SEMANTICS (`BelegArt`)

| BelegArt | DB amounts | Financial effect | Number prefix |
|---|---|---|---|
| `Rechnung (0)` | Positive | Positive (customer owes) | RE |
| `Gutschrift (1)` | Positive | Negative (credit to customer) | GS |

**Presentation rule:** Anywhere Gutschrift amounts are displayed (grid, print, PDF), the UI/print layer must prepend a minus sign. The DB itself never stores negative invoice amounts.

---

## 6. USER WORKFLOW

### Normal invoice flow
```
Auftrag ? buchen ? Lieferschein (Offen)
Lieferschein ? FrmRechnungErfassung ? BuchenAsync ? Rechnung (Gebucht)
```

### Invoice correction flow (only path)
```
Rechnung (Gebucht) ? Gutschrift erstellen ? CreateGutschriftAsync
  ? original: Status = Gutgeschrieben
  ? new Gutschrift: BelegArt = Gutschrift, Status = Gebucht
  ? MengeFakturiert rolled back
  ? Delivery reappears in FrmRechnungErfassung (re-invoiceable)
```

### Re-invoice after credit memo
```
Delivery reappears (MengeFakturiert = 0)
FrmRechnungErfassung ? BuchenAsync ? new Rechnung (Gebucht)
```

---

## 7. KNOWN EDGE CASES

### 7.1 Teil-Storno (partial delivery cancellation)
- User cancels a single `DeliveryLine` via context menu in `FrmDeliveryList`
- A storno line (`Menge < 0`) is appended — the original line is not deleted
- `OrderLine.MengeGeliefert` is reduced ? `Offen` increases ? Order becomes re-bookable
- **Blocked if `MengeFakturiert > 0`** — credit memo must be created first

### 7.2 Re-invoicing after credit memo
- After `CreateGutschriftAsync`: `DeliveryLine.MengeFakturiert = 0`, `DeliveryHeader.Status` stays `Fakturiert`
- `GetOffeneLieferscheineAsync` and `GetKundenMitOffenenLsAsync` filter by `MengeFakturiert < Menge` (line level), NOT by `DeliveryHeader.Status`
- The delivery reappears in `FrmRechnungErfassung` and can be invoiced again

### 7.3 Reopened order after Teil-Storno (re-booking)
- After Teil-Storno, `Order.Status` = `Offen`
- `CreateFromOrderAsync` only creates `DeliveryLine` for lines where `Menge - MengeGeliefert > 0`
- Lines with `Offen = 0` are skipped — no duplicate delivery lines created
- `SaveAuftragAsync` preserves `MengeGeliefert` / `MengeFakturiert` so re-booking uses correct open quantities

### 7.4 Partial credit memo (future)
- Phase 1 credit memo always covers the full original invoice
- Partial credit memos (per line) are not yet implemented

---

## 8. CURRENT STATUS

### Working
- `DeliveryService.CreateFromOrderAsync` — correctly skips fully delivered lines, uses `Menge - MengeGeliefert`
- `InvoiceService.BuchenAsync` — explicit load pattern for `MengeFakturiert` updates
- `InvoiceService.CreateGutschriftAsync` — full credit memo with positive amounts, `MengeFakturiert` rollback, double-credit protection
- `GetOffeneLieferscheineAsync` / `GetKundenMitOffenenLsAsync` — filter by line-level open quantity, not header status
- `SaveAuftragAsync` — preserves tracking values (`MengeGeliefert`, `MengeFakturiert`) when replacing order lines
- `StornierenZeileAsync` — blocks Teil-Storno on invoiced lines (`MengeFakturiert > 0`)
- `FrmRechnungList` — Gutschrift button is the only correction action; Stornieren hidden

### Deprecated
| Method | Reason | Replacement |
|---|---|---|
| `InvoiceService.StornierenAsync` | No Gegendokument, direct cancellation | `CreateGutschriftAsync` |

### Recently Added (Credit Memo — Phase 1)
| Change | Description |
|---|---|
| `InvoiceDocumentType` enum | `Rechnung = 0`, `Gutschrift = 1` |
| `InvoiceStatus.Gutgeschrieben = 3` | Original invoice status after credit memo created |
| `InvoiceHeader.BelegArt` | Document type field, default `Rechnung` |
| `InvoiceHeader.OriginalRechnungId` | Nullable FK ? original invoice, set on Gutschrift rows |
| `InvoiceService.CreateGutschriftAsync` | Full credit memo creation with rollback |
| `GenerateGutschriftnummerAsync` | Number series using `GutschriftPraefix` (GS) |
| Migration `Module6_InvoiceDocumentType` | Adds `BelegArt` + `OriginalRechnungId` to `Invoices` table |
| `FrmRechnungList` | `btnStornieren` hidden; `btnGutschrift` is the only correction action |

### Still Open / Not Yet Implemented
- Presentation-layer minus sign rendering for Gutschrift amounts (grid CellFormatting, print)
- Partial credit memo (per-line selection)
- Audit log for invoice booking and credit memo events
- Print / PDF output for Rechnung and Gutschrift
- Payment tracking (`ZahlungPraefix` prefix and `ZahlungszielTage` setup fields exist but no payment module yet)
