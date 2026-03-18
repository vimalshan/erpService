using MediatR;
using VendorService.Application.DTOs;

namespace VendorService.Application.Queries;

public sealed record GetAllVendorsQuery(char? Status = null) : IRequest<IEnumerable<VendorDto>>;
