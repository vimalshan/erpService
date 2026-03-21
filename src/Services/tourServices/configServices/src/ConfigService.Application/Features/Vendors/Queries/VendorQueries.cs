using ConfigService.Application.DTOs;
using MediatR;

namespace ConfigService.Application.Features.Vendors.Queries;

public record GetAllVendorsQuery : IRequest<IReadOnlyList<VendorDto>>;
public record GetVendorByIdQuery(string Id) : IRequest<VendorDto?>;
public record GetActiveVendorsQuery : IRequest<IReadOnlyList<VendorDto>>;
