using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionManagement.Application.Commands.ProductionPlants;
using ProductionManagement.Application.DTOs;
using ProductionManagement.Application.Queries.ProductionPlants;

namespace ProductionManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductionPlantsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductionPlantsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ProductionPlantDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllProductionPlantsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProductionPlantDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProductionPlantByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProductionPlantDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProductionPlantDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateProductionPlantCommand(dto), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.ProductionPlantId }, result);
    }

    [HttpPut]
    [ProducesResponseType(typeof(ProductionPlantDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromBody] UpdateProductionPlantDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateProductionPlantCommand(dto), cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteProductionPlantCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpPost("{plantId:int}/products")]
    [ProducesResponseType(typeof(ProductionPlantProductMapDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> MapProduct(int plantId, [FromBody] CreateProductionPlantProductMapDto dto, CancellationToken cancellationToken)
    {
        var command = new MapProductToPlantCommand(dto with { ProductionPlantId = plantId });
        var result = await _mediator.Send(command, cancellationToken);
        return Created(string.Empty, result);
    }
}
