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
public class ApplicationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ApplicationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all applications
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ApplicationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllApplications()
    {
        var query = new GetAllApplicationsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Get application by ID
    /// </summary>
    [HttpGet("{applicationNumber}")]
    [ProducesResponseType(typeof(ApplicationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetApplicationById(decimal applicationNumber)
    {
        var query = new GetApplicationByIdQuery { ApplicationNumber = applicationNumber };
        var result = await _mediator.Send(query);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Get applications by job ID
    /// </summary>
    [HttpGet("job/{jobId}")]
    [ProducesResponseType(typeof(IEnumerable<ApplicationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetApplicationsByJobId(decimal jobId)
    {
        var query = new GetApplicationsByJobIdQuery { JobId = jobId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Get applications by Sparsh ID
    /// </summary>
    [HttpGet("sparsh/{sparshId}")]
    [ProducesResponseType(typeof(IEnumerable<ApplicationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetApplicationsBySparshId(string sparshId)
    {
        var query = new GetApplicationsBySparshIdQuery { SparshId = sparshId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Create a new application
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateApplication([FromBody] CreateApplicationDto applicationDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var command = new CreateApplicationCommand { ApplicationData = applicationDto };
        var result = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetApplicationById), new { applicationNumber = result }, result);
    }

    /// <summary>
    /// Update an existing application
    /// </summary>
    [HttpPut("{applicationNumber}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateApplication(decimal applicationNumber, [FromBody] UpdateApplicationDto applicationDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        applicationDto.ApplicationNumber = applicationNumber;
        var command = new UpdateApplicationCommand { ApplicationData = applicationDto };
        var result = await _mediator.Send(command);

        if (!result)
            return NotFound();

        return Ok();
    }

    /// <summary>
    /// Change application status
    /// </summary>
    [HttpPatch("{applicationNumber}/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeApplicationStatus(decimal applicationNumber, [FromBody] ChangeApplicationStatusRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var command = new ChangeApplicationStatusCommand
        {
            ApplicationNumber = applicationNumber,
            Status = request.Status,
            Remark = request.Remark,
            UpdatedBy = User.Identity?.Name ?? "System"
        };

        var result = await _mediator.Send(command);

        if (!result)
            return NotFound();

        return Ok();
    }

    /// <summary>
    /// Set application marks
    /// </summary>
    [HttpPut("{applicationNumber}/marks")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SetApplicationMarks(decimal applicationNumber, [FromBody] SetMarksRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var command = new SetApplicationMarksCommand
        {
            ApplicationNumber = applicationNumber,
            CrtMarks = request.CrtMarks,
            DomainMarks = request.DomainMarks
        };

        var result = await _mediator.Send(command);

        if (!result)
            return NotFound();

        return Ok();
    }
}

public class ChangeApplicationStatusRequest
{
    public string Status { get; set; }
    public string Remark { get; set; }
}

public class SetMarksRequest
{
    public decimal CrtMarks { get; set; }
    public decimal DomainMarks { get; set; }
}
