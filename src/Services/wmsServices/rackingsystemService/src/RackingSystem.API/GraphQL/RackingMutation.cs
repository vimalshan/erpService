using MediatR;
using RackingSystem.Application.Features.Bins.Commands;
using RackingSystem.Application.Features.Bins.DTOs;
using RackingSystem.Application.Features.Racks.Commands.CreateRack;
using RackingSystem.Application.Features.Racks.DTOs;

namespace RackingSystem.API.GraphQL;

/// <summary>Hot Chocolate GraphQL Mutation type.</summary>
public sealed class RackingMutation
{
    public async Task<RackDto> CreateRackAsync(
        [Service] IMediator mediator,
        CreateRackInput input,
        CancellationToken ct)
    {
        return await mediator.Send(
            new CreateRackCommand(input.ZoneId, input.Code, input.RackType, input.MaxLoadWeight), ct);
    }

    public async Task<BinDto> UpdateBinStatusAsync(
        [Service] IMediator mediator,
        int binId,
        string newStatus,
        CancellationToken ct)
    {
        return await mediator.Send(new UpdateBinStatusCommand(binId, newStatus), ct);
    }
}

public record CreateRackInput(int ZoneId, string Code, string? RackType, decimal? MaxLoadWeight);
