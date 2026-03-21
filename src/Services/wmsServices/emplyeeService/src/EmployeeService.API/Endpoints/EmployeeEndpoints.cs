using MediatR;
using EmployeeService.Application.Commands.CreateEmployee;
using EmployeeService.Application.Commands.DeleteEmployee;
using EmployeeService.Application.Commands.UpdateEmployee;
using EmployeeService.Application.DTOs;
using EmployeeService.Application.Queries.GetAllEmployees;
using EmployeeService.Application.Queries.GetEmployee;

namespace EmployeeService.API.Endpoints;

public static class EmployeeEndpoints
{
    public static void MapEmployeeEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/minimal/employees")
            .WithTags("Employees (Minimal)")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllEmployeesQuery(), ct);
            return Results.Ok(result);
        })
        .WithName("GetAllEmployeesMinimal")
        .Produces<IReadOnlyList<EmployeeDto>>();

        group.MapGet("/{id:int}", async (int id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetEmployeeByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetEmployeeByIdMinimal")
        .Produces<EmployeeDto>()
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (CreateEmployeeCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/minimal/employees/{result.EmployeeId}", result);
        })
        .WithName("CreateEmployeeMinimal")
        .Produces<EmployeeDto>(StatusCodes.Status201Created);

        group.MapPut("/{id:int}", async (int id, UpdateEmployeeCommand command, IMediator mediator, CancellationToken ct) =>
        {
            if (id != command.EmployeeId)
                return Results.BadRequest("Route ID does not match command ID.");

            var result = await mediator.Send(command, ct);
            return Results.Ok(result);
        })
        .WithName("UpdateEmployeeMinimal")
        .Produces<EmployeeDto>();

        group.MapDelete("/{id:int}", async (int id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new DeleteEmployeeCommand(id), ct);
            return result ? Results.NoContent() : Results.NotFound();
        })
        .WithName("DeleteEmployeeMinimal")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);
    }
}
