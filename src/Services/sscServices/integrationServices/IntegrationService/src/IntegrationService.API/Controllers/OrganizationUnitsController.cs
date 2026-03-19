using IntegrationService.Application.DTOs;
using IntegrationService.Application.OrganizationUnits.Commands;
using IntegrationService.Application.OrganizationUnits.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrganizationUnitsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrganizationUnitDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllOrganizationUnitsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrganizationUnitDto>> GetById(string id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetOrganizationUnitByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<OrganizationUnitDto>> Create(
        [FromBody] CreateOrganizationUnitCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.OuId }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<OrganizationUnitDto>> Update(string id,
        [FromBody] UpdateOrganizationUnitCommand command, CancellationToken cancellationToken)
    {
        if (id != command.OuId) return BadRequest("ID mismatch");
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteOrganizationUnitCommand(id), cancellationToken);
        return NoContent();
    }
}
