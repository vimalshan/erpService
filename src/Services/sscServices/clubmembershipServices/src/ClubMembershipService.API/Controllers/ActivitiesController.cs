using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClubMembershipService.Application.Commands.RecordActivity;
using ClubMembershipService.Application.DTOs;
using ClubMembershipService.Application.Queries.GetActivities;

namespace ClubMembershipService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ActivitiesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ActivitiesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType<IEnumerable<ActivityDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetAllActivitiesQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType<ActivityDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetActivityByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("by-club/{clubId:long}")]
    [ProducesResponseType<IEnumerable<ActivityDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByClub(long clubId, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetActivitiesByClubQuery(clubId), ct);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType<ActivityDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Record([FromBody] RecordActivityRequest request, CancellationToken ct = default)
    {
        var userId = GetCurrentUserId();
        var command = new RecordActivityCommand(
            request.ClubId, request.ActivityName, request.ActivityDate,
            request.Budget, userId);
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.ActivityId }, result);
    }

    private long GetCurrentUserId()
    {
        var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        return long.TryParse(claim, out var id) ? id : 1L;
    }
}

public record RecordActivityRequest(
    long ClubId,
    string ActivityName,
    DateOnly ActivityDate,
    decimal? Budget);
