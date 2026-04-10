using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SparshTransactional.Application.Commands;
using SparshTransactional.Application.DTOs;
using SparshTransactional.Application.Queries;

namespace SparshTransactional.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ApplicationsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ScholarshipApplicationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllApplicationsQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ScholarshipApplicationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetApplicationByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("status/{status}")]
    [ProducesResponseType(typeof(IReadOnlyList<ScholarshipApplicationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByStatus(string status, CancellationToken ct)
    {
        var result = await mediator.Send(new GetApplicationsByStatusQuery(status), ct);
        return Ok(result);
    }

    [HttpGet("student/{studentId:long}")]
    [ProducesResponseType(typeof(IReadOnlyList<ScholarshipApplicationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByStudent(long studentId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetApplicationsByStudentQuery(studentId), ct);
        return Ok(result);
    }

    [HttpGet("scholarship/{scholarshipId:long}")]
    [ProducesResponseType(typeof(IReadOnlyList<ScholarshipApplicationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByScholarship(long scholarshipId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetApplicationsByScholarshipQuery(scholarshipId), ct);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ScholarshipApplicationDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Submit([FromBody] SubmitApplicationCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.ApplicationId }, result);
    }

    [HttpPost("{id:long}/approve")]
    [Authorize(Roles = "Admin,Approver")]
    [ProducesResponseType(typeof(ScholarshipApplicationDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Approve(long id, [FromBody] ApproveApplicationCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command with { ApplicationId = id }, ct);
        return Ok(result);
    }

    [HttpPost("{id:long}/reject")]
    [Authorize(Roles = "Admin,Approver")]
    [ProducesResponseType(typeof(ScholarshipApplicationDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Reject(long id, [FromBody] RejectApplicationCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command with { ApplicationId = id }, ct);
        return Ok(result);
    }
}
