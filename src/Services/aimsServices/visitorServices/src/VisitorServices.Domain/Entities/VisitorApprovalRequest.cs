using VisitorServices.Domain.Common;
using VisitorServices.Domain.Enums;
using VisitorServices.Domain.Events;

namespace VisitorServices.Domain.Entities;

public sealed class VisitorApprovalRequest : Entity
{
    public long VisitorId { get; private set; }
    public long RequiredApproverId { get; private set; }
    public ApprovalStatus ApprovalStatus { get; private set; }
    public DateTime? ApprovalDate { get; private set; }
    public string? ApprovalRemarks { get; private set; }
    public DateTime RequestedOn { get; private set; }
    public long RequestedBy { get; private set; }
    public long LastModifiedBy { get; private set; }
    public DateTime LastModifiedOn { get; private set; }

    private VisitorApprovalRequest() { }

    public static VisitorApprovalRequest Create(
        long id,
        long visitorId,
        long requiredApproverId,
        long requestedBy)
    {
        return new VisitorApprovalRequest
        {
            Id = id,
            VisitorId = visitorId,
            RequiredApproverId = requiredApproverId,
            ApprovalStatus = ApprovalStatus.Pending,
            RequestedOn = DateTime.UtcNow,
            RequestedBy = requestedBy,
            LastModifiedBy = requestedBy,
            LastModifiedOn = DateTime.UtcNow
        };
    }

    public void Approve(string? remarks, long approvedBy)
    {
        if (ApprovalStatus != ApprovalStatus.Pending)
            throw new InvalidOperationException("Only pending requests can be approved.");

        ApprovalStatus = ApprovalStatus.Approved;
        ApprovalDate = DateTime.UtcNow;
        ApprovalRemarks = remarks;
        LastModifiedBy = approvedBy;
        LastModifiedOn = DateTime.UtcNow;
    }

    public void Reject(string? remarks, long rejectedBy)
    {
        if (ApprovalStatus != ApprovalStatus.Pending)
            throw new InvalidOperationException("Only pending requests can be rejected.");

        ApprovalStatus = ApprovalStatus.Rejected;
        ApprovalDate = DateTime.UtcNow;
        ApprovalRemarks = remarks;
        LastModifiedBy = rejectedBy;
        LastModifiedOn = DateTime.UtcNow;
    }
}
