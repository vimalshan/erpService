namespace ReceivingService.Domain.Exceptions;

public sealed class ReceivingDomainException : Exception
{
    public ReceivingDomainException(string message) : base(message) { }
    public ReceivingDomainException(string message, Exception innerException)
        : base(message, innerException) { }
}
