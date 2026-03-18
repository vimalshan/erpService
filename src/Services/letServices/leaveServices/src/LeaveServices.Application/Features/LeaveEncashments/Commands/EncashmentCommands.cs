using MediatR;
using LeaveServices.Application.DTOs;

namespace LeaveServices.Application.Features.LeaveEncashments.Commands;

public record ApplyLeaveEncashmentCommand(
    long EmpSysId,
    string LeaveType,
    int EncashmentDays,
    decimal BasicSalary,
    DateOnly RequestDate,
    long RequestedBy) : IRequest<LeaveEncashmentDto>;

public record UpdateEncashmentStatusCommand(
    long EncashmentId,
    char NewStatus,
    long ModifiedBy) : IRequest<LeaveEncashmentDto>;
