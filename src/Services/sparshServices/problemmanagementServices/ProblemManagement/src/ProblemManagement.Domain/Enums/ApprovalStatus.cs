namespace ProblemManagement.Domain.Enums;

public enum ApprovalStatus
{
    Approved = 'A',
    Rejected = 'R',
    Pending = 'P'
}

public static class ApprovalStatusExtensions
{
    public static char ToChar(this ApprovalStatus status) => (char)status;

    public static ApprovalStatus FromChar(char c) => c switch
    {
        'A' => ApprovalStatus.Approved,
        'R' => ApprovalStatus.Rejected,
        'P' => ApprovalStatus.Pending,
        _ => throw new ArgumentOutOfRangeException(nameof(c), $"Invalid approval status: {c}")
    };
}
