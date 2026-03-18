using MediatR;
using TimeAttendance.Application.AbsenteeismDetails.Queries.GetAbsenteeismDetail;
using TimeAttendance.Application.AbsenteeismDetails.Queries.GetAllAbsenteeismDetails;
using TimeAttendance.Application.AbsenteeismDetails.Commands.CreateAbsenteeismDetail;
using TimeAttendance.Application.AbsenteeismMis.Queries.GetAllAbsenteeismMis;
using TimeAttendance.Infrastructure.Repositories.Dapper;

namespace TimeAttendance.API.MinimalApis;

public static class AbsenteeismEndpoints
{
    public static WebApplication MapAbsenteeismEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v1/minimal/absenteeism")
            .WithTags("Absenteeism (Minimal API)")
            .RequireAuthorization("ReadPolicy");

        group.MapGet("/", async (
            IMediator mediator,
            int pageNumber = 1,
            int pageSize = 20,
            CancellationToken ct = default)
            => await mediator.Send(new GetAllAbsenteeismDetailsQuery(pageNumber, pageSize), ct))
            .WithName("MinimalGetAllAbsenteeism")
            .WithSummary("Gets all absenteeism details.");

        group.MapGet("/{id:long}", async (
            long id,
            IMediator mediator,
            CancellationToken ct = default) =>
        {
            var result = await mediator.Send(new GetAbsenteeismDetailQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("MinimalGetAbsenteeismById")
        .WithSummary("Gets absenteeism detail by ID.");

        group.MapPost("/", async (
            CreateAbsenteeismDetailCommand command,
            IMediator mediator,
            CancellationToken ct = default) =>
        {
            var id = await mediator.Send(command, ct);
            return Results.Created($"/api/v1/minimal/absenteeism/{id}", new { id });
        })
        .WithName("MinimalCreateAbsenteeism")
        .WithSummary("Creates a new absenteeism detail record.")
        .RequireAuthorization("WritePolicy");

        // Dapper-backed summary endpoint
        group.MapGet("/summary/{unitId:long}/{year:int}", async (
            long unitId, int year,
            AbsenteeismDapperRepository dapperRepo,
            CancellationToken ct = default)
            => Results.Ok(await dapperRepo.GetAbsenteeismSummaryByUnitAsync(unitId, year, ct)))
            .WithName("MinimalGetAbsenteeismSummary")
            .WithSummary("Gets absenteeism summary via Dapper.");

        // MIS Minimal API
        var misGroup = app.MapGroup("/api/v1/minimal/absmis")
            .WithTags("AbsenteeismMIS (Minimal API)")
            .RequireAuthorization("ReadPolicy");

        misGroup.MapGet("/", async (
            IMediator mediator,
            int pageNumber = 1, int pageSize = 20,
            CancellationToken ct = default)
            => await mediator.Send(new GetAllAbsenteeismMisQuery(pageNumber, pageSize), ct))
            .WithName("MinimalGetAllAbsMis")
            .WithSummary("Gets all absenteeism MIS records.");

        return app;
    }
}
