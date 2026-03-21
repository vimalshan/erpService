using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using InventoryService.Application.Commands.ReceiveStock;
using InventoryService.Application.Commands.TransferInventory;
using InventoryService.Application.Commands.AdjustInventory;
using InventoryService.Application.Commands.AllocateStock;
using InventoryService.Application.DTOs;
using InventoryService.Application.Queries.GetStockLevel;
using InventoryService.Application.Queries.GetInventoryByWarehouse;
using InventoryService.Application.Queries.GetAvailableStock;
using InventoryService.Application.Queries.GetTransactionHistory;
using InventoryService.Application.Queries.GetLowStockItems;

namespace InventoryService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InventoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public InventoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("stock/{stockLevelId:long}")]
    public async Task<ActionResult<StockLevelDto>> GetStockLevel(long stockLevelId)
    {
        var result = await _mediator.Send(new GetStockLevelQuery(stockLevelId));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("warehouse/{warehouseId:int}")]
    public async Task<ActionResult<IEnumerable<StockLevelDto>>> GetByWarehouse(int warehouseId)
    {
        var result = await _mediator.Send(new GetInventoryByWarehouseQuery(warehouseId));
        return Ok(result);
    }

    [HttpGet("available")]
    public async Task<ActionResult<decimal>> GetAvailableStock(
        [FromQuery] int productId,
        [FromQuery] int? warehouseId = null,
        [FromQuery] int? binId = null)
    {
        var result = await _mediator.Send(new GetAvailableStockQuery(productId, warehouseId, binId));
        return Ok(result);
    }

    [HttpGet("low-stock")]
    public async Task<ActionResult<IEnumerable<StockLevelDto>>> GetLowStockItems()
    {
        var result = await _mediator.Send(new GetLowStockItemsQuery());
        return Ok(result);
    }

    [HttpGet("transactions")]
    public async Task<ActionResult<IEnumerable<InventoryTransactionDto>>> GetTransactions(
        [FromQuery] int? productId = null,
        [FromQuery] int? warehouseId = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null)
    {
        var result = await _mediator.Send(new GetTransactionHistoryQuery
        {
            ProductId = productId,
            WarehouseId = warehouseId,
            FromDate = fromDate,
            ToDate = toDate
        });
        return Ok(result);
    }

    [HttpPost("receive")]
    public async Task<ActionResult<StockLevelDto>> ReceiveStock([FromBody] ReceiveStockCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetStockLevel), new { stockLevelId = result.StockLevelId }, result);
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> TransferInventory([FromBody] TransferInventoryCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("adjust")]
    public async Task<ActionResult<StockLevelDto>> AdjustInventory([FromBody] AdjustInventoryCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("allocate")]
    public async Task<IActionResult> AllocateStock([FromBody] AllocateStockCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
}
