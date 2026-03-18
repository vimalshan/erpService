using MediatR;
using TdsService.Application.DTOs;

namespace TdsService.Application.Vendors.Queries.GetAllTdsVendors;

public sealed record GetAllTdsVendorsQuery(int Page = 1, int PageSize = 20)
    : IRequest<PagedResult<TdsVendorDto>>;
