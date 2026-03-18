using FillingOperationService.Application.FillingLines.Commands.CreateFillingLine;
using FillingOperationService.Application.FillingLines.Queries.GetFillingLines;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FillingOperationService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FillingLinesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetByPlant([FromQuery] int plantId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetFillingLinesQuery(plantId), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateFillingLineCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return Created($"api/fillinglines/{id}", new { id });
    }
}
