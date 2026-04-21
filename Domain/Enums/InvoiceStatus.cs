namespace DNR26V2.Domain.Enums;

public enum InvoiceStatus
{
    Offen       = 0,
    TeilBezahlt = 1,
    Bezahlt     = 2,
    Storniert   = 3,
    Mahnung1    = 4,
    Mahnung2    = 5,
    Inkasso     = 6
}