using MediatR;
using SalesOrderService.Application.SalesOrders.DTOs;
using SalesOrderService.Application.SalesOrders.Queries.GetAllSalesOrders;
using SalesOrderService.Application.SalesOrders.Queries.GetSalesOrderById;
using SalesOrderService.Application.SalesOrders.Queries.GetSalesOrdersByCustomer;

namespace SalesOrderService.API.GraphQL;

public sealed class SalesOrderQuery
{
    public async Task<IEnumerable<SalesOrderSummaryDto>> GetSalesOrdersAsync(
        [Service] ISender mediator,
        CancellationToken cancellationToken) =>
        await mediator.Send(new GetAllSalesOrdersQuery(), cancellationToken);

    public async Task<SalesOrderDto?> GetSalesOrderByIdAsync(
        int soId,
        [Service] ISender mediator,
        CancellationToken cancellationToken) =>
        await mediator.Send(new GetSalesOrderByIdQuery(soId), cancellationToken);

    public async Task<IEnumerable<SalesOrderSummaryDto>> GetSalesOrdersByCustomerAsync(
        int customerId,
        [Service] ISender mediator,
        CancellationToken cancellationToken) =>
        await mediator.Send(new GetSalesOrdersByCustomerQuery(customerId), cancellationToken);
}
