using MediatR;
using PurchaseOrderService.Application.Commands.CancelPurchaseOrder;
using PurchaseOrderService.Application.Commands.ConfirmPurchaseOrder;
using PurchaseOrderService.Application.Commands.CreatePurchaseOrder;
using PurchaseOrderService.Application.Commands.ReceivePurchaseOrderLine;
using PurchaseOrderService.Application.Commands.UpdatePurchaseOrder;

namespace PurchaseOrderService.API.GraphQL;

public class PurchaseOrderMutation
{
    public async Task<int> CreatePurchaseOrder(CreatePurchaseOrderCommand input, [Service] IMediator mediator)
    {
        return await mediator.Send(input);
    }

    public async Task<bool> UpdatePurchaseOrder(UpdatePurchaseOrderCommand input, [Service] IMediator mediator)
    {
        await mediator.Send(input);
        return true;
    }

    public async Task<bool> ConfirmPurchaseOrder(int poId, [Service] IMediator mediator)
    {
        await mediator.Send(new ConfirmPurchaseOrderCommand(poId));
        return true;
    }

    public async Task<bool> CancelPurchaseOrder(int poId, [Service] IMediator mediator)
    {
        await mediator.Send(new CancelPurchaseOrderCommand(poId));
        return true;
    }

    public async Task<bool> ReceivePurchaseOrderLine(ReceivePurchaseOrderLineCommand input, [Service] IMediator mediator)
    {
        await mediator.Send(input);
        return true;
    }
}
