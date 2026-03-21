using MediatR;
using InventoryService.Application.Commands.ReceiveStock;
using InventoryService.Application.Commands.TransferInventory;
using InventoryService.Application.Commands.AdjustInventory;
using InventoryService.Application.Commands.AllocateStock;
using InventoryService.Application.DTOs;

namespace InventoryService.API.GraphQL;

public class InventoryMutation
{
    public async Task<StockLevelDto> ReceiveStock(
        [Service] IMediator mediator,
        ReceiveStockCommand input)
    {
        return await mediator.Send(input);
    }

    public async Task<bool> TransferInventory(
        [Service] IMediator mediator,
        TransferInventoryCommand input)
    {
        await mediator.Send(input);
        return true;
    }

    public async Task<StockLevelDto> AdjustInventory(
        [Service] IMediator mediator,
        AdjustInventoryCommand input)
    {
        return await mediator.Send(input);
    }

    public async Task<bool> AllocateStock(
        [Service] IMediator mediator,
        AllocateStockCommand input)
    {
        await mediator.Send(input);
        return true;
    }
}
