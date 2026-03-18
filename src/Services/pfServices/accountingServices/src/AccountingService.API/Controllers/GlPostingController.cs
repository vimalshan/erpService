using AccountingService.Application.Features.GlPosting.Commands.PostGlEntry;
using AccountingService.Application.Features.GlPosting.Queries.GetTrialBalance;
using AccountingService.Infrastructure.DapperQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountingService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GlPostingController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly TrialBalanceDapperQuery _dapperQuery;

    public GlPostingController(IMediator mediator, TrialBalanceDapperQuery dapperQuery)
    {
        _mediator = mediator;
        _dapperQuery = dapperQuery;
    }

    /// <summary>Post a GL entry</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] PostGlEntryCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetTrialBalance), result);
    }

    /// <summary>Get trial balance (EF-based)</summary>
    [HttpGet("trial-balance")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTrialBalance(CancellationToken ct)
        => Ok(await _mediator.Send(new GetTrialBalanceQuery(), ct));

    /// <summary>Get trial balance via Dapper (fast read)</summary>
    [HttpGet("trial-balance/dapper")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTrialBalanceDapper(CancellationToken ct)
        => Ok(await _dapperQuery.GetTrialBalanceAsync(ct));

    /// <summary>Get transaction journal via Dapper</summary>
    [HttpGet("journal")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetJournal([FromQuery] string? trustCode, CancellationToken ct)
        => Ok(await _dapperQuery.GetTransactionJournalAsync(trustCode, ct));
}
