using ClubMembershipService.Domain.Common;

namespace ClubMembershipService.Domain.ValueObjects;

public sealed class ClubStatus : ValueObject
{
    public static readonly ClubStatus Active = new("A");
    public static readonly ClubStatus Inactive = new("I");

    public string Value { get; }

    private ClubStatus(string value) => Value = value;

    public static ClubStatus From(string value) => value switch
    {
        "A" => Active,
        "I" => Inactive,
        _ => throw new ArgumentException($"Invalid club status: {value}")
    };

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
