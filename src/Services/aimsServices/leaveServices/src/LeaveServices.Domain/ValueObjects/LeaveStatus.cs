using LeaveServices.Domain.Common;

namespace LeaveServices.Domain.ValueObjects;

public sealed class LeaveStatus : ValueObject
{
    public static readonly LeaveStatus Pending   = new("P");
    public static readonly LeaveStatus Approved  = new("Y");
    public static readonly LeaveStatus Rejected  = new("R");
    public static readonly LeaveStatus Cancelled = new("C");
    public static readonly LeaveStatus Draft     = new("D");

    public string Code { get; }

    private LeaveStatus(string code) => Code = code;

    public static LeaveStatus From(string code) =>
        code switch
        {
            "P" => Pending,
            "Y" => Approved,
            "R" => Rejected,
            "C" => Cancelled,
            "D" => Draft,
            _ => throw new ArgumentException($"Unknown leave status: {code}", nameof(code))
        };

    public string DisplayName => Code switch
    {
        "P" => "Pending",
        "Y" => "Approved",
        "R" => "Rejected",
        "C" => "Cancelled",
        "D" => "Draft",
        _   => Code
    };

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Code;
    }

    public override string ToString() => DisplayName;
}
