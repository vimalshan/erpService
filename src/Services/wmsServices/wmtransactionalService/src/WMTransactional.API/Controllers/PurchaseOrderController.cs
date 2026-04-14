using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using WMTransactional.Application.Commands.CreatePurchaseOrder;
using WMTransactional.Application.Commands.ConfirmPurchaseOrder;
using WMTransactional.Application.Commands.CancelPurchaseOrder;
using WMTransactional.Application.Commands.CreateReceiving;
using WMTransactional.Application.Commands.CloseReceiving;
using WMTransactional.Application.DTOs;
using WMTransactional.Application.Queries.GetPurchaseOrder;
using WMTransactional.Application.Queries.GetPurchaseOrders;
using WMTransactional.Application.Queries.GetReceiving;
using WMTransactional.Application.Queries.GetReceivings;

namespace WMTransactional.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PurchaseOrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public PurchaseOrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PurchaseOrderDto>> GetPurchaseOrder(int id)
    {
        var result = await _mediator.Send(new GetPurchaseOrderQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PurchaseOrderDto>>> GetPurchaseOrders([FromQuery] string? status = null)
    {
        var result = await _mediator.Send(new GetPurchaseOrdersQuery { Status = status });
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<PurchaseOrderDto>> CreatePurchaseOrder([FromBody] CreatePurchaseOrderCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetPurchaseOrder), new { id = result.PoId }, result);
    }

    [HttpPut("{id:int}/confirm")]
    public async Task<IActionResult> ConfirmPurchaseOrder(int id)
    {
        await _mediator.Send(new ConfirmPurchaseOrderCommand(id));
        return NoContent();
    }

    [HttpPut("{id:int}/cancel")]
    public async Task<IActionResult> CancelPurchaseOrder(int id)
    {
        await _mediator.Send(new CancelPurchaseOrderCommand(id));
        return NoContent();
    }

    [HttpGet("{id:int}/receivings")]
    public async Task<ActionResult<IEnumerable<ReceivingDto>>> GetReceivingsForPo(int id)
    {
        var result = await _mediator.Send(new GetReceivingsQuery { PoId = id });
        return Ok(result);
    }

    [HttpPost("{id:int}/receivings")]
    public async Task<ActionResult<ReceivingDto>> CreateReceiving(int id, [FromBody] CreateReceivingCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetReceiving), new { id = result.ReceivingId }, result);
    }

    [HttpGet("receivings/{id:int}")]
    public async Task<ActionResult<ReceivingDto>> GetReceiving(int id)
    {
        var result = await _mediator.Send(new GetReceivingQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPut("receivings/{id:int}/close")]
    public async Task<IActionResult> CloseReceiving(int id)
    {
        await _mediator.Send(new CloseReceivingCommand(id));
        return NoContent();
    }
}
