using TourServices.Domain.Common;

namespace TourServices.Domain.Events;

public sealed record TourPackageStatusChangedEvent(
    Guid EventId,
    DateTime OccurredOn,
    long TourId,
    string OldStatus,
    string NewStatus,
    long ChangedBy) : IDomainEvent;
