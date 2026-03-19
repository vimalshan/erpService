using HotChocolate;
using MediatR;
using PurchaseSalesService.Application.DTOs;
using PurchaseSalesService.Application.Purchases.Queries.GetAllPurchases;
using PurchaseSalesService.Application.Purchases.Queries.GetPurchaseById;
using PurchaseSalesService.Application.Sales.Queries.GetAllSales;
using PurchaseSalesService.Application.Sales.Queries.GetSaleById;

namespace PurchaseSalesService.API.GraphQL;

public sealed class Query
{
    public async Task<IEnumerable<PurchaseDetailDto>> GetPurchasesAsync(
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllPurchasesQuery(), ct);

    public async Task<PurchaseDetailDto?> GetPurchaseAsync(
        long serialNumber,
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(new GetPurchaseByIdQuery(serialNumber), ct);

    public async Task<IEnumerable<SaleMainDto>> GetSalesAsync(
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllSalesQuery(), ct);

    public async Task<SaleMainDto?> GetSaleAsync(
        long serialNumber,
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(new GetSaleByIdQuery(serialNumber), ct);
}
