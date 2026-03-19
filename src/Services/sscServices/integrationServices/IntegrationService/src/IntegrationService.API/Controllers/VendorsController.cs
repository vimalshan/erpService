using IntegrationService.Application.DTOs;
using IntegrationService.Application.Vendors.Commands;
using IntegrationService.Application.Vendors.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VendorsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VendorDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllVendorsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<VendorDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetVendorByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("{id:int}/with-sites")]
    public async Task<ActionResult<VendorDto>> GetWithSites(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetVendorWithSitesQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<VendorDto>> Create(
        [FromBody] CreateVendorCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.VendorId }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<VendorDto>> Update(int id,
        [FromBody] UpdateVendorCommand command, CancellationToken cancellationToken)
    {
        if (id != command.VendorId) return BadRequest("ID mismatch");
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteVendorCommand(id), cancellationToken);
        return NoContent();
    }
}
