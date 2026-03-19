namespace OrderScheduleService.Domain.Entities;

using OrderScheduleService.Domain.Common;

public class CapacityChange : Entity
{
    public int? FillingLineId { get; set; }
    public int? FillingGroupId { get; set; }
    public DateTime ChangeDate { get; set; }
    public string RerunStatus { get; set; } = null!;
    public DateTime? RerunDate { get; set; }

    public CapacityChange() { }

    public CapacityChange(
        int? fillingLineId,
        int? fillingGroupId,
        DateTime changeDate,
        string rerunStatus,
        DateTime? rerunDate = null)
    {
        FillingLineId = fillingLineId;
        FillingGroupId = fillingGroupId;
        ChangeDate = changeDate;
        RerunStatus = rerunStatus;
        RerunDate = rerunDate;
    }
}
