namespace CalendarService.Domain.ValueObjects;

public sealed record ShiftTime
{
    public TimeOnly InTime { get; }
    public TimeOnly OutTime { get; }
    public decimal Duration { get; }

    public ShiftTime(TimeOnly inTime, TimeOnly outTime)
    {
        if (outTime <= inTime)
            throw new ArgumentException("OutTime must be later than InTime");

        InTime = inTime;
        OutTime = outTime;
        Duration = Math.Round((decimal)outTime.ToTimeSpan().Subtract(inTime.ToTimeSpan()).TotalHours, 2);
    }
}
