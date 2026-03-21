using OrderService.Domain.Common;

namespace OrderService.Domain.Events;

public sealed record OrderCreatedEvent(Aggregates.Order Order) : IDomainEvent;
