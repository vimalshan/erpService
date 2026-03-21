using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourServices.Application.TourRegistrations.Commands.CancelRegistration;
using TourServices.Application.TourRegistrations.Commands.RegisterParticipant;
using TourServices.Application.TourRegistrations.Queries.GetRegistrationsByTour;

namespace TourServices.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public sealed class TourRegistrationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TourRegistrationsController(IMediator mediator) => _mediator = mediator;

    [HttpGet("by-tour/{tourId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByTour(long tourId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetRegistrationsByTourQuery(tourId), ct));

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterParticipantCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetByTour), new { tourId = result.TourId }, result);
    }

    [HttpDelete("{registrationId:long}/tour/{tourId:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(long registrationId, long tourId,
        [FromQuery] long cancelledBy, CancellationToken ct)
    {
        await _mediator.Send(new CancelRegistrationCommand(tourId, registrationId, cancelledBy), ct);
        return NoContent();
    }
}
