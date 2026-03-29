using HealthTransaction.Application.DTOs;
using HealthTransaction.Application.Features.DynamicHealthDetails.Commands.Save;
using HealthTransaction.Application.Features.DynamicHealthDetails.Queries.GetByHlthNum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthTransaction.API.Controllers;

[ApiController]
[Route("api/dynamic-health")]
[Authorize]
public class DynamicHealthDetailController : ControllerBase
{
    private readonly IMediator _mediator;
    public DynamicHealthDetailController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{hlthNum}")]
    public async Task<ActionResult<IReadOnlyList<DynamicHealthDetailDto>>> GetByHlthNum(decimal hlthNum, CancellationToken ct)
        => Ok(await _mediator.Send(new GetDynamicHealthDetailsByHlthNumQuery(hlthNum), ct));

    [HttpPost]
    public async Task<ActionResult<IList<DynamicHealthDetailDto>>> Save([FromBody] IList<SaveDynamicHealthDetailDto> items, CancellationToken ct)
        => Ok(await _mediator.Send(new SaveDynamicHealthDetailsCommand(items), ct));
}
