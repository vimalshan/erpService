using EnergyService.Domain.Exceptions;

namespace EnergyService.Domain.ValueObjects;

public sealed class DeliveryType : IEquatable<DeliveryType>
{
    public static readonly DeliveryType To = new("TO");
    public static readonly DeliveryType Cc = new("CC");
    public static readonly DeliveryType Bcc = new("BCC");

    public string Value { get; }

    public DeliveryType(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 3)
            throw new DomainException("DeliveryType must be a non-empty string of max 3 characters.");
        Value = value.ToUpperInvariant();
    }

    public bool Equals(DeliveryType? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is DeliveryType other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;

    public static implicit operator string(DeliveryType type) => type.Value;
    public static implicit operator DeliveryType(string type) => new(type);
}
