namespace BookingService.Domain.ValueObjects;

public sealed class RecordStatus
{
    public static readonly RecordStatus Active = new("ACTIVE");
    public static readonly RecordStatus Inactive = new("INACTIVE");

    private static readonly Dictionary<string, RecordStatus> _all = new(StringComparer.OrdinalIgnoreCase)
    {
        [Active.Value] = Active,
        [Inactive.Value] = Inactive,
    };

    public string Value { get; }
    private RecordStatus(string value) => Value = value;

    public static RecordStatus From(string value)
    {
        if (!_all.TryGetValue(value, out var status))
            throw new ArgumentException($"'{value}' is not a valid RecordStatus.");
        return status;
    }

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is RecordStatus s && Value == s.Value;
    public override int GetHashCode() => Value.GetHashCode(StringComparison.OrdinalIgnoreCase);
}
