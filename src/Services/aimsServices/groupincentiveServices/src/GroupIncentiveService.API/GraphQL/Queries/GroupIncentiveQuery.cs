using GroupIncentiveService.Application.DTOs;
using GroupIncentiveService.Application.Queries.GetAllGroups;
using GroupIncentiveService.Application.Queries.GetGroupById;
using GroupIncentiveService.Application.Queries.GetGroupIncentiveById;
using GroupIncentiveService.Application.Queries.GetGroupIncentives;
using GroupIncentiveService.Infrastructure.Persistence;
using HotChocolate;
using HotChocolate.Data;
using MediatR;

namespace GroupIncentiveService.API.GraphQL.Queries;

public class GroupIncentiveQuery
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public async Task<IEnumerable<GroupMasterDto>> GetGroups(
        [Service] IMediator mediator, bool activeOnly = true, CancellationToken ct = default)
        => await mediator.Send(new GetAllGroupsQuery(activeOnly), ct);

    public async Task<GroupMasterDto> GetGroupById(
        [Service] IMediator mediator, int id, CancellationToken ct = default)
        => await mediator.Send(new GetGroupByIdQuery(id), ct);

    public async Task<GroupIncentiveMainDto> GetGroupIncentive(
        [Service] IMediator mediator, long id, CancellationToken ct = default)
        => await mediator.Send(new GetGroupIncentiveByIdQuery(id), ct);

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public async Task<IEnumerable<GroupIncentiveMainDto>> GetGroupIncentives(
        [Service] IMediator mediator, int groupId, CancellationToken ct = default)
        => await mediator.Send(new GetGroupIncentivesQuery(groupId), ct);

    [UseProjection]
    [UseFiltering]
    public async Task<IEnumerable<GroupIncentiveMainDto>> GetPendingIncentives(
        [Service] IMediator mediator, CancellationToken ct = default)
        => await mediator.Send(new GetPendingIncentivesQuery(), ct);
}
