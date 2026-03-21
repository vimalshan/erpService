namespace TransactionService.API.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransactionService.Application.Commands.CreateOrder;
using TransactionService.Application.Commands.ReceiveOrder;
using TransactionService.Application.DTOs;
using TransactionService.Application.ExternalServices;
using TransactionService.Application.Queries.GetOrders;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public sealed class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IVendorServiceClient _vendorClient;
    private readonly ILocationServiceClient _locationClient;

    public OrdersController(
        IMediator mediator,
        IVendorServiceClient vendorClient,
        ILocationServiceClient locationClient)
    {
        _mediator = mediator;
        _vendorClient = vendorClient;
        _locationClient = locationClient;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OrderSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] long? locationId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllOrdersQuery(locationId), ct);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(OrderMainDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetOrderByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("vendor/{vendorId:long}")]
    [ProducesResponseType(typeof(IEnumerable<OrderSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByVendor(long vendorId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetOrdersByVendorQuery(vendorId), ct);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        [FromBody] CreateOrderCommand command, CancellationToken ct)
    {
        var id = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{orderSubId:long}/receive")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Receive(
        long orderSubId, [FromBody] ReceiveOrderCommand command, CancellationToken ct)
    {
        if (orderSubId != command.OrderSubId)
            return BadRequest("ID mismatch.");

        var result = await _mediator.Send(command, ct);
        return result ? NoContent() : NotFound();
    }

    // ── External Service Lookups ──

    /// <summary>Get all vendors from VendorService for order creation.</summary>
    [HttpGet("lookup/vendors")]
    [ProducesResponseType(typeof(IReadOnlyList<VendorDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetVendors(
        [FromQuery] char? status, CancellationToken ct)
    {
        var vendors = await _vendorClient.GetAllVendorsAsync(status, ct);
        return Ok(vendors);
    }

    /// <summary>Get a specific vendor by ID from VendorService.</summary>
    [HttpGet("lookup/vendors/{vendorId:long}")]
    [ProducesResponseType(typeof(VendorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetVendorById(long vendorId, CancellationToken ct)
    {
        var vendor = await _vendorClient.GetVendorByIdAsync(vendorId, ct);
        return vendor is null ? NotFound() : Ok(vendor);
    }

    /// <summary>Get active locations from LocationService.</summary>
    [HttpGet("lookup/locations")]
    [ProducesResponseType(typeof(IReadOnlyList<LocationAppMapDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLocations(CancellationToken ct)
    {
        var locations = await _locationClient.GetActiveLocationsAsync(ct);
        return Ok(locations);
    }
}
