using TdsService.Application.DTOs;
using TdsService.Application.Files.Queries.GetAllTdsFiles;
using TdsService.Application.Files.Queries.GetTdsFileById;
using TdsService.Application.Vendors.Queries.GetAllTdsVendors;
using TdsService.Application.Vendors.Queries.GetTdsVendorByPan;
using MediatR;

namespace TdsService.API.GraphQL;

[QueryType]
public sealed class TdsQuery
{
    public async Task<PagedResult<TdsVendorDto>> GetVendors(
        [Service] IMediator mediator,
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default)
        => await mediator.Send(new GetAllTdsVendorsQuery(page, pageSize), ct);

    public async Task<TdsVendorDto?> GetVendorByPan(
        [Service] IMediator mediator,
        string panNo,
        CancellationToken ct = default)
        => await mediator.Send(new GetTdsVendorByPanQuery(panNo), ct);

    public async Task<PagedResult<TdsFileDto>> GetFiles(
        [Service] IMediator mediator,
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default)
        => await mediator.Send(new GetAllTdsFilesQuery(page, pageSize), ct);

    public async Task<TdsFileDto?> GetFileById(
        [Service] IMediator mediator,
        long fileId,
        CancellationToken ct = default)
        => await mediator.Send(new GetTdsFileByIdQuery(fileId), ct);
}
