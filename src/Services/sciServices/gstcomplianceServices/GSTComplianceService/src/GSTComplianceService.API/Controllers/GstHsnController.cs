using GSTComplianceService.Application.Common.DTOs;
using GSTComplianceService.Application.Features.HsnDetails.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GSTComplianceService.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/gst/{gstId:long}/hsn")]
[Produces("application/json")]
public class GstHsnController : ControllerBase
{
    private readonly IMediator _mediator;

    public GstHsnController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(long gstId, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetHsnDetailsByGstIdQuery(gstId), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Add(long gstId, [FromBody] AddHsnDetailCommand command, CancellationToken ct = default)
    {
        if (gstId != command.GstId) return BadRequest("GST ID mismatch.");
        var id = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetAll), new { gstId }, id);
    }
}
