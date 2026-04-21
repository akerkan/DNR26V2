namespace DNR26V2.Domain.Enums;

public enum InvoiceStatus
{
    Offen      = 0,   // erfasst, noch nicht gebucht
    Gebucht    = 1,   // finalisiert, LS als Fakturiert markiert
    Storniert  = 2,   // storniert (Gutschrift — zukünftig)
}