namespace EmployeePrideManagement.Domain.ValueObjects;

public record Location
{
    public string Value { get; }

    public Location(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Location cannot be empty.", nameof(value));

        if (value.Length > 100)
            throw new ArgumentException("Location cannot exceed 100 characters.", nameof(value));

        Value = value;
    }

    public override string ToString() => Value;
}
