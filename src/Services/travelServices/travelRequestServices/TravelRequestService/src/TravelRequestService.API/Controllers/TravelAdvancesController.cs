using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelRequestService.Application.Commands;
using TravelRequestService.Application.DTOs;

namespace TravelRequestService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TravelAdvancesController : ControllerBase
{
    private readonly IMediator _mediator;

    public TravelAdvancesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(TravelAdvanceDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddAdvance([FromBody] AddTravelAdvanceCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Created(string.Empty, result);
    }
}
