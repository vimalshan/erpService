using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupplierService.Application.DTOs;
using SupplierService.Application.Features.Suppliers.Commands;
using SupplierService.Application.Features.Suppliers.Queries;

namespace SupplierService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SuppliersController : ControllerBase
{
    private readonly IMediator _mediator;

    public SuppliersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<SupplierDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllSuppliersQuery());
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<SupplierDto>> GetById(int id)
    {
        var result = await _mediator.Send(new GetSupplierByIdQuery(id));
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpGet("paged")]
    [AllowAnonymous]
    public async Task<ActionResult<PagedResultDto<SupplierDto>>> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
    {
        var result = await _mediator.Send(new GetSuppliersPagedQuery(page, pageSize, search));
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<SupplierDto>> Create([FromBody] CreateSupplierDto dto)
    {
        var result = await _mediator.Send(new CreateSupplierCommand(dto));
        return CreatedAtAction(nameof(GetById), new { id = result.SupplierId }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<SupplierDto>> Update(int id, [FromBody] UpdateSupplierDto dto)
    {
        var result = await _mediator.Send(new UpdateSupplierCommand(id, dto));
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteSupplierCommand(id));
        return NoContent();
    }

    [HttpPut("{id:int}/activate")]
    public async Task<IActionResult> Activate(int id)
    {
        await _mediator.Send(new ActivateSupplierCommand(id));
        return NoContent();
    }

    [HttpPut("{id:int}/deactivate")]
    public async Task<IActionResult> Deactivate(int id)
    {
        await _mediator.Send(new DeactivateSupplierCommand(id));
        return NoContent();
    }
}
