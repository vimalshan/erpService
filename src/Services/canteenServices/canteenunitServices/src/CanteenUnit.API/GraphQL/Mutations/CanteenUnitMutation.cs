using CanteenUnit.Application.DTOs;
using CanteenUnit.Application.Features.CanteenUnits.Commands.CreateCanteenUnit;
using CanteenUnit.Application.Features.CanteenUnits.Commands.DeleteCanteenUnit;
using CanteenUnit.Application.Features.CanteenUnits.Commands.UpdateCanteenUnit;
using HotChocolate;
using MediatR;

namespace CanteenUnit.API.GraphQL.Mutations;

public class CanteenUnitMutation
{
    public async Task<CanteenUnitMasterDto> CreateCanteenUnitAsync(
        CreateCanteenUnitCommand input,
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<bool> UpdateCanteenUnitAsync(
        UpdateCanteenUnitCommand input,
        [Service] IMediator mediator,
        CancellationToken ct)
    {
        await mediator.Send(input, ct);
        return true;
    }

    public async Task<bool> DeleteCanteenUnitAsync(
        decimal comCode,
        [Service] IMediator mediator,
        CancellationToken ct)
    {
        await mediator.Send(new DeleteCanteenUnitCommand(comCode), ct);
        return true;
    }
}
