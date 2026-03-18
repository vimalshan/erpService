using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Masters.Application.Commands;
using Masters.Application.Queries;
using Masters.Application.DTOs;

namespace Masters.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LovMasterController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<LovMasterController> _logger;

    public LovMasterController(IMediator mediator, ILogger<LovMasterController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all LOV Masters
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<LovMasterDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<LovMasterDto>>> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllLovMastersQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get LOV Masters by Type
    /// </summary>
    [HttpGet("type/{lovType}")]
    [ProducesResponseType(typeof(IEnumerable<LovMasterDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<LovMasterDto>>> GetByType(string lovType, CancellationToken cancellationToken)
    {
        var query = new GetLovMastersByTypeQuery(lovType);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get LOV Master by ID
    /// </summary>
    [HttpGet("{lovId}")]
    [ProducesResponseType(typeof(LovMasterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LovMasterDto>> GetById(long lovId, CancellationToken cancellationToken)
    {
        var query = new GetLovMasterByIdQuery(lovId);
        var result = await _mediator.Send(query, cancellationToken);
        
        if (result == null)
            return NotFound();
        
        return Ok(result);
    }

    /// <summary>
    /// Create a new LOV Master
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(LovMasterDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LovMasterDto>> Create([FromBody] CreateLovMasterDto dto, CancellationToken cancellationToken)
    {
        var command = new CreateLovMasterCommand(dto.LovId, dto.LovType, dto.LovName);
        var result = await _mediator.Send(command, cancellationToken);
        
        return CreatedAtAction(nameof(GetById), new { lovId = result.LovId }, result);
    }

    /// <summary>
    /// Update an existing LOV Master
    /// </summary>
    [HttpPut("{lovId}")]
    [ProducesResponseType(typeof(LovMasterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LovMasterDto>> Update(long lovId, [FromBody] UpdateLovMasterDto dto, CancellationToken cancellationToken)
    {
        var command = new UpdateLovMasterCommand(lovId, dto.LovName);
        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }

    /// <summary>
    /// Delete a LOV Master
    /// </summary>
    [HttpDelete("{lovId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long lovId, CancellationToken cancellationToken)
    {
        var command = new DeleteLovMasterCommand(lovId);
        var result = await _mediator.Send(command, cancellationToken);
        
        if (!result)
            return NotFound();
        
        return NoContent();
    }
}
