using AccountingService.Application.Features.MainAccounts.Commands.CreateMainAccount;
using AccountingService.Application.Features.MainAccounts.Queries.GetMainAccounts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountingService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MainAccountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public MainAccountsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get all main accounts (chart of accounts)</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _mediator.Send(new GetMainAccountsQuery(), ct));

    /// <summary>Create a new main account</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateMainAccountCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetAll), new { code = result.MainAccountCode }, result);
    }
}
