namespace TransactionService.Domain.ValueObjects;

public sealed class WorkflowStatus
{
    public static readonly WorkflowStatus Submitted = new("SUBMITTED");
    public static readonly WorkflowStatus InReview = new("IN_REVIEW");
    public static readonly WorkflowStatus Approved = new("APPROVED");
    public static readonly WorkflowStatus Rejected = new("REJECTED");
    public static readonly WorkflowStatus Cancelled = new("CANCELLED");

    public string Code { get; }

    private WorkflowStatus(string code) => Code = code;

    public static WorkflowStatus FromCode(string code) => code switch
    {
        "SUBMITTED" => Submitted,
        "IN_REVIEW" => InReview,
        "APPROVED" => Approved,
        "REJECTED" => Rejected,
        "CANCELLED" => Cancelled,
        _ => throw new ArgumentException($"Unknown workflow status: {code}", nameof(code))
    };

    public bool IsTerminal => this == Approved || this == Rejected || this == Cancelled;

    public override string ToString() => Code;
    public override bool Equals(object? obj) => obj is WorkflowStatus other && Code == other.Code;
    public override int GetHashCode() => Code.GetHashCode();
}
