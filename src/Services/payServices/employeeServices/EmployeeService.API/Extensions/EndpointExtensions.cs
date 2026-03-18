using EmployeeService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EmployeeService.API.Extensions;

/// <summary>
/// Extension methods for minimal API endpoints
/// </summary>
public static class EndpointExtensions
{
    public static void MapEmployeeEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v1/minimal/employees")
            .WithName("Employee Minimal APIs")
            .WithOpenApi();

        group.MapGet("/", GetAllEmployees)
            .WithName("GetAllEmployeesMinimal")
            .WithOpenApi();

        group.MapGet("/{employeeSystemId}", GetEmployeeById)
            .WithName("GetEmployeeByIdMinimal")
            .WithOpenApi();

        group.MapGet("/search", GetEmployeesByStatus)
            .WithName("GetEmployeesByStatusMinimal")
            .WithOpenApi();
    }

    private static async Task<IResult> GetAllEmployees(IMediator mediator, CancellationToken cancellationToken)
    {
        var query = new GetAllEmployeesQuery { PageNumber = 1, PageSize = 50 };
        var result = await mediator.Send(query, cancellationToken);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetEmployeeById(long employeeSystemId, IMediator mediator, CancellationToken cancellationToken)
    {
        var query = new GetEmployeeByIdQuery(employeeSystemId);
        var result = await mediator.Send(query, cancellationToken);
        return result != null ? Results.Ok(result) : Results.NotFound();
    }

    private static async Task<IResult> GetEmployeesByStatus(string? status, IMediator mediator, CancellationToken cancellationToken)
    {
        var query = new SearchEmployeesQuery { EmploymentStatus = status };
        var result = await mediator.Send(query, cancellationToken);
        return Results.Ok(result);
    }
}
