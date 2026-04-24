using DNR26V2.Domain.DTOs;
using DNR26V2.Domain.DTOs.Payments;

namespace DNR26V2.Services.Payments;

public interface ICustomerLedgerService
{
    Task<IReadOnlyList<CustomerLedgerEntryDto>> GetLedgerAsync(int kundeId, DateTime von, DateTime bis);

    /// <summary>Calculates the current customer balance (Saldo) from the ledger entries.</summary>
    Task<decimal> GetSaldoAsync(int kundeId, DateTime von, DateTime bis);

    /// <summary>Returns already-posted payments for the selected customer and period.</summary>
    Task<IReadOnlyList<PaymentHistoryDto>> GetPaymentHistoryAsync(int kundeId, DateTime von, DateTime bis);
}
