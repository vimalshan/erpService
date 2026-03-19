namespace SecurityService.Domain.ValueObjects;

public sealed class PhoneNumber : IEquatable<PhoneNumber>
{
    public long Value { get; }

    private PhoneNumber(long value) => Value = value;

    public static PhoneNumber? Create(long? value)
    {
        if (value is null) return null;
        if (value <= 0) throw new ArgumentException("Phone number must be positive.");
        return new PhoneNumber(value.Value);
    }

    public bool Equals(PhoneNumber? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is PhoneNumber other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString();
}
