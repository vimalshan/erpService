using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recruitment.Application.CQRS.Commands;
using Recruitment.Application.CQRS.Queries;
using Recruitment.Application.DTOs;

namespace Recruitment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class JobsController : ControllerBase
{
    private readonly IMediator _mediator;

    public JobsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all jobs
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<JobDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllJobs()
    {
        var query = new GetAllJobsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Get job by ID
    /// </summary>
    [HttpGet("{jobId}")]
    [ProducesResponseType(typeof(JobDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetJobById(decimal jobId)
    {
        var query = new GetJobByIdQuery { JobId = jobId };
        var result = await _mediator.Send(query);
        
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Get active jobs
    /// </summary>
    [HttpGet("active")]
    [ProducesResponseType(typeof(IEnumerable<JobDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActiveJobs()
    {
        var query = new GetActiveJobsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Create a new job
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateJob([FromBody] CreateJobDto jobDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var command = new CreateJobCommand { JobData = jobDto };
        var result = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetJobById), new { jobId = result }, result);
    }

    /// <summary>
    /// Update an existing job
    /// </summary>
    [HttpPut("{jobId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateJob(decimal jobId, [FromBody] UpdateJobDto jobDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        jobDto.JobId = jobId;
        var command = new UpdateJobCommand { JobData = jobDto };
        var result = await _mediator.Send(command);

        if (!result)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Deactivate a job
    /// </summary>
    [HttpPatch("{jobId}/deactivate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeactivateJob(decimal jobId)
    {
        var command = new DeactivateJobCommand { JobId = jobId };
        var result = await _mediator.Send(command);

        if (!result)
            return NotFound();

        return Ok();
    }
}
