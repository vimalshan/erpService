namespace ExitManagement.Domain.ValueObjects;

/// <summary>
/// Represents the status of an employee exit request.
/// </summary>
public sealed class ExitStatus
{
    public static readonly ExitStatus Initiated = new("I");
    public static readonly ExitStatus Approved = new("A");
    public static readonly ExitStatus Revoked = new("R");
    public static readonly ExitStatus Completed = new("C");
    public static readonly ExitStatus Pending = new("P");

    public string Code { get; }

    private ExitStatus(string code) => Code = code;

    public static ExitStatus FromCode(string code) => code switch
    {
        "I" => Initiated,
        "A" => Approved,
        "R" => Revoked,
        "C" => Completed,
        "P" => Pending,
        _ => throw new ArgumentException($"Unknown exit status code: {code}", nameof(code))
    };

    public override string ToString() => Code;

    public override bool Equals(object? obj) =>
        obj is ExitStatus other && Code == other.Code;

    public override int GetHashCode() => Code.GetHashCode();
}
