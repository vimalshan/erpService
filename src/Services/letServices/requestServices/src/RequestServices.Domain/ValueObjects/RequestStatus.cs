namespace RequestServices.Domain.ValueObjects;

public sealed class RequestStatus
{
    public static readonly RequestStatus Pending    = new("P");
    public static readonly RequestStatus Submitted  = new("S");
    public static readonly RequestStatus Approved   = new("A");
    public static readonly RequestStatus Cancelled  = new("C");
    public static readonly RequestStatus Rejected   = new("R");

    public string Code { get; }

    private RequestStatus(string code) => Code = code;

    public static RequestStatus From(string code) => code switch
    {
        "P" => Pending,
        "S" => Submitted,
        "A" => Approved,
        "C" => Cancelled,
        "R" => Rejected,
        _ => throw new ArgumentException($"Unknown status code: {code}")
    };

    public override string ToString() => Code;
    public static implicit operator string(RequestStatus s) => s.Code;
}

public sealed class RequestSource
{
    public static readonly RequestSource Employee   = new("E");
    public static readonly RequestSource Manager    = new("M");
    public static readonly RequestSource System     = new("S");

    public string Code { get; }

    private RequestSource(string code) => Code = code;

    public static RequestSource From(string code) => code switch
    {
        "E" => Employee,
        "M" => Manager,
        "S" => System,
        _ => throw new ArgumentException($"Unknown source code: {code}")
    };

    public override string ToString() => Code;
}

public sealed class GoalDesignation
{
    public static readonly GoalDesignation Individual = new("I");
    public static readonly GoalDesignation Group      = new("G");

    public string Code { get; }

    private GoalDesignation(string code) => Code = code;

    public static GoalDesignation From(string code) => code switch
    {
        "I" => Individual,
        "G" => Group,
        _ => throw new ArgumentException($"Unknown goal designation: {code}")
    };

    public override string ToString() => Code;
}
