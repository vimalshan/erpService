namespace VendorService.Domain.Exceptions;

public sealed class VendorDomainException : Exception
{
    public VendorDomainException(string message) : base(message) { }
    public VendorDomainException(string message, Exception inner) : base(message, inner) { }
}
