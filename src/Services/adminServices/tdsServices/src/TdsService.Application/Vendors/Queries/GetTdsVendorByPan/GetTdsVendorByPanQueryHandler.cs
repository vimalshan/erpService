using MediatR;
using TdsService.Application.DTOs;
using TdsService.Domain.Repositories;

namespace TdsService.Application.Vendors.Queries.GetTdsVendorByPan;

public sealed class GetTdsVendorByPanQueryHandler
    : IRequestHandler<GetTdsVendorByPanQuery, TdsVendorDto?>
{
    private readonly ITdsVendorRepository _repository;

    public GetTdsVendorByPanQueryHandler(ITdsVendorRepository repository)
        => _repository = repository;

    public async Task<TdsVendorDto?> Handle(
        GetTdsVendorByPanQuery request,
        CancellationToken cancellationToken)
    {
        var vendor = await _repository.GetByPanAsync(request.PanNo, cancellationToken);
        if (vendor is null) return null;

        return new TdsVendorDto(
            vendor.Id,
            vendor.VendorName,
            vendor.EmailAddress?.Value,
            vendor.PanNumber?.Value);
    }
}
