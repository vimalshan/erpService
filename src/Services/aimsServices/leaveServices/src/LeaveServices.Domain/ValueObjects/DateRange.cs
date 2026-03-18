using LeaveServices.Domain.Common;

namespace LeaveServices.Domain.ValueObjects;

public sealed class DateRange : ValueObject
{
    public DateTime From { get; }
    public DateTime To   { get; }

    public DateRange(DateTime from, DateTime to)
    {
        if (from > to)
            throw new ArgumentException("From date cannot be after To date.");
        From = from;
        To   = to;
    }

    public int CalendarDays => (To - From).Days + 1;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return From;
        yield return To;
    }
}
