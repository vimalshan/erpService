using GroupIncentiveService.Application.Commands.ApproveGroupIncentive;
using GroupIncentiveService.Application.Commands.CreateGroupIncentive;
using GroupIncentiveService.Application.Commands.CreateGroupMaster;
using GroupIncentiveService.Application.Commands.RejectGroupIncentive;
using HotChocolate;
using MediatR;

namespace GroupIncentiveService.API.GraphQL.Mutations;

public class GroupIncentiveMutation
{
    public async Task<int> CreateGroup(
        [Service] IMediator mediator,
        CreateGroupMasterCommand input,
        CancellationToken ct = default)
        => await mediator.Send(input, ct);

    public async Task<long> CreateGroupIncentive(
        [Service] IMediator mediator,
        CreateGroupIncentiveCommand input,
        CancellationToken ct = default)
        => await mediator.Send(input, ct);

    public async Task<bool> ApproveGroupIncentive(
        [Service] IMediator mediator,
        ApproveGroupIncentiveCommand input,
        CancellationToken ct = default)
    {
        await mediator.Send(input, ct);
        return true;
    }

    public async Task<bool> RejectGroupIncentive(
        [Service] IMediator mediator,
        RejectGroupIncentiveCommand input,
        CancellationToken ct = default)
    {
        await mediator.Send(input, ct);
        return true;
    }
}
