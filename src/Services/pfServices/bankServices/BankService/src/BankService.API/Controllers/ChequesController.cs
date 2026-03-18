using BankService.Application.Commands.Cheques;
using BankService.Application.DTOs;
using BankService.Application.Queries.Cheques;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChequesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ChequeDetailDto>>> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetAllChequesQuery(), ct));

    [HttpGet("{chequeId:long}")]
    public async Task<ActionResult<ChequeDetailDto>> GetById(long chequeId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetChequeByIdQuery(chequeId), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<IReadOnlyList<ChequeDetailDto>>> GetByStatus(string status, CancellationToken ct)
        => Ok(await mediator.Send(new GetChequesByStatusQuery(status), ct));

    [HttpPost]
    public async Task<ActionResult<ChequeDetailDto>> Issue([FromBody] IssueChequeCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { chequeId = result.ChequeId }, result);
    }

    [HttpPut("{chequeId:long}/clear")]
    public async Task<IActionResult> Clear(long chequeId, [FromBody] DateTime clearedDate, CancellationToken ct)
    {
        var result = await mediator.Send(new ClearChequeCommand(chequeId, clearedDate), ct);
        return result ? NoContent() : NotFound();
    }
}
