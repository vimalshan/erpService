using VendorService.Domain.Common;

namespace VendorService.Domain.Events;

public sealed record VendorStatusChangedEvent(
    long VendorId,
    char NewStatus,
    DateTime OccurredOn) : IDomainEvent
{
    public VendorStatusChangedEvent(long vendorId, char newStatus)
        : this(vendorId, newStatus, DateTime.UtcNow) { }
}
