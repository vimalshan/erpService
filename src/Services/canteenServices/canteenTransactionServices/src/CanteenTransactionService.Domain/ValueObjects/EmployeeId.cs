namespace CanteenTransactionService.Domain.ValueObjects;

public sealed class EmployeeId : IEquatable<EmployeeId>
{
    public long Value { get; }

    private EmployeeId(long value) => Value = value;

    public static EmployeeId Create(long value)
    {
        if (value <= 0) throw new ArgumentException("Employee ID must be positive.", nameof(value));
        return new EmployeeId(value);
    }

    public bool Equals(EmployeeId? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is EmployeeId id && Equals(id);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString();
    public static implicit operator long(EmployeeId id) => id.Value;
}
