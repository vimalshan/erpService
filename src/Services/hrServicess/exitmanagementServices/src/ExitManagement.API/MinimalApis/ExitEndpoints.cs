using ExitManagement.Application.Features.EmployeeExits.Commands;
using ExitManagement.Application.Features.EmployeeExits.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace ExitManagement.API.MinimalApis;

/// <summary>
/// Minimal API endpoints for Exit Management (alternate to controller routing).
/// </summary>
public static class ExitEndpoints
{
    public static IEndpointRouteBuilder MapExitEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/exits")
            .WithTags("ExitsMinimal")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllExitsQuery(), ct);
            return Results.Ok(result);
        }).WithName("MinimalGetAllExits").WithSummary("Get all exits (minimal API)");

        group.MapGet("/{exitNo:decimal}", async (decimal exitNo, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetExitByIdQuery(exitNo), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("MinimalGetExitById").WithSummary("Get exit by ID (minimal API)");

        group.MapPost("/", async (CreateExitCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var exitNo = await mediator.Send(command, ct);
            return Results.Created($"/exits/{exitNo}", new { ExitNo = exitNo });
        }).WithName("MinimalCreateExit").WithSummary("Create exit (minimal API)");

        return app;
    }
}
