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
public class SubmitsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<SubmitsController> _logger;

    public SubmitsController(IMediator mediator, ILogger<SubmitsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<long>> CreateSubmit(CreateSubmitCommand command)
    {
        try
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAllSubmits), null, id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating submit");
            return BadRequest("Failed to create submit");
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SaaSubmitDto>>> GetAllSubmits()
    {
        var result = await _mediator.Send(new GetAllSubmitsQuery());
        return Ok(result);
    }
}
