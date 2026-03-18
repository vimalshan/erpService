using ItemMasterService.Domain.Common;

namespace ItemMasterService.Domain.Events;

public sealed record CanteenItemCreatedEvent(
    long CanteenUnitCode,
    long ItemCode,
    string? ItemDescription) : IDomainEvent;

public sealed record CanteenItemUpdatedEvent(
    long CanteenUnitCode,
    long ItemCode) : IDomainEvent;

public sealed record CanteenItemDeletedEvent(
    long CanteenUnitCode,
    long ItemCode) : IDomainEvent;

public sealed record CanteenItemPriceUpdatedEvent(
    long CanteenUnitCode,
    long ItemCode,
    decimal? EmployeeContribution,
    decimal? EmployerContribution,
    DateTime EffectiveDate) : IDomainEvent;
