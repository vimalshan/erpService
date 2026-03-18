namespace LovService.Domain.ValueObjects;

public sealed class Price : IEquatable<Price>
{
    public int Value { get; }

    private Price(int value) => Value = value;

    public static Price Create(int value)
    {
        if (value < 0)
            throw new ArgumentException("Price cannot be negative.", nameof(value));
        return new Price(value);
    }

    public bool Equals(Price? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is Price other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString();

    public static implicit operator int(Price price) => price.Value;
}
