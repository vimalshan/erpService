using MediatR;
using MedicineManagement.Application.DTOs;
using MedicineManagement.Application.Features.Medicines.Commands;
using MedicineManagement.Application.Features.Medicines.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicineManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MedicinesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<MedicineDto>>> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetAllMedicinesQuery(), ct));

    [HttpGet("{medicineCode}")]
    [AllowAnonymous]
    public async Task<ActionResult<MedicineDto>> GetByCode(string medicineCode, CancellationToken ct)
    {
        var result = await mediator.Send(new GetMedicineByCodeQuery(medicineCode), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<MedicineDto>>> Search([FromQuery] string name, CancellationToken ct)
        => Ok(await mediator.Send(new SearchMedicinesQuery(name), ct));

    [HttpGet("low-stock")]
    public async Task<ActionResult<IReadOnlyList<StockSummaryDto>>> GetLowStock(CancellationToken ct)
        => Ok(await mediator.Send(new GetLowStockMedicinesQuery(), ct));

    [HttpPost]
    public async Task<ActionResult<MedicineDto>> Create([FromBody] CreateMedicineDto dto, CancellationToken ct)
    {
        var user = User.Identity?.Name ?? "API";
        var result = await mediator.Send(new CreateMedicineCommand(
            dto.MedicineCode, dto.MedicineName, dto.MedicineTypeCode,
            dto.Category, dto.OrderLevelMin, dto.OrderLevelMax, user, null), ct);
        return CreatedAtAction(nameof(GetByCode), new { medicineCode = result.MedicineCode }, result);
    }

    [HttpPut("{medicineCode}")]
    public async Task<ActionResult<MedicineDto>> Update(string medicineCode, [FromBody] UpdateMedicineDto dto, CancellationToken ct)
    {
        var user = User.Identity?.Name ?? "API";
        return Ok(await mediator.Send(new UpdateMedicineCommand(
            medicineCode, dto.MedicineName, dto.MedicineTypeCode,
            dto.Category, dto.OrderLevelMin, dto.OrderLevelMax, user, null), ct));
    }

    [HttpDelete("{medicineCode}")]
    public async Task<IActionResult> Delete(string medicineCode, CancellationToken ct)
    {
        await mediator.Send(new DeleteMedicineCommand(medicineCode), ct);
        return NoContent();
    }
}
