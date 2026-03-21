using HotChocolate;
using MediatR;
using TourPlanService.Application.Commands.ApproveTourPlan;
using TourPlanService.Application.Commands.CreateTourPlan;

namespace TourPlanService.API.GraphQL;

public sealed class TourPlanMutation
{
    public async Task<string> CreateTourPlan(
        CreateTourPlanCommand input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(input, cancellationToken);
        if (!result.IsSuccess)
            throw new GraphQLException(result.Error ?? "Failed to create tour plan.");
        return result.Value!;
    }

    public async Task<bool> ApproveTourPlan(
        string tpId, string approvedBy, string? remarks,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new ApproveTourPlanCommand(tpId, approvedBy, remarks), cancellationToken);
        if (!result.IsSuccess)
            throw new GraphQLException(result.Error ?? "Failed to approve tour plan.");
        return true;
    }

    public async Task<bool> RejectTourPlan(
        string tpId, string rejectedBy, string remarks,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new RejectTourPlanCommand(tpId, rejectedBy, remarks), cancellationToken);
        if (!result.IsSuccess)
            throw new GraphQLException(result.Error ?? "Failed to reject tour plan.");
        return true;
    }
}
