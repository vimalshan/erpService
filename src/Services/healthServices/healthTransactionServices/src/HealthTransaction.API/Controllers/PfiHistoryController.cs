using HealthTransaction.Application.DTOs;
using HealthTransaction.Application.Features.PfiHistories.Commands.Create;
using HealthTransaction.Application.Features.PfiHistories.Queries.GetByHlthNum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthTransaction.API.Controllers;

[ApiController]
[Route("api/pfi-history")]
[Authorize]
public class PfiHistoryController : ControllerBase
{
    private readonly IMediator _mediator;
    public PfiHistoryController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{hlthNum}")]
    public async Task<ActionResult<IReadOnlyList<PfiHistoryDto>>> GetByHlthNum(decimal hlthNum, CancellationToken ct)
        => Ok(await _mediator.Send(new GetPfiHistoriesByHlthNumQuery(hlthNum), ct));

    [HttpPost]
    public async Task<ActionResult<PfiHistoryDto>> Create([FromBody] CreatePfiHistoryDto dto, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreatePfiHistoryCommand(dto), ct);
        return Ok(result);
    }
}
