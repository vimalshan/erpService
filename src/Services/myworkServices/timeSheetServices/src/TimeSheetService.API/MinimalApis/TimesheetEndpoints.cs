using MediatR;
using Microsoft.AspNetCore.Mvc;
using TimeSheetService.Application.Commands.SubmitTimesheet;
using TimeSheetService.Application.Commands.UpdateTimesheet;
using TimeSheetService.Application.Commands.DeleteTimesheet;
using TimeSheetService.Application.Queries.GetAllTimesheets;
using TimeSheetService.Application.Queries.GetTimesheetById;
using TimeSheetService.Application.Queries.GetTimesheetsByEmployee;
using TimeSheetService.Application.Queries.GetTcProjects;
using TimeSheetService.Application.Queries.GetTsProjects;

namespace TimeSheetService.API.MinimalApis;

public static class TimesheetEndpoints
{
    public static void MapTimesheetEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/timesheets")
            .WithTags("Timesheets-MinimalAPI")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllTimesheetsQuery(), ct);
            return Results.Ok(result);
        }).WithName("GetAllTimesheetsV2");

        group.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetTimesheetByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetTimesheetByIdV2");

        group.MapGet("/employee/{employeeSysId:long}", async (
            long employeeSysId,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetTimesheetsByEmployeeQuery(employeeSysId, from, to), ct);
            return Results.Ok(result);
        }).WithName("GetTimesheetsByEmployeeV2");

        group.MapPost("/", async ([FromBody] SubmitTimesheetCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/timesheets/{result.TimeId}", result);
        }).WithName("SubmitTimesheetV2");

        group.MapPut("/{id:long}", async (long id, [FromBody] UpdateTimesheetCommand command, IMediator mediator, CancellationToken ct) =>
        {
            if (id != command.TimeId) return Results.BadRequest("ID mismatch");
            var result = await mediator.Send(command, ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("UpdateTimesheetV2");

        group.MapDelete("/{id:long}", async (long id, [FromQuery] long modifiedBy, IMediator mediator, CancellationToken ct) =>
        {
            var deleted = await mediator.Send(new DeleteTimesheetCommand(id, modifiedBy), ct);
            return deleted ? Results.NoContent() : Results.NotFound();
        }).WithName("DeleteTimesheetV2");

        // TC Projects minimal API
        var tcGroup = app.MapGroup("/api/v2/tc-projects")
            .WithTags("TcProjects-MinimalAPI")
            .RequireAuthorization();

        tcGroup.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetTcProjectsQuery(), ct))
        ).WithName("GetTcProjectsV2");

        // TS Projects minimal API
        var tsGroup = app.MapGroup("/api/v2/ts-projects")
            .WithTags("TsProjects-MinimalAPI")
            .RequireAuthorization();

        tsGroup.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetTsProjectsQuery(), ct))
        ).WithName("GetTsProjectsV2");
    }
}
