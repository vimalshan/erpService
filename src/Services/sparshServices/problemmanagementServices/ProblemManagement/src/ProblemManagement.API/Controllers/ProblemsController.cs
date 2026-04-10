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
public class ProblemsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ProblemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllProblemsQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ProblemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetProblemByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("status/{status}")]
    [ProducesResponseType(typeof(IReadOnlyList<ProblemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByStatus(string status, CancellationToken ct)
    {
        var result = await mediator.Send(new GetProblemsByStatusQuery(status), ct);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProblemDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProblemCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.PrId }, result);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ProblemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateProblemCommand command, CancellationToken ct)
    {
        if (id != command.PrId) return BadRequest("ID mismatch.");
        var result = await mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        await mediator.Send(new DeleteProblemCommand(id), ct);
        return NoContent();
    }

    [HttpPost("{id:long}/approve")]
    [Authorize(Roles = "Admin,Approver")]
    [ProducesResponseType(typeof(ProblemApprovalDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Approve(long id, [FromBody] ApproveProblemCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command with { ProblemId = id }, ct);
        return Ok(result);
    }

    [HttpGet("{id:long}/solutions")]
    [ProducesResponseType(typeof(IReadOnlyList<ProblemSolutionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSolutions(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetSolutionsByProblemQuery(id), ct);
        return Ok(result);
    }

    [HttpPost("{id:long}/solutions")]
    [ProducesResponseType(typeof(ProblemSolutionDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> AddSolution(long id, [FromBody] RecordSolutionCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command with { ProblemId = id }, ct);
        return CreatedAtAction(nameof(GetSolutions), new { id }, result);
    }

    [HttpGet("{id:long}/attachments")]
    [ProducesResponseType(typeof(IReadOnlyList<ProblemAttachmentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAttachments(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetAttachmentsByProblemQuery(id), ct);
        return Ok(result);
    }
}
