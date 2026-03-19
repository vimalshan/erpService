using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClubMembershipService.Application.Commands.CreateMembership;
using ClubMembershipService.Application.DTOs;
using ClubMembershipService.Application.Queries.GetMemberships;

namespace ClubMembershipService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MembershipsController : ControllerBase
{
    private readonly IMediator _mediator;

    public MembershipsController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{id:long}")]
    [ProducesResponseType<MembershipDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetMembershipByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("by-club/{clubId:long}")]
    [ProducesResponseType<IEnumerable<MembershipDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByClub(long clubId, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetMembershipsByClubQuery(clubId), ct);
        return Ok(result);
    }

    [HttpGet("by-member/{memberId:long}")]
    [ProducesResponseType<IEnumerable<MembershipDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByMember(long memberId, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetMembershipsByMemberQuery(memberId), ct);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType<MembershipDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateMembershipRequest request, CancellationToken ct = default)
    {
        var userId = GetCurrentUserId();
        var command = new CreateMembershipCommand(
            request.ClubId, request.MemberId, request.JoinDate,
            request.MembershipFee, userId);
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.MembershipId }, result);
    }

    private long GetCurrentUserId()
    {
        var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        return long.TryParse(claim, out var id) ? id : 1L;
    }
}

public record CreateMembershipRequest(
    long ClubId,
    long MemberId,
    DateOnly JoinDate,
    decimal? MembershipFee);
