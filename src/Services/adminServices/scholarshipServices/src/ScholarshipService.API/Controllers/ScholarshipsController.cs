using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScholarshipService.Application.Commands.ApproveScholarship;
using ScholarshipService.Application.Commands.CreateScholarship;
using ScholarshipService.Application.Commands.StopScholarship;
using ScholarshipService.Application.Common;
using ScholarshipService.Application.DTOs;
using ScholarshipService.Application.Queries.GetScholarshipById;
using ScholarshipService.Application.Queries.GetScholarships;

namespace ScholarshipService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public sealed class ScholarshipsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ScholarshipsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get all scholarships (paged).</summary>
    [HttpGet]
    [ProducesResponseType(typeof(BaseResponse<PagedResult<ScholarshipMainDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetScholarshipsQuery(null, page, pageSize), ct);
        return Ok(BaseResponse<PagedResult<ScholarshipMainDto>>.Ok(result));
    }

    /// <summary>Get scholarship by ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BaseResponse<ScholarshipMainDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetScholarshipByIdQuery(id), ct);
        return result is not null
            ? Ok(BaseResponse<ScholarshipMainDto>.Ok(result))
            : NotFound(BaseResponse<ScholarshipMainDto>.Fail($"Scholarship {id} not found."));
    }

    /// <summary>Get scholarships for a specific employee.</summary>
    [HttpGet("employee/{employeeId:int}")]
    [ProducesResponseType(typeof(BaseResponse<PagedResult<ScholarshipMainDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmployee(
        int employeeId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetScholarshipsQuery(employeeId, page, pageSize), ct);
        return Ok(BaseResponse<PagedResult<ScholarshipMainDto>>.Ok(result));
    }

    /// <summary>Submit a new scholarship application.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(BaseResponse<int>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateScholarshipCommand command,
        CancellationToken ct = default)
    {
        var id = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id },
            BaseResponse<int>.Ok(id, "Scholarship application submitted successfully."));
    }

    /// <summary>Approve a scholarship application.</summary>
    [HttpPut("{id:int}/approve")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Approve(
        int id,
        [FromBody] ApproveScholarshipRequest request,
        CancellationToken ct = default)
    {
        await _mediator.Send(new ApproveScholarshipCommand(id, request.ApprovedBy, request.Remarks), ct);
        return Ok(BaseResponse<bool>.Ok(true, "Scholarship approved successfully."));
    }

    /// <summary>Stop an active scholarship.</summary>
    [HttpPut("{id:int}/stop")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Stop(
        int id,
        [FromBody] StopScholarshipRequest request,
        CancellationToken ct = default)
    {
        await _mediator.Send(new StopScholarshipCommand(id, request.Reason, request.StoppedBy), ct);
        return Ok(BaseResponse<bool>.Ok(true, "Scholarship stopped successfully."));
    }
}

public record ApproveScholarshipRequest(int ApprovedBy, string? Remarks = null);
public record StopScholarshipRequest(string Reason, int StoppedBy);
