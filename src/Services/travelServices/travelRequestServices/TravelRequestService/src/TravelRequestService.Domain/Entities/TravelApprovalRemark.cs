using TravelRequestService.Domain.Common;

namespace TravelRequestService.Domain.Entities;

public class TravelApprovalRemark : BaseEntity
{
    public long RequestNumber { get; private set; }
    public string? RequestType { get; private set; }
    public string? Remarks { get; private set; }
    public string? ApprovedBy { get; private set; }
    public DateTime? ApprovedOn { get; private set; }
    public long SerialNumber { get; private set; }

    private TravelApprovalRemark() { }

    public static TravelApprovalRemark Create(
        long requestNumber,
        string requestType,
        string remarks,
        string approvedBy)
    {
        return new TravelApprovalRemark
        {
            RequestNumber = requestNumber,
            RequestType = requestType,
            Remarks = remarks,
            ApprovedBy = approvedBy,
            ApprovedOn = DateTime.UtcNow
        };
    }
}
