namespace DNR26V2.Domain.Exceptions;

/// <summary>Wird bei Verstößen gegen Geschäftsregeln geworfen (z.B. Pflichtfeld fehlt).</summary>
public sealed class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
}