namespace SwipeTransactionService.Domain.ValueObjects;

public sealed record EmployeeId
{
    public string Value { get; }

    public EmployeeId(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Employee ID cannot be empty.", nameof(value));
        if (value.Length > 20) throw new ArgumentException("Employee ID must not exceed 20 characters.", nameof(value));
        Value = value.Trim();
    }

    public static implicit operator string(EmployeeId id) => id.Value;
    public static implicit operator EmployeeId(string value) => new(value);
    public override string ToString() => Value;
}
