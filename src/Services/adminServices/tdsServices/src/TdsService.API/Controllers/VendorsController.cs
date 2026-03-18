using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TdsService.Application.DTOs;
using TdsService.Application.Vendors.Commands.CreateTdsVendor;
using TdsService.Application.Vendors.Commands.DeleteTdsVendor;
using TdsService.Application.Vendors.Commands.UpdateTdsVendor;
using TdsService.Application.Vendors.Queries.GetAllTdsVendors;
using TdsService.Application.Vendors.Queries.GetTdsVendorByPan;

namespace TdsService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public sealed class VendorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public VendorsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get paged list of all TDS vendors.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<TdsVendorDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetAllTdsVendorsQuery(page, pageSize), ct);
        return Ok(result);
    }

    /// <summary>Get a vendor by PAN number.</summary>
    [HttpGet("{panNo}")]
    [ProducesResponseType(typeof(TdsVendorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByPan(string panNo, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetTdsVendorByPanQuery(panNo), ct);
        return result is not null ? Ok(result) : NotFound();
    }

    /// <summary>Create a new TDS vendor.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateTdsVendorCommand command, CancellationToken ct = default)
    {
        var id = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetByPan), new { panNo = command.PanNo }, id);
    }

    /// <summary>Update an existing TDS vendor.</summary>
    [HttpPut("{vendorId:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        long vendorId,
        [FromBody] UpdateTdsVendorCommand command,
        CancellationToken ct = default)
    {
        if (vendorId != command.VendorId) return BadRequest("Route ID and body ID mismatch.");
        await _mediator.Send(command, ct);
        return NoContent();
    }

    /// <summary>Delete a TDS vendor.</summary>
    [HttpDelete("{vendorId:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long vendorId, CancellationToken ct = default)
    {
        await _mediator.Send(new DeleteTdsVendorCommand(vendorId), ct);
        return NoContent();
    }
}
