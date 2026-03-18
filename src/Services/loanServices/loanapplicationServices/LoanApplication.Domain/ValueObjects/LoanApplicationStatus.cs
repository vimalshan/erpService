namespace LoanApplication.Domain.ValueObjects;

/// <summary>
/// Loan Application Status value object
/// </summary>
public class LoanApplicationStatus : IEquatable<LoanApplicationStatus>
{
    public const char CreatedStatus = 'C';      // Created
    public const char AppliedStatus = 'P';      // Applied/Pending
    public const char ApprovedStatus = 'A';     // Approved
    public const char RejectedStatus = 'R';     // Rejected
    public const char DisbursedStatus = 'D';    // Disbursed

    public char Value { get; private set; }

    private LoanApplicationStatus(char status)
    {
        if (!IsValidStatus(status))
            throw new ArgumentException($"Invalid loan application status: {status}");

        Value = status;
    }

    public static LoanApplicationStatus CreateNew() => new(CreatedStatus);
    public static LoanApplicationStatus Apply() => new(AppliedStatus);
    public static LoanApplicationStatus Approve() => new(ApprovedStatus);
    public static LoanApplicationStatus Reject() => new(RejectedStatus);
    public static LoanApplicationStatus Disburse() => new(DisbursedStatus);

    public static LoanApplicationStatus FromValue(char value) => new(value);

    public bool IsCreated => Value == CreatedStatus;
    public bool IsApplied => Value == AppliedStatus;
    public bool IsApproved => Value == ApprovedStatus;
    public bool IsRejected => Value == RejectedStatus;
    public bool IsDisbursed => Value == DisbursedStatus;

    private static bool IsValidStatus(char status) =>
        status is CreatedStatus or AppliedStatus or ApprovedStatus or RejectedStatus or DisbursedStatus;

    public override bool Equals(object? obj) => Equals(obj as LoanApplicationStatus);

    public bool Equals(LoanApplicationStatus? other) =>
        other is not null && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString();
}
