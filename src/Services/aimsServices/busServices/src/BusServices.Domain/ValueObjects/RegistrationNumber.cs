using BusServices.Domain.Common;

namespace BusServices.Domain.ValueObjects;

public sealed class RegistrationNumber : ValueObject
{
    public string Value { get; }

    private RegistrationNumber(string value) => Value = value;

    public static RegistrationNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Registration number cannot be empty.");
        if (value.Length > 50)
            throw new ArgumentException("Registration number cannot exceed 50 characters.");
        return new RegistrationNumber(value.Trim().ToUpperInvariant());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
