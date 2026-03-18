using MediatR;
using VendorService.Application.Commands;
using VendorService.Application.Queries;
using VendorService.Application.DTOs;

namespace VendorService.API.GraphQL;

public sealed class VendorQuery
{
    public async Task<IEnumerable<VendorDto>> GetVendors(
        [Service] IMediator mediator,
        char? status = null,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new GetAllVendorsQuery(status), cancellationToken);
    }

    public async Task<VendorDto?> GetVendorById(
        long id,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new GetVendorByIdQuery(id), cancellationToken);
    }
}
