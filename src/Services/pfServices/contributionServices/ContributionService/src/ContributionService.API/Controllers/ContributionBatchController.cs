using ContributionService.Application.Commands.ContributionBatch;
using ContributionService.Application.DTOs;
using ContributionService.Application.Queries.ContributionBatch;
using ContributionService.Application.Queries.ContributionDetail;
using ContributionService.Application.Queries.Superannuation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContributionService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ContributionBatchController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ContributionMainDto>>> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllContributionBatchesQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{batchNo:long}")]
    public async Task<ActionResult<ContributionMainDto>> GetById(long batchNo, CancellationToken ct)
    {
        var result = await mediator.Send(new GetContributionBatchByIdQuery(batchNo), ct);
        return Ok(result);
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<IReadOnlyList<ContributionMainDto>>> GetByStatus(string status, CancellationToken ct)
    {
        var result = await mediator.Send(new GetContributionBatchesByStatusQuery(status), ct);
        return Ok(result);
    }

    [HttpGet("daterange")]
    public async Task<ActionResult<IReadOnlyList<ContributionMainDto>>> GetByDateRange(
        [FromQuery] DateTime start, [FromQuery] DateTime end, CancellationToken ct)
    {
        var result = await mediator.Send(new GetContributionBatchesByDateRangeQuery(start, end), ct);
        return Ok(result);
    }

    [HttpGet("{batchNo:decimal}/details")]
    public async Task<ActionResult<IReadOnlyList<ContributionDetailDto>>> GetDetails(decimal batchNo, CancellationToken ct)
    {
        var result = await mediator.Send(new GetContributionDetailsByBatchQuery(batchNo), ct);
        return Ok(result);
    }

    [HttpGet("summary")]
    public async Task<ActionResult<IReadOnlyList<ContributionSummaryDto>>> GetSummary(
        [FromQuery] DateTime startDate, [FromQuery] DateTime endDate, CancellationToken ct)
    {
        var result = await mediator.Send(new GetContributionSummaryQuery(startDate, endDate), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ContributionMainDto>> Create(
        [FromBody] CreateContributionBatchCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { batchNo = result.ContributionBatchNo }, result);
    }

    [HttpPost("{batchNo:long}/post")]
    public async Task<ActionResult<ContributionMainDto>> Post(long batchNo,
        [FromQuery] long postedByUserId, CancellationToken ct)
    {
        var result = await mediator.Send(new PostContributionBatchCommand(batchNo, postedByUserId), ct);
        return Ok(result);
    }

    [HttpPut("{batchNo:long}/status")]
    public async Task<ActionResult<ContributionMainDto>> UpdateStatus(long batchNo,
        [FromQuery] string status, CancellationToken ct)
    {
        var result = await mediator.Send(new UpdateContributionBatchStatusCommand(batchNo, status), ct);
        return Ok(result);
    }

    [HttpPost("process")]
    public async Task<ActionResult<ProcessContributionResultDto>> ProcessMonthly(
        [FromBody] ProcessMonthlyContributionCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return Ok(result);
    }
}
