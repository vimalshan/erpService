using CategoryAndVendorService.Application.DTOs;
using CategoryAndVendorService.Application.SubCategories.Commands;
using CategoryAndVendorService.Application.SubCategories.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CategoryAndVendorService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SubCategoriesController : ControllerBase
{
    private readonly IMediator _mediator;
    public SubCategoriesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SubCategoryDto>>> GetAll(CancellationToken ct)
        => Ok(await _mediator.Send(new GetAllSubCategoriesQuery(), ct));

    [HttpGet("{id:long}")]
    public async Task<ActionResult<SubCategoryDto>> GetById(long id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetSubCategoryByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("by-main-category/{mainCatId:long}")]
    public async Task<ActionResult<IReadOnlyList<SubCategoryDto>>> GetByMainCategory(long mainCatId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetSubCategoriesByMainCategoryQuery(mainCatId), ct));

    [HttpPost]
    public async Task<ActionResult<SubCategoryDto>> Create([FromBody] CreateSubCategoryCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.SubCatId }, result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<SubCategoryDto>> Update(long id, [FromBody] UpdateSubCategoryCommand command, CancellationToken ct)
    {
        if (id != command.SubCatId) return BadRequest("ID mismatch");
        return Ok(await _mediator.Send(command, ct));
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteSubCategoryCommand(id), ct);
        return result ? NoContent() : NotFound();
    }
}
