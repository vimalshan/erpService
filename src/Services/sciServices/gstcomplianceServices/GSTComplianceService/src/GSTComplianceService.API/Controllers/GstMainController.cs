using GSTComplianceService.Application.Common.DTOs;
using GSTComplianceService.Application.Features.GstMain.Commands;
using GSTComplianceService.Application.Features.GstMain.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GSTComplianceService.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class GstMainController : ControllerBase
{
    private readonly IMediator _mediator;

    public GstMainController(IMediator mediator) => _mediator = mediator;

    /// <summary>Gets all GST registrations (paged).</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<GstMainDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetAllGstQuery(page, pageSize), ct);
        return Ok(result);
    }

    /// <summary>Gets a GST registration by ID.</summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(GstMainDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetGstDetailsQuery(id), ct);
        return Ok(result);
    }

    /// <summary>Gets a GST registration by PAN number.</summary>
    [HttpGet("by-pan/{panNo}")]
    [ProducesResponseType(typeof(GstMainDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByPan(string panNo, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetGstByPanQuery(panNo), ct);
        if (result is null) return NotFound();
        return Ok(result);
    }

    /// <summary>Registers a new GST entity.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterGstCommand command, CancellationToken ct = default)
    {
        var id = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>Updates vendor info for a GST registration.</summary>
    [HttpPut("{id:long}/vendor")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateVendor(long id, [FromBody] UpdateGstVendorCommand command, CancellationToken ct = default)
    {
        if (id != command.GstId) return BadRequest("ID mismatch.");
        await _mediator.Send(command, ct);
        return NoContent();
    }

    /// <summary>Activates a GST registration.</summary>
    [HttpPost("{id:long}/activate")]
    [Authorize(Roles = "Admin,GSTManager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Activate(long id, CancellationToken ct = default)
    {
        await _mediator.Send(new ActivateGstCommand(id), ct);
        return NoContent();
    }

    /// <summary>Deactivates a GST registration.</summary>
    [HttpPost("{id:long}/deactivate")]
    [Authorize(Roles = "Admin,GSTManager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Deactivate(long id, CancellationToken ct = default)
    {
        await _mediator.Send(new DeactivateGstCommand(id), ct);
        return NoContent();
    }

    /// <summary>Deletes a GST registration.</summary>
    [HttpDelete("{id:long}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long id, CancellationToken ct = default)
    {
        await _mediator.Send(new DeleteGstCommand(id), ct);
        return NoContent();
    }
}
