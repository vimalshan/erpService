using MediatR;
using EmployeeService.Application.DTOs;

namespace EmployeeService.Application.Queries.GetCalendars;

public record GetCalendarsByEmployeeQuery(long EmpSysId) : IRequest<IEnumerable<EmployeeCalendarDto>>;
