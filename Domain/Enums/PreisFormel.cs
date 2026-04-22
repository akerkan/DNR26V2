namespace DNR26V2.Domain.Enums;

public enum PreisFormel
{
    MengeXPreis         = 0,  // Unit price:   GrossAmount = Menge * Preis
    MengeXGewichtXPreis = 1   // Weight price: GrossAmount = Menge * Gewicht * Preis
}