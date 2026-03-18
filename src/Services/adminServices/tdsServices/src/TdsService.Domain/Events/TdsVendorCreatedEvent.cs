using TdsService.Domain.Common;

namespace TdsService.Domain.Events;

public sealed record TdsVendorCreatedEvent(
    long VendorId,
    string VendorName,
    string? PanNo) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}
