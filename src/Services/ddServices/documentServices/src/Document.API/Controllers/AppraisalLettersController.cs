using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Document.Application.DTOs;
using Document.Application.Features.AppraisalLetters.Commands;
using Document.Application.Features.AppraisalLetters.Queries;

namespace Document.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AppraisalLettersController : ControllerBase
{
    private readonly IMediator _mediator;
    public AppraisalLettersController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AppraisalLetterDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] string? letterType = null, CancellationToken ct = default)
        => Ok(await _mediator.Send(new GetAppraisalLettersQuery(letterType), ct));

    [HttpGet("{serialNo:decimal}")]
    [ProducesResponseType(typeof(AppraisalLetterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(decimal serialNo, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetAppraisalLetterByIdQuery(serialNo), ct);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AppraisalLetterDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateAppraisalLetterRequest req, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new CreateAppraisalLetterCommand(
            req.SerialNo, req.LetterType, req.FromDate, req.EndDate,
            req.Paragraph1, req.Paragraph2, req.EffectiveDate), ct);
        return CreatedAtAction(nameof(GetById), new { serialNo = result.SerialNo }, result);
    }
}
