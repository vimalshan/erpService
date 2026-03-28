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
public class PeriodsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<PeriodsController> _logger;

    public PeriodsController(IMediator mediator, ILogger<PeriodsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<long>> CreatePeriod(CreatePeriodCommand command)
    {
        try
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAllPeriods), null, id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating period");
            return BadRequest("Failed to create period");
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SaaPeriodDto>>> GetAllPeriods()
    {
        var result = await _mediator.Send(new GetAllPeriodsQuery());
        return Ok(result);
    }
}
