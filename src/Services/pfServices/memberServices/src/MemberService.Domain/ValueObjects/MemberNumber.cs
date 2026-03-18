using MemberService.Domain.Common;

namespace MemberService.Domain.ValueObjects;

public sealed class MemberNumber : IEquatable<MemberNumber>
{
    public long Value { get; }

    private MemberNumber(long value) => Value = value;

    public static MemberNumber Create(long value)
    {
        if (value <= 0) throw new ArgumentException("Member number must be positive.", nameof(value));
        return new MemberNumber(value);
    }

    public static MemberNumber New(long nextValue) => new(nextValue);

    public bool Equals(MemberNumber? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is MemberNumber mn && Equals(mn);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString();
    public static implicit operator long(MemberNumber mn) => mn.Value;
}
