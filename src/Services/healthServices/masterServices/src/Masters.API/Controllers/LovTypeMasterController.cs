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
public class LovTypeMasterController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<LovTypeMasterController> _logger;

    public LovTypeMasterController(IMediator mediator, ILogger<LovTypeMasterController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all LOV Type Masters
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<LovTypeMasterDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<LovTypeMasterDto>>> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllLovTypeMastersQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get LOV Type Master by ID
    /// </summary>
    [HttpGet("{lovTypeCode}")]
    [ProducesResponseType(typeof(LovTypeMasterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LovTypeMasterDto>> GetById(string lovTypeCode, CancellationToken cancellationToken)
    {
        var query = new GetLovTypeMasterByIdQuery(lovTypeCode);
        var result = await _mediator.Send(query, cancellationToken);
        
        if (result == null)
            return NotFound();
        
        return Ok(result);
    }

    /// <summary>
    /// Create a new LOV Type Master
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(LovTypeMasterDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LovTypeMasterDto>> Create([FromBody] CreateLovTypeMasterDto dto, CancellationToken cancellationToken)
    {
        var command = new CreateLovTypeMasterCommand(dto.LovTypeCode, dto.LovTypeName);
        var result = await _mediator.Send(command, cancellationToken);
        
        return CreatedAtAction(nameof(GetById), new { lovTypeCode = result.LovTypeCode }, result);
    }

    /// <summary>
    /// Update an existing LOV Type Master
    /// </summary>
    [HttpPut("{lovTypeCode}")]
    [ProducesResponseType(typeof(LovTypeMasterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LovTypeMasterDto>> Update(string lovTypeCode, [FromBody] UpdateLovTypeMasterDto dto, CancellationToken cancellationToken)
    {
        var command = new UpdateLovTypeMasterCommand(lovTypeCode, dto.LovTypeName);
        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }

    /// <summary>
    /// Delete a LOV Type Master
    /// </summary>
    [HttpDelete("{lovTypeCode}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string lovTypeCode, CancellationToken cancellationToken)
    {
        var command = new DeleteLovTypeMasterCommand(lovTypeCode);
        var result = await _mediator.Send(command, cancellationToken);
        
        if (!result)
            return NotFound();
        
        return NoContent();
    }
}
