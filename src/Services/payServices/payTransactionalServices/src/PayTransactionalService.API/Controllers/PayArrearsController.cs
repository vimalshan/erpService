using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayTransactionalService.Application.Commands;
using PayTransactionalService.Application.DTOs;
using PayTransactionalService.Application.Queries;

namespace PayTransactionalService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PayArrearsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<PayArrearsController> _logger;

    public PayArrearsController(IMediator mediator, ILogger<PayArrearsController> logger)
    { _mediator = mediator; _logger = logger; }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PayArrearDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PayArrearDto>> GetById(long id)
    {
        var result = await _mediator.Send(new GetPayArrearByIdQuery(id));
        return result.IsSuccess ? Ok(result.Data) : NotFound(new { message = result.Error });
    }

    [HttpGet("employee/{employeeSystemId}")]
    [ProducesResponseType(typeof(IEnumerable<PayArrearDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PayArrearDto>>> GetByEmployee(long employeeSystemId)
    {
        var result = await _mediator.Send(new GetPayArrearsByEmployeeQuery(employeeSystemId));
        return result.IsSuccess ? Ok(result.Data) : NotFound(new { message = result.Error });
    }

    [HttpGet("type/{type}")]
    [ProducesResponseType(typeof(IEnumerable<PayArrearDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PayArrearDto>>> GetByType(string type, [FromQuery] string? monthYear = null)
    {
        var result = await _mediator.Send(new GetPayArrearsByTypeQuery(type, monthYear));
        return result.IsSuccess ? Ok(result.Data) : NotFound(new { message = result.Error });
    }

    [HttpGet("unprocessed/{employeeSystemId}")]
    [ProducesResponseType(typeof(IEnumerable<PayArrearDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PayArrearDto>>> GetUnprocessed(long employeeSystemId)
    {
        var result = await _mediator.Send(new GetUnprocessedArrearsQuery(employeeSystemId));
        return result.IsSuccess ? Ok(result.Data) : NotFound(new { message = result.Error });
    }

    [HttpPost]
    [ProducesResponseType(typeof(PayArrearDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PayArrearDto>> Create([FromBody] CreatePayArrearDto dto)
    {
        var userId = User.FindFirst("sub")?.Value ?? "system";
        var result = await _mediator.Send(new CreatePayArrearCommand(dto, userId));
        if (!result.IsSuccess) return BadRequest(new { message = result.Error });
        return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result.Data);
    }

    [HttpPost("{id}/process")]
    [ProducesResponseType(typeof(PayArrearDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PayArrearDto>> MarkProcessed(long id)
    {
        var result = await _mediator.Send(new MarkArrearProcessedCommand(id));
        return result.IsSuccess ? Ok(result.Data) : BadRequest(new { message = result.Error });
    }
}
