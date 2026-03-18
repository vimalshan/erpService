namespace UtilityService.Domain.ValueObjects;

public sealed class StatementId : IEquatable<StatementId>
{
    public string Value { get; }

    private StatementId(string value) => Value = value;

    public static StatementId Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("StatementId cannot be null or empty.", nameof(value));
        if (value.Length > 32)
            throw new ArgumentException("StatementId cannot exceed 32 characters.", nameof(value));

        return new StatementId(value.Trim());
    }

    public bool Equals(StatementId? other) =>
        other is not null && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => obj is StatementId sid && Equals(sid);
    public override int GetHashCode() => Value.GetHashCode(StringComparison.OrdinalIgnoreCase);
    public override string ToString() => Value;

    public static implicit operator string(StatementId sid) => sid.Value;
}
