namespace BookingService.Domain.Exceptions;

public sealed class BookingDomainException : Exception
{
    public BookingDomainException(string message) : base(message) { }
    public BookingDomainException(string message, Exception inner) : base(message, inner) { }
}
