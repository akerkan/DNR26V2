# ORDER_FLOW_TRACKING.md
# DNR26V2 — Order → Delivery → Invoice Flow

Technical and business reference for the complete ERP document chain.

---

## 1. ENTITIES

### Order (`Orders` table)
Represents a customer's purchase order for a specific delivery date.

| Field | Type | Purpose |
|---|---|---|
| `Status` | `OrderStatus` | `Offen` → `Freigegeben` → `Gebucht` → `Storniert` / `Geloescht` |
| `LieferDatum` | `DateTime` | Requested delivery date |
| `Auftragsnummer` | `string` | System-generated document number |

---

### OrderLine (`OrderLines` table)
One product position within an Order.

| Field | Type | Purpose |
|---|---|---|
| `Menge` | `decimal(10,3)` | Ordered quantity |
| `MengeGeliefert` | `decimal(10,3)` | Cumulative delivered quantity (sum of all positive DeliveryLine.Menge referencing this line) |
| `MengeFakturiert` | `decimal(10,3)` | Cumulative invoiced quantity (sum of all InvoiceLine.Menge referencing this line) |
| `Preis` | `decimal` | Unit price |
| `ArtikelId` | `int` | FK → Product |
| `AuftragId` | `int` | FK → Order |

**Computed display value (not stored):**
```
Offen = Menge - MengeGeliefert
```

---

### DeliveryHeader (`Deliveries` table)
Represents a physical shipment (Lieferschein) for an Order.

| Field | Type | Purpose |
|---|---|---|
| `Status` | `DeliveryStatus` | `Offen` → `TeilStorniert` → `Fakturiert` → `Storniert` |
| `AuftragId` | `int?` | FK → Order (nullable for manual deliveries) |
| `LieferDatum` | `DateTime` | Actual delivery date |
| `Lieferscheinnummer` | `string` | System-generated document number |

---

### DeliveryLine (`DeliveryLines` table)
One product position within a DeliveryHeader.

| Field | Type | Purpose |
|---|---|---|
| `Menge` | `decimal(10,3)` | Delivered quantity. **Negative for storno lines.** |
| `MengeGeliefert` | `decimal(10,3)` | Always `0` — unused on DeliveryLine, exists for schema symmetry |
| `MengeFakturiert` | `decimal(18,2)` | Cumulative invoiced quantity for this delivery line |
| `AuftragZeileId` | `int?` | FK → OrderLine — traceability link set during delivery creation |
| `LieferscheinId` | `int` | FK → DeliveryHeader |
| `ArtikelId` | `int` | FK → Product |

**Important:** `MengeGeliefert` on `DeliveryLine` is always `0`. Do NOT use it for invoicing logic. Use `Menge` instead.

---

### InvoiceHeader (`Invoices` table)
Represents a posted invoice for one or more deliveries.

| Field | Type | Purpose |
|---|---|---|
| `Status` | `InvoiceStatus` | `Gebucht` → `Storniert` |
| `KundeId` | `int` | FK → Customer |
| `Von` / `Bis` | `DateTime` | Billing period |
| `Rechnungsnummer` | `string` | System-generated document number |
| `IstSammelrechnung` | `bool` | True when multiple deliveries batched into one invoice |

---

### InvoiceLine (`InvoiceLines` table)
One product position within an InvoiceHeader.

| Field | Type | Purpose |
|---|---|---|
| `Menge` | `decimal(10,3)` | Invoiced quantity — **source of truth for tracking rollback** |
| `FakturierteMenge` | `decimal(10,3)` | Mirror of `Menge` — kept for display/legacy |
| `DeliveryLineId` | `int` | FK → DeliveryLine — the physical line being invoiced |
| `LieferscheinId` | `int` | FK → DeliveryHeader |
| `ArtikelId` | `int` | FK → Product |

**Always use `InvoiceLine.Menge` — not `FakturierteMenge` — as the source of truth in booking and rollback logic.**

---

## 2. CORE LOGIC

### 2.1 Auftrag buchen → Lieferschein erstellen (`OrderService.BuchenAsync` + `DeliveryService.CreateFromOrderAsync`)

