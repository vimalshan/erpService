namespace CardManagement.Domain.ValueObjects;

public sealed class CardNumber : IEquatable<CardNumber>
{
    public string Value { get; }

    private CardNumber(string value) => Value = value;

    public static CardNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Card number cannot be empty.", nameof(value));
        if (value.Length > 50)
            throw new ArgumentException("Card number cannot exceed 50 characters.", nameof(value));

        return new CardNumber(value.Trim());
    }

    public bool Equals(CardNumber? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is CardNumber cn && Equals(cn);
    public override int GetHashCode() => Value.GetHashCode(StringComparison.OrdinalIgnoreCase);
    public override string ToString() => Value;

    public static implicit operator string(CardNumber cn) => cn.Value;
}
