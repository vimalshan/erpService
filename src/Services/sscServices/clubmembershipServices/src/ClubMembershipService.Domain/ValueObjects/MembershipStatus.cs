using ClubMembershipService.Domain.Common;

namespace ClubMembershipService.Domain.ValueObjects;

public sealed class MembershipStatus : ValueObject
{
    public static readonly MembershipStatus Active = new("A");
    public static readonly MembershipStatus Inactive = new("I");

    public string Value { get; }

    private MembershipStatus(string value) => Value = value;

    public static MembershipStatus From(string value) => value switch
    {
        "A" => Active,
        "I" => Inactive,
        _ => throw new ArgumentException($"Invalid membership status: {value}")
    };

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
