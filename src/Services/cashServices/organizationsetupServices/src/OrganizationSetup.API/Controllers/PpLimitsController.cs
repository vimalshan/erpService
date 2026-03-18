using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizationSetup.Application.DTOs;
using OrganizationSetup.Application.PpLimits.Commands;
using OrganizationSetup.Application.PpLimits.Queries;

namespace OrganizationSetup.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PpLimitsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PpLimitsController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{limitId}")]
    public async Task<ActionResult<PpLimitDto>> GetById(long limitId, CancellationToken ct) =>
        Ok(await _mediator.Send(new GetPpLimitByIdQuery(limitId), ct));

    [HttpGet("org/{orgId}/year/{finYear}")]
    public async Task<ActionResult<IEnumerable<PpLimitDto>>> GetByOrgAndYear(long orgId, int finYear, CancellationToken ct) =>
        Ok(await _mediator.Send(new GetPpLimitsByOrgAndYearQuery(orgId, finYear), ct));

    [HttpPost]
    public async Task<ActionResult<PpLimitDto>> Create([FromBody] CreatePpLimitCommand command, CancellationToken ct) =>
        Ok(await _mediator.Send(command, ct));

    [HttpPut]
    public async Task<ActionResult<PpLimitDto>> Update([FromBody] UpdatePpLimitCommand command, CancellationToken ct) =>
        Ok(await _mediator.Send(command, ct));

    [HttpPost("{limitId}/certificate")]
    public async Task<ActionResult<string>> UploadCertificate(long limitId, IFormFile file, CancellationToken ct)
    {
        if (file.Length == 0) return BadRequest("File is empty");
        using var stream = file.OpenReadStream();
        var command = new UploadPpCertificateCommand(limitId, stream, file.FileName);
        var result = await _mediator.Send(command, ct);
        return Ok(new { url = result });
    }
}
