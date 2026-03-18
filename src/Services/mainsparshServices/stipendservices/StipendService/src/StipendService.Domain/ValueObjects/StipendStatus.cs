namespace StipendService.Domain.ValueObjects;

public sealed class StipendStatus
{
    public static readonly StipendStatus Active = new("A");
    public static readonly StipendStatus Inactive = new("I");

    public string Code { get; }

    private StipendStatus(string code) => Code = code;

    public static StipendStatus FromCode(string code) => code switch
    {
        "A" => Active,
        "I" => Inactive,
        _ => throw new ArgumentException($"Unknown stipend status code: {code}", nameof(code))
    };

    public bool IsActive => Code == "A";

    public override string ToString() => Code;
    public override bool Equals(object? obj) => obj is StipendStatus other && Code == other.Code;
    public override int GetHashCode() => Code.GetHashCode();
}
