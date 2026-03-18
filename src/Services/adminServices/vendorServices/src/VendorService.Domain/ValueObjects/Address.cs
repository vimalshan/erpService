namespace VendorService.Domain.ValueObjects;

public sealed class Address : IEquatable<Address>
{
    public string Value { get; }

    public Address(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Address cannot be empty.", nameof(value));
        if (value.Length > 200)
            throw new ArgumentException("Address cannot exceed 200 characters.", nameof(value));
        Value = value.Trim();
    }

    public bool Equals(Address? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is Address a && Equals(a);
    public override int GetHashCode() => Value.GetHashCode(StringComparison.OrdinalIgnoreCase);
    public override string ToString() => Value;

    public static implicit operator string(Address address) => address.Value;
}
