using Asp.Versioning;
using LocationServices.Application.Commands;
using LocationServices.Application.DTOs;
using LocationServices.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocationServices.API.Controllers.v1;

/// <summary>Location App Map REST API — v1</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/location-app-maps")]
[Authorize]
[Produces("application/json")]
public sealed class LocationAppMapController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<LocationAppMapController> _logger;

    public LocationAppMapController(IMediator mediator, ILogger<LocationAppMapController> logger)
    {
        _mediator = mediator;
        _logger   = logger;
    }

    // ── GET ALL ──────────────────────────────────────────────────────────────
    /// <summary>Get all location-to-app mappings</summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<LocationAppMapDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllLocationAppMapsQuery(), ct);
        return result.IsSuccess ? Ok(result.Value) : Problem(result.Error);
    }

    // ── GET ACTIVE ───────────────────────────────────────────────────────────
    /// <summary>Get all active mappings</summary>
    [HttpGet("active")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<LocationAppMapDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActive(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetActiveLocationAppMapsQuery(), ct);
        return result.IsSuccess ? Ok(result.Value) : Problem(result.Error);
    }

    // ── GET BY LOCATION ──────────────────────────────────────────────────────
    /// <summary>Get all mappings for a specific location</summary>
    [HttpGet("by-location/{locationId:decimal}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<LocationAppMapDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByLocation(decimal locationId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetLocationAppMapsByLocationQuery(locationId), ct);
        return result.IsSuccess ? Ok(result.Value) : Problem(result.Error);
    }

    // ── GET SINGLE ───────────────────────────────────────────────────────────
    /// <summary>Get a specific mapping by locationId and appName</summary>
    [HttpGet("{locationId:decimal}/{appName}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LocationAppMapDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOne(decimal locationId, string appName, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetLocationAppMapQuery(locationId, appName), ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new { message = result.Error });
    }

    // ── GET COUNT ─────────────────────────────────────────────────────────────
    /// <summary>Get total count of all mappings</summary>
    [HttpGet("count")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCount(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetLocationAppMapCountQuery(), ct);
        return result.IsSuccess ? Ok(result.Value) : Problem(result.Error);
    }

    // ── CREATE ───────────────────────────────────────────────────────────────
    /// <summary>Create a new location-to-app mapping</summary>
    [HttpPost]
    [Authorize(Roles = "Admin,LocationManager")]
    [ProducesResponseType(typeof(LocationAppMapDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateLocationAppMapRequest request, CancellationToken ct)
    {
        var command = new CreateLocationAppMapCommand(
            request.LocationId, request.AppName, request.SiteCategoryCode,
            request.SelfAccess, request.DeemedApproval,
            User.Identity?.Name ?? "system");

        var result = await _mediator.Send(command, ct);
        if (!result.IsSuccess)
            return Conflict(new { message = result.Error });

        return CreatedAtAction(nameof(GetOne),
            new { locationId = result.Value!.LocationId, appName = result.Value.AppName },
            result.Value);
    }

    // ── UPDATE ───────────────────────────────────────────────────────────────
    /// <summary>Update an existing mapping</summary>
    [HttpPut("{locationId:decimal}/{appName}")]
    [Authorize(Roles = "Admin,LocationManager")]
    [ProducesResponseType(typeof(LocationAppMapDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(decimal locationId, string appName,
        [FromBody] UpdateLocationAppMapRequest request, CancellationToken ct)
    {
        var command = new UpdateLocationAppMapCommand(
            locationId, appName, request.SiteCategoryCode,
            request.SelfAccess, request.DeemedApproval,
            request.IsActive,
            User.Identity?.Name ?? "system");

        var result = await _mediator.Send(command, ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new { message = result.Error });
    }

    // ── DELETE ───────────────────────────────────────────────────────────────
    /// <summary>Soft-delete (deactivate) a mapping</summary>
    [HttpDelete("{locationId:decimal}/{appName}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(decimal locationId, string appName, CancellationToken ct)
    {
        var command = new DeleteLocationAppMapCommand(locationId, appName,
            User.Identity?.Name ?? "system");

        var result = await _mediator.Send(command, ct);
        return result.IsSuccess ? NoContent() : NotFound(new { message = result.Error });
    }
}
