using VendorService.Domain.Common;

namespace VendorService.Domain.Events;

public sealed record VendorCreatedEvent(
    long VendorId,
    string VendorName,
    string Address,
    long LocationId,
    long CategoryId,
    DateTime OccurredOn) : IDomainEvent
{
    public VendorCreatedEvent(long vendorId, string vendorName, string address, long locationId, long categoryId)
        : this(vendorId, vendorName, address, locationId, categoryId, DateTime.UtcNow) { }
}
