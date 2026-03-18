using MediatR;

namespace HRService.Application.Queries;

public class GetEmployeeLeaveQuery : IRequest<DTOs.LeaveDto>
{
    public Guid LeaveId { get; set; }

    public GetEmployeeLeaveQuery(Guid leaveId)
    {
        LeaveId = leaveId;
    }
}

public class GetEmployeeLeavesQuery : IRequest<List<DTOs.LeaveDto>>
{
    public Guid EmployeeId { get; set; }
}

public class GetPendingLeavesQuery : IRequest<List<DTOs.LeaveDto>>
{
}

public class GetApprovedLeavesQuery : IRequest<List<DTOs.LeaveDto>>
{
}
