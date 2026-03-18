namespace AuditService.Domain.ValueObjects;

/// <summary>
/// Value object representing a date range used in audit planning.
/// </summary>
public sealed class DateRange : IEquatable<DateRange>
{
    public DateTime From { get; }
    public DateTime To { get; }
    public int DurationDays => (To - From).Days;

    private DateRange(DateTime from, DateTime to)
    {
        From = from;
        To = to;
    }

    public static DateRange Create(DateTime from, DateTime to)
    {
        if (from >= to) throw new ArgumentException("From date must be before To date.");
        return new DateRange(from, to);
    }

    public bool Overlaps(DateRange other) => From < other.To && To > other.From;

    public bool Equals(DateRange? other) => other is not null && From == other.From && To == other.To;
    public override bool Equals(object? obj) => obj is DateRange other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(From, To);
    public override string ToString() => $"{From:yyyy-MM-dd} to {To:yyyy-MM-dd}";
}
