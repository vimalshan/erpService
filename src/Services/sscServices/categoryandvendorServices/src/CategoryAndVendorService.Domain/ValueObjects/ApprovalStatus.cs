using CategoryAndVendorService.Domain.Common;

namespace CategoryAndVendorService.Domain.ValueObjects;

public class ApprovalStatus : ValueObject
{
    public char Code { get; }
    public string Description { get; }

    public static readonly ApprovalStatus PendingSubmission = new('N', "Pending for Submission");
    public static readonly ApprovalStatus PendingApproval = new('P', "Pending for Approval");
    public static readonly ApprovalStatus Approved = new('A', "Approved");
    public static readonly ApprovalStatus Rejected = new('R', "Rejected");

    private ApprovalStatus(char code, string description)
    {
        Code = code;
        Description = description;
    }

    public static ApprovalStatus FromCode(char code) => code switch
    {
        'N' => PendingSubmission,
        'P' => PendingApproval,
        'A' => Approved,
        'R' => Rejected,
        _ => throw new ArgumentException($"Invalid approval status code: {code}")
    };

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
    }

    public override string ToString() => Description;
}
