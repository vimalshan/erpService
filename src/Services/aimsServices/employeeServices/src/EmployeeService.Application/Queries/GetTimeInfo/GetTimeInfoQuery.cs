using MediatR;
using EmployeeService.Application.DTOs;

namespace EmployeeService.Application.Queries.GetTimeInfo;

public record GetTimeInfoByEmployeeQuery(long EmpSysId) : IRequest<IEnumerable<EmployeeTimeInfoDto>>;

public record GetTimeInfoByIdQuery(long TimeInfoId) : IRequest<EmployeeTimeInfoDto?>;
