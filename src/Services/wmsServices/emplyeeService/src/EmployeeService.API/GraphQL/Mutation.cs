using MediatR;
using EmployeeService.Application.Commands.CreateEmployee;
using EmployeeService.Application.Commands.DeleteEmployee;
using EmployeeService.Application.Commands.UpdateEmployee;
using EmployeeService.Application.DTOs;

namespace EmployeeService.API.GraphQL;

public class Mutation
{
    public async Task<EmployeeDto> CreateEmployee(
        [Service] IMediator mediator,
        string firstName,
        string lastName,
        string employeeCode,
        DateTime hireDate,
        string? jobTitle,
        string? department,
        int? userId,
        int? warehouseId,
        string? phone,
        string? email,
        CancellationToken ct)
    {
        return await mediator.Send(new CreateEmployeeCommand(
            firstName, lastName, employeeCode, hireDate,
            jobTitle, department, userId, warehouseId, phone, email), ct);
    }

    public async Task<EmployeeDto> UpdateEmployee(
        [Service] IMediator mediator,
        int employeeId,
        string firstName,
        string lastName,
        DateTime hireDate,
        string? jobTitle,
        string? department,
        int? userId,
        int? warehouseId,
        string? phone,
        string? email,
        CancellationToken ct)
    {
        return await mediator.Send(new UpdateEmployeeCommand(
            employeeId, firstName, lastName, hireDate,
            jobTitle, department, userId, warehouseId, phone, email), ct);
    }

    public async Task<bool> DeleteEmployee([Service] IMediator mediator, int employeeId, CancellationToken ct)
    {
        return await mediator.Send(new DeleteEmployeeCommand(employeeId), ct);
    }
}
