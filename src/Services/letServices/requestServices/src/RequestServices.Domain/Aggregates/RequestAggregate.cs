using RequestServices.Domain.Common;
using RequestServices.Domain.Entities;
using RequestServices.Domain.Events;

namespace RequestServices.Domain.Aggregates;

/// <summary>
/// Request Aggregate Root — manages the lifecycle of a training request,
/// including sub-items, approvals, and actions.
/// </summary>
public class RequestAggregate : Entity
{
    public long RequestId        { get; private set; }
    public string EmployeeUser   { get; private set; } = default!;
    public DateTime RequestDate  { get; private set; }
    public string SupervisorUser { get; private set; } = default!;

    private readonly List<RequestSub> _subRequests = new();
    private readonly List<RequestApp> _approvals   = new();
    private readonly List<RequestNew> _newSkills   = new();

    public IReadOnlyCollection<RequestSub> SubRequests => _subRequests.AsReadOnly();
    public IReadOnlyCollection<RequestApp> Approvals   => _approvals.AsReadOnly();
    public IReadOnlyCollection<RequestNew> NewSkills   => _newSkills.AsReadOnly();

    private RequestAggregate() { }

    public static RequestAggregate Create(
        long requestId, string employeeUser,
        DateTime requestDate, string supervisorUser)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(employeeUser);
        ArgumentException.ThrowIfNullOrWhiteSpace(supervisorUser);

        var aggregate = new RequestAggregate
        {
            RequestId      = requestId,
            EmployeeUser   = employeeUser,
            RequestDate    = requestDate,
            SupervisorUser = supervisorUser
        };

        aggregate.AddDomainEvent(new RequestCreatedEvent(requestId, employeeUser, supervisorUser));
        return aggregate;
    }

    public void AddSubRequest(RequestSub sub)
    {
        ArgumentNullException.ThrowIfNull(sub);
        _subRequests.Add(sub);
    }

    public void Approve(long serialNumber, long approvalNumber,
        string approvalRemark, string approvalUser)
    {
        var sub = _subRequests.FirstOrDefault(s => s.SerialNumber == serialNumber)
            ?? throw new InvalidOperationException($"Sub-request {serialNumber} not found.");

        sub.Approve(approvalNumber);

        var approval = RequestApp.Create(RequestId, serialNumber,
            DateTime.UtcNow, approvalNumber, approvalRemark, approvalUser);
        _approvals.Add(approval);

        AddDomainEvent(new RequestApprovedEvent(RequestId, serialNumber, approvalUser));
    }

    public void Cancel(long serialNumber, string remark)
    {
        var sub = _subRequests.FirstOrDefault(s => s.SerialNumber == serialNumber)
            ?? throw new InvalidOperationException($"Sub-request {serialNumber} not found.");

        sub.Cancel(DateTime.UtcNow, remark);
        AddDomainEvent(new RequestCancelledEvent(RequestId, serialNumber, remark));
    }
}
