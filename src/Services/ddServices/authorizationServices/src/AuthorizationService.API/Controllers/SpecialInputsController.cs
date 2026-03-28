using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthorizationService.Application.Commands;
using AuthorizationService.Application.DTOs;
using AuthorizationService.Application.Queries;

namespace AuthorizationService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SpecialInputsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<SpecialInputsController> _logger;

    public SpecialInputsController(IMediator mediator, ILogger<SpecialInputsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<long>> CreateSpecialInput(CreateSpecialInputCommand command)
    {
        try
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAllSpecialInputs), null, id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating special input");
            return BadRequest("Failed to create special input");
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SpecialInputDto>>> GetAllSpecialInputs()
    {
        var query = new GetAllSpecialInputsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
