namespace OrderScheduleService.Domain.Entities;

using OrderScheduleService.Domain.Common;

public class OrderDetail : Entity
{
    public long TiedOrderId { get; set; }
    public decimal ItemId { get; set; }
    public string? ItemName { get; set; }
    public long OrderQuantity { get; set; }
    public DateTime? DispatchDate { get; set; }
    public long? QuantityFromCurrentStock { get; set; }
    public long? FillingAllotted { get; set; }
    public string? CancelFlag { get; set; }
    public DateTime? CancelDate { get; set; }
    public int? CancelUserId { get; set; }
    public int? ModifiedUserId { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public decimal? Price { get; set; }
    public string? ScheduleFlag { get; set; }
    public string? IgnoreEmptiesCheck { get; set; }
    public string? IgnoreCurrentStock { get; set; }

    public OrderDetail() { }

    public OrderDetail(
        long tiedOrderId,
        decimal itemId,
        string itemName,
        long orderQuantity,
        DateTime? dispatchDate = null,
        decimal? price = null)
    {
        TiedOrderId = tiedOrderId;
        ItemId = itemId;
        ItemName = itemName;
        OrderQuantity = orderQuantity;
        DispatchDate = dispatchDate;
        Price = price;
        CancelFlag = "N";
        ScheduleFlag = "N";
        ModifiedDate = DateTime.UtcNow;
    }

    public void Cancel(int userId, string cancelFlag = "Y")
    {
        CancelFlag = cancelFlag;
        CancelDate = DateTime.UtcNow;
        CancelUserId = userId;
        ModifiedDate = DateTime.UtcNow;
    }

    public void AllocateFilling(long allocatedQty, int userId)
    {
        FillingAllotted = allocatedQty;
        ModifiedUserId = userId;
        ModifiedDate = DateTime.UtcNow;
    }
}
