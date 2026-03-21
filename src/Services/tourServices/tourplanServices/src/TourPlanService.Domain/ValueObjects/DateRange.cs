using TourPlanService.Domain.Common;

namespace TourPlanService.Domain.ValueObjects;

public sealed class DateRange : ValueObject
{
    public DateTime StartDate { get; }
    public DateTime? EndDate { get; }

    private DateRange(DateTime startDate, DateTime? endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }

    public static DateRange Create(DateTime startDate, DateTime? endDate = null)
    {
        if (endDate.HasValue && endDate < startDate)
            throw new ArgumentException("End date cannot be before start date.");
        return new DateRange(startDate, endDate);
    }

    public int? DaysCount =>
        EndDate.HasValue ? (int)(EndDate.Value - StartDate).TotalDays + 1 : null;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return StartDate;
        yield return EndDate;
    }
}
