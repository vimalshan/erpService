using MediatR;
using EmployeeService.Application.DTOs;

namespace EmployeeService.Application.Commands.UpdateEmployee;

public sealed record UpdateEmployeeCommand(
    int EmployeeId,
    string FirstName,
    string LastName,
    DateTime HireDate,
    string? JobTitle,
    string? Department,
    int? UserId,
    int? WarehouseId,
    string? Phone,
    string? Email) : IRequest<EmployeeDto>;
