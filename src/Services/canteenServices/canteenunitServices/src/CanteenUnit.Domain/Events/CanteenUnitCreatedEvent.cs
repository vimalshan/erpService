using CanteenUnit.Domain.Common;

namespace CanteenUnit.Domain.Events;

public sealed record CanteenUnitCreatedEvent(
    decimal CompanyCode,
    string UnitName,
    DateTime OccurredOn) : IDomainEvent;
