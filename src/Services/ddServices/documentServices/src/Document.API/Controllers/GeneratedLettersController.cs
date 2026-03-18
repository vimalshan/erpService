using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Document.Application.DTOs;
using Document.Application.Features.GeneratedLetters.Commands;
using Document.Application.Features.LetterLog.Commands;

namespace Document.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GeneratedLettersController : ControllerBase
{
    private readonly IMediator _mediator;
    public GeneratedLettersController(IMediator mediator) => _mediator = mediator;

    [HttpPost("generate")]
    [ProducesResponseType(typeof(GeneratedLetterDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Generate([FromBody] GenerateLetterRequest req, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GenerateLetterCommand(
            null, req.EmployeePin, req.EmployeeName, req.LetterType,
            req.EffectiveDate, req.FinalRating, req.SignatoryName,
            req.SignatoryDesignation, req.AppraisalBasicPay, req.AppraisalFlexiPay), ct);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpPost("log-open")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LetterLogHistoryDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> LogOpen([FromBody] LogLetterOpenRequest req, CancellationToken ct = default)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? req.IpAddress;
        var result = await _mediator.Send(new LogLetterOpenCommand(
            req.LogSysId, ip, req.EmployeeSysId, req.LetterType, req.FinancialYearId), ct);
        return StatusCode(StatusCodes.Status201Created, result);
    }
}
