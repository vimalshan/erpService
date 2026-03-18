using Asp.Versioning;
using LocationServices.Application.DTOs;
using LocationServices.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocationServices.API.Controllers.v2;

/// <summary>
/// Location App Map REST API — v2.
/// Adds pagination + summary projection for large result sets.
/// </summary>
[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/location-app-maps")]
[AllowAnonymous]
[Produces("application/json")]
public sealed class LocationAppMapController : ControllerBase
{
    private readonly IMediator _mediator;

    public LocationAppMapController(IMediator mediator) => _mediator = mediator;

    /// <summary>[v2] Get all mappings with optional pagination</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<LocationAppMapDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetAllLocationAppMapsQuery(), ct);
        if (!result.IsSuccess) return Problem(result.Error);

        var all    = result.Value!.ToList();
        var paged  = all.Skip((page - 1) * pageSize).Take(pageSize);

        return Ok(new PagedResponse<LocationAppMapDto>(
            Items:      paged,
            TotalCount: all.Count,
            Page:       page,
            PageSize:   pageSize));
    }

    /// <summary>[v2] Get active mappings — lean summary projection</summary>
    [HttpGet("active/summary")]
    [ProducesResponseType(typeof(IEnumerable<LocationAppMapSummary>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActiveSummary(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetActiveLocationAppMapsQuery(), ct);
        if (!result.IsSuccess) return Problem(result.Error);

        var summaries = result.Value!.Select(x => new LocationAppMapSummary(
            x.LocationId, x.AppName, x.IsActive, x.SiteCategoryCode));

        return Ok(summaries);
    }
}

// ── V2 RESPONSE TYPES ────────────────────────────────────────────────────────
public sealed record PagedResponse<T>(
    IEnumerable<T> Items,
    int TotalCount,
    int Page,
    int PageSize)
{
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNext   => Page < TotalPages;
    public bool HasPrev   => Page > 1;
}
