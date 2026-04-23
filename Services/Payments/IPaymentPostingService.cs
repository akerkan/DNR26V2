using DNR26V2.Domain.DTOs.Payments;
using DNR26V2.Domain.Entities.Payments;

namespace DNR26V2.Services.Payments;

public interface IPaymentPostingService
{
    /// <summary>
    /// Validate and post a batch of payment rows for one customer.
    /// Returns the created PaymentHeader (with populated Id after save).
    /// </summary>
    Task<PaymentHeader> PostAsync(PaymentPostingRequest request);
}
