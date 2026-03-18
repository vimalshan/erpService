using MediatR;

namespace HRService.Application.Commands;

public class RequestLeaveCommand : IRequest<Guid>
{
    public Guid EmployeeId { get; set; }
    public Guid LeaveTypeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Reason { get; set; }
}

public class ApproveLeaveCommand : IRequest<bool>
{
    public Guid LeaveId { get; set; }
    public Guid ApprovedBy { get; set; }
}

public class RejectLeaveCommand : IRequest<bool>
{
    public Guid LeaveId { get; set; }
}

public class CancelLeaveCommand : IRequest<bool>
{
    public Guid LeaveId { get; set; }
}
