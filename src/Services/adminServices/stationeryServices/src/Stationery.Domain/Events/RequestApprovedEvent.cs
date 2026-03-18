using Stationery.Domain.Common;
using Stationery.Domain.Entities;

namespace Stationery.Domain.Events;

public class RequestApprovedEvent : DomainEvent
{
    public long RequestSubId { get; init; }
    public long RequestId { get; init; }
    public long ApprovedQty { get; init; }
    public long ApproverId { get; init; }
    public long DeptId { get; init; }

    private RequestApprovedEvent() { }

    public RequestApprovedEvent(RequestSub requestSub)
    {
        RequestSubId = requestSub.Id;
        RequestId = requestSub.RequestId;
        ApprovedQty = requestSub.ApprovedQty ?? 0;
        ApproverId = requestSub.ApproverSysId ?? 0;
        DeptId = requestSub.DeptId;
    }
}
