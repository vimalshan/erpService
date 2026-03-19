using MediatR;
using ApprovalGroup.Application.ApprovalGroups.Queries;
using ApprovalGroup.Application.PullMatrix.Queries;
using ApprovalGroup.Infrastructure.Repositories;

namespace ApprovalGroup.API.MinimalApis;

public static class MinimalApiEndpoints
{
    public static WebApplication MapMinimalApiEndpoints(this WebApplication app)
    {
        var groups = app.MapGroup("/api/v2/approval-groups")
            .WithTags("ApprovalGroups-MinimalAPI")
            .RequireAuthorization();

        groups.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllApprovalGroupsQuery(), ct)));

        groups.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetApprovalGroupByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        // Paged list using Dapper
        var paged = app.MapGroup("/api/v2/approval-groups/paged")
            .WithTags("ApprovalGroups-Paged")
            .RequireAuthorization();

        paged.MapGet("/", async (int page, int pageSize, IApprovalGroupDapperQuery dapper, CancellationToken ct) =>
            Results.Ok(await dapper.GetApprovalGroupsPagedAsync(page, pageSize, ct)));

        // Pull Matrix minimal APIs
        var pullMatrix = app.MapGroup("/api/v2/pull-matrix")
            .WithTags("PullMatrix-MinimalAPI")
            .RequireAuthorization();

        pullMatrix.MapGet("/{matId:long}", async (long matId, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetPullMatrixByIdQuery(matId), ct)));

        return app;
    }
}
