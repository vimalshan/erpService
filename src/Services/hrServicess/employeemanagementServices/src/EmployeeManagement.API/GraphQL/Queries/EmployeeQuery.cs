using EmployeeManagement.Application.Employees.DTOs;
using EmployeeManagement.Application.Employees.Queries.GetEmployee;
using EmployeeManagement.Application.Employees.Queries.GetEmployees;
using HotChocolate.Authorization;
using MediatR;

namespace EmployeeManagement.API.GraphQL.Queries;

public sealed class EmployeeQuery
{
    /// <summary>Get a single employee by ID via GraphQL.</summary>
    [Authorize]
    public async Task<EmployeeDto> GetEmployeeAsync(long id, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetEmployeeByIdQuery(id), ct);

    /// <summary>Get employees list via GraphQL.</summary>
    [Authorize]
    public async Task<IReadOnlyList<EmployeeSummaryDto>> GetEmployeesAsync(int page, int pageSize,
        [Service] IMediator mediator, CancellationToken ct)
    {
        var result = await mediator.Send(new GetEmployeesQuery(page, pageSize), ct);
        return result.Items;
    }
}
