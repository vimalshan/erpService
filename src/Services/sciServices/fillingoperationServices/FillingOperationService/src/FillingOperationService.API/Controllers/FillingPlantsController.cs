using FillingOperationService.Application.FillingPlants.Commands.CreateFillingPlant;
using FillingOperationService.Application.FillingPlants.Commands.UpdateFillingPlant;
using FillingOperationService.Application.FillingPlants.Queries.GetFillingPlantById;
using FillingOperationService.Application.FillingPlants.Queries.GetFillingPlants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FillingOperationService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FillingPlantsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? companyUnitId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetFillingPlantsQuery(companyUnitId), ct);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetFillingPlantByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateFillingPlantCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateFillingPlantCommand command, CancellationToken ct)
    {
        if (id != command.FillingPlantId) return BadRequest("ID mismatch.");
        await mediator.Send(command, ct);
        return NoContent();
    }
}
