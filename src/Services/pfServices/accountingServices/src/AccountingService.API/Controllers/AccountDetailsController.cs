using AccountingService.Application.Features.AccountDetails.Commands.CreateAccountDetail;
using AccountingService.Application.Features.AccountDetails.Queries.GetAccountDetails;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountingService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccountDetailsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountDetailsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get account details by trust code with optional date range</summary>
    [HttpGet("{trustCode}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByTrustCode(
        string trustCode,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        CancellationToken ct)
        => Ok(await _mediator.Send(new GetAccountDetailsQuery(trustCode, from, to), ct));

    /// <summary>Create a new account detail entry</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateAccountDetailCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetByTrustCode), new { trustCode = result.AcTrustCode }, result);
    }
}
