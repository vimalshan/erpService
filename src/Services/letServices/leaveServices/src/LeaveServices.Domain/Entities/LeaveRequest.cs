using LeaveServices.Domain.Common;
using LeaveServices.Domain.Events;

namespace LeaveServices.Domain.Entities;

/// <summary>
/// Aggregate Root: Leave Request (maps to LET_MAIN + LET_SUB)
/// </summary>
public sealed class LeaveRequest : BaseEntity
{
    private readonly List<LeaveRequestDetail> _details = new();

    public long ReqNum { get; private set; }
    public int FinyearSrlno { get; private set; }
    public string EmpUserId { get; private set; } = default!;
    public string? SupUserId { get; private set; }
    public DateTime? ReqDate { get; private set; }

    public IReadOnlyList<LeaveRequestDetail> Details => _details.AsReadOnly();

    private LeaveRequest() { }

    public static LeaveRequest Create(long reqNum, int finyearSrlno, string empUserId, string? supUserId = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(empUserId);

        var request = new LeaveRequest
        {
            ReqNum = reqNum,
            FinyearSrlno = finyearSrlno,
            EmpUserId = empUserId,
            SupUserId = supUserId,
            ReqDate = DateTime.UtcNow
        };

        request.RaiseDomainEvent(new LeaveRequestCreatedEvent(reqNum, empUserId, request.ReqDate.Value));
        return request;
    }

    public LeaveRequestDetail AddDetail(
        int srlNum,
        string? modUser = null,
        char? prefModDev = null,
        string? actTaken = null)
    {
        var detail = LeaveRequestDetail.Create(this.ReqNum, srlNum, modUser, prefModDev, actTaken);
        _details.Add(detail);
        RaiseDomainEvent(new LeaveRequestDetailAddedEvent(ReqNum, srlNum));
        return detail;
    }
}
