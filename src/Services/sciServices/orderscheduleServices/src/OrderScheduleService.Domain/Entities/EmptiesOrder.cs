namespace OrderScheduleService.Domain.Entities;

using OrderScheduleService.Domain.Common;

public class EmptiesOrder : Entity
{
    public decimal? SciItemId { get; set; }
    public decimal? ItemId { get; set; }
    public decimal? OrderQuantity { get; set; }
    public DateTime? NeedDate { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? EntryDate { get; set; }

    public EmptiesOrder() { }

    public EmptiesOrder(
        decimal? sciItemId,
        decimal? itemId,
        decimal? orderQuantity,
        DateTime? needDate,
        DateTime? orderDate,
        DateTime? entryDate)
    {
        SciItemId = sciItemId;
        ItemId = itemId;
        OrderQuantity = orderQuantity;
        NeedDate = needDate;
        OrderDate = orderDate;
        EntryDate = entryDate;
    }
}
