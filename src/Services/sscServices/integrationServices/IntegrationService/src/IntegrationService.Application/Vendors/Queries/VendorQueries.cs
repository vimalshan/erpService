using IntegrationService.Application.DTOs;
using MediatR;

namespace IntegrationService.Application.Vendors.Queries;

public record GetVendorByIdQuery(int VendorId) : IRequest<VendorDto?>;
public record GetAllVendorsQuery : IRequest<IEnumerable<VendorDto>>;
public record GetVendorWithSitesQuery(int VendorId) : IRequest<VendorDto?>;
