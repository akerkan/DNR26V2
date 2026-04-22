using DNR26V2.Domain.Enums;

namespace DNR26V2.Domain.Helpers;

/// <summary>
/// Central price calculation helper.
/// NEVER calculate prices inline — always use this class.
/// NEVER round intermediate values — only round at the final step.
/// </summary>
public static class InvoiceCalculator
{
    /// <summary>
    /// Step 1: Gross amount before discount. No rounding — full decimal(18,4) precision.
    /// </summary>
    public static decimal CalcGrossAmount(
        decimal menge, decimal gewicht, decimal preis, PreisFormel formel)
        => formel == PreisFormel.MengeXGewichtXPreis
            ? menge * gewicht * preis
            : menge * preis;

    /// <summary>
    /// Step 2: Discount amount — rounded at this step.
    /// </summary>
    public static decimal CalcDiscountAmount(decimal grossAmount, decimal discountProzent)
        => Math.Round(grossAmount * discountProzent / 100, 2);

    /// <summary>
    /// Step 3: Net line amount after discount. This is the VAT base.
    /// </summary>
    public static decimal CalcLineAmount(decimal grossAmount, decimal discountAmount)
        => grossAmount - discountAmount;

    /// <summary>
    /// Step 4: VAT amount — display only, NEVER summed for header total.
    /// </summary>
    public static decimal CalcVatAmount(decimal lineAmount, decimal mwstProzent)
        => Math.Round(lineAmount * mwstProzent / 100, 2);

    /// <summary>
    /// Step 5: Brutto per line — display only.
    /// </summary>
    public static decimal CalcAmountInclVat(decimal lineAmount, decimal vatAmount)
        => lineAmount + vatAmount;

    /// <summary>
    /// Header totals — single source of truth for printing (UStG §14).
    /// Gesamtmwst is calculated ONCE per VAT group — never summed from line VatAmounts.
    /// </summary>
    public static (decimal Netto, decimal Mwst, decimal Brutto) CalcHeader(
        IEnumerable<(decimal LineAmount, decimal MwstProzent)> zeilen)
    {
        var list  = zeilen.ToList();
        var netto = list.Sum(z => z.LineAmount);
        var mwst  = list
            .GroupBy(z => z.MwstProzent)
            .Sum(g => Math.Round(g.Sum(z => z.LineAmount) * g.Key / 100, 2));
        return (Math.Round(netto, 2), Math.Round(mwst, 2), Math.Round(netto + mwst, 2));
    }
}