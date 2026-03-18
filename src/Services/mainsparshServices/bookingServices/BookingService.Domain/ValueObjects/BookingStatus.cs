namespace BookingService.Domain.ValueObjects;

public sealed class BookingStatus
{
    public static readonly BookingStatus Draft = new("DRAFT");
    public static readonly BookingStatus Submitted = new("SUBMITTED");
    public static readonly BookingStatus Approved = new("APPROVED");
    public static readonly BookingStatus Rejected = new("REJECTED");
    public static readonly BookingStatus Cancelled = new("CANCELLED");

    private static readonly Dictionary<string, BookingStatus> _all = new(StringComparer.OrdinalIgnoreCase)
    {
        [Draft.Value] = Draft,
        [Submitted.Value] = Submitted,
        [Approved.Value] = Approved,
        [Rejected.Value] = Rejected,
        [Cancelled.Value] = Cancelled,
    };

    public string Value { get; }

    private BookingStatus(string value) => Value = value;

    public static BookingStatus From(string value)
    {
        if (!_all.TryGetValue(value, out var status))
            throw new ArgumentException($"'{value}' is not a valid BookingStatus.");
        return status;
    }

    public bool CanTransitionTo(BookingStatus next)
    {
        return (this, next) switch
        {
            var (c, n) when c == Draft && n == Submitted => true,
            var (c, n) when c == Draft && n == Cancelled => true,
            var (c, n) when c == Submitted && n == Approved => true,
            var (c, n) when c == Submitted && n == Rejected => true,
            var (c, n) when c == Submitted && n == Cancelled => true,
            _ => false
        };
    }

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is BookingStatus s && Value == s.Value;
    public override int GetHashCode() => Value.GetHashCode(StringComparison.OrdinalIgnoreCase);
    public static bool operator ==(BookingStatus? a, BookingStatus? b) => a is null ? b is null : a.Equals(b);
    public static bool operator !=(BookingStatus? a, BookingStatus? b) => !(a == b);
}
