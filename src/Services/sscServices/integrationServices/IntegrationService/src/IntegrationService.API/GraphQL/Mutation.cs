using IntegrationService.Application.DTOs;
using IntegrationService.Application.OrganizationUnits.Commands;
using IntegrationService.Application.PurchaseOrders.Commands;
using IntegrationService.Application.Vendors.Commands;
using MediatR;

namespace IntegrationService.API.GraphQL;

public class Mutation
{
    public async Task<PurchaseOrderDto> CreatePurchaseOrder(
        CreatePurchaseOrderCommand input, [Service] IMediator mediator, CancellationToken cancellationToken)
        => await mediator.Send(input, cancellationToken);

    public async Task<PurchaseOrderDto> UpdatePurchaseOrder(
        UpdatePurchaseOrderCommand input, [Service] IMediator mediator, CancellationToken cancellationToken)
        => await mediator.Send(input, cancellationToken);

    public async Task<bool> DeletePurchaseOrder(
        long poSeqId, [Service] IMediator mediator, CancellationToken cancellationToken)
        => await mediator.Send(new DeletePurchaseOrderCommand(poSeqId), cancellationToken);

    public async Task<MaterialReceiptDto> AddMaterialReceipt(
        AddMaterialReceiptCommand input, [Service] IMediator mediator, CancellationToken cancellationToken)
        => await mediator.Send(input, cancellationToken);

    public async Task<VendorDto> CreateVendor(
        CreateVendorCommand input, [Service] IMediator mediator, CancellationToken cancellationToken)
        => await mediator.Send(input, cancellationToken);

    public async Task<VendorDto> UpdateVendor(
        UpdateVendorCommand input, [Service] IMediator mediator, CancellationToken cancellationToken)
        => await mediator.Send(input, cancellationToken);

    public async Task<bool> DeleteVendor(
        int vendorId, [Service] IMediator mediator, CancellationToken cancellationToken)
        => await mediator.Send(new DeleteVendorCommand(vendorId), cancellationToken);

    public async Task<OrganizationUnitDto> CreateOrganizationUnit(
        CreateOrganizationUnitCommand input, [Service] IMediator mediator, CancellationToken cancellationToken)
        => await mediator.Send(input, cancellationToken);

    public async Task<OrganizationUnitDto> UpdateOrganizationUnit(
        UpdateOrganizationUnitCommand input, [Service] IMediator mediator, CancellationToken cancellationToken)
        => await mediator.Send(input, cancellationToken);

    public async Task<bool> DeleteOrganizationUnit(
        string ouId, [Service] IMediator mediator, CancellationToken cancellationToken)
        => await mediator.Send(new DeleteOrganizationUnitCommand(ouId), cancellationToken);
}
