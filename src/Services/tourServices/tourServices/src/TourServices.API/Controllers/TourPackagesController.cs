using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourServices.Application.TourPackages.Commands.ChangeTourStatus;
using TourServices.Application.TourPackages.Commands.CreateTourPackage;
using TourServices.Application.TourPackages.Commands.UpdateTourPackage;
using TourServices.Application.TourPackages.Queries.GetAllTourPackages;
using TourServices.Application.TourPackages.Queries.GetTourPackageById;

namespace TourServices.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public sealed class TourPackagesController : ControllerBase
{
    private readonly IMediator _mediator;

    public TourPackagesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] string? status, CancellationToken ct)
        => Ok(await _mediator.Send(new GetAllTourPackagesQuery(status), ct));

    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetTourPackageByIdQuery(id), ct));

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateTourPackageCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.TourId }, result);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateTourPackageCommand command, CancellationToken ct)
    {
        if (id != command.TourId) return BadRequest("ID mismatch.");
        return Ok(await _mediator.Send(command, ct));
    }

    [HttpPatch("{id:long}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Activate(long id, [FromQuery] long updatedBy, CancellationToken ct)
    {
        await _mediator.Send(new ActivateTourPackageCommand(id, updatedBy), ct);
        return NoContent();
    }

    [HttpPatch("{id:long}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Cancel(long id, [FromQuery] long updatedBy, CancellationToken ct)
    {
        await _mediator.Send(new CancelTourPackageCommand(id, updatedBy), ct);
        return NoContent();
    }

    [HttpPatch("{id:long}/complete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Complete(long id, [FromQuery] long updatedBy, CancellationToken ct)
    {
        await _mediator.Send(new CompleteTourPackageCommand(id, updatedBy), ct);
        return NoContent();
    }
}
