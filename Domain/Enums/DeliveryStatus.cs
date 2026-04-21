namespace DNR26V2.Domain.Enums;

public enum DeliveryStatus
{
    Offen         = 0,   // Displayed as "Aktiv" in UI
    TeilStorniert = 4,   // New — some lines storniert
    Fakturiert    = 2,
    Storniert     = 3,
}