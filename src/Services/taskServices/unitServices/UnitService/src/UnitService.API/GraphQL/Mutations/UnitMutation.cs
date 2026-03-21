using MediatR;
using UnitService.Application.Commands.GrantAccess;
using UnitService.Application.Commands.RegisterEquipment;
using UnitService.Application.Commands.UpdateEquipmentStatus;

namespace UnitService.API.GraphQL.Mutations;

public class UnitMutation
{
    public async Task<int> RegisterEquipment(
        RegisterEquipmentCommand input,
        [Service] IMediator mediator)
    {
        return await mediator.Send(input);
    }

    public async Task<int> UpdateEquipmentStatus(
        UpdateEquipmentStatusCommand input,
        [Service] IMediator mediator)
    {
        return await mediator.Send(input);
    }

    public async Task<int> GrantAccess(
        GrantAccessCommand input,
        [Service] IMediator mediator)
    {
        return await mediator.Send(input);
    }
}
