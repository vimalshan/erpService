using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransactionService.Application.Commands;
using TransactionService.Application.DTOs;
using TransactionService.Application.Queries;

namespace TransactionService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DemandsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<DemandsController> _logger;

    public DemandsController(IMediator mediator, ILogger<DemandsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<long>> CreateDemand(CreateDemandCommand command)
    {
        try
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetDemand), new { id }, id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating demand");
            return BadRequest("Failed to create demand");
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DemandMasterDto>> GetDemand(long id)
    {
        var result = await _mediator.Send(new GetDemandByIdQuery { Id = id });
        if (result == null)
            return NotFound($"Demand with ID {id} not found");
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DemandMasterDto>>> GetAllDemands()
    {
        var result = await _mediator.Send(new GetAllDemandsQuery());
        return Ok(result);
    }

    [HttpGet("status/{status}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DemandMasterDto>>> GetDemandsByStatus(char status)
    {
        var result = await _mediator.Send(new GetDemandsByStatusQuery { Status = status });
        return Ok(result);
    }

    [HttpGet("status-count/{status}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GetDemandStatusCount(char status)
    {
        var result = await _mediator.Send(new GetDemandStatusCountQuery { Status = status });
        return Ok(result);
    }

    [HttpPut("{id}/approve")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<bool>> ApproveDemand(long id, [FromBody] ApproveDemandCommand command)
    {
        command.DemandId = id;
        var result = await _mediator.Send(command);
        if (!result)
            return BadRequest("Failed to approve demand");
        return Ok(result);
    }

    [HttpPut("{id}/complete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<bool>> CompleteDemand(long id, [FromBody] CompleteDemandCommand command)
    {
        command.DemandId = id;
        var result = await _mediator.Send(command);
        if (!result)
            return BadRequest("Failed to complete demand");
        return Ok(result);
    }
}
