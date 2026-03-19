using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProblemManagement.Application.Commands;
using ProblemManagement.Application.DTOs;
using ProblemManagement.Application.Queries;

namespace ProblemManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SolutionsController(IMediator mediator) : ControllerBase
{
    [HttpPost("{solutionId:long}/approve")]
    [Authorize(Roles = "Admin,Approver")]
    [ProducesResponseType(typeof(SolutionApprovalDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Approve(long solutionId, [FromBody] ApproveSolutionCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command with { SolutionId = solutionId }, ct);
        return Ok(result);
    }

    [HttpGet("{solutionId:long}/comments")]
    [ProducesResponseType(typeof(IReadOnlyList<SolutionCommentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetComments(long solutionId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetCommentsBySolutionQuery(solutionId), ct);
        return Ok(result);
    }

    [HttpPost("{solutionId:long}/comments")]
    [ProducesResponseType(typeof(SolutionCommentDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> AddComment(long solutionId, [FromBody] AddSolutionCommentCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command with { SolutionId = solutionId }, ct);
        return CreatedAtAction(nameof(GetComments), new { solutionId }, result);
    }
}
