using TourServices.Domain.Common;

namespace TourServices.Domain.Events;

public sealed record TourPackagePlannedEvent(
    Guid EventId,
    DateTime OccurredOn,
    long TourId,
    string TourName,
    string Destination,
    DateOnly StartDate,
    DateOnly EndDate,
    decimal TotalCost,
    int MaxParticipants,
    long PlannedBy) : IDomainEvent;
