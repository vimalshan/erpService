using BookingService.Domain.Common;

namespace BookingService.Domain.ValueObjects;

public sealed class DateRange : ValueObject
{
    public DateTime From { get; }
    public DateTime To { get; }

    private DateRange() { }
    private DateRange(DateTime from, DateTime to)
    {
        From = from;
        To = to;
    }

    public static DateRange Create(DateTime from, DateTime to)
    {
        if (to < from)
            throw new ArgumentException("End date must be on or after start date.");
        return new DateRange(from, to);
    }

    public int DurationInDays => (int)(To - From).TotalDays;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return From;
        yield return To;
    }

    public override string ToString() => $"{From:dd-MMM-yyyy} to {To:dd-MMM-yyyy}";
}
