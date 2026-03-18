using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Document.Application.DTOs;
using Document.Application.Features.Signatories.Commands;
using Document.Application.Features.Signatories.Queries;

namespace Document.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SignatoriesController : ControllerBase
{
    private readonly IMediator _mediator;
    public SignatoriesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SignatoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] bool activeOnly = true, CancellationToken ct = default)
        => Ok(await _mediator.Send(new GetSignatoriesQuery(activeOnly), ct));

    [HttpGet("{signatoryNumber:decimal}")]
    [ProducesResponseType(typeof(SignatoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(decimal signatoryNumber, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetSignatoryByIdQuery(signatoryNumber), ct);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(SignatoryDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateSignatoryRequest req, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new CreateSignatoryCommand(
            req.SignatoryNumber, req.Name, req.Designation, req.EmployeeSysId, req.ImageFileName), ct);
        return CreatedAtAction(nameof(GetById), new { signatoryNumber = result.SignatoryNumber }, result);
    }

    [HttpPut("{signatoryNumber:decimal}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(decimal signatoryNumber, [FromBody] UpdateSignatoryRequest req, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new UpdateSignatoryCommand(signatoryNumber, req.Name, req.Designation, req.ImageFileName), ct);
        return result ? NoContent() : NotFound();
    }

    [HttpDelete("{signatoryNumber:decimal}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(decimal signatoryNumber, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new DeleteSignatoryCommand(signatoryNumber), ct);
        return result ? NoContent() : NotFound();
    }
}
