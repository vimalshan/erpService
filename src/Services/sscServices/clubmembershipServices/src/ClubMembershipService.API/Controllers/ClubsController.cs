using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClubMembershipService.Application.Commands.CreateClub;
using ClubMembershipService.Application.DTOs;
using ClubMembershipService.Application.Queries.GetClubs;

namespace ClubMembershipService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClubsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClubsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType<IEnumerable<ClubDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] bool activeOnly = false, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetClubsQuery(activeOnly), ct);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType<ClubDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetClubByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [ProducesResponseType<ClubDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateClubRequest request, CancellationToken ct = default)
    {
        var userId = GetCurrentUserId();
        var result = await _mediator.Send(new CreateClubCommand(request.ClubName, userId), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.ClubId }, result);
    }

    private long GetCurrentUserId()
    {
        var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        return long.TryParse(claim, out var id) ? id : 1L;
    }
}

public record CreateClubRequest(string ClubName);
