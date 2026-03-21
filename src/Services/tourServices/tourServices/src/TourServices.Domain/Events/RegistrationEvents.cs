using TourServices.Domain.Common;

namespace TourServices.Domain.Events;

public sealed record ParticipantRegisteredEvent(
    Guid EventId,
    DateTime OccurredOn,
    long RegistrationId,
    long TourId,
    long ParticipantId,
    DateOnly RegistrationDate) : IDomainEvent;

public sealed record RegistrationCancelledEvent(
    Guid EventId,
    DateTime OccurredOn,
    long RegistrationId,
    long TourId,
    long ParticipantId,
    long CancelledBy) : IDomainEvent;
