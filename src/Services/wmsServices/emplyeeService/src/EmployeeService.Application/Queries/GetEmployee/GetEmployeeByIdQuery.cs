using MediatR;
using EmployeeService.Application.DTOs;

namespace EmployeeService.Application.Queries.GetEmployee;

public sealed record GetEmployeeByIdQuery(int EmployeeId) : IRequest<EmployeeDto?>;
