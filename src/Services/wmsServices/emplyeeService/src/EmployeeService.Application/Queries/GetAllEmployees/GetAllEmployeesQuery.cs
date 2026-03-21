using MediatR;
using EmployeeService.Application.DTOs;

namespace EmployeeService.Application.Queries.GetAllEmployees;

public sealed record GetAllEmployeesQuery : IRequest<IReadOnlyList<EmployeeDto>>;
