using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Commands;
using OrderService.Application.DTOs;
using OrderService.Application.Queries;

namespace OrderService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetAll(CancellationToken ct)
    {
        var orders = await _mediator.Send(new GetAllOrdersQuery(), ct);
        return Ok(orders);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderDto>> GetById(int id, CancellationToken ct)
    {
        var order = await _mediator.Send(new GetOrderByIdQuery(id), ct);
        return order == null ? NotFound() : Ok(order);
    }

    [HttpGet("by-number/{orderNumber}")]
    public async Task<ActionResult<OrderDto>> GetByNumber(string orderNumber, CancellationToken ct)
    {
        var order = await _mediator.Send(new GetOrderByNumberQuery(orderNumber), ct);
        return order == null ? NotFound() : Ok(order);
    }

    [HttpGet("by-customer/{customerId:int}")]
    public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetByCustomer(int customerId, CancellationToken ct)
    {
        var orders = await _mediator.Send(new GetOrdersByCustomerQuery(customerId), ct);
        return Ok(orders);
    }

    [HttpPost]
    public async Task<ActionResult<OrderDto>> Create([FromBody] CreateOrderRequest request, CancellationToken ct)
    {
        var order = await _mediator.Send(new CreateOrderCommand(request), ct);
        return CreatedAtAction(nameof(GetById), new { id = order.OrderId }, order);
    }

    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateOrderStatusRequest request, CancellationToken ct)
    {
        await _mediator.Send(new UpdateOrderStatusCommand(id, request.Status), ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteOrderCommand(id), ct);
        return NoContent();
    }
}
