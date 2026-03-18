namespace DevelopmentService.Domain.ValueObjects;

public sealed class Priority : IEquatable<Priority>, IComparable<Priority>
{
    public static readonly Priority Low    = new(1);
    public static readonly Priority Medium = new(2);
    public static readonly Priority High   = new(3);

    public long Value { get; }

    private Priority(long value)
    {
        if (value < 1) throw new ArgumentException("Priority must be at least 1.");
        Value = value;
    }

    public static Priority From(long value) => new(value);

    public bool Equals(Priority? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is Priority p && Equals(p);
    public override int GetHashCode() => Value.GetHashCode();
    public int CompareTo(Priority? other) => other is null ? 1 : Value.CompareTo(other.Value);
    public override string ToString() => Value.ToString();
}
