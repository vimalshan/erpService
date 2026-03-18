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
public class TaxMarginalDetailsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<TaxMarginalDetailsController> _logger;

    public TaxMarginalDetailsController(IMediator mediator, ILogger<TaxMarginalDetailsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get tax marginal detail by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TaxMarginalDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TaxMarginalDetailDto>> GetById(long id)
    {
        var query = new GetTaxMarginalDetailByIdQuery(id);
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return NotFound(new { message = result.Error });

        return Ok(result.Data);
    }

    /// <summary>
    /// Get tax details for employee and financial year
    /// </summary>
    [HttpGet("employee/{employeeSystemId}/year/{financialYear}")]
    [ProducesResponseType(typeof(TaxMarginalDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaxMarginalDetailDto>> GetByEmployeeAndYear(
        long employeeSystemId,
        int financialYear)
    {
        var query = new GetTaxByEmployeeAndYearQuery(employeeSystemId, financialYear);
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return NotFound(new { message = result.Error });

        return Ok(result.Data);
    }

    /// <summary>
    /// Get all tax details for an employee
    /// </summary>
    [HttpGet("employee/{employeeSystemId}")]
    [ProducesResponseType(typeof(IEnumerable<TaxMarginalDetailDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TaxMarginalDetailDto>>> GetByEmployee(long employeeSystemId)
    {
        var query = new GetEmployeeTaxDetailsQuery(employeeSystemId);
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return NotFound(new { message = result.Error });

        return Ok(result.Data);
    }

    /// <summary>
    /// Create a new tax marginal detail
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(TaxMarginalDetailDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TaxMarginalDetailDto>> Create([FromBody] CreateTaxMarginalDetailDto dto)
    {
        var userId = User.FindFirst("sub")?.Value ?? "system";
        var command = new CreateTaxMarginalDetailCommand(dto, userId);

        try
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.Error });

            return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result.Data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tax marginal detail");
            return BadRequest(new { message = "Error creating tax details", errors = ex.Message });
        }
    }

    /// <summary>
    /// Calculate tax for a marginal detail
    /// </summary>
    [HttpPost("{id}/calculate")]
    [ProducesResponseType(typeof(TaxMarginalDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaxMarginalDetailDto>> CalculateTax(long id)
    {
        var command = new CalculateTaxCommand(id);
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return NotFound(new { message = result.Error });

        return Ok(result.Data);
    }
}
