using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaxService.Application.Commands;
using TaxService.Application.DTOs;
using TaxService.Application.Queries;

namespace TaxService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ConditionalMastersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ConditionalMastersController> _logger;

    public ConditionalMastersController(IMediator mediator, ILogger<ConditionalMastersController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get conditional master by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ConditionalMasterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ConditionalMasterDto>> GetById(long id)
    {
        var query = new GetConditionalMasterByIdQuery(id);
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return NotFound(new { message = result.Error });

        return Ok(result.Data);
    }

    /// <summary>
    /// Get conditional master by payee ID
    /// </summary>
    [HttpGet("payee/{payeeId}")]
    [ProducesResponseType(typeof(ConditionalMasterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ConditionalMasterDto>> GetByPayeeId(
        string payeeId,
        [FromQuery] int? financialYear = null)
    {
        var query = new GetConditionalMasterByPayeeIdQuery(payeeId, financialYear);
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return NotFound(new { message = result.Error });

        return Ok(result.Data);
    }

    /// <summary>
    /// Get all active conditional masters
    /// </summary>
    [HttpGet("active")]
    [ProducesResponseType(typeof(IEnumerable<ConditionalMasterDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ConditionalMasterDto>>> GetActive([FromQuery] int? financialYear = null)
    {
        var query = new GetActiveConditionalMastersQuery(financialYear);
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return NotFound(new { message = result.Error });

        return Ok(result.Data);
    }

    /// <summary>
    /// Create a new conditional master
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ConditionalMasterDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ConditionalMasterDto>> Create([FromBody] CreateConditionalMasterDto dto)
    {
        var userId = User.FindFirst("sub")?.Value ?? "system";
        var command = new CreateConditionalMasterCommand(dto, userId);

        try
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.Error });

            return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result.Data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating conditional master");
            return BadRequest(new { message = "Error creating conditional master", errors = ex.Message });
        }
    }

    /// <summary>
    /// Add exemption to conditional master
    /// </summary>
    [HttpPost("exemption")]
    [ProducesResponseType(typeof(ConditionalMasterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ConditionalMasterDto>> AddExemption([FromBody] CreateTaxExemptionDto dto)
    {
        var userId = User.FindFirst("sub")?.Value ?? "system";
        var command = new AddExemptionCommand(dto, userId);

        try
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.Error });

            return Ok(result.Data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding exemption");
            return BadRequest(new { message = "Error adding exemption", errors = ex.Message });
        }
    }

    /// <summary>
    /// Add deduction to conditional master
    /// </summary>
    [HttpPost("deduction")]
    [ProducesResponseType(typeof(ConditionalMasterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ConditionalMasterDto>> AddDeduction([FromBody] CreateTaxDeductionDto dto)
    {
        var userId = User.FindFirst("sub")?.Value ?? "system";
        var command = new AddDeductionCommand(dto, userId);

        try
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.Error });

            return Ok(result.Data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding deduction");
            return BadRequest(new { message = "Error adding deduction", errors = ex.Message });
        }
    }
}
