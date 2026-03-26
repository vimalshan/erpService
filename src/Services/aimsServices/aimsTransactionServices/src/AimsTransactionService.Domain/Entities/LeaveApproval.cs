using AimsTransactionService.Domain.Common;

namespace AimsTransactionService.Domain.Entities;

public class LeaveApproval : Entity
{
    public long LeaveDetailId { get; private set; }
    public long ApprovedBy { get; private set; }
    public DateTime ApprovedOn { get; private set; }

    private LeaveApproval() { }

    public static LeaveApproval Create(long id, long leaveDetailId, long approvedBy)
    {
        return new LeaveApproval
        {
            Id = id,
            LeaveDetailId = leaveDetailId,
            ApprovedBy = approvedBy,
            ApprovedOn = DateTime.UtcNow
        };
    }
}
