using Stationery.Domain.Common;
using Stationery.Domain.Entities;

namespace Stationery.Domain.Events;

public class OrderReceivedEvent : DomainEvent
{
    public long OrderSubId { get; init; }
    public long OrderMainId { get; init; }
    public long ReceivedQty { get; init; }
    public long ReceivedBy { get; init; }

    private OrderReceivedEvent() { }

    public OrderReceivedEvent(OrderSub orderSub)
    {
        OrderSubId = orderSub.Id;
        OrderMainId = orderSub.OrderMainId;
        ReceivedQty = orderSub.OrderedQty;
        ReceivedBy = orderSub.ReceivedBy;
    }
}
