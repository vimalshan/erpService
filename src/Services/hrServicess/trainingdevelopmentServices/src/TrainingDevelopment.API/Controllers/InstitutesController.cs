using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainingDevelopment.Application.DTOs;
using TrainingDevelopment.Application.Features.Institutes.Commands;
using TrainingDevelopment.Application.Features.Institutes.Queries;

namespace TrainingDevelopment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class InstitutesController : ControllerBase
{
    private readonly ISender _sender;

    public InstitutesController(ISender sender) => _sender = sender;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<InstituteMasterDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetInstituteListQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{code:decimal}")]
    [ProducesResponseType(typeof(InstituteMasterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByCode(decimal code, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetInstituteByCodeQuery(code), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(InstituteMasterDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateInstituteCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetByCode), new { code = result.InstituteCode }, result);
    }

    [HttpDelete("{code:decimal}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(decimal code, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteInstituteCommand(code), cancellationToken);
        return NoContent();
    }
}
