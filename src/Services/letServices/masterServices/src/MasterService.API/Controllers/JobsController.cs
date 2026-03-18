using MasterService.Application.Features.Jobs.Commands;
using MasterService.Application.Features.Jobs.Queries;
using MediatoR = MediatR;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MasterService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class JobsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? categoryCode, CancellationToken ct)
        => Ok(await mediator.Send(new GetJobsQuery(categoryCode), ct));

    [HttpGet("{jobCode:long}")]
    public async Task<IActionResult> GetById(long jobCode, CancellationToken ct)
    {
        var result = await mediator.Send(new GetJobByCodeQuery(jobCode), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateJobCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { jobCode = result.JobCode }, result);
    }

    [HttpPut("{jobCode:long}")]
    public async Task<IActionResult> Update(long jobCode, [FromBody] UpdateJobCommand command, CancellationToken ct)
    {
        if (jobCode != command.JobCode) return BadRequest("Route code and body code mismatch.");
        return Ok(await mediator.Send(command, ct));
    }
}
