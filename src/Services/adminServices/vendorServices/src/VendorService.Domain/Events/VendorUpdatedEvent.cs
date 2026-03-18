using VendorService.Domain.Common;

namespace VendorService.Domain.Events;

public sealed record VendorUpdatedEvent(
    long VendorId,
    string VendorName,
    DateTime OccurredOn) : IDomainEvent
{
    public VendorUpdatedEvent(long vendorId, string vendorName)
        : this(vendorId, vendorName, DateTime.UtcNow) { }
}
