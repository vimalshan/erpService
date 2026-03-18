namespace BookingService.Domain.ValueObjects;

public sealed class AttendanceStatus
{
    public static readonly AttendanceStatus Registered = new("REGISTERED");
    public static readonly AttendanceStatus Attended = new("ATTENDED");
    public static readonly AttendanceStatus Cancelled = new("CANCELLED");

    private static readonly Dictionary<string, AttendanceStatus> _all = new(StringComparer.OrdinalIgnoreCase)
    {
        [Registered.Value] = Registered,
        [Attended.Value] = Attended,
        [Cancelled.Value] = Cancelled,
    };

    public string Value { get; }
    private AttendanceStatus(string value) => Value = value;

    public static AttendanceStatus From(string value)
    {
        if (!_all.TryGetValue(value, out var status))
            throw new ArgumentException($"'{value}' is not a valid AttendanceStatus.");
        return status;
    }

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is AttendanceStatus s && Value == s.Value;
    public override int GetHashCode() => Value.GetHashCode(StringComparison.OrdinalIgnoreCase);
    public static bool operator ==(AttendanceStatus? a, AttendanceStatus? b) => a is null ? b is null : a.Equals(b);
    public static bool operator !=(AttendanceStatus? a, AttendanceStatus? b) => !(a == b);
}
