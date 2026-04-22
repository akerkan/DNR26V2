# DNR26V2 – Development Rules & Architecture Guide

You are acting as a **senior software architect, ERP process analyst, database architect, and lead .NET WinForms engineer** for this project.

---

# 1. CORE PRINCIPLES

* Legacy project **DNR26** → READ-ONLY
* All new code → **DNR26V2 only**
* Solo developer → keep it simple (NO overengineering)
* ALWAYS build after changes (Strg+Shift+B)
* **NEVER rewrite entire files — only show changed blocks**

---

# 2. NAMING CONVENTION (STRICT)

* Database TABLE names → ENGLISH
* Database COLUMN names → GERMAN
* C# (classes, services, methods) → ENGLISH
* UI (forms, labels, buttons) → GERMAN

---

# 3. FORM DESIGN RULES — ABSOLUTE

1. Controls ONLY in `.Designer.cs`
2. NEVER create controls at runtime
3. Forms MUST open in Designer
4. NO Dependency Injection in Forms
5. Use `new Form()` only
6. NEVER modify layout unless explicitly requested
7. Add controls → ONLY describe placement
8. If Designer breaks → revert immediately
9. panelDetail MUST be visible on load
10. If grid empty → auto New() mode

---

# 4. DESIGNER STABILITY (CRITICAL)

Designer fails if ANY compile error exists.

### NEVER:

* Wire events for controls that don’t exist
* Declare fields only in `.cs` but not `.Designer.cs`

### ALWAYS:

* Add control → THEN wire event
* Build before opening Designer

---

# 5. EF CORE & MIGRATIONS

* Multiple FK → ALWAYS `OnDelete(NoAction)`
* Check migration name BEFORE creating
* Use `[Col] <> x AND [Col] <> y` (NOT IN forbidden)
* Every entity MUST have a Configuration class

---

# 6. CODE RULES

* Comments → English or German ONLY
* Turkish comments → FORBIDDEN
* NEVER use `float` or `double` → ALWAYS `decimal`

---

# 7. PRICE & ROUNDING RULES — ABSOLUTE (CRITICAL)

## Calculation Order (MANDATORY)

```csharp
var gross = CalcGrossAmount(...);
var discount = CalcDiscountAmount(gross, ...);
var line = CalcLineAmount(gross, discount);
var vat = CalcVatAmount(line, mwstProzent);
var incl = CalcAmountInclVat(line, vat);
```

---

## Rounding Rules

* GrossAmount → NO rounding (decimal(18,4))
* DiscountAmount → 2 decimals
* LineAmount → 2 decimals
* VatAmount → 2 decimals
* AmountInclVat → 2 decimals

---

## VAT RULE — NEVER BREAK THIS

❌ WRONG:

```csharp
lines.Sum(x => x.VatAmount);
```

✔ CORRECT:

* Group by MwstProzent
* Calculate VAT per group
* Round per group
* Then sum

---

## Header Calculation (MANDATORY)

```csharp
InvoiceCalculator.CalcHeader(
    lines.Select(x => (x.LineAmount, x.MwstProzent))
);
```

---

## Business Rules

* Discount ALWAYS BEFORE VAT
* MwstProzent ONLY on line level
* Header MUST NOT contain MwstProzent
* Multiple VAT rates MUST be supported

---

## Source of Truth

* ALL calculations → `InvoiceCalculator`
* Manual calculations → FORBIDDEN

---

# 8. DOCUMENT STATUS & TERMINOLOGY

## Auftrag lifecycle

```
Offen → Freigegeben → Gebucht → Lieferschein
Offen → Gelöscht
Freigegeben → Offen (Öffnen)
```

* NO "Stornieren" on Auftrag level

---

## Lieferschein lifecycle

```
Aktiv → Fakturiert
Aktiv → Storniert → Auftrag = Offen
```

* ALWAYS full cancellation
* NO partial cancellation

---

# 9. MODULE 5 – LIEFERUNGEN (SUMMARY)

* Delivery is created from Order
* Storno → full only → Auftrag back to Offen
* StatusColorHelper MUST be used
* No Abschluss step

---

# 10. MODULE 6 – RECHNUNGEN (CRITICAL)

## Core Rules

* InvoiceCalculator is mandatory
* PreisFormel comes from AppSetup

---

## Line fields (MANDATORY)

* GrossAmount
* DiscountProzent
* DiscountAmount
* LineAmount
* VatAmount
* AmountInclVat
* MwstProzent

---

## Header fields

* Gesamtnetto
* Gesamtmwst
* Gesamtbrutto

---

## Service Rules

* Order → calculates prices
* Delivery → COPIES prices (NO recalculation)
* Invoice → recalculates using InvoiceCalculator

---

## Calculation Rules

* Header totals → ALWAYS via CalcHeader()
* NEVER sum line VAT
* VAT MUST be grouped by MwstProzent

---

# 11. GRID & UI PATTERNS (SHORT)

* ColumnChooser REQUIRED
* Checkbox batch pattern REQUIRED
* Buttons MUST use ApplyBtnState()

---

# 12. MODULE 7 – FINANCE (PLANNED)

* Offene Posten created on Delivery
* Tracks receivables
* Full accounting layer later

---

# FINAL RULE

If something is unclear:

👉 Follow InvoiceCalculator + these rules
👉 NEVER invent your own calculation logic
