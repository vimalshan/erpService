using MediatR;
using VendorService.Application.Commands;

namespace VendorService.API.GraphQL;

public sealed class VendorMutation
{
    public async Task<long> CreateVendor(
        long categoryId,
        long locationId,
        string name,
        string address,
        long updatedBy,
        string? email,
        char liveStatus,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(
            new CreateVendorCommand(categoryId, locationId, name, email, address, updatedBy, liveStatus),
            cancellationToken);
    }

    public async Task<bool> UpdateVendor(
        long vendorId,
        long categoryId,
        long locationId,
        string name,
        string address,
        long updatedBy,
        char liveStatus,
        string? email,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(
            new UpdateVendorCommand(vendorId, categoryId, locationId, name, email, address, updatedBy, liveStatus),
            cancellationToken);
    }

    public async Task<bool> DeactivateVendor(
        long vendorId,
        long updatedBy,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new DeactivateVendorCommand(vendorId, updatedBy), cancellationToken);
    }
}
