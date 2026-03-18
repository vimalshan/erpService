using Stationery.Domain.Common;
using Stationery.Domain.Entities;

namespace Stationery.Domain.Events;

public class OrderCreatedEvent : DomainEvent
{
    public long OrderId { get; init; }
    public long VendorId { get; init; }
    public long LocationId { get; init; }
    public int ItemCount { get; init; }

    private OrderCreatedEvent() { }

    public OrderCreatedEvent(OrderMain order)
    {
        OrderId = order.Id;
        VendorId = order.VendorId;
        LocationId = order.LocationId;
        ItemCount = order.Details.Count;
    }
}
