using MediatR;
using SupplierService.Application.DTOs;
using SupplierService.Application.Features.Suppliers.Commands;

namespace SupplierService.API.GraphQL;

public class SupplierMutation
{
    public async Task<SupplierDto> CreateSupplier([Service] IMediator mediator, CreateSupplierDto input)
    {
        return await mediator.Send(new CreateSupplierCommand(input));
    }

    public async Task<SupplierDto> UpdateSupplier([Service] IMediator mediator, int id, UpdateSupplierDto input)
    {
        return await mediator.Send(new UpdateSupplierCommand(id, input));
    }

    public async Task<bool> DeleteSupplier([Service] IMediator mediator, int id)
    {
        await mediator.Send(new DeleteSupplierCommand(id));
        return true;
    }

    public async Task<bool> ActivateSupplier([Service] IMediator mediator, int id)
    {
        await mediator.Send(new ActivateSupplierCommand(id));
        return true;
    }

    public async Task<bool> DeactivateSupplier([Service] IMediator mediator, int id)
    {
        await mediator.Send(new DeactivateSupplierCommand(id));
        return true;
    }
}
