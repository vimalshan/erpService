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
public class MailTriggersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<MailTriggersController> _logger;

    public MailTriggersController(IMediator mediator, ILogger<MailTriggersController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<long>> CreateMailTrigger(CreateMailTriggerCommand command)
    {
        try
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAllMailTriggers), null, id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating mail trigger");
            return BadRequest("Failed to create mail trigger");
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SaaMailTriggerDto>>> GetAllMailTriggers()
    {
        var result = await _mediator.Send(new GetAllMailTriggersQuery());
        return Ok(result);
    }
}
