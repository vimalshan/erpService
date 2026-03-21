using MediatR;
using EmployeeService.Application.DTOs;

namespace EmployeeService.Application.Commands.CreateEmployee;

public sealed record CreateEmployeeCommand(
    string FirstName,
    string LastName,
    string EmployeeCode,
    DateTime HireDate,
    string? JobTitle,
    string? Department,
    int? UserId,
    int? WarehouseId,
    string? Phone,
    string? Email) : IRequest<EmployeeDto>;
