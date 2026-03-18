using MediatR;
using MemberService.Application.Commands.AddNominee;
using MemberService.Application.Commands.CloseMember;
using MemberService.Application.Commands.CreateMember;
using MemberService.Application.DTOs;
using MemberService.Application.Queries.GetMember;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemberService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class MembersController : ControllerBase
{
    private readonly IMediator _mediator;
    public MembersController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get all active members (optionally filtered by trust code)</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<MemberSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] string? trustCode, [FromQuery] string? status, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllMembersQuery(trustCode, status), ct);
        return Ok(result);
    }

    /// <summary>Get full member profile by member number</summary>
    [HttpGet("{memberNo:long}")]
    [ProducesResponseType(typeof(MemberProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long memberNo, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetMemberQuery(memberNo), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Get member by employee system ID</summary>
    [HttpGet("by-employee/{employeeSysId:long}")]
    [ProducesResponseType(typeof(MemberDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByEmployee(long employeeSysId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetMemberByEmployeeQuery(employeeSysId), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Enroll a new member</summary>
    [HttpPost]
    [ProducesResponseType(typeof(MemberDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateMemberCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { memberNo = result.MemberNo }, result);
    }

    /// <summary>Close a member account</summary>
    [HttpPost("{memberNo:long}/close")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Close(long memberNo, [FromBody] CloseMemberRequest request, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        await _mediator.Send(new CloseMemberCommand(memberNo, request.LeaveReason, request.LeaveDate, userId), ct);
        return NoContent();
    }

    /// <summary>Add a nominee to a member</summary>
    [HttpPost("{memberNo:long}/nominees")]
    [ProducesResponseType(typeof(NomineeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddNominee(long memberNo, [FromBody] AddNomineeCommand command, CancellationToken ct)
    {
        var mergedCommand = command with { MemberNo = memberNo, CreatedBy = GetCurrentUserId() };
        var result = await _mediator.Send(mergedCommand, ct);
        return CreatedAtAction(nameof(GetById), new { memberNo }, result);
    }

    private long GetCurrentUserId()
    {
        var claim = User.FindFirst("sub") ?? User.FindFirst("userId");
        return claim is not null && long.TryParse(claim.Value, out var id) ? id : 1;
    }
}

public record CloseMemberRequest(string LeaveReason, DateTime LeaveDate);
