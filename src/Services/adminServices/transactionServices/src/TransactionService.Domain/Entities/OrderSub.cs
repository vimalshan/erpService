namespace TransactionService.Domain.Entities;

using TransactionService.Domain.Common;

public sealed class OrderSub : Entity
{
    public long OrderSubId { get; private set; }
    public long OrderMainId { get; private set; }
    public long RequestSubId { get; private set; }
    public long OrderedQty { get; private set; }
    public DateTime? ReceivedOn { get; private set; }
    public long ReceivedBy { get; private set; }
    public long OrderPrice { get; private set; }
    public long ActualPrice { get; private set; }
    public DateTime ReceivedDate { get; private set; }
    public DateTime DeliveryDate { get; private set; }
    public long? ReceiptEntryBy { get; private set; }
    public DateTime? ReceiptEntryOn { get; private set; }

    public OrderMain? OrderMain { get; private set; }

    private OrderSub() { }

    public static OrderSub Create(
        long orderSubId, long orderMainId, long requestSubId,
        long orderedQty, long orderPrice, DateTime deliveryDate)
    {
        return new OrderSub
        {
            OrderSubId = orderSubId,
            OrderMainId = orderMainId,
            RequestSubId = requestSubId,
            OrderedQty = orderedQty,
            OrderPrice = orderPrice,
            ActualPrice = orderPrice,
            DeliveryDate = deliveryDate,
            ReceivedDate = deliveryDate
        };
    }

    public void MarkReceived(long receivedQty, long receivedBy)
    {
        ReceivedOn = DateTime.UtcNow;
        ReceivedBy = receivedBy;
        ReceiptEntryBy = receivedBy;
        ReceiptEntryOn = DateTime.UtcNow;
        ReceivedDate = DateTime.UtcNow;
    }

    public void UpdateActualPrice(long actualPrice)
    {
        ActualPrice = actualPrice;
    }
}
