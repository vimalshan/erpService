using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizationStructureService.Application.Commands;
using OrganizationStructureService.Application.DTOs;
using OrganizationStructureService.Application.Queries;
using OrganizationStructureService.Domain.Exceptions;

namespace OrganizationStructureService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class BusinessController : ControllerBase
{
    private readonly IMediator _mediator;
    public BusinessController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<BusinessDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await _mediator.Send(new GetAllBusinessesQuery(), ct));

    [HttpGet("active")]
    [ProducesResponseType(typeof(IReadOnlyList<BusinessDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActive(CancellationToken ct) =>
        Ok(await _mediator.Send(new GetActiveBusinessesQuery(), ct));

    [HttpGet("{id:decimal}")]
    [ProducesResponseType(typeof(BusinessDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(decimal id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetBusinessByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(BusinessDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateBusinessCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.BusinessId }, result);
    }

    [HttpPut("{id:decimal}")]
    [ProducesResponseType(typeof(BusinessDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(decimal id, [FromBody] UpdateBusinessCommand command, CancellationToken ct)
    {
        if (id != command.BusinessId) return BadRequest("ID mismatch.");
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpDelete("{id:decimal}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(decimal id, [FromQuery] decimal updatedBy, CancellationToken ct)
    {
        await _mediator.Send(new DeactivateBusinessCommand(id, updatedBy), ct);
        return NoContent();
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class UnitController : ControllerBase
{
    private readonly IMediator _mediator;
    public UnitController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await _mediator.Send(new GetAllUnitsQuery(), ct));

    [HttpGet("active")]
    public async Task<IActionResult> GetActive(CancellationToken ct) =>
        Ok(await _mediator.Send(new GetActiveUnitsQuery(), ct));

    [HttpGet("{id:decimal}")]
    public async Task<IActionResult> GetById(decimal id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetUnitByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("business/{businessId:decimal}")]
    public async Task<IActionResult> GetByBusiness(decimal businessId, CancellationToken ct) =>
        Ok(await _mediator.Send(new GetUnitsByBusinessIdQuery(businessId), ct));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUnitCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.UnitId }, result);
    }

    [HttpPut("{id:decimal}")]
    public async Task<IActionResult> Update(decimal id, [FromBody] UpdateUnitCommand command, CancellationToken ct)
    {
        if (id != command.UnitId) return BadRequest("ID mismatch.");
        return Ok(await _mediator.Send(command, ct));
    }

    [HttpDelete("{id:decimal}")]
    public async Task<IActionResult> Deactivate(decimal id, [FromQuery] decimal updatedBy, CancellationToken ct)
    {
        await _mediator.Send(new DeactivateUnitCommand(id, updatedBy), ct);
        return NoContent();
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class GradeController : ControllerBase
{
    private readonly IMediator _mediator;
    public GradeController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await _mediator.Send(new GetAllGradesQuery(), ct));

    [HttpGet("active")]
    public async Task<IActionResult> GetActive(CancellationToken ct) =>
        Ok(await _mediator.Send(new GetActiveGradesQuery(), ct));

    [HttpGet("{id:decimal}")]
    public async Task<IActionResult> GetById(decimal id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetGradeByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGradeCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.GradeId }, result);
    }

    [HttpPut("{id:decimal}")]
    public async Task<IActionResult> Update(decimal id, [FromBody] UpdateGradeCommand command, CancellationToken ct)
    {
        if (id != command.GradeId) return BadRequest("ID mismatch.");
        return Ok(await _mediator.Send(command, ct));
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class PositionController : ControllerBase
{
    private readonly IMediator _mediator;
    public PositionController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await _mediator.Send(new GetAllPositionsQuery(), ct));

    [HttpGet("{id:decimal}")]
    public async Task<IActionResult> GetById(decimal id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetPositionByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("unit/{unitCode}")]
    public async Task<IActionResult> GetByUnit(string unitCode, CancellationToken ct) =>
        Ok(await _mediator.Send(new GetPositionsByUnitCodeQuery(unitCode), ct));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePositionCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.PositionId }, result);
    }

    [HttpPut("{id:decimal}/close")]
    public async Task<IActionResult> Close(decimal id, [FromQuery] DateTime closeDate, [FromQuery] decimal modifiedBy, CancellationToken ct)
    {
        await _mediator.Send(new ClosePositionCommand(id, closeDate, modifiedBy), ct);
        return NoContent();
    }

    [HttpDelete("{id:decimal}")]
    public async Task<IActionResult> Delete(decimal id, [FromQuery] decimal modifiedBy, CancellationToken ct)
    {
        await _mediator.Send(new DeletePositionCommand(id, modifiedBy), ct);
        return NoContent();
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class SiteController : ControllerBase
{
    private readonly IMediator _mediator;
    public SiteController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await _mediator.Send(new GetAllSitesQuery(), ct));

    [HttpGet("{id:decimal}")]
    public async Task<IActionResult> GetById(decimal id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetSiteByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSiteCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.SiteId }, result);
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class DepartmentController : ControllerBase
{
    private readonly IMediator _mediator;
    public DepartmentController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await _mediator.Send(new GetAllDepartmentsQuery(), ct));

    [HttpGet("{id:decimal}")]
    public async Task<IActionResult> GetById(decimal id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetDepartmentByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDepartmentCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.DepartmentId }, result);
    }

    [HttpPut("{id:decimal}")]
    public async Task<IActionResult> Update(decimal id, [FromBody] UpdateDepartmentCommand command, CancellationToken ct)
    {
        if (id != command.DepartmentId) return BadRequest("ID mismatch.");
        return Ok(await _mediator.Send(command, ct));
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class DivisionController : ControllerBase
{
    private readonly IMediator _mediator;
    public DivisionController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await _mediator.Send(new GetAllDivisionsQuery(), ct));

    [HttpGet("{id:decimal}")]
    public async Task<IActionResult> GetById(decimal id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetDivisionByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDivisionCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.DivisionId }, result);
    }
}
