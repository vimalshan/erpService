namespace UserSecurityService.Domain.Common;

public abstract record ValueObject;

public sealed record EmployeePin
{
    public decimal Value { get; }

    public EmployeePin(decimal value)
    {
        if (value <= 0)
            throw new ArgumentException("Employee pin must be positive.", nameof(value));
        Value = value;
    }

    public static implicit operator decimal(EmployeePin pin) => pin.Value;
    public static implicit operator EmployeePin(decimal value) => new(value);
    public override string ToString() => Value.ToString();
}

public sealed record UserId
{
    public string Value { get; }

    public UserId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("UserId cannot be empty.", nameof(value));
        if (value.Length > 25)
            throw new ArgumentException("UserId cannot exceed 25 characters.", nameof(value));
        Value = value;
    }

    public static implicit operator string(UserId id) => id.Value;
    public static implicit operator UserId(string value) => new(value);
    public override string ToString() => Value;
}
