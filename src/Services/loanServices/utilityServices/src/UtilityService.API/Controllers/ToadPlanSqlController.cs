using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UtilityService.Application.Commands.CreateToadPlanSql;
using UtilityService.Application.Commands.DeleteToadPlanSql;
using UtilityService.Application.Commands.UpdateToadPlanSql;
using UtilityService.Application.DTOs;
using UtilityService.Application.Queries.GetAllToadPlanSql;
using UtilityService.Application.Queries.GetToadPlanSqlById;
using UtilityService.Application.Queries.GetToadPlanSqlByUser;

namespace UtilityService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
[Produces("application/json")]
public class ToadPlanSqlController : ControllerBase
{
    private readonly IMediator _mediator;

    public ToadPlanSqlController(IMediator mediator) => _mediator = mediator;

    /// <summary>Gets a paginated list of all TOAD plan SQL entries.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDto<ToadPlanSqlDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetAllToadPlanSqlQuery(pageNumber, pageSize), cancellationToken);
        return Ok(result);
    }

    /// <summary>Gets a single TOAD plan SQL entry by ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ToadPlanSqlDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetToadPlanSqlByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Gets TOAD plan SQL entries by username.</summary>
    [HttpGet("user/{username}")]
    [ProducesResponseType(typeof(IEnumerable<ToadPlanSqlDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUser(string username, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetToadPlanSqlByUserQuery(username), cancellationToken);
        return Ok(result);
    }

    /// <summary>Creates a new TOAD plan SQL entry.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ToadPlanSqlDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateToadPlanSqlDto dto, CancellationToken cancellationToken)
    {
        var command = new CreateToadPlanSqlCommand(dto.Username, dto.StatementId, dto.Statement, dto.Timestamp);
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Updates an existing TOAD plan SQL entry.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateToadPlanSqlDto dto, CancellationToken cancellationToken)
    {
        var updated = await _mediator.Send(new UpdateToadPlanSqlCommand(id, dto.Username, dto.Statement, dto.Timestamp), cancellationToken);
        return updated ? NoContent() : NotFound();
    }

    /// <summary>Soft-deletes a TOAD plan SQL entry.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _mediator.Send(new DeleteToadPlanSqlCommand(id), cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
