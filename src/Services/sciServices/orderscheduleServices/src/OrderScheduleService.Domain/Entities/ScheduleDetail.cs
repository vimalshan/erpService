namespace OrderScheduleService.Domain.Entities;

using OrderScheduleService.Domain.Common;

public class ScheduleDetail : Entity
{
    public long ScheduleId { get; set; }
    public DateTime? FillingDate { get; set; }
    public char? FillingShift { get; set; }
    public string? StartTime { get; set; }
    public string? EndTime { get; set; }
    public decimal? FillQuantity { get; set; }
    public decimal? FillingPointGroupId { get; set; }
    public long? ReferenceScheduleId { get; set; }

    public ScheduleDetail() { }

    public ScheduleDetail(
        long scheduleId,
        DateTime fillingDate,
        char fillingShift,
        string startTime,
        string endTime,
        decimal fillQuantity,
        decimal fillingPointGroupId)
    {
        ScheduleId = scheduleId;
        FillingDate = fillingDate;
        FillingShift = fillingShift;
        StartTime = startTime;
        EndTime = endTime;
        FillQuantity = fillQuantity;
        FillingPointGroupId = fillingPointGroupId;
    }

    public bool IsForShift(char shift) => FillingShift == shift;
    
    public TimeSpan? GetDuration()
    {
        if (TimeSpan.TryParse(StartTime, out var start) && 
            TimeSpan.TryParse(EndTime, out var end))
        {
            return end - start;
        }
        return null;
    }
}
