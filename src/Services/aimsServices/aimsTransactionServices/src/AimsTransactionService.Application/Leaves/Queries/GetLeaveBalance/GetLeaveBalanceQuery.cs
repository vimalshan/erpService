using MediatR;
using AimsTransactionService.Application.DTOs;

namespace AimsTransactionService.Application.Leaves.Queries.GetLeaveBalance;

public sealed record GetLeaveBalanceQuery(long EmployeeSysId, int LeaveId) : IRequest<LeaveBalanceDto>;
