namespace LoanManagement.Domain.ValueObjects;

public sealed class LoanKey : IEquatable<LoanKey>
{
    public string Value { get; }

    private LoanKey(string value) => Value = value;

    public static LoanKey Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("LoanKey cannot be empty.", nameof(value));

        if (value.Length > 15)
            throw new ArgumentException("LoanKey cannot exceed 15 characters.", nameof(value));

        return new LoanKey(value.ToUpperInvariant());
    }

    public bool Equals(LoanKey? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is LoanKey lk && Equals(lk);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;

    public static implicit operator string(LoanKey key) => key.Value;
}
