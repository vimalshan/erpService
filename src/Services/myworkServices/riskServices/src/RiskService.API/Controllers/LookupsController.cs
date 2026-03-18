using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RiskService.Application.DTOs;
using RiskService.Application.Queries.RiskType;

namespace RiskService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LookupsController(IMediator mediator) : ControllerBase
{
    [HttpGet("risk-types")]
    public async Task<ActionResult<IReadOnlyList<RiskTypeDto>>> GetRiskTypes(CancellationToken ct) =>
        Ok(await mediator.Send(new GetAllRiskTypesQuery(), ct));

    [HttpGet("impacts")]
    public async Task<ActionResult<IReadOnlyList<RiskImpactDto>>> GetImpacts(CancellationToken ct) =>
        Ok(await mediator.Send(new GetAllRiskImpactsQuery(), ct));

    [HttpGet("probabilities")]
    public async Task<ActionResult<IReadOnlyList<RiskProbabilityDto>>> GetProbabilities(CancellationToken ct) =>
        Ok(await mediator.Send(new GetAllRiskProbabilitiesQuery(), ct));

    [HttpGet("ratings")]
    public async Task<ActionResult<IReadOnlyList<RiskRatingDto>>> GetRatings(CancellationToken ct) =>
        Ok(await mediator.Send(new GetAllRiskRatingsQuery(), ct));

    [HttpGet("responses")]
    public async Task<ActionResult<IReadOnlyList<RiskResponseDto>>> GetResponses(CancellationToken ct) =>
        Ok(await mediator.Send(new GetAllRiskResponsesQuery(), ct));
}
