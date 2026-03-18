using EmployeeManagement.Application.Employees.Queries.GetEmployee;
using EmployeeManagement.Application.Employees.Commands.CreateEmployee;
using EmployeeManagement.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Endpoints;

/// <summary>Minimal API endpoints for Employee Management.</summary>
public static class EmployeeEndpoints
{
    public static void MapEmployeeEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v2/employees")
            .WithTags("Employees (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetEmployeeByIdQuery(id), ct);
            return Results.Ok(result);
        })
        .WithName("MinimalGetEmployee")
        .WithSummary("Get employee by ID (Minimal API)");

        group.MapPost("/", async ([FromBody] CreateEmployeeCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/employees/{result.Id}", result);
        })
        .WithName("MinimalCreateEmployee")
        .WithSummary("Create employee (Minimal API)");
    }
}
