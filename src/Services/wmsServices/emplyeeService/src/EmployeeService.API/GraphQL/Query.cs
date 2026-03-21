using MediatR;
using EmployeeService.Application.DTOs;
using EmployeeService.Application.Queries.GetAllEmployees;
using EmployeeService.Application.Queries.GetEmployee;

namespace EmployeeService.API.GraphQL;

public class Query
{
    public async Task<IReadOnlyList<EmployeeDto>> GetEmployees([Service] IMediator mediator, CancellationToken ct)
    {
        return await mediator.Send(new GetAllEmployeesQuery(), ct);
    }

    public async Task<EmployeeDto?> GetEmployeeById([Service] IMediator mediator, int employeeId, CancellationToken ct)
    {
        return await mediator.Send(new GetEmployeeByIdQuery(employeeId), ct);
    }
}
