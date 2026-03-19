using System.Text.RegularExpressions;

namespace MobileAppManagement.Domain.ValueObjects;

public partial record MobileNumber
{
    public string Value { get; }

    public MobileNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Mobile number cannot be empty.", nameof(value));
        if (value.Length > 255)
            throw new ArgumentException("Mobile number cannot exceed 255 characters.", nameof(value));
        if (!MobileRegex().IsMatch(value))
            throw new ArgumentException("Mobile number format is invalid.", nameof(value));
        Value = value;
    }

    public static implicit operator string(MobileNumber mobile) => mobile.Value;
    public static explicit operator MobileNumber(string value) => new(value);

    [GeneratedRegex(@"^[\d\+\-\s]+$")]
    private static partial Regex MobileRegex();
}
