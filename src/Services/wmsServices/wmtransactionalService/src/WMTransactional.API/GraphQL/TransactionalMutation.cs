using MediatR;
using WMTransactional.Application.Commands.CreatePurchaseOrder;
using WMTransactional.Application.Commands.ConfirmPurchaseOrder;
using WMTransactional.Application.Commands.CancelPurchaseOrder;
using WMTransactional.Application.Commands.CreateReceiving;
using WMTransactional.Application.Commands.CloseReceiving;
using WMTransactional.Application.Commands.CreateSalesOrder;
using WMTransactional.Application.Commands.ConfirmSalesOrder;
using WMTransactional.Application.Commands.CancelSalesOrder;
using WMTransactional.Application.Commands.CreateShipment;
using WMTransactional.Application.Commands.ShipShipment;
using WMTransactional.Application.DTOs;

namespace WMTransactional.API.GraphQL;

public class TransactionalMutation
{
    public async Task<PurchaseOrderDto> CreatePurchaseOrder(
        [Service] IMediator mediator,
        CreatePurchaseOrderCommand input)
    {
        return await mediator.Send(input);
    }

    public async Task<bool> ConfirmPurchaseOrder(
        [Service] IMediator mediator,
        ConfirmPurchaseOrderCommand input)
    {
        await mediator.Send(input);
        return true;
    }

    public async Task<bool> CancelPurchaseOrder(
        [Service] IMediator mediator,
        CancelPurchaseOrderCommand input)
    {
        await mediator.Send(input);
        return true;
    }

    public async Task<ReceivingDto> CreateReceiving(
        [Service] IMediator mediator,
        CreateReceivingCommand input)
    {
        return await mediator.Send(input);
    }

    public async Task<bool> CloseReceiving(
        [Service] IMediator mediator,
        CloseReceivingCommand input)
    {
        await mediator.Send(input);
        return true;
    }

    public async Task<SalesOrderDto> CreateSalesOrder(
        [Service] IMediator mediator,
        CreateSalesOrderCommand input)
    {
        return await mediator.Send(input);
    }

    public async Task<bool> ConfirmSalesOrder(
        [Service] IMediator mediator,
        ConfirmSalesOrderCommand input)
    {
        await mediator.Send(input);
        return true;
    }

    public async Task<bool> CancelSalesOrder(
        [Service] IMediator mediator,
        CancelSalesOrderCommand input)
    {
        await mediator.Send(input);
        return true;
    }

    public async Task<ShipmentDto> CreateShipment(
        [Service] IMediator mediator,
        CreateShipmentCommand input)
    {
        return await mediator.Send(input);
    }

    public async Task<bool> ShipShipment(
        [Service] IMediator mediator,
        ShipShipmentCommand input)
    {
        await mediator.Send(input);
        return true;
    }
}
