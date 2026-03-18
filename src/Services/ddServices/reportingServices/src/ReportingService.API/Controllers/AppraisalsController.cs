using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportingService.Application.Commands;
using ReportingService.Application.DTOs;
using ReportingService.Application.Queries;

namespace ReportingService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AppraisalsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AppraisalsController> _logger;

    public AppraisalsController(IMediator mediator, ILogger<AppraisalsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<long>> CreateAppraisal(CreateAppraisalCommand command)
    {
        try
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAppraisal), new { id }, id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating appraisal");
            return BadRequest("Failed to create appraisal");
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppraisalDto>> GetAppraisal(long id)
    {
        var query = new GetAppraisalByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (result == null)
            return NotFound($"Appraisal with ID {id} not found");

        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AppraisalDto>>> GetAllAppraisals()
    {
        var query = new GetAllApprisalsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPut("{id}/complete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> CompleteAppraisal(long id)
    {
        var command = new CompleteAppraisalCommand { AppraisalId = id };
        var result = await _mediator.Send(command);

        if (!result)
            return NotFound($"Appraisal with ID {id} not found");

        return Ok(true);
    }
}
