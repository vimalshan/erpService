namespace VendorService.Domain.ValueObjects;

public sealed class LiveStatus : IEquatable<LiveStatus>
{
    public static readonly LiveStatus Active = new('A');
    public static readonly LiveStatus Inactive = new('I');

    public char Value { get; }

    public LiveStatus(char value)
    {
        if (value != 'A' && value != 'I')
            throw new ArgumentException("LiveStatus must be 'A' (Active) or 'I' (Inactive).", nameof(value));
        Value = char.ToUpperInvariant(value);
    }

    public bool IsActive => Value == 'A';

    public bool Equals(LiveStatus? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is LiveStatus ls && Equals(ls);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString();

    public static implicit operator char(LiveStatus status) => status.Value;
    public static implicit operator string(LiveStatus status) => status.Value.ToString();
}
