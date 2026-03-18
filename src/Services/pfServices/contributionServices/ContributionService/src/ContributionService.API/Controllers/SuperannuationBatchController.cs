using ContributionService.Application.Commands.SuperannuationBatch;
using ContributionService.Application.DTOs;
using ContributionService.Application.Queries.SuperannuationBatch;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContributionService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SuperannuationBatchController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SuperannuationBatchDto>>> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllSuperannuationBatchesQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{batchNo:long}")]
    public async Task<ActionResult<SuperannuationBatchDto>> GetById(long batchNo, CancellationToken ct)
    {
        var result = await mediator.Send(new GetSuperannuationBatchByIdQuery(batchNo), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<SuperannuationBatchDto>> Create(
        [FromBody] CreateSuperannuationBatchCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { batchNo = result.SnBatchNo }, result);
    }

    [HttpPost("{batchNo:long}/approve")]
    public async Task<ActionResult<SuperannuationBatchDto>> Approve(long batchNo, CancellationToken ct)
    {
        var result = await mediator.Send(new ApproveSuperannuationBatchCommand(batchNo), ct);
        return Ok(result);
    }
}
