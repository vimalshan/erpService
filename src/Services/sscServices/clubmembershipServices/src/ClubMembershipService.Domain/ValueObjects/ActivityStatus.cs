using ClubMembershipService.Domain.Common;

namespace ClubMembershipService.Domain.ValueObjects;

public sealed class ActivityStatus : ValueObject
{
    public static readonly ActivityStatus Planned = new("P");
    public static readonly ActivityStatus Ongoing = new("O");
    public static readonly ActivityStatus Completed = new("C");

    public string Value { get; }

    private ActivityStatus(string value) => Value = value;

    public static ActivityStatus From(string value) => value switch
    {
        "P" => Planned,
        "O" => Ongoing,
        "C" => Completed,
        _ => throw new ArgumentException($"Invalid activity status: {value}")
    };

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
