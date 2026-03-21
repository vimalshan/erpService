using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesOrderService.Application.SalesOrders.Commands.AddOrderLine;
using SalesOrderService.Application.SalesOrders.Commands.CancelSalesOrder;
using SalesOrderService.Application.SalesOrders.Commands.ConfirmSalesOrder;
using SalesOrderService.Application.SalesOrders.Commands.CreateSalesOrder;
using SalesOrderService.Application.SalesOrders.Queries.GetAllSalesOrders;
using SalesOrderService.Application.SalesOrders.Queries.GetSalesOrderById;
using SalesOrderService.Application.SalesOrders.Queries.GetSalesOrdersByCustomer;

namespace SalesOrderService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public sealed class SalesOrdersController(ISender mediator) : ControllerBase
{
    // ── Queries ──────────────────────────────────────────────────────────────

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await mediator.Send(new GetAllSalesOrdersQuery(), ct));

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetSalesOrderByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("customer/{customerId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCustomer(int customerId, CancellationToken ct) =>
        Ok(await mediator.Send(new GetSalesOrdersByCustomerQuery(customerId), ct));

    // ── Commands ─────────────────────────────────────────────────────────────

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateSalesOrderCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.SoId }, result);
    }

    [HttpPost("{id:int}/confirm")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Confirm(int id, CancellationToken ct)
    {
        await mediator.Send(new ConfirmSalesOrderCommand(id), ct);
        return NoContent();
    }

    [HttpPost("{id:int}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Cancel(int id, [FromBody] CancelRequest body, CancellationToken ct)
    {
        await mediator.Send(new CancelSalesOrderCommand(id, body.Reason), ct);
        return NoContent();
    }

    [HttpPost("{id:int}/lines")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddLine(int id, [FromBody] AddOrderLineCommand command, CancellationToken ct)
    {
        var actualCommand = command with { SoId = id };
        var line = await mediator.Send(actualCommand, ct);
        return CreatedAtAction(nameof(GetById), new { id }, line);
    }
}

public sealed record CancelRequest(string Reason);
