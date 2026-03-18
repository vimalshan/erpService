using CanteenUnit.Domain.Common;

namespace CanteenUnit.Domain.Events;

public sealed record CanteenAccessGrantedEvent(
    long CompanyCode,
    long UserId,
    long AccessNumber,
    DateTime OccurredOn) : IDomainEvent;
