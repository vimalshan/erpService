using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using WMTransactional.Application.Commands.CreateSalesOrder;
using WMTransactional.Application.Commands.ConfirmSalesOrder;
using WMTransactional.Application.Commands.CancelSalesOrder;
using WMTransactional.Application.Commands.CreateShipment;
using WMTransactional.Application.Commands.ShipShipment;
using WMTransactional.Application.DTOs;
using WMTransactional.Application.Queries.GetSalesOrder;
using WMTransactional.Application.Queries.GetSalesOrders;
using WMTransactional.Application.Queries.GetShipment;
using WMTransactional.Application.Queries.GetShipments;

namespace WMTransactional.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SalesOrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public SalesOrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SalesOrderDto>> GetSalesOrder(int id)
    {
        var result = await _mediator.Send(new GetSalesOrderQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SalesOrderDto>>> GetSalesOrders([FromQuery] string? status = null)
    {
        var result = await _mediator.Send(new GetSalesOrdersQuery { Status = status });
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<SalesOrderDto>> CreateSalesOrder([FromBody] CreateSalesOrderCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetSalesOrder), new { id = result.SoId }, result);
    }

    [HttpPut("{id:int}/confirm")]
    public async Task<IActionResult> ConfirmSalesOrder(int id)
    {
        await _mediator.Send(new ConfirmSalesOrderCommand(id));
        return NoContent();
    }

    [HttpPut("{id:int}/cancel")]
    public async Task<IActionResult> CancelSalesOrder(int id)
    {
        await _mediator.Send(new CancelSalesOrderCommand(id));
        return NoContent();
    }

    [HttpGet("{id:int}/shipments")]
    public async Task<ActionResult<IEnumerable<ShipmentDto>>> GetShipmentsForSo(int id)
    {
        var result = await _mediator.Send(new GetShipmentsQuery { SoId = id });
        return Ok(result);
    }

    [HttpPost("{id:int}/shipments")]
    public async Task<ActionResult<ShipmentDto>> CreateShipment(int id, [FromBody] CreateShipmentCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetShipment), new { id = result.ShipmentId }, result);
    }

    [HttpGet("shipments/{id:int}")]
    public async Task<ActionResult<ShipmentDto>> GetShipment(int id)
    {
        var result = await _mediator.Send(new GetShipmentQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPut("shipments/{id:int}/ship")]
    public async Task<IActionResult> ShipShipment(int id)
    {
        await _mediator.Send(new ShipShipmentCommand(id));
        return NoContent();
    }
}
