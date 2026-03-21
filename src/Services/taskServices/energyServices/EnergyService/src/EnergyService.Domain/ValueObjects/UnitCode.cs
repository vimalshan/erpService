using EnergyService.Domain.Exceptions;

namespace EnergyService.Domain.ValueObjects;

public sealed class UnitCode : IEquatable<UnitCode>
{
    public string Value { get; }

    public UnitCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 3)
            throw new DomainException("UnitCode must be a non-empty string of max 3 characters.");
        Value = value.ToUpperInvariant();
    }

    public bool Equals(UnitCode? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is UnitCode other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;

    public static implicit operator string(UnitCode code) => code.Value;
    public static implicit operator UnitCode(string code) => new(code);
}
