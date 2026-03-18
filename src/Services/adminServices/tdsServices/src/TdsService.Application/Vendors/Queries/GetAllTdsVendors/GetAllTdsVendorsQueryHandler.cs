using MediatR;
using TdsService.Application.DTOs;
using TdsService.Domain.Repositories;

namespace TdsService.Application.Vendors.Queries.GetAllTdsVendors;

public sealed class GetAllTdsVendorsQueryHandler
    : IRequestHandler<GetAllTdsVendorsQuery, PagedResult<TdsVendorDto>>
{
    private readonly ITdsVendorRepository _repository;

    public GetAllTdsVendorsQueryHandler(ITdsVendorRepository repository)
        => _repository = repository;

    public async Task<PagedResult<TdsVendorDto>> Handle(
        GetAllTdsVendorsQuery request,
        CancellationToken cancellationToken)
    {
        var all = await _repository.GetAllAsync(cancellationToken);
        var totalCount = all.Count;

        var paged = all
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(v => new TdsVendorDto(
                v.Id,
                v.VendorName,
                v.EmailAddress?.Value,
                v.PanNumber?.Value))
            .ToList();

        return new PagedResult<TdsVendorDto>(paged, totalCount, request.Page, request.PageSize);
    }
}
