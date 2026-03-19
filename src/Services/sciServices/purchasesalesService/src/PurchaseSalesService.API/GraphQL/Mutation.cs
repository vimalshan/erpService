using HotChocolate;
using MediatR;
using PurchaseSalesService.Application.DTOs;
using PurchaseSalesService.Application.Purchases.Commands.CancelPurchase;
using PurchaseSalesService.Application.Purchases.Commands.CreatePurchase;
using PurchaseSalesService.Application.Sales.Commands.CancelSale;
using PurchaseSalesService.Application.Sales.Commands.CreateSale;

namespace PurchaseSalesService.API.GraphQL;

public sealed class Mutation
{
    public async Task<PurchaseDetailDto> CreatePurchaseAsync(
        long trackingNumber, long purposeCode, long stageCode,
        string? supplierCode, string userId, long userNumber,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new CreatePurchaseCommand(
            trackingNumber, purposeCode, stageCode, supplierCode, userId, userNumber), ct);

    public async Task<bool> CancelPurchaseAsync(
        long serialNumber, string cancelledBy,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new CancelPurchaseCommand(serialNumber, cancelledBy), ct);

    public async Task<SaleMainDto> CreateSaleAsync(
        long trackingNumber, long purposeCode, long stageCode,
        string userId, long userNumber, string? vehicleCustomer,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new CreateSaleCommand(
            trackingNumber, purposeCode, stageCode, userId, userNumber, vehicleCustomer), ct);

    public async Task<bool> CancelSaleAsync(
        long serialNumber, string cancelledBy,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new CancelSaleCommand(serialNumber, cancelledBy), ct);
}
