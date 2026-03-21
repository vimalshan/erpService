using MediatR;
using SalesOrderService.Application.SalesOrders.Commands.CreateSalesOrder;
using SalesOrderService.Application.SalesOrders.Commands.ConfirmSalesOrder;
using SalesOrderService.Application.SalesOrders.Commands.CancelSalesOrder;
using SalesOrderService.Application.SalesOrders.DTOs;
using SalesOrderService.Application.SalesOrders.Queries.GetSalesOrderById;

namespace SalesOrderService.API.GraphQL;

public sealed class SalesOrderMutation
{
    public async Task<SalesOrderDto> CreateSalesOrderAsync(
        CreateSalesOrderInput input,
        [Service] ISender mediator,
        CancellationToken cancellationToken)
    {
        var command = new CreateSalesOrderCommand(
            input.SoNumber, input.CustomerId, input.WarehouseId,
            input.OrderDate, input.RequestedDate, input.Notes, input.CreatedBy,
            input.Lines.Select(l => new CreateSalesOrderLineRequest(
                l.ProductId, l.LineNumber, l.QuantityOrdered, l.UnitPrice, l.Discount, l.Notes))
            .ToList());

        return await mediator.Send(command, cancellationToken);
    }

    public async Task<SalesOrderDto> ConfirmSalesOrderAsync(
        int soId,
        [Service] ISender mediator,
        CancellationToken cancellationToken)
    {
        await mediator.Send(new ConfirmSalesOrderCommand(soId), cancellationToken);
        return (await mediator.Send(new GetSalesOrderByIdQuery(soId), cancellationToken))!;
    }

    public async Task<SalesOrderDto> CancelSalesOrderAsync(
        int soId,
        string reason,
        [Service] ISender mediator,
        CancellationToken cancellationToken)
    {
        await mediator.Send(new CancelSalesOrderCommand(soId, reason), cancellationToken);
        return (await mediator.Send(new GetSalesOrderByIdQuery(soId), cancellationToken))!;
    }
}

public sealed record CreateSalesOrderInput(
    string SoNumber,
    int CustomerId,
    int WarehouseId,
    DateOnly OrderDate,
    DateOnly? RequestedDate,
    string? Notes,
    string? CreatedBy,
    IReadOnlyList<CreateLineInput> Lines);

public sealed record CreateLineInput(
    int ProductId,
    int LineNumber,
    decimal QuantityOrdered,
    decimal? UnitPrice,
    decimal Discount,
    string? Notes);
