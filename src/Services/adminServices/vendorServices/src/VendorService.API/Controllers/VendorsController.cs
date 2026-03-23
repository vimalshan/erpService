using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorService.Application.Commands;
using VendorService.Application.DTOs;
using VendorService.Application.Queries;

namespace VendorService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public sealed class VendorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public VendorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Gets all vendors, optionally filtered by live status.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<VendorDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? status,
        CancellationToken cancellationToken)
    {
        var vendors = await _mediator.Send(new GetAllVendorsQuery(status), cancellationToken);
        return Ok(vendors);
    }

    /// <summary>Gets a vendor by ID.</summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(VendorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var vendor = await _mediator.Send(new GetVendorByIdQuery(id), cancellationToken);
        return vendor is null ? NotFound() : Ok(vendor);
    }

    /// <summary>Creates a new vendor.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateVendorCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>Updates an existing vendor.</summary>
    [HttpPut("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] UpdateVendorCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.VendorId) return BadRequest("Route ID and body ID do not match.");
        var updated = await _mediator.Send(command, cancellationToken);
        return updated ? NoContent() : NotFound();
    }

    /// <summary>Deactivates a vendor.</summary>
    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(
        long id,
        [FromQuery] long updatedBy,
        CancellationToken cancellationToken)
    {
        var deactivated = await _mediator.Send(new DeactivateVendorCommand(id, updatedBy), cancellationToken);
        return deactivated ? NoContent() : NotFound();
    }
}
