using MediatR;
using EmployeeService.Application.DTOs;

namespace EmployeeService.Application.Commands.RecordTimeInfo;

public record RecordTimeInfoCommand(
    long EmpSysId,
    char AttFlag,
    long ModifiedBy
) : IRequest<EmployeeTimeInfoDto>;
