using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelRequestService.Application.Commands;
using TravelRequestService.Application.DTOs;
using TravelRequestService.Application.Queries;

namespace TravelRequestService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TravelRequestsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TravelRequestsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<TravelRequestDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllTravelRequestsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{planNumber}/{companyCode}")]
    [ProducesResponseType(typeof(TravelRequestDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long planNumber, string companyCode, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTravelRequestByIdQuery(planNumber, companyCode), cancellationToken);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpGet("user/{userNumber}")]
    [ProducesResponseType(typeof(IReadOnlyList<TravelRequestDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUser(long userNumber, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTravelRequestsByUserQuery(userNumber), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TravelRequestDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateTravelRequestCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { planNumber = result.PlanNumber, companyCode = result.CompanyCode }, result);
    }

    [HttpPut("{planNumber}/approve")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Approve(long planNumber, [FromBody] ApproveTravelRequestCommand command, CancellationToken cancellationToken)
    {
        var updatedCommand = command with { PlanNumber = planNumber };
        var result = await _mediator.Send(updatedCommand, cancellationToken);
        return Ok(new { Success = result });
    }

    [HttpPut("{planNumber}/reject")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Reject(long planNumber, [FromBody] RejectTravelRequestCommand command, CancellationToken cancellationToken)
    {
        var updatedCommand = command with { PlanNumber = planNumber };
        var result = await _mediator.Send(updatedCommand, cancellationToken);
        return Ok(new { Success = result });
    }

    [HttpPut("{planNumber}/cancel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(long planNumber, [FromBody] CancelTravelRequestCommand command, CancellationToken cancellationToken)
    {
        var updatedCommand = command with { PlanNumber = planNumber };
        var result = await _mediator.Send(updatedCommand, cancellationToken);
        return Ok(new { Success = result });
    }
}
