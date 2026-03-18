using CanteenUnit.Domain.Common;

namespace CanteenUnit.Domain.Events;

public sealed record CanteenUnitUpdatedEvent(
    decimal CompanyCode,
    string? OldUnitName,
    string? NewUnitName,
    DateTime OccurredOn) : IDomainEvent;
