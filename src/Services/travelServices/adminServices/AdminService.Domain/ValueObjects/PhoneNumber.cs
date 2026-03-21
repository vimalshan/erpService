namespace AdminService.Domain.ValueObjects;

/// <summary>
/// Represents a phone number value object
/// </summary>
public record PhoneNumber
{
    /// <summary>
    /// Phone number value
    /// </summary>
    public string Value { get; init; }

    public PhoneNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Phone number cannot be empty", nameof(value));

        // Basic phone validation
        var cleanedNumber = new string(value.Where(char.IsDigit).ToArray());
        if (cleanedNumber.Length < 10)
            throw new ArgumentException("Phone number must have at least 10 digits", nameof(value));

        Value = value;
    }

    public override string ToString() => Value;
}
