using OrderService.Domain.Common;
using OrderService.Domain.Enums;

namespace OrderService.Domain.Events;

public sealed record OrderStatusChangedEvent(int OrderId, OrderStatus PreviousStatus, OrderStatus NewStatus) : IDomainEvent;
