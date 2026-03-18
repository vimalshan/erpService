using DealTicketing.Application.Features.DealDetails.Queries;
using MediatR;

namespace DealTicketing.API.Endpoints;

/// <summary>Minimal API endpoints as an alternative to traditional controllers.</summary>
public static class DealMinimalEndpoints
{
    public static void MapDealEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v2/deals")
            .WithTags("Deals (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/pending-approvals", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetPendingApprovalsQuery(), ct);
            return Results.Ok(result);
        })
        .WithName("GetPendingApprovalsMinimal")
        .WithSummary("Get all deals pending approval (minimal API)")
        .Produces(200);

        group.MapGet("/{id:long}/settlements", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetDealSettlementsByDealQuery(id), ct);
            return Results.Ok(result);
        })
        .WithName("GetDealSettlementsMinimal")
        .Produces(200)
        .Produces(404);

        group.MapGet("/summary", async (
            DateTime fromDate, DateTime toDate,
            DealTicketing.Infrastructure.ReadRepositories.DealDapperReadRepository dapperRepo,
            CancellationToken ct) =>
        {
            var result = await dapperRepo.GetDealSummaryAsync(fromDate, toDate, ct);
            return Results.Ok(result);
        })
        .WithName("GetDealSummaryMinimal")
        .Produces(200);
    }
}
