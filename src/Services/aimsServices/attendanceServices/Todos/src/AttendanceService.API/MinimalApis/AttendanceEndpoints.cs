using AttendanceService.Application.Commands.SwipePunch;
using AttendanceService.Application.DTOs;
using AttendanceService.Application.Queries.Attendance;
using AttendanceService.Application.Queries.SwipePunch;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceService.API.MinimalApis;

public static class AttendanceEndpoints
{
    public static IEndpointRouteBuilder MapAttendanceEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/attendance")
            .WithTags("Attendance (Minimal API)")
            .RequireAuthorization();

        group.MapPost("/swipe", async (
            [FromBody] RecordSwipePunchRequest req,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(
                new RecordSwipePunchCommand(req.EmpSysId, req.PunchTime, req.GateNo, req.PunchStatus), ct);
            return Results.Created($"/api/v2/attendance/swipe/{result.SwipeId}", result);
        })
        .WithName("RecordSwipePunchV2")
        .Produces<SwipePunchDto>(201)
        .ProducesValidationProblem();

        group.MapGet("/swipe/employee/{empSysId:long}", async (
            long empSysId,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetSwipePunchesByEmployeeQuery(empSysId, from, to), ct);
            return Results.Ok(result);
        })
        .WithName("GetSwipePunchesV2")
        .Produces<IEnumerable<SwipePunchDto>>();

        group.MapGet("/percentage/{empSysId:long}", async (
            long empSysId,
            [FromQuery] DateTime monthStart,
            [FromQuery] DateTime monthEnd,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAttendancePercentageQuery(empSysId, monthStart, monthEnd), ct);
            return Results.Ok(result);
        })
        .WithName("GetAttendancePercentageV2")
        .Produces<AttendancePercentageDto>();

        return app;
    }
}
