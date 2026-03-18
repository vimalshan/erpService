using EmployeeService.Domain.Common;

namespace EmployeeService.Domain.ValueObjects;

/// <summary>Strong-typed Employee system identifier.</summary>
public sealed class EmployeeId : IEquatable<EmployeeId>
{
    public long Value { get; }

    private EmployeeId(long value)
    {
        if (value <= 0) throw new ArgumentException("Employee ID must be positive.", nameof(value));
        Value = value;
    }

    public static EmployeeId Of(long value) => new(value);

    public bool Equals(EmployeeId? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is EmployeeId eid && Equals(eid);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString();

    public static implicit operator long(EmployeeId id) => id.Value;
    public static implicit operator EmployeeId(long value) => new(value);
}
