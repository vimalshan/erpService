using MediatR;
using AimsTransactionService.Application.DTOs;

namespace AimsTransactionService.Application.Leaves.Queries.GetLeavesByEmployee;

public sealed record GetLeavesByEmployeeQuery(long EmployeeSysId) : IRequest<IEnumerable<LeaveDetailDto>>;
