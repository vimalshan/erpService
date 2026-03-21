using ExpenseService.Application.Commands;
using ExpenseService.Application.DTOs;
using ExpenseService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SettlementsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SettlementsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Settle expenses for a travel request
    /// </summary>
    [HttpPost("{requestNumber}")]
    [ProducesResponseType(typeof(SettlementResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Settle(long requestNumber)
    {
        var result = await _mediator.Send(new SettleExpensesCommand { RequestNumber = requestNumber });
        return Ok(result);
    }

    /// <summary>
    /// Get settlement reports for a request
    /// </summary>
    [HttpGet("reports/{requestNumber}")]
    [ProducesResponseType(typeof(IReadOnlyList<SettlementDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReports(long requestNumber)
    {
        var result = await _mediator.Send(new GetSettlementReportsQuery { RequestNumber = requestNumber });
        return Ok(result);
    }
}
