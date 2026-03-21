namespace TourServices.Domain.Exceptions;

public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message) { }
}

public sealed class TourNotFoundException : DomainException
{
    public TourNotFoundException(long tourId)
        : base($"Tour package with ID {tourId} was not found.") { }
}

public sealed class TourFullyBookedException : DomainException
{
    public TourFullyBookedException(long tourId)
        : base($"Tour package {tourId} is fully booked.") { }
}

public sealed class TourNotActiveException : DomainException
{
    public TourNotActiveException(long tourId, string status)
        : base($"Tour package {tourId} cannot accept registrations in status '{status}'.") { }
}
