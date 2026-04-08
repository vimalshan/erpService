using DispatchPlanning.Application.Features.DispatchPlans.Commands;
using MediatR;

namespace DispatchPlanning.API.GraphQL;

public class DispatchPlanMutation
{
    public async Task<int> CreateDispatchPlanAsync(
        string planType, DateTime planMonth, int companyUnitId, int modifiedBy,
        string? mPlus1, string? mPlus2, string? mPlus3, string? mPlus4,
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(
            new CreateDispatchPlanCommand(planType[0], planMonth, companyUnitId, modifiedBy, mPlus1, mPlus2, mPlus3, mPlus4),
            ct);

    public async Task<bool> AddDispatchPlanItemAsync(
        int planHeaderId, int breakupItemId,
        long? w1, long? w2, long? w3, long? w4, long? w5,
        long? m1, long? m2, long? m3, long? m4,
        int modifiedBy,
        [Service] IMediator mediator,
        CancellationToken ct)
    {
        await mediator.Send(
            new AddDispatchPlanItemCommand(planHeaderId, breakupItemId, w1, w2, w3, w4, w5, m1, m2, m3, m4, modifiedBy),
            ct);
        return true;
    }

    public async Task<bool> DeleteDispatchPlanAsync(int planHeaderId, int deletedBy,
        [Service] IMediator mediator, CancellationToken ct)
    {
        await mediator.Send(new DeleteDispatchPlanCommand(planHeaderId, deletedBy), ct);
        return true;
    }
}
