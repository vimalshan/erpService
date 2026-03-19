using ApprovalGroup.Application.DTOs;
using ApprovalGroup.Application.ApprovalGroups.Queries;
using ApprovalGroup.Application.PullMatrix.Queries;
using MediatR;

namespace ApprovalGroup.API.GraphQL;

public class ApprovalGroupQuery
{
    public async Task<IEnumerable<ApprovalGroupDto>> GetApprovalGroups([Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(new GetAllApprovalGroupsQuery(), ct);

    public async Task<ApprovalGroupDto> GetApprovalGroupById([Service] IMediator mediator,
        long groupId, CancellationToken ct)
        => await mediator.Send(new GetApprovalGroupByIdQuery(groupId), ct);

    public async Task<IEnumerable<PullMatrixDetailDto>> GetPullMatrixByUnitId(
        [Service] IMediator mediator, long unitId, CancellationToken ct)
        => await mediator.Send(new GetPullMatrixByUnitIdQuery(unitId), ct);
}

public class ApprovalGroupMutation
{
    public async Task<ApprovalGroupDto> CreateApprovalGroup(
        [Service] IMediator mediator,
        string groupName, long createdBy, long? priorityId,
        CancellationToken ct)
        => await mediator.Send(new Application.ApprovalGroups.Commands.CreateApprovalGroupCommand(groupName, createdBy, priorityId), ct);

    public async Task<ApprovalGroupDto> UpdateApprovalGroup(
        [Service] IMediator mediator,
        long groupId, string groupName, long modifiedBy, long? priorityId,
        CancellationToken ct)
        => await mediator.Send(new Application.ApprovalGroups.Commands.UpdateApprovalGroupCommand(groupId, groupName, modifiedBy, priorityId), ct);
}
