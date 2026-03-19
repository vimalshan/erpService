using CategoryAndVendorService.Application.DTOs;
using CategoryAndVendorService.Application.MainCategories.Commands;
using CategoryAndVendorService.Application.MainCategories.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CategoryAndVendorService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MainCategoriesController : ControllerBase
{
    private readonly IMediator _mediator;
    public MainCategoriesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MainCategoryDto>>> GetAll(CancellationToken ct)
        => Ok(await _mediator.Send(new GetAllMainCategoriesQuery(), ct));

    [HttpGet("{id:long}")]
    public async Task<ActionResult<MainCategoryDto>> GetById(long id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetMainCategoryByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<MainCategoryDto>> Create([FromBody] CreateMainCategoryCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.MainCatId }, result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<MainCategoryDto>> Update(long id, [FromBody] UpdateMainCategoryCommand command, CancellationToken ct)
    {
        if (id != command.MainCatId) return BadRequest("ID mismatch");
        return Ok(await _mediator.Send(command, ct));
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteMainCategoryCommand(id), ct);
        return result ? NoContent() : NotFound();
    }
}
