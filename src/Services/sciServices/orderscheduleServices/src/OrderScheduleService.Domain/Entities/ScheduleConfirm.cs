namespace OrderScheduleService.Domain.Entities;

public class ScheduleConfirm
{
    public DateTime ScheduleDate { get; set; }
    public string ScheduleStatus { get; set; } = null!;
    public DateTime ModifiedDate { get; set; }

    public ScheduleConfirm() { }

    public ScheduleConfirm(DateTime scheduleDate, string scheduleStatus, DateTime modifiedDate)
    {
        ScheduleDate = scheduleDate;
        ScheduleStatus = scheduleStatus;
        ModifiedDate = modifiedDate;
    }
}
