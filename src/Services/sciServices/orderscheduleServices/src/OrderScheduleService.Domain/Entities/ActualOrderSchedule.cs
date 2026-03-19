namespace OrderScheduleService.Domain.Entities;

public class ActualOrderSchedule
{
    public decimal CtOrderId { get; set; }
    public decimal LineId { get; set; }
    public decimal? OrderedItemId { get; set; }
    public DateTime? NewScheduleDate { get; set; }
    public int? NewFillQuantity { get; set; }
    public int? FillingAllotted { get; set; }
    public int? SciUserIdModified { get; set; }
    public DateTime? ModifiedDate { get; set; }

    public ActualOrderSchedule() { }

    public ActualOrderSchedule(
        decimal ctOrderId,
        decimal lineId,
        decimal? orderedItemId,
        DateTime? newScheduleDate,
        int? newFillQuantity,
        int? fillingAllotted)
    {
        CtOrderId = ctOrderId;
        LineId = lineId;
        OrderedItemId = orderedItemId;
        NewScheduleDate = newScheduleDate;
        NewFillQuantity = newFillQuantity;
        FillingAllotted = fillingAllotted;
    }
}
