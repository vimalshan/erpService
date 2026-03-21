using LookupService.Application.Commands;
using LookupService.Application.DTOs;
using LookupService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LookupService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LovTypesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LovTypeMasterDto>>> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetAllLovTypesQuery(), ct));

    [HttpGet("{typeCode}")]
    public async Task<ActionResult<LovTypeMasterDto>> GetByCode(string typeCode, CancellationToken ct)
    {
        var result = await mediator.Send(new GetLovTypeByCodeQuery(typeCode), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<string>> Create([FromBody] CreateLovTypeCommand command, CancellationToken ct)
    {
        var code = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetByCode), new { typeCode = code }, code);
    }

    [HttpPut("{typeCode}")]
    public async Task<IActionResult> Update(string typeCode, [FromBody] UpdateLovTypeCommand command, CancellationToken ct)
    {
        if (typeCode != command.LovTypeCode) return BadRequest();
        return await mediator.Send(command, ct) ? NoContent() : NotFound();
    }

    [HttpDelete("{typeCode}")]
    public async Task<IActionResult> Delete(string typeCode, CancellationToken ct)
        => await mediator.Send(new DeleteLovTypeCommand(typeCode), ct) ? NoContent() : NotFound();
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LovsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LovMasterDto>>> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetAllLovsQuery(), ct));

    [HttpGet("{lovId:long}")]
    public async Task<ActionResult<LovMasterDto>> GetById(long lovId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetLovByIdQuery(lovId), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("type/{lovType}")]
    public async Task<ActionResult<IEnumerable<LovMasterDto>>> GetByType(string lovType, CancellationToken ct)
        => Ok(await mediator.Send(new GetLovsByTypeQuery(lovType), ct));

    [HttpPost]
    public async Task<ActionResult<long>> Create([FromBody] CreateLovCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { lovId = id }, id);
    }

    [HttpPut("{lovId:long}")]
    public async Task<IActionResult> Update(long lovId, [FromBody] UpdateLovCommand command, CancellationToken ct)
    {
        if (lovId != command.LovId) return BadRequest();
        return await mediator.Send(command, ct) ? NoContent() : NotFound();
    }

    [HttpDelete("{lovId:long}")]
    public async Task<IActionResult> Delete(long lovId, CancellationToken ct)
        => await mediator.Send(new DeleteLovCommand(lovId), ct) ? NoContent() : NotFound();

    [HttpPost("{lovId}/map-unit")]
    public async Task<ActionResult<decimal>> MapToUnit(long lovId, [FromBody] MapLovToUnitCommand command, CancellationToken ct)
    {
        var mapId = await mediator.Send(command, ct);
        return Created($"api/lovs/{lovId}/unit-maps/{mapId}", mapId);
    }

    [HttpGet("{lovId}/unit-maps")]
    public async Task<ActionResult<IEnumerable<LovUnitMapDto>>> GetUnitMaps(long lovId, CancellationToken ct)
        => Ok(await mediator.Send(new GetLovUnitMapsByLovIdQuery(lovId), ct));
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProcessesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProcessMasterDto>>> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetAllProcessesQuery(), ct));

    [HttpGet("{processId}")]
    public async Task<ActionResult<ProcessMasterDto>> GetById(decimal processId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetProcessByIdQuery(processId), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<decimal>> Create([FromBody] CreateProcessCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { processId = id }, id);
    }

    [HttpPut("{processId}")]
    public async Task<IActionResult> Update(decimal processId, [FromBody] UpdateProcessCommand command, CancellationToken ct)
    {
        if (processId != command.ProcessId) return BadRequest();
        return await mediator.Send(command, ct) ? NoContent() : NotFound();
    }

    [HttpDelete("{processId}")]
    public async Task<IActionResult> Delete(decimal processId, CancellationToken ct)
        => await mediator.Send(new DeleteProcessCommand(processId), ct) ? NoContent() : NotFound();

    [HttpPost("map-unit")]
    public async Task<ActionResult<decimal>> MapUnit([FromBody] MapUnitProcessCommand command, CancellationToken ct)
    {
        var mapId = await mediator.Send(command, ct);
        return Created($"api/processes/unit-maps/{mapId}", mapId);
    }

    [HttpGet("unit/{unitCode}")]
    public async Task<ActionResult<IEnumerable<UnitProcessMapDto>>> GetByUnit(string unitCode, CancellationToken ct)
        => Ok(await mediator.Send(new GetUnitProcessesByUnitCodeQuery(unitCode), ct));
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PanelsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PanelMasterDto>>> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetAllPanelsQuery(), ct));

    [HttpGet("{panelId}")]
    public async Task<ActionResult<PanelMasterDto>> GetById(decimal panelId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetPanelByIdQuery(panelId), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<decimal>> Create([FromBody] CreatePanelCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { panelId = id }, id);
    }

    [HttpPut("{panelId}")]
    public async Task<IActionResult> Update(decimal panelId, [FromBody] UpdatePanelCommand command, CancellationToken ct)
    {
        if (panelId != command.PanelId) return BadRequest();
        return await mediator.Send(command, ct) ? NoContent() : NotFound();
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccessMastersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UnitLovAccessMasterDto>>> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetAllAccessMastersQuery(), ct));

    [HttpGet("{accessMastId}")]
    public async Task<ActionResult<UnitLovAccessMasterDto>> GetById(decimal accessMastId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetAccessMasterByIdQuery(accessMastId), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<decimal>> Create([FromBody] CreateAccessMasterCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { accessMastId = id }, id);
    }
}
