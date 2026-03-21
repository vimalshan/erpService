using TravelService.Domain.Common;

namespace TravelService.Domain.ValueObjects;

public sealed class DateRange : ValueObject
{
    public DateTime StartDate { get; }
    public DateTime? EndDate { get; }

    public DateRange(DateTime startDate, DateTime? endDate = null)
    {
        if (endDate.HasValue && endDate < startDate)
            throw new ArgumentException("End date must be after start date.");
        StartDate = startDate;
        EndDate = endDate;
    }

    public int Days => EndDate.HasValue ? (int)(EndDate.Value - StartDate).TotalDays + 1 : 1;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return StartDate;
        yield return EndDate;
    }
}
