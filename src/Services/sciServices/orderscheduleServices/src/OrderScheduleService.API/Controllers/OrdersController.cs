namespace OrderScheduleService.API.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderScheduleService.Application.Commands;
using OrderScheduleService.Application.DTOs;
using OrderScheduleService.Application.Queries;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IMediator mediator, ILogger<OrdersController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all orders
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TiedOrderDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllOrders()
    {
        try
        {
            var orders = await _mediator.Send(new GetAllOrdersQuery());
            return Ok(orders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving orders");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Get order by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TiedOrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrderById(long id)
    {
        try
        {
            var order = await _mediator.Send(new GetTiedOrderByIdQuery(id));
            if (order == null)
                return NotFound();

            return Ok(order);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving order {id}");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Get orders by customer code
    /// </summary>
    [HttpGet("customer/{customerCode}")]
    [ProducesResponseType(typeof(IEnumerable<TiedOrderDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrdersByCustomer(string customerCode)
    {
        try
        {
            var orders = await _mediator.Send(new GetOrdersByCustomerQuery(customerCode));
            return Ok(orders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving orders for customer {customerCode}");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Create a new order
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOrder([FromBody] CreateTiedOrderDto orderDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var orderId = await _mediator.Send(new CreateTiedOrderCommand(orderDto));
            return CreatedAtAction(nameof(GetOrderById), new { id = orderId }, new { orderId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating order");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Update order status
    /// </summary>
    [HttpPut("{id}/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateOrderStatus(long id, [FromQuery] char status)
    {
        try
        {
            var userId = User.FindFirst("sub")?.Value ?? "SYSTEM";
            var result = await _mediator.Send(new UpdateOrderStatusCommand(id, status, userId));
            return Ok(new { success = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating order {id} status");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Delete order
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteOrder(long id)
    {
        try
        {
            var result = await _mediator.Send(new DeleteTiedOrderCommand(id));
            return Ok(new { success = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting order {id}");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }
}

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrderDetailsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<OrderDetailsController> _logger;

    public OrderDetailsController(IMediator mediator, ILogger<OrderDetailsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all order details
    /// </summary>
    [HttpGet("order/{orderId}")]
    [ProducesResponseType(typeof(IEnumerable<TiedOrderDetailDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrderDetails(long orderId)
    {
        try
        {
            var details = await _mediator.Send(new GetAllOrderDetailsQuery(orderId));
            return Ok(details);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving details for order {orderId}");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Add order detail
    /// </summary>
    [HttpPost("order/{orderId}")]
    [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
    public async Task<IActionResult> AddOrderDetail(long orderId, [FromBody] CreateOrderDetailsDto detailDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var detailId = await _mediator.Send(new AddOrderDetailCommand(
                orderId,
                detailDto.ItemId,
                detailDto.ItemName,
                detailDto.OrderQuantity,
                detailDto.DispatchDate,
                detailDto.Price));

            return CreatedAtAction(nameof(GetOrderDetails), new { orderId }, new { detailId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error adding detail to order {orderId}");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Schedule order detail
    /// </summary>
    [HttpPut("order/{orderId}/detail/{detailId}/schedule")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ScheduleOrderDetail(long orderId, long detailId, [FromQuery] DateTime scheduledDate, [FromQuery] long allocatedQuantity)
    {
        try
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0";
            var result = await _mediator.Send(new ScheduleOrderDetailCommand(orderId, detailId, scheduledDate, allocatedQuantity, int.Parse(userId)));
            return Ok(new { success = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error scheduling detail {detailId} for order {orderId}");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Cancel order detail
    /// </summary>
    [HttpPut("order/{orderId}/detail/{detailId}/cancel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CancelOrderDetail(long orderId, long detailId)
    {
        try
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0";
            var result = await _mediator.Send(new CancelOrderDetailCommand(orderId, detailId, int.Parse(userId)));
            return Ok(new { success = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error cancelling detail {detailId} for order {orderId}");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }
}
