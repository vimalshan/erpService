using RequestServices.Domain.Common;

namespace RequestServices.Domain.Events;

public sealed class RequestCreatedEvent(long requestId, string employeeUser, string supervisorUser)
    : DomainEvent
{
    public long   RequestId      { get; } = requestId;
    public string EmployeeUser   { get; } = employeeUser;
    public string SupervisorUser { get; } = supervisorUser;
}

public sealed class RequestApprovedEvent(long requestId, long serialNumber, string approvalUser)
    : DomainEvent
{
    public long   RequestId    { get; } = requestId;
    public long   SerialNumber { get; } = serialNumber;
    public string ApprovalUser { get; } = approvalUser;
}

public sealed class RequestCancelledEvent(long requestId, long serialNumber, string remark)
    : DomainEvent
{
    public long   RequestId    { get; } = requestId;
    public long   SerialNumber { get; } = serialNumber;
    public string Remark       { get; } = remark;
}
