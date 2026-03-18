namespace LeaveServices.Domain.ValueObjects;

public sealed class LeaveType : IEquatable<LeaveType>
{
    public static readonly LeaveType Annual = new("ANNUAL");
    public static readonly LeaveType Sick = new("SICK");
    public static readonly LeaveType Casual = new("CASUAL");
    public static readonly LeaveType Maternity = new("MATERNITY");
    public static readonly LeaveType Paternity = new("PATERNITY");
    public static readonly LeaveType LossOfPay = new("LOP");

    private static readonly IReadOnlyList<LeaveType> _all = [Annual, Sick, Casual, Maternity, Paternity, LossOfPay];

    public string Value { get; }

    private LeaveType(string value) => Value = value;

    public static LeaveType From(string value)
    {
        var match = _all.FirstOrDefault(t => t.Value.Equals(value, StringComparison.OrdinalIgnoreCase));
        if (match is null)
            throw new ArgumentException($"'{value}' is not a valid leave type.", nameof(value));
        return match;
    }

    public override string ToString() => Value;
    public bool Equals(LeaveType? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is LeaveType lt && Equals(lt);
    public override int GetHashCode() => Value.GetHashCode(StringComparison.Ordinal);
}
