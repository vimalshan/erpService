using AuditService.Application.Commands.GoodPractices;
using AuditService.Application.DTOs;
using AuditService.Application.Queries.GoodPractices;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class GoodPracticeController : ControllerBase
{
    private readonly ISender _sender;

    public GoodPracticeController(ISender sender) => _sender = sender;

    /// <summary>Returns all good practices.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GoodPracticeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetAllGoodPracticesQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>Returns good practice by ID.</summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(GoodPracticeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetGoodPracticeByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Returns good practices by unit.</summary>
    [HttpGet("unit/{unitId:long}")]
    [ProducesResponseType(typeof(IEnumerable<GoodPracticeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUnit(long unitId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetGoodPracticesByUnitQuery(unitId), cancellationToken);
        return Ok(result);
    }

    /// <summary>Creates a new good practice.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(GoodPracticeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateGoodPracticeRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateGoodPracticeCommand(
            request.PracticeId, request.PracticeTitle, request.PracticeDescription,
            request.PracticeBenefits, request.PracticeRemarks, request.PracticeProcess,
            request.PracticeEmpSysId, request.PracticeUnit, request.CreatedBy);

        var result = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.PracticeId }, result);
    }

    /// <summary>Rates a good practice (1–5).</summary>
    [HttpPost("{id:long}/rate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Rate(long id, [FromBody] RateGoodPracticeRequest request, CancellationToken cancellationToken)
    {
        var command = new RateGoodPracticeCommand(id, request.RatingId, request.RatedBy, request.Rating);
        var success = await _sender.Send(command, cancellationToken);
        return success ? NoContent() : NotFound();
    }
}
