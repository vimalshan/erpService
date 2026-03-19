namespace EmployeePrideManagement.Domain.ValueObjects;

public record ImagePath
{
    public string Value { get; }

    public ImagePath(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Image path cannot be empty.", nameof(value));

        if (value.Length > 200)
            throw new ArgumentException("Image path cannot exceed 200 characters.", nameof(value));

        Value = value;
    }

    public override string ToString() => Value;
}
