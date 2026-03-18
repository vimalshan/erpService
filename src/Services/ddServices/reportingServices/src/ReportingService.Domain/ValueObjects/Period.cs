namespace ReportingService.Domain.ValueObjects;

/// <summary>
/// Period value object representing a time period
/// </summary>
public class Period
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public Period(DateTime startDate, DateTime endDate)
    {
        if (endDate <= startDate)
            throw new ArgumentException("End date must be after start date");
        StartDate = startDate;
        EndDate = endDate;
    }

    public int GetDurationInDays() => (int)(EndDate - StartDate).TotalDays;

    public bool IsWithinPeriod(DateTime date) => date >= StartDate && date <= EndDate;

    public override bool Equals(object? obj)
    {
        if (obj is Period other)
            return StartDate == other.StartDate && EndDate == other.EndDate;
        return false;
    }

    public override int GetHashCode() => HashCode.Combine(StartDate, EndDate);
}
