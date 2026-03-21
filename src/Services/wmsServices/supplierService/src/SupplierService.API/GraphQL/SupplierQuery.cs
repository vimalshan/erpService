using MediatR;
using SupplierService.Application.DTOs;
using SupplierService.Application.Features.Suppliers.Queries;

namespace SupplierService.API.GraphQL;

public class SupplierQuery
{
    [UseFiltering]
    [UseSorting]
    public async Task<IReadOnlyList<SupplierDto>> GetSuppliers([Service] IMediator mediator)
    {
        return await mediator.Send(new GetAllSuppliersQuery());
    }

    public async Task<SupplierDto?> GetSupplierById([Service] IMediator mediator, int id)
    {
        return await mediator.Send(new GetSupplierByIdQuery(id));
    }

    public async Task<PagedResultDto<SupplierDto>> GetSuppliersPaged(
        [Service] IMediator mediator, int page = 1, int pageSize = 10, string? search = null)
    {
        return await mediator.Send(new GetSuppliersPagedQuery(page, pageSize, search));
    }
}
