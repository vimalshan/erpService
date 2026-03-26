using MediatR;
using AimsTransactionService.Application.DTOs;

namespace AimsTransactionService.Application.Leaves.Commands.ApplyLeave;

public sealed record ApplyLeaveCommand(
    long EmployeeSysId,
    int LeaveId,
    DateTime FromDate,
    DateTime ToDate,
    decimal LeaveDays,
    string? Reason,
    long AppliedBy) : IRequest<LeaveDetailDto>;
