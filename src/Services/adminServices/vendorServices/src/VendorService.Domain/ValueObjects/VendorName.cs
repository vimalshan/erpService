namespace VendorService.Domain.ValueObjects;

public sealed class VendorName : IEquatable<VendorName>
{
    public string Value { get; }

    public VendorName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Vendor name cannot be empty.", nameof(value));
        if (value.Length > 100)
            throw new ArgumentException("Vendor name cannot exceed 100 characters.", nameof(value));
        Value = value.Trim();
    }

    public bool Equals(VendorName? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is VendorName vn && Equals(vn);
    public override int GetHashCode() => Value.GetHashCode(StringComparison.OrdinalIgnoreCase);
    public override string ToString() => Value;

    public static implicit operator string(VendorName name) => name.Value;
}
