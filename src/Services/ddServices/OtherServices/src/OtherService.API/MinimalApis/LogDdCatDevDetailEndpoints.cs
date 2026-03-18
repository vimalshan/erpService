using MediatR;
using Microsoft.AspNetCore.Authorization;
using OtherService.Application.CQRS.Commands.CreateLogDdCatDevDetail;
using OtherService.Application.CQRS.Commands.DeleteLogDdCatDevDetail;
using OtherService.Application.CQRS.Queries.GetAllLogDdCatDevDetails;
using OtherService.Application.CQRS.Queries.GetLogDdCatDevDetailByKey;
using OtherService.Application.DTOs;

namespace OtherService.API.MinimalApis;

/// <summary>
/// Minimal API endpoints (alternative thin entry-points alongside controllers).
/// </summary>
public static class LogDdCatDevDetailEndpoints
{
    public static WebApplication MapLogDdCatDevDetailEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/minimal/log-dev-detail")
            .WithTags("LogDdCatDevDetail-Minimal")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllLogDdCatDevDetailsQuery(), ct);
            return Results.Ok(result);
        })
        .WithSummary("Get all log entries (Minimal API)");

        group.MapGet("/{appId}/{appNum:decimal}", async (
            string appId,
            decimal appNum,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLogDdCatDevDetailByKeyQuery(appId, appNum), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithSummary("Get entry by composite key (Minimal API)");

        group.MapPost("/", async (
            CreateLogDdCatDevDetailDto dto,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var cmd = new CreateLogDdCatDevDetailCommand(
                dto.ReqNum, dto.QtnNum, dto.AnsSrl,
                dto.AppId, dto.AppNum, dto.EntDat,
                dto.Desc, dto.Need);
            var result = await mediator.Send(cmd, ct);
            return Results.Created($"/api/minimal/log-dev-detail/{result.AppId}/{result.AppNum}", result);
        })
        .WithSummary("Create a log entry (Minimal API)");

        group.MapDelete("/{appId}/{appNum:decimal}", async (
            string appId,
            decimal appNum,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var deleted = await mediator.Send(new DeleteLogDdCatDevDetailCommand(appId, appNum), ct);
            return deleted ? Results.NoContent() : Results.NotFound();
        })
        .WithSummary("Delete a log entry (Minimal API)");

        return app;
    }
}
