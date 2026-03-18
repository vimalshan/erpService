using MediatR;
using TimesheetService.Application.Commands.CreateTimesheet;
using TimesheetService.Application.Commands.SubmitTimesheet;
using TimesheetService.Application.DTOs;
using TimesheetService.Application.Queries.GetTimesheetById;
using TimesheetService.Application.Queries.GetTimesheetsByEmployee;
using Microsoft.AspNetCore.Mvc;

namespace TimesheetService.API.MinimalApis;

public static class TimesheetEndpoints
{
    public static IEndpointRouteBuilder MapTimesheetEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/min/timesheets")
                       .WithTags("Timesheets (Minimal API)")
                       .RequireAuthorization();

        group.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetTimesheetByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("MinGetTimesheetById")
        .Produces<TimesheetDto>()
        .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/employee/{employeeId:long}", async (
            long employeeId, [FromQuery] DateOnly? from, [FromQuery] DateOnly? to,
            IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetTimesheetsByEmployeeQuery(employeeId, from, to), ct);
            return Results.Ok(result);
        })
        .WithName("MinGetByEmployee")
        .Produces<IEnumerable<TimesheetSummaryDto>>();

        group.MapPost("/", async ([FromBody] CreateTimesheetCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/min/timesheets/{result.TimesheetId}", result);
        })
        .WithName("MinCreateTimesheet")
        .Produces<TimesheetDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);

        group.MapPost("/{id:long}/submit", async (long id, [FromBody] long updatedBy, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new SubmitTimesheetCommand(id, updatedBy), ct);
            return Results.Ok(result);
        })
        .WithName("MinSubmitTimesheet");

        return app;
    }
}
