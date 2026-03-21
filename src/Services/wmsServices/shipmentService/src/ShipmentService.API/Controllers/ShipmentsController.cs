using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipmentService.Application.DTOs;
using ShipmentService.Application.Features.Shipments.Commands.AddPackage;
using ShipmentService.Application.Features.Shipments.Commands.CreateShipment;
using ShipmentService.Application.Features.Shipments.Commands.ShipSalesOrder;
using ShipmentService.Application.Features.Shipments.Commands.UpdateShipmentStatus;
using ShipmentService.Application.Features.Shipments.Queries.GetAllShipments;
using ShipmentService.Application.Features.Shipments.Queries.GetShipmentById;
using ShipmentService.Application.Features.Shipments.Queries.GetShipmentsByCustomer;
using ShipmentService.Application.Features.Shipments.Queries.GetTrackingHistory;

namespace ShipmentService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public sealed class ShipmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ShipmentsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get all shipments (paginated).</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<ShipmentSummaryDto>), 200)]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
        => Ok(await _mediator.Send(new GetAllShipmentsQuery(page, pageSize), ct));

    /// <summary>Get shipment by ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ShipmentDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct = default)
        => Ok(await _mediator.Send(new GetShipmentByIdQuery(id), ct));

    /// <summary>Get shipments by customer ID.</summary>
    [HttpGet("customer/{customerId:int}")]
    [ProducesResponseType(typeof(IEnumerable<ShipmentSummaryDto>), 200)]
    public async Task<IActionResult> GetByCustomer(int customerId, CancellationToken ct = default)
        => Ok(await _mediator.Send(new GetShipmentsByCustomerQuery(customerId), ct));

    /// <summary>Get tracking history for a shipment.</summary>
    [HttpGet("{id:int}/tracking")]
    [ProducesResponseType(typeof(IEnumerable<TrackingHistoryDto>), 200)]
    public async Task<IActionResult> GetTracking(int id, CancellationToken ct = default)
        => Ok(await _mediator.Send(new GetTrackingHistoryQuery(id), ct));

    /// <summary>Create a new shipment.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ShipmentDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] CreateShipmentCommand command, CancellationToken ct = default)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.ShipmentId }, result);
    }

    /// <summary>Update shipment status.</summary>
    [HttpPut("{id:int}/status")]
    [ProducesResponseType(typeof(ShipmentDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(422)]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest request, CancellationToken ct = default)
        => Ok(await _mediator.Send(new UpdateShipmentStatusCommand(id, request.NewStatus, request.Location, request.Description, request.UpdatedBy), ct));

    /// <summary>Add a package to a shipment.</summary>
    [HttpPost("{id:int}/packages")]
    [ProducesResponseType(typeof(PackageDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> AddPackage(int id, [FromBody] AddPackageRequest request, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new AddPackageCommand(id, request.PackageNumber, request.Weight, request.Volume,
            request.Dimensions, request.TrackingNumber, request.ContentsDescription), ct);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    /// <summary>Ship a sales order.</summary>
    [HttpPost("ship-sales-order")]
    [Authorize(Roles = "Warehouse,Admin")]
    [ProducesResponseType(typeof(ShipmentDto), 201)]
    public async Task<IActionResult> ShipSalesOrder([FromBody] ShipSalesOrderCommand command, CancellationToken ct = default)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.ShipmentId }, result);
    }
}

public sealed record UpdateStatusRequest(string NewStatus, string? Location, string? Description, string? UpdatedBy);
public sealed record AddPackageRequest(string PackageNumber, decimal? Weight, decimal? Volume,
    string? Dimensions, string? TrackingNumber, string? ContentsDescription);
