using DispatchPlanning.Application.DTOs;
using DispatchPlanning.Application.Features.DispatchPlans.Queries;
using MediatR;

namespace DispatchPlanning.API.GraphQL;

public class DispatchPlanQuery
{
    public async Task<DispatchPlanDetailDto?> GetDispatchPlanByIdAsync(
        int planHeaderId,
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(new GetDispatchPlanByIdQuery(planHeaderId), ct);

    public async Task<IEnumerable<DispatchPlanHeaderDto>> GetDispatchPlansAsync(
        int companyUnitId,
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(new GetAllDispatchPlansQuery(companyUnitId), ct);

    public async Task<IEnumerable<MainGroupDto>> GetMainGroupsAsync(
        int companyUnitId,
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(new GetAllMainGroupsQuery(companyUnitId), ct);

    public async Task<IEnumerable<SubGroupDto>> GetSubGroupsByMainGroupAsync(
        int mainGroupId,
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(new GetSubGroupsByMainGroupQuery(mainGroupId), ct);

    public async Task<IEnumerable<BreakupItemDto>> GetBreakupItemsBySubGroupAsync(
        int subGroupId,
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(new GetBreakupItemsBySubGroupQuery(subGroupId), ct);
}
