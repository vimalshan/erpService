using MediatR;
using EmployeeService.Application.DTOs;

namespace EmployeeService.Application.Commands.AssignApprover;

public record AssignApproverCommand(
    long EmpSysId,
    long ApproverSysId,
    int Level,
    long AssignedBy
) : IRequest<EmployeeApproverDto>;