1. Order status set to `Gebucht`
2. For each `OrderLine` where `Menge - MengeGeliefert > 0` (Offen > 0):
   - A `DeliveryLine` is created with `Menge = offeneMenge = OrderLine.Menge - OrderLine.MengeGeliefert`
   - `DeliveryLine.AuftragZeileId = OrderLine.Id` (traceability)
   - `OrderLine.MengeGeliefert += offeneMenge`
3. Lines where `Offen = 0` are **skipped completely** — no DeliveryLine created

```
DeliveryLine.Menge     = OrderLine.Menge - OrderLine.MengeGeliefert
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
4. Guard: if `InvoiceLine.Menge > (DeliveryLine.Menge - DeliveryLine.MengeFakturiert)` → `ValidationException`

```
InvoiceLine.Menge              = DeliveryLine.Menge - DeliveryLine.MengeFakturiert
DeliveryLine.MengeFakturiert   += InvoiceLine.Menge
OrderLine.MengeFakturiert      += InvoiceLine.Menge
```

**Implementation note:** Tracking updates use explicit `_db.DeliveryLine.Where(...)` / `_db.OrderLine.Where(...)` loads — NOT the `Include`-loaded navigation collections. This mirrors `StornierenAsync` and ensures EF generates the correct `UPDATE` statements.

---

### 2.3 Rechnung stornieren (`InvoiceService.StornierenAsync`)

1. For each `InvoiceLine` in the invoice:
   - `DeliveryLine.MengeFakturiert = Max(0, MengeFakturiert - InvoiceLine.Menge)`
   - `OrderLine.MengeFakturiert = Max(0, MengeFakturiert - InvoiceLine.Menge)`
2. `InvoiceHeader.Status` = `Storniert`
3. **`DeliveryHeader.Status` is NOT changed** — physical delivery is not reversed

```
DeliveryLine.MengeFakturiert   = Max(0, DeliveryLine.MengeFakturiert - InvoiceLine.Menge)
OrderLine.MengeFakturiert      = Max(0, OrderLine.MengeFakturiert - InvoiceLine.Menge)
```

---

### 2.4 Teil-Storno Lieferung (`DeliveryService.StornierenZeileAsync`)

1. A new `DeliveryLine` with **negative** `Menge` is appended to the same `DeliveryHeader`
2. `OrderLine.MengeGeliefert = Max(0, MengeGeliefert - originalDeliveryLine.Menge)`
3. If all `OrderLine.Menge - MengeGeliefert > 0` for any line → `Order.Status = Offen` (reopened)
4. `DeliveryHeader.Status` = `TeilStorniert`

```
new DeliveryLine.Menge         = -originalDeliveryLine.Menge
OrderLine.MengeGeliefert       = Max(0, MengeGeliefert - originalDeliveryLine.Menge)
```

**Guard:** A line with `MengeFakturiert > 0` cannot be Teil-storniert. The invoice must be reversed first.

---

## 3. IMPORTANT FORMULAS

```
Offen (OrderLine)         = Menge - MengeGeliefert
Fakturierbar (DeliveryLine) = Menge - MengeFakturiert
```

- `Offen` is a **display-only computed value** — never stored in DB
- `Fakturierbar` determines whether a delivery line appears in the invoice capture list

---

## 4. RULES

| Rule | Detail |
|---|---|
| Never invoice negative lines | `DeliveryLine.Menge < 0` → storno line → skipped in `BuchenAsync` |
| Never exceed `Menge` | Guard: `InvoiceLine.Menge > (DeliveryLine.Menge - MengeFakturiert)` → throws |
| Delivery is physical truth | `DeliveryHeader.Status` is set by delivery operations only — invoice storno does NOT reset it |
| Invoice is financial layer | Invoice storno only reverses `MengeFakturiert` — physical delivery records are untouched |
| Re-invoiceability by quantity | A delivery is invoiceable again when `DeliveryLine.MengeFakturiert < DeliveryLine.Menge`, regardless of `DeliveryHeader.Status` |
| Teil-Storno blocked if invoiced | `DeliveryLine.MengeFakturiert > 0` → `ValidationException` on Teil-Storno attempt |
| Tracking values survive save | `SaveAuftragAsync` preserves `MengeGeliefert` and `MengeFakturiert` per `ArtikelId` when replacing order lines |

---

## 5. KNOWN EDGE CASES

### 5.1 Teil-Storno (partial delivery cancellation)
- User cancels a single `DeliveryLine` via context menu in `FrmDeliveryList`
- A storno line (`Menge < 0`) is appended — the original line is not deleted
- `OrderLine.MengeGeliefert` is reduced → `Offen` increases → Order becomes re-bookable
- **Blocked if `MengeFakturiert > 0`** — invoice must be reversed first

### 5.2 Re-invoicing after invoice storno
- After `StornierenAsync`: `DeliveryLine.MengeFakturiert = 0`, `DeliveryHeader.Status` stays `Fakturiert`
- `GetOffeneLieferscheineAsync` and `GetKundenMitOffenenLsAsync` filter by `MengeFakturiert < Menge` (line level), NOT by `DeliveryHeader.Status`
- `BuchenAsync` accepts deliveries with `Status != Storniert` (not just `Status == Offen`)
- The delivery reappears in `FrmRechnungErfassung` and can be invoiced again

### 5.3 Reopened order after Teil-Storno (re-booking)
- After Teil-Storno, `Order.Status` = `Offen`
- `CreateFromOrderAsync` only creates `DeliveryLine` for lines where `Menge - MengeGeliefert > 0`
- Lines with `Offen = 0` are skipped — no duplicate delivery lines created
- `SaveAuftragAsync` preserves `MengeGeliefert` / `MengeFakturiert` so re-booking uses correct open quantities

### 5.4 Partial invoice (Menge > MengeFakturiert after storno)
- After invoice storno, `fakturiert = DeliveryLine.Menge - DeliveryLine.MengeFakturiert` equals the full original quantity (since `MengeFakturiert` was reset to `0`)
- `Gewicht` is recalculated proportionally: `Gewicht * fakturiert / Menge`

---

## 6. CURRENT STATUS

### Working
- `DeliveryService.CreateFromOrderAsync` — correctly skips fully delivered lines, uses `Menge - MengeGeliefert`
- `InvoiceService.BuchenAsync` — explicitly loads and updates `DeliveryLine.MengeFakturiert` and `OrderLine.MengeFakturiert` via direct DB queries (not via Include navigation)
- `InvoiceService.StornierenAsync` — correctly rolls back `MengeFakturiert`, does NOT reset `DeliveryHeader.Status`
- `GetOffeneLieferscheineAsync` / `GetKundenMitOffenenLsAsync` — filter by line-level open quantity, not header status
- `SaveAuftragAsync` — preserves tracking values (`MengeGeliefert`, `MengeFakturiert`) when replacing order lines
- `StornierenZeileAsync` — blocks Teil-Storno on invoiced lines (`MengeFakturiert > 0`)

### Recently Fixed
| Fix | Description |
|---|---|
| `BuchenAsync` — storno lines invoiced | Added filter `z.Menge > 0 && z.MengeFakturiert < z.Menge` to inner loop |
| `BuchenAsync` — `fakturiert` used full Menge | Changed to `DeliveryLine.Menge - DeliveryLine.MengeFakturiert` |
| `BuchenAsync` — `MengeFakturiert` not persisted | Moved from Include-based tracking to explicit `_db.DeliveryLine.Where(...)` load pattern |
| `StornierenAsync` — delivery status reset | Removed `ls.Status = DeliveryStatus.Offen` block; delivery stays `Fakturiert` |
| `SaveAuftragAsync` — tracking reset on save | Added `trackingMap` to preserve `MengeGeliefert` / `MengeFakturiert` before `RemoveRange` |
| Invoice selection queries | Changed `dh.Status = 0` to `dh.Status <> 3` + line-level `MengeFakturiert < Menge` filter |

### Still Open / Not Yet Implemented
- `OrderLine.MengeGeliefert` display in `FrmRechnungList` detail panel (currently not shown)
- Audit log for invoice booking and storno events (AuditLogService not yet wired into `InvoiceService`)
- Print / PDF output for Lieferschein and Rechnung
- Payment tracking (`ZahlungPraefix` prefix and `ZahlungszielTage` setup fields exist but no payment module yet)
