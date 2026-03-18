using MediatR;
using EmployeeService.Application.DTOs;

namespace EmployeeService.Application.Queries.GetApprovers;

public record GetApproversByEmployeeQuery(long EmpSysId) : IRequest<IEnumerable<EmployeeApproverDto>>;
