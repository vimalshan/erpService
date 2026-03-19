using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionManagement.Application.Commands.ProductionPlans;
using ProductionManagement.Application.DTOs;
using ProductionManagement.Application.Queries.ProductionPlans;

namespace ProductionManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductionPlansController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductionPlansController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ProductionPlanDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllProductionPlansQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("plant/{plantId:int}")]
    [ProducesResponseType(typeof(IReadOnlyList<ProductionPlanDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByPlantId(int plantId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProductionPlansByPlantIdQuery(plantId), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{plantId:int}/{itemId:int}")]
    [ProducesResponseType(typeof(ProductionPlanDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int plantId, int itemId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProductionPlanByIdQuery(plantId, itemId), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProductionPlanDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateProductionPlanDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateProductionPlanCommand(dto), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { plantId = result.ProductionPlantId, itemId = result.SciItemId }, result);
    }

    [HttpPut]
    [ProducesResponseType(typeof(ProductionPlanDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromBody] UpdateProductionPlanDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateProductionPlanCommand(dto), cancellationToken);
        return Ok(result);
    }

    [HttpPost("{plantId:int}/{itemId:int}/close")]
    [ProducesResponseType(typeof(ProductionPlanDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Close(int plantId, int itemId, [FromQuery] int modifiedBy, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CloseProductionPlanCommand(plantId, itemId, modifiedBy), cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{plantId:int}/{itemId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int plantId, int itemId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteProductionPlanCommand(plantId, itemId), cancellationToken);
        return NoContent();
    }
}
