using CompensationBenefits.Application.Features.SalaryStructures;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompensationBenefits.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SalaryStructuresController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetAllSalaryStructuresQuery(), ct));

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetSalaryStructureByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSalaryStructureCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateSalaryStructureCommand command, CancellationToken ct)
    {
        if (id != command.StructureId) return BadRequest("ID mismatch.");
        var success = await mediator.Send(command, ct);
        return success ? NoContent() : NotFound();
    }
}
