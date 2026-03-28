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
public class LevelsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<LevelsController> _logger;

    public LevelsController(IMediator mediator, ILogger<LevelsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<long>> CreateLevel(CreateLevelCommand command)
    {
        try
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAllLevels), null, id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating level");
            return BadRequest("Failed to create level");
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SaaLevelDto>>> GetAllLevels()
    {
        var result = await _mediator.Send(new GetAllLevelsQuery());
        return Ok(result);
    }
}
