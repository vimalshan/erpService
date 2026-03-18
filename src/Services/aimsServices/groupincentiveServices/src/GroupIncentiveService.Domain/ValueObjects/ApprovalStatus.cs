namespace GroupIncentiveService.Domain.ValueObjects;

/// <summary>
/// Approval status: P=Pending, Y=Approved, N=Rejected
/// </summary>
public sealed record ApprovalStatus
{
    public const string Pending = "P";
    public const string Approved = "Y";
    public const string Rejected = "N";

    public string Value { get; }

    private static readonly HashSet<string> _valid = [Pending, Approved, Rejected];

    public ApprovalStatus(string value)
    {
        if (!_valid.Contains(value))
            throw new ArgumentException($"Invalid approval status: '{value}'. Must be P, Y, or N.");
        Value = value;
    }

    public bool IsPending => Value == Pending;
    public bool IsApproved => Value == Approved;
    public bool IsRejected => Value == Rejected;

    public override string ToString() => Value switch
    {
        Pending => "Pending",
        Approved => "Approved",
        Rejected => "Rejected",
        _ => Value
    };
}
