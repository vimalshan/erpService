using HealthTransaction.Application.DTOs;
using HealthTransaction.Application.Features.CheckupCards.Commands.Create;
using HealthTransaction.Application.Features.CheckupCards.Queries.GetAll;
using HealthTransaction.Application.Features.CheckupCards.Queries.GetByHlthNum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthTransaction.API.Controllers;

[ApiController]
[Route("api/checkup-cards")]
[Authorize]
public class CheckupCardController : ControllerBase
{
    private readonly IMediator _mediator;
    public CheckupCardController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CheckupCardDto>>> GetAll(CancellationToken ct)
        => Ok(await _mediator.Send(new GetAllCheckupCardsQuery(), ct));

    [HttpGet("{hlthNum}")]
    public async Task<ActionResult<CheckupCardDto>> GetByHlthNum(decimal hlthNum, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetCheckupCardByHlthNumQuery(hlthNum), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CheckupCardDto>> Create([FromBody] CreateCheckupCardDto dto, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateCheckupCardCommand(dto), ct);
        return CreatedAtAction(nameof(GetByHlthNum), new { hlthNum = result.HlthNum }, result);
    }
}
