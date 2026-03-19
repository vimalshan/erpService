using MamAllocationService.Application.Commands;
using MamAllocationService.Application.DTOs;
using MamAllocationService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MamAllocationService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AllocationsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AllocationDetailDto>>> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllAllocationsQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{date:datetime}/{rmCode:int}")]
    public async Task<ActionResult<AllocationDetailDto>> GetById(DateTime date, int rmCode, CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllocationByIdQuery(date, rmCode), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("by-date/{date:datetime}")]
    public async Task<ActionResult<IEnumerable<AllocationDetailDto>>> GetByDate(DateTime date, CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllocationsByDateQuery(date), ct);
        return Ok(result);
    }

    [HttpGet("summary/{date:datetime}/{rmCode:int}")]
    public async Task<ActionResult<AllocationSummaryDto>> GetSummary(DateTime date, int rmCode, CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllocationSummaryQuery(date, rmCode), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<AllocationDetailDto>> Create([FromBody] AllocationDetailDto dto, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateAllocationDetailCommand(dto), ct);
        return CreatedAtAction(nameof(GetById), new { date = result.AllDate, rmCode = result.AllRm }, result);
    }

    [HttpPut("{date:datetime}/{rmCode:int}")]
    public async Task<ActionResult<AllocationDetailDto>> Update(DateTime date, int rmCode, [FromBody] AllocationDetailDto dto, CancellationToken ct)
    {
        var result = await mediator.Send(new UpdateAllocationDetailCommand(date, rmCode, dto), ct);
        return Ok(result);
    }

    [HttpDelete("{date:datetime}/{rmCode:int}")]
    public async Task<IActionResult> Delete(DateTime date, int rmCode, CancellationToken ct)
    {
        var deleted = await mediator.Send(new DeleteAllocationDetailCommand(date, rmCode), ct);
        return deleted ? NoContent() : NotFound();
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ArrivalsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ArrivalDetailDto>>> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllArrivalsQuery(), ct);
        return Ok(result);
    }

    [HttpGet("by-item/{itemCode:int}")]
    public async Task<ActionResult<IEnumerable<ArrivalDetailDto>>> GetByItem(int itemCode, CancellationToken ct)
    {
        var result = await mediator.Send(new GetArrivalsByItemQuery(itemCode), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ArrivalDetailDto>> Create([FromBody] ArrivalDetailDto dto, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateArrivalDetailCommand(dto), ct);
        return Created(string.Empty, result);
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ConsumptionsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ConsumptionDetailDto>>> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllConsumptionsQuery(), ct);
        return Ok(result);
    }

    [HttpGet("by-rm/{rmCode:int}")]
    public async Task<ActionResult<IEnumerable<ConsumptionDetailDto>>> GetByRm(int rmCode, CancellationToken ct)
    {
        var result = await mediator.Send(new GetConsumptionsByRmQuery(rmCode), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ConsumptionDetailDto>> Create([FromBody] ConsumptionDetailDto dto, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateConsumptionDetailCommand(dto), ct);
        return Created(string.Empty, result);
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DispatchesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DispatchDetailDto>>> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllDispatchesQuery(), ct);
        return Ok(result);
    }

    [HttpGet("by-fg/{fgCode:int}")]
    public async Task<ActionResult<IEnumerable<DispatchDetailDto>>> GetByFg(int fgCode, CancellationToken ct)
    {
        var result = await mediator.Send(new GetDispatchesByFgQuery(fgCode), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<DispatchDetailDto>> Create([FromBody] DispatchDetailDto dto, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateDispatchDetailCommand(dto), ct);
        return Created(string.Empty, result);
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductAllocationsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductAllocationDto>>> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllProductAllocationsQuery(), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ProductAllocationDto>> Create([FromBody] ProductAllocationDto dto, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateProductAllocationCommand(dto), ct);
        return Created(string.Empty, result);
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FgAllocationsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FgAllocationDto>>> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllFgAllocationsQuery(), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<FgAllocationDto>> Create([FromBody] FgAllocationDto dto, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateFgAllocationCommand(dto), ct);
        return Created(string.Empty, result);
    }
}
