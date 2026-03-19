using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionManagement.Application.Commands.Norms;
using ProductionManagement.Application.DTOs;
using ProductionManagement.Application.Queries.Norms;

namespace ProductionManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NormsController : ControllerBase
{
    private readonly IMediator _mediator;

    public NormsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<NormsMainDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllNormsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{normNo:long}")]
    [ProducesResponseType(typeof(NormsMainDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long normNo, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetNormByIdQuery(normNo), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(NormsMainDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateNormsMainDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateNormsMainCommand(dto), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { normNo = result.NormNo }, result);
    }

    [HttpPost("{normNo:long}/close")]
    [ProducesResponseType(typeof(NormsMainDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Close(long normNo, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CloseNormsMainCommand(normNo), cancellationToken);
        return Ok(result);
    }

    [HttpPost("master")]
    [ProducesResponseType(typeof(NormsMasterDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> AddMaster([FromBody] CreateNormsMasterDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new AddNormsMasterCommand(dto), cancellationToken);
        return Created(string.Empty, result);
    }
}
