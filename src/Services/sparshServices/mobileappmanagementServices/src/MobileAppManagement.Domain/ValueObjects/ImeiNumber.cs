using System.Text.RegularExpressions;

namespace MobileAppManagement.Domain.ValueObjects;

public partial record ImeiNumber
{
    public string Value { get; }

    public ImeiNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("IMEI number cannot be empty.", nameof(value));
        if (value.Length > 200)
            throw new ArgumentException("IMEI number cannot exceed 200 characters.", nameof(value));
        if (!ImeiRegex().IsMatch(value))
            throw new ArgumentException("IMEI number must contain only digits.", nameof(value));
        Value = value;
    }

    public static implicit operator string(ImeiNumber imei) => imei.Value;
    public static explicit operator ImeiNumber(string value) => new(value);

    [GeneratedRegex(@"^\d+$")]
    private static partial Regex ImeiRegex();
}
