using ContributionService.Application.Commands.ContributionBatch;
using ContributionService.Application.DTOs;
using ContributionService.Application.Queries.ContributionBatch;
using ContributionService.Application.Queries.ContributionDetail;
using ContributionService.Application.Queries.Superannuation;
using ContributionService.Application.Queries.SuperannuationBatch;
using MediatR;

namespace ContributionService.API.Extensions;

public static class MinimalApiExtensions
{
    public static WebApplication MapMinimalApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/minimal").RequireAuthorization();

        // Contribution Batches
        group.MapGet("/batches", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllContributionBatchesQuery(), ct)))
            .WithName("GetAllBatches");

        group.MapGet("/batches/{batchNo:long}", async (long batchNo, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetContributionBatchByIdQuery(batchNo), ct)))
            .WithName("GetBatchById");

        group.MapGet("/batches/status/{status}", async (string status, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetContributionBatchesByStatusQuery(status), ct)))
            .WithName("GetBatchesByStatus");

        group.MapPost("/batches", async (CreateContributionBatchCommand cmd, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(cmd, ct);
            return Results.Created($"/api/minimal/batches/{result.ContributionBatchNo}", result);
        }).WithName("CreateBatchMinimal");

        group.MapPost("/batches/{batchNo:long}/post", async (long batchNo, long postedByUserId, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new PostContributionBatchCommand(batchNo, postedByUserId), ct)))
            .WithName("PostBatchMinimal");

        group.MapPost("/batches/process", async (ProcessMonthlyContributionCommand cmd, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(cmd, ct)))
            .WithName("ProcessMonthlyMinimal");

        // Contribution Details
        group.MapGet("/details/batch/{batchNo:decimal}", async (decimal batchNo, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetContributionDetailsByBatchQuery(batchNo), ct)))
            .WithName("GetDetailsByBatch");

        group.MapGet("/details/member/{memberNo:decimal}", async (decimal memberNo, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetContributionDetailsByMemberQuery(memberNo), ct)))
            .WithName("GetDetailsByMember");

        // Summary
        group.MapGet("/summary", async (DateTime startDate, DateTime endDate, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetContributionSummaryQuery(startDate, endDate), ct)))
            .WithName("GetSummaryMinimal");

        // Superannuation Batches
        group.MapGet("/superannuation/batches", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllSuperannuationBatchesQuery(), ct)))
            .WithName("GetAllSnBatches");

        group.MapGet("/superannuation/batches/{batchNo:long}", async (long batchNo, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetSuperannuationBatchByIdQuery(batchNo), ct)))
            .WithName("GetSnBatchById");

        return app;
    }
}
