using MediatR;
using Microsoft.AspNetCore.Mvc;
using BatchService.Application.Commands.CloseBatch;
using BatchService.Application.Commands.CreateBatch;
using BatchService.Application.Commands.DeleteBatch;
using BatchService.Application.Commands.UpdateBatch;
using BatchService.Application.DTOs;
using BatchService.Application.Queries.GetAllBatches;
using BatchService.Application.Queries.GetBatch;
using BatchService.Application.Queries.GetBatchesByMonth;

namespace BatchService.API.MinimalApis;

public static class BatchEndpoints
{
    public static WebApplication MapBatchEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v2/batches")
                       .WithTags("Batches (Minimal API)")
                       .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllBatchesQuery(), ct)));

        group.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var dto = await mediator.Send(new GetBatchQuery(id), ct);
            return dto is null ? Results.NotFound() : Results.Ok(dto);
        });

        group.MapGet("/month/{monthNo:int}", async (int monthNo, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetBatchesByMonthQuery(monthNo), ct)));

        group.MapPost("/", async (CreateBatchRequest req, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new CreateBatchCommand(req.BatchId, req.MonthNo, req.ModifiedBy), ct);
            return Results.Created($"/api/v2/batches/{result.BatchId}", result);
        });

        group.MapPut("/{id:long}", async (long id, UpdateBatchRequest req, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new UpdateBatchCommand(id, req.MonthNo, req.ModifiedBy), ct)));

        group.MapPost("/{id:long}/close", async (long id, [FromQuery] long modifiedBy, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new CloseBatchCommand(id, modifiedBy), ct);
            return Results.NoContent();
        });

        group.MapDelete("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new DeleteBatchCommand(id), ct);
            return Results.NoContent();
        });

        return app;
    }
}
