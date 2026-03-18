using MediatR;
using LeaveServices.Application.DTOs;

namespace LeaveServices.Application.Features.LeaveRequests.Queries;

public record GetLeaveRequestByIdQuery(long ReqNum) : IRequest<LeaveRequestDto?>;

public record GetLeaveRequestsByEmployeeQuery(string EmpUserId) : IRequest<IEnumerable<LeaveRequestDto>>;
