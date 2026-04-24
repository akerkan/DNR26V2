using DNR26V2.Domain.DTOs.Payments;

namespace DNR26V2.Services.Payments;

public interface IPaymentPostingService
{
    /// <summary>
    /// Validates and posts a payment. Returns the Id of the created PaymentHeader.
    /// </summary>
    Task<int> BuchenAsync(PaymentPostingRequest request);

    /// <summary>
    /// Reverses a single PaymentLine by creating a new PaymentHeader with a negative
    /// counter-line and a storno note. Returns the new PaymentHeader Id.
    /// </summary>
    Task<int> StornierenAsync(int paymentLineId, string stornoNotiz);
}
