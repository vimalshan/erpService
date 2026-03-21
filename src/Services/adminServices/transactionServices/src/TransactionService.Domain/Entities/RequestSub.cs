namespace TransactionService.Domain.Entities;

using TransactionService.Domain.Common;
using TransactionService.Domain.Events;
using TransactionService.Domain.ValueObjects;

public sealed class RequestSub : Entity
{
    public long RequestSubId { get; private set; }
    public long RequestId { get; private set; }
    public long StationaryId { get; private set; }
    public long DeptId { get; private set; }
    public DateTime ExpectedDate { get; private set; }
    public long? UserSysId { get; private set; }
    public long RequestedQty { get; private set; }
    public long? IndentedQty { get; private set; }
    public long? ApprovedQty { get; private set; }
    public long? ApproverSysId { get; private set; }
    public string? ApproverRemarks { get; private set; }
    public DateTime? ReceivedDate { get; private set; }
    public RequestStatus Status { get; private set; }
    public long UpdatedBy { get; private set; }
    public DateTime UpdatedOn { get; private set; }
    public DateTime? ApprovedOn { get; private set; }

    public RequestMain? RequestMain { get; private set; }

    private RequestSub() { Status = RequestStatus.Pending; }

    public static RequestSub Create(
        long requestSubId, long requestId, long stationaryId, long deptId,
        DateTime expectedDate, long? userSysId, long requestedQty, long updatedBy)
    {
        return new RequestSub
        {
            RequestSubId = requestSubId,
            RequestId = requestId,
            StationaryId = stationaryId,
            DeptId = deptId,
            ExpectedDate = expectedDate,
            UserSysId = userSysId,
            RequestedQty = requestedQty,
            Status = RequestStatus.Pending,
            UpdatedBy = updatedBy,
            UpdatedOn = DateTime.UtcNow
        };
    }

    public void Approve(long approvedQty, long approverSysId, string? remarks)
    {
        ApprovedQty = approvedQty;
        ApproverSysId = approverSysId;
        ApproverRemarks = remarks;
        Status = approvedQty > 0 ? RequestStatus.Approved : RequestStatus.Rejected;
        ApprovedOn = DateTime.UtcNow;
        UpdatedBy = approverSysId;
        UpdatedOn = DateTime.UtcNow;

        RaiseDomainEvent(new RequestApprovedEvent(
            RequestSubId, RequestId, approvedQty, approverSysId, DateTime.UtcNow));
    }

    public void MarkIndented(long indentedQty, long updatedBy)
    {
        IndentedQty = indentedQty;
        Status = RequestStatus.Indented;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }

    public void MarkReceived(DateTime receivedDate, long updatedBy)
    {
        ReceivedDate = receivedDate;
        Status = RequestStatus.Completed;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }
}
