using EmployeeManagement.Application.Probations.Commands.ReviewProbation;
using EmployeeManagement.Application.Probations.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class ProbationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProbationsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Review an employee probation record.</summary>
    [HttpPut("{id:long}/review")]
    [ProducesResponseType(typeof(ProbationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Review(long id, [FromBody] ReviewProbationCommand command, CancellationToken ct = default)
    {
        if (id != command.ProbationId) return BadRequest("ID mismatch");
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }
}
