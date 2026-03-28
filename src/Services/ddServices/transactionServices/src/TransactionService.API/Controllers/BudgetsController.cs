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
public class BudgetsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<BudgetsController> _logger;

    public BudgetsController(IMediator mediator, ILogger<BudgetsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<long>> CreateBudget(CreateBudgetCommand command)
    {
        try
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAllBudgets), null, id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating budget");
            return BadRequest("Failed to create budget");
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SaaBudgetDto>>> GetAllBudgets()
    {
        var result = await _mediator.Send(new GetAllBudgetsQuery());
        return Ok(result);
    }

    [HttpGet("year/{yearId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SaaBudgetDto>>> GetBudgetsByYear(long yearId)
    {
        var result = await _mediator.Send(new GetBudgetsByYearQuery { YearId = yearId });
        return Ok(result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<bool>> UpdateBudget(long id, [FromBody] UpdateBudgetCommand command)
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        if (!result)
            return BadRequest("Failed to update budget");
        return Ok(result);
    }
}
