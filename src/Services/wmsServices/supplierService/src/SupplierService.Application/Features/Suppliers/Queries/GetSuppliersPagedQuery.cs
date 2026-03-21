using MediatR;
using SupplierService.Application.DTOs;

namespace SupplierService.Application.Features.Suppliers.Queries;

public record GetSuppliersPagedQuery(int Page = 1, int PageSize = 10, string? Search = null)
    : IRequest<PagedResultDto<SupplierDto>>;
