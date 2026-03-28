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
public class RecommendsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<RecommendsController> _logger;

    public RecommendsController(IMediator mediator, ILogger<RecommendsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<long>> CreateRecommend(CreateRecommendCommand command)
    {
        try
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetRecommend), new { id }, id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating recommendation");
            return BadRequest("Failed to create recommendation");
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SaaRecommendDto>> GetRecommend(long id)
    {
        var result = await _mediator.Send(new GetRecommendByIdQuery { Id = id });
        if (result == null)
            return NotFound($"Recommendation with ID {id} not found");
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SaaRecommendDto>>> GetAllRecommends()
    {
        var result = await _mediator.Send(new GetAllRecommendsQuery());
        return Ok(result);
    }

    [HttpGet("period/{periodId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SaaRecommendDto>>> GetRecommendsByPeriod(long periodId)
    {
        var result = await _mediator.Send(new GetRecommendsByPeriodQuery { PeriodId = periodId });
        return Ok(result);
    }

    [HttpGet("employee/{empSysId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SaaRecommendDto>>> GetRecommendsByEmployee(long empSysId)
    {
        var result = await _mediator.Send(new GetRecommendsByEmployeeQuery { EmpSysId = empSysId });
        return Ok(result);
    }

    [HttpPut("{id}/submit")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<bool>> SubmitRecommend(long id, [FromBody] SubmitRecommendCommand command)
    {
        command.RecommendId = id;
        var result = await _mediator.Send(command);
        if (!result)
            return BadRequest("Failed to submit recommendation");
        return Ok(result);
    }

    [HttpPut("{id}/reject")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<bool>> RejectRecommend(long id, [FromBody] RejectRecommendCommand command)
    {
        command.RecommendId = id;
        var result = await _mediator.Send(command);
        if (!result)
            return BadRequest("Failed to reject recommendation");
        return Ok(result);
    }
}
