namespace MemberService.Domain.ValueObjects;

public sealed class TrustCode : IEquatable<TrustCode>
{
    public string Value { get; }

    private TrustCode(string value) => Value = value;

    public static TrustCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Trust code cannot be empty.", nameof(value));
        if (value.Length > 3) throw new ArgumentException("Trust code cannot exceed 3 characters.", nameof(value));
        return new TrustCode(value.ToUpperInvariant().PadRight(3));
    }

    public bool Equals(TrustCode? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is TrustCode tc && Equals(tc);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;
}
