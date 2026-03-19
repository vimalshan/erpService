namespace OrderScheduleService.Domain.Events;

using OrderScheduleService.Domain.Common;

public record OrderCreatedEvent(
    long OrderId,
    string CustomerCode,
    decimal CompanyUnitId,
    DateTime OrderedDate) : DomainEvent;

public record OrderDetailAddedEvent(
    long OrderId,
    long DetailId,
    decimal ItemId,
    long OrderQty) : DomainEvent;

public record OrderScheduledEvent(
    long OrderId,
    long DetailId,
    DateTime ScheduledDate,
    long AllocatedQuantity) : DomainEvent;

public record OrderCancelledEvent(
    long OrderId,
    long DetailId,
    string Reason) : DomainEvent;

public record OrderFulfilledEvent(
    long OrderId,
    long DetailId,
    long FulfilledQuantity) : DomainEvent;

public record ScheduleConfirmedEvent(
    DateTime ScheduleDate,
    string Status) : DomainEvent;

public record CapacityChangedEvent(
    decimal FillingLineId,
    decimal FillingGroupId,
    DateTime ChangeDate) : DomainEvent;
