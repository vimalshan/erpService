using MediatR;
using VendorService.Application.DTOs;

namespace VendorService.Application.Queries;

public sealed record GetVendorByIdQuery(long Id) : IRequest<VendorDto?>;
