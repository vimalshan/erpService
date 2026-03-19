using AutoMapper;
using IntegrationService.Application.DTOs;
using IntegrationService.Domain.Interfaces;
using MediatR;

namespace IntegrationService.Application.Vendors.Queries;

public class GetVendorByIdHandler(
    IVendorRepository repository,
    IMapper mapper) : IRequestHandler<GetVendorByIdQuery, VendorDto?>
{
    public async Task<VendorDto?> Handle(GetVendorByIdQuery request, CancellationToken cancellationToken)
    {
        var vendor = await repository.GetByIdAsync(request.VendorId, cancellationToken);
        return vendor is null ? null : mapper.Map<VendorDto>(vendor);
    }
}

public class GetAllVendorsHandler(
    IVendorRepository repository,
    IMapper mapper) : IRequestHandler<GetAllVendorsQuery, IEnumerable<VendorDto>>
{
    public async Task<IEnumerable<VendorDto>> Handle(GetAllVendorsQuery request, CancellationToken cancellationToken)
    {
        var vendors = await repository.GetAllAsync(cancellationToken);
        return mapper.Map<IEnumerable<VendorDto>>(vendors);
    }
}

public class GetVendorWithSitesHandler(
    IVendorRepository repository,
    IMapper mapper) : IRequestHandler<GetVendorWithSitesQuery, VendorDto?>
{
    public async Task<VendorDto?> Handle(GetVendorWithSitesQuery request, CancellationToken cancellationToken)
    {
        var vendor = await repository.GetWithSitesAsync(request.VendorId, cancellationToken);
        return vendor is null ? null : mapper.Map<VendorDto>(vendor);
    }
}
