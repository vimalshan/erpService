namespace TransactionService.Domain.Entities;

using TransactionService.Domain.Common;
using TransactionService.Domain.Events;

public sealed class OrderMain : AggregateRoot
{
    private readonly List<OrderSub> _details = [];

    public long OrderMainId { get; private set; }
    public long LocationId { get; private set; }
    public long VendorId { get; private set; }
    public DateTime DeliveryDate { get; private set; }
    public DateTime OrderedDate { get; private set; }
    public long OrderedBy { get; private set; }

    public IReadOnlyCollection<OrderSub> Details => _details.AsReadOnly();

    private OrderMain() { }

    public static OrderMain Create(
        long orderMainId, long locationId, long vendorId,
        DateTime deliveryDate, long orderedBy)
    {
        var order = new OrderMain
        {
            OrderMainId = orderMainId,
            LocationId = locationId,
            VendorId = vendorId,
            DeliveryDate = deliveryDate,
            OrderedDate = DateTime.UtcNow,
            OrderedBy = orderedBy
        };

        order.RaiseDomainEvent(new OrderCreatedEvent(
            order.OrderMainId, order.VendorId, order.LocationId, DateTime.UtcNow));

        return order;
    }

    public void AddDetail(OrderSub detail)
    {
        _details.Add(detail);
    }

    public void ReceiveItem(long orderSubId, long receivedQty, long receivedBy)
    {
        var detail = _details.FirstOrDefault(d => d.OrderSubId == orderSubId)
            ?? throw new InvalidOperationException($"Order sub {orderSubId} not found.");

        detail.MarkReceived(receivedQty, receivedBy);

        RaiseDomainEvent(new OrderReceivedEvent(
            OrderMainId, orderSubId, receivedQty, receivedBy, DateTime.UtcNow));
    }
}
