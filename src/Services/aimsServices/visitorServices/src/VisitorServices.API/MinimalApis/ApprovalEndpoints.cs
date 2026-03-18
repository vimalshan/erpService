using MediatR;
using VisitorServices.Application.Approvals.Commands.ProcessApproval;
using VisitorServices.Application.Approvals.Queries.GetPendingApprovals;

namespace VisitorServices.API.MinimalApis;

public static class ApprovalEndpoints
{
    public static IEndpointRouteBuilder MapApprovalEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/approvals")
            .WithTags("Approvals v2 (Minimal APIs)")
            .RequireAuthorization();

        group.MapGet("/pending", async (long approverId, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetPendingApprovalsQuery(approverId), ct);
            return Results.Ok(result);
        }).WithName("GetPendingApprovalsMinimal").WithSummary("Get pending approvals");

        group.MapPost("/{id:long}/process", async (long id, ProcessApprovalCommand command, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(command with { RequestId = id }, ct);
            return Results.Ok(result);
        }).WithName("ProcessApprovalMinimal").WithSummary("Process approval request");

        return app;
    }
}
