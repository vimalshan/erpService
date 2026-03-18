using MediatR;
using MedicineManagement.Application.DTOs;
using MedicineManagement.Application.Features.MedicineTypes.Commands;
using MedicineManagement.Application.Features.MedicineTypes.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicineManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MedicineTypesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<MedicineTypeDto>>> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetAllMedicineTypesQuery(), ct));

    [HttpGet("{typeCode}")]
    [AllowAnonymous]
    public async Task<ActionResult<MedicineTypeDto>> GetByCode(string typeCode, CancellationToken ct)
    {
        var result = await mediator.Send(new GetMedicineTypeByCodeQuery(typeCode), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<MedicineTypeDto>> Create([FromBody] CreateMedicineTypeDto dto, CancellationToken ct)
    {
        var user = User.Identity?.Name ?? "API";
        var result = await mediator.Send(new CreateMedicineTypeCommand(dto.TypeCode, dto.TypeName, user, null), ct);
        return CreatedAtAction(nameof(GetByCode), new { typeCode = result.TypeCode }, result);
    }

    [HttpPut("{typeCode}")]
    public async Task<ActionResult<MedicineTypeDto>> Update(string typeCode, [FromBody] UpdateMedicineTypeDto dto, CancellationToken ct)
    {
        var user = User.Identity?.Name ?? "API";
        return Ok(await mediator.Send(new UpdateMedicineTypeCommand(typeCode, dto.TypeName, user, null), ct));
    }

    [HttpDelete("{typeCode}")]
    public async Task<IActionResult> Delete(string typeCode, CancellationToken ct)
    {
        await mediator.Send(new DeleteMedicineTypeCommand(typeCode), ct);
        return NoContent();
    }
}
