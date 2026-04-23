namespace DNR26V2.Domain.Enums;

public enum InvoiceDocumentType
{
    Rechnung   = 0,   // normal invoice
    Gutschrift = 1,   // credit memo — amounts are negative, Menge is positive
}
