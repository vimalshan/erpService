namespace EmployeeService.Domain.ValueObjects;

/// <summary>Represents an approver level (1 = primary, 2 = secondary, etc.).</summary>
public sealed class ApproverLevel : IEquatable<ApproverLevel>
{
    public int Value { get; }

    private ApproverLevel(int value)
    {
        if (value < 1 || value > 10)
            throw new ArgumentOutOfRangeException(nameof(value), "Approver level must be between 1 and 10.");
        Value = value;
    }

    public static ApproverLevel Of(int value) => new(value);

    public bool Equals(ApproverLevel? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is ApproverLevel al && Equals(al);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString();
}
