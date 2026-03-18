using OtherService.Domain.Exceptions;

namespace OtherService.Domain.ValueObjects;

/// <summary>
/// Value Object representing a request number. Ensures positive value.
/// </summary>
public sealed class RequestNumber : IEquatable<RequestNumber>
{
    public decimal Value { get; }

    private RequestNumber(decimal value) => Value = value;

    public static RequestNumber Create(decimal value)
    {
        if (value < 0)
            throw new DomainException("Request number cannot be negative.");
        return new RequestNumber(value);
    }

    public bool Equals(RequestNumber? other) =>
        other is not null && Value == other.Value;

    public override bool Equals(object? obj) => obj is RequestNumber rn && Equals(rn);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString();

    public static implicit operator decimal(RequestNumber rn) => rn.Value;
}
