namespace OrderScheduleService.Domain.ValueObjects;

using OrderScheduleService.Domain.Common;

public class TimeRange : ValueObject
{
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }

    public TimeRange(TimeSpan startTime, TimeSpan endTime)
    {
        if (endTime <= startTime)
            throw new ArgumentException("End time must be after start time");
        
        StartTime = startTime;
        EndTime = endTime;
    }

    public bool IsInRange(TimeSpan time) => time >= StartTime && time <= EndTime;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return StartTime;
        yield return EndTime;
    }
}
