using MediatR;
using VisitorServices.Application.DTOs;
using VisitorServices.Application.Visitors.Queries.GetActiveVisitors;
using VisitorServices.Application.Visitors.Queries.GetVisitorById;
using VisitorServices.Application.Approvals.Queries.GetPendingApprovals;

namespace VisitorServices.API.GraphQL;

public class Query
{
    [GraphQLDescription("Get a visitor by ID.")]
    public async Task<VisitorDto?> GetVisitorById(
        [Service] ISender sender,
        long id,
        CancellationToken cancellationToken)
        => await sender.Send(new GetVisitorByIdQuery(id), cancellationToken);

    [GraphQLDescription("Get all currently checked-in visitors.")]
    public async Task<IEnumerable<VisitorDto>> GetActiveVisitors(
        [Service] ISender sender,
        CancellationToken cancellationToken)
        => await sender.Send(new GetActiveVisitorsQuery(), cancellationToken);

    [GraphQLDescription("Get pending approval requests for an approver.")]
    public async Task<IEnumerable<ApprovalRequestDto>> GetPendingApprovals(
        [Service] ISender sender,
        long approverId,
        CancellationToken cancellationToken)
        => await sender.Send(new GetPendingApprovalsQuery(approverId), cancellationToken);
}
