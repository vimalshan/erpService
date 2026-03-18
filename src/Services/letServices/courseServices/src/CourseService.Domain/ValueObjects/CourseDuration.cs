namespace CourseService.Domain.ValueObjects;

/// <summary>
/// Represents the date range and number of days for a course.
/// </summary>
public sealed record CourseDuration
{
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }
    public long NumberOfDays { get; }
    public string? DurationDisplay { get; }

    public CourseDuration(DateTime startDate, DateTime endDate, long numberOfDays, string? durationDisplay = null)
    {
        if (startDate >= endDate) throw new ArgumentException("Start date must be before end date.");
        if (numberOfDays <= 0) throw new ArgumentException("Number of days must be positive.");

        StartDate = startDate;
        EndDate = endDate;
        NumberOfDays = numberOfDays;
        DurationDisplay = durationDisplay;
    }
}
