using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScholarshipService.Application.Common;
using ScholarshipService.Application.DTOs;
using ScholarshipService.Application.Queries.GetScholarshipAmounts;

namespace ScholarshipService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public sealed class ScholarshipAmountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ScholarshipAmountsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get all scholarship amount configurations.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(BaseResponse<IEnumerable<ScholarshipAmountDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetScholarshipAmountsQuery(), ct);
        return Ok(BaseResponse<IEnumerable<ScholarshipAmountDto>>.Ok(result));
    }
}
