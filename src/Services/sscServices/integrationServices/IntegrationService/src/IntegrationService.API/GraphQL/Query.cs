using IntegrationService.Application.DTOs;
using IntegrationService.Application.OrganizationUnits.Queries;
using IntegrationService.Application.PurchaseOrders.Queries;
using IntegrationService.Application.Vendors.Queries;
using MediatR;

namespace IntegrationService.API.GraphQL;

public class Query
{
    [UseFiltering]
    [UseSorting]
    public async Task<IEnumerable<PurchaseOrderDto>> GetPurchaseOrders(
        [Service] IMediator mediator, CancellationToken cancellationToken)
        => await mediator.Send(new GetAllPurchaseOrdersQuery(), cancellationToken);

    public async Task<PurchaseOrderDto?> GetPurchaseOrderById(
        long id, [Service] IMediator mediator, CancellationToken cancellationToken)
        => await mediator.Send(new GetPurchaseOrderByIdQuery(id), cancellationToken);

    public async Task<PurchaseOrderDto?> GetPurchaseOrderWithMrc(
        long id, [Service] IMediator mediator, CancellationToken cancellationToken)
        => await mediator.Send(new GetPurchaseOrderWithMrcQuery(id), cancellationToken);

    [UseFiltering]
    [UseSorting]
    public async Task<IEnumerable<VendorDto>> GetVendors(
        [Service] IMediator mediator, CancellationToken cancellationToken)
        => await mediator.Send(new GetAllVendorsQuery(), cancellationToken);

    public async Task<VendorDto?> GetVendorById(
        int id, [Service] IMediator mediator, CancellationToken cancellationToken)
        => await mediator.Send(new GetVendorByIdQuery(id), cancellationToken);

    public async Task<VendorDto?> GetVendorWithSites(
        int id, [Service] IMediator mediator, CancellationToken cancellationToken)
        => await mediator.Send(new GetVendorWithSitesQuery(id), cancellationToken);

    [UseFiltering]
    [UseSorting]
    public async Task<IEnumerable<OrganizationUnitDto>> GetOrganizationUnits(
        [Service] IMediator mediator, CancellationToken cancellationToken)
        => await mediator.Send(new GetAllOrganizationUnitsQuery(), cancellationToken);

    public async Task<OrganizationUnitDto?> GetOrganizationUnitById(
        string id, [Service] IMediator mediator, CancellationToken cancellationToken)
        => await mediator.Send(new GetOrganizationUnitByIdQuery(id), cancellationToken);
}
