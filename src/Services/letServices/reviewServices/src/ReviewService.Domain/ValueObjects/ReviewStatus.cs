namespace ReviewService.Domain.ValueObjects;

public sealed record ReviewStatus
{
    public static readonly ReviewStatus Active = new("A");
    public static readonly ReviewStatus Inactive = new("I");
    public static readonly ReviewStatus Pending = new("P");
    public static readonly ReviewStatus Completed = new("C");

    public string Code { get; }

    private ReviewStatus(string code) => Code = code;

    public static ReviewStatus From(string code) => code switch
    {
        "A" => Active,
        "I" => Inactive,
        "P" => Pending,
        "C" => Completed,
        _ => throw new ArgumentException($"Invalid status code: {code}", nameof(code))
    };

    public override string ToString() => Code;
}
