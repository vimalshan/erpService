using ContributionService.Application.Commands.ContributionDetail;
using ContributionService.Application.DTOs;
using ContributionService.Application.Queries.ContributionDetail;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContributionService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ContributionDetailController(IMediator mediator) : ControllerBase
{
    [HttpGet("batch/{batchNo:decimal}")]
    public async Task<ActionResult<IReadOnlyList<ContributionDetailDto>>> GetByBatch(decimal batchNo, CancellationToken ct)
    {
        var result = await mediator.Send(new GetContributionDetailsByBatchQuery(batchNo), ct);
        return Ok(result);
    }

    [HttpGet("member/{memberNo:decimal}")]
    public async Task<ActionResult<IReadOnlyList<ContributionDetailDto>>> GetByMember(decimal memberNo, CancellationToken ct)
    {
        var result = await mediator.Send(new GetContributionDetailsByMemberQuery(memberNo), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ContributionDetailDto>> Create(
        [FromBody] CreateContributionDetailCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return Created(string.Empty, result);
    }

    [HttpPost("{id:decimal}/validate")]
    public async Task<ActionResult<string>> Validate(decimal id, CancellationToken ct)
    {
        var result = await mediator.Send(new ValidateContributionDetailCommand(id), ct);
        return Ok(result);
    }
}
