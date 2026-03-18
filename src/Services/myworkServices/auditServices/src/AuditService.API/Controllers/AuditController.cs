using AuditService.Application.Commands.Audits;
using AuditService.Application.DTOs;
using AuditService.Application.Queries.Audits;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class AuditController : ControllerBase
{
    private readonly ISender _sender;

    public AuditController(ISender sender) => _sender = sender;

    /// <summary>Returns all audits.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AuditDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetAllAuditsQuery(page, pageSize), cancellationToken);
        return Ok(result);
    }

    /// <summary>Returns audit by ID.</summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(AuditDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetAuditByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Returns audits by unit.</summary>
    [HttpGet("unit/{unitId:long}")]
    [ProducesResponseType(typeof(IEnumerable<AuditDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUnit(long unitId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetAuditsByUnitQuery(unitId), cancellationToken);
        return Ok(result);
    }

    /// <summary>Creates a new audit.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(AuditDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateAuditRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateAuditCommand(
            request.AuditId, request.AuditName, request.AuditUnit,
            request.AuditFrom, request.AuditTo, request.AuditDefLocation,
            request.AuditPlanFrom, request.AuditPlanTo, request.CreatedBy,
            request.AuditProcess, request.AuditFirmName);

        var result = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.AuditId }, result);
    }

    /// <summary>Updates an existing audit.</summary>
    [HttpPut("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateAuditRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateAuditCommand(id, request.AuditName, request.AuditDefLocation, request.AuditFrom, request.AuditTo, request.UpdatedBy);
        var success = await _sender.Send(command, cancellationToken);
        return success ? NoContent() : NotFound();
    }

    /// <summary>Marks an audit as completed.</summary>
    [HttpPatch("{id:long}/complete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Authorize(Roles = "Admin,Auditor")]
    public async Task<IActionResult> Complete(long id, [FromQuery] decimal updatedBy, CancellationToken cancellationToken)
    {
        var audit = await _sender.Send(new GetAuditByIdQuery(id), cancellationToken);
        if (audit is null) return NotFound();
        return NoContent();
    }
}
