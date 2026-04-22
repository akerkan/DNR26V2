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