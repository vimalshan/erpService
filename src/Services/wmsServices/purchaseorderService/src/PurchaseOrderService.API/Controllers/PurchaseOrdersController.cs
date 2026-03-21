using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PurchaseOrderService.Application.Commands.CancelPurchaseOrder;
using PurchaseOrderService.Application.Commands.ConfirmPurchaseOrder;
using PurchaseOrderService.Application.Commands.CreatePurchaseOrder;
using PurchaseOrderService.Application.Commands.ReceivePurchaseOrderLine;
using PurchaseOrderService.Application.Commands.UpdatePurchaseOrder;
using PurchaseOrderService.Application.DTOs;
using PurchaseOrderService.Application.Queries.GetPurchaseOrderById;
using PurchaseOrderService.Application.Queries.GetPurchaseOrderByNumber;
using PurchaseOrderService.Application.Queries.GetPurchaseOrders;

namespace PurchaseOrderService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PurchaseOrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public PurchaseOrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PurchaseOrdersResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? status = null)
    {
        var result = await _mediator.Send(new GetPurchaseOrdersQuery { Page = page, PageSize = pageSize, Status = status });
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PurchaseOrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetPurchaseOrderByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("by-number/{poNumber}")]
    [ProducesResponseType(typeof(PurchaseOrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByNumber(string poNumber)
    {
        var result = await _mediator.Send(new GetPurchaseOrderByNumberQuery(poNumber));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreatePurchaseOrderCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePurchaseOrderCommand command)
    {
        if (id != command.PoId)
            return BadRequest("ID mismatch");

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("{id:int}/confirm")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Confirm(int id)
    {
        await _mediator.Send(new ConfirmPurchaseOrderCommand(id));
        return NoContent();
    }

    [HttpPost("{id:int}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(int id)
    {
        await _mediator.Send(new CancelPurchaseOrderCommand(id));
        return NoContent();
    }

    [HttpPost("{id:int}/lines/{lineNumber:int}/receive")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReceiveLine(int id, int lineNumber, [FromBody] ReceiveLineRequest request)
    {
        await _mediator.Send(new ReceivePurchaseOrderLineCommand
        {
            PoId = id,
            LineNumber = lineNumber,
            Quantity = request.Quantity
        });
        return NoContent();
    }
}

public record ReceiveLineRequest(decimal Quantity);
