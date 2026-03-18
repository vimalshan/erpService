namespace StipendService.Domain.ValueObjects;

public sealed class DisbursementStatus
{
    public static readonly DisbursementStatus Draft = new("D");
    public static readonly DisbursementStatus Processed = new("P");
    public static readonly DisbursementStatus Rejected = new("R");

    public string Code { get; }

    private DisbursementStatus(string code) => Code = code;

    public static DisbursementStatus FromCode(string code) => code switch
    {
        "D" => Draft,
        "P" => Processed,
        "R" => Rejected,
        _ => throw new ArgumentException($"Unknown disbursement status code: {code}", nameof(code))
    };

    public override string ToString() => Code;
    public override bool Equals(object? obj) => obj is DisbursementStatus other && Code == other.Code;
    public override int GetHashCode() => Code.GetHashCode();
}
