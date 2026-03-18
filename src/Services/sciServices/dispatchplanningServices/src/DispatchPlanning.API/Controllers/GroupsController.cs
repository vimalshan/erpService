using DispatchPlanning.Application.DTOs;
using DispatchPlanning.Application.Features.DispatchPlans.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DispatchPlanning.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MainGroupsController : ControllerBase
{
    private readonly IMediator _mediator;

    public MainGroupsController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{companyUnitId:int}")]
    [ProducesResponseType(typeof(IEnumerable<MainGroupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(int companyUnitId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllMainGroupsQuery(companyUnitId), ct);
        return Ok(result);
    }

    [HttpGet("{mainGroupId:int}/subgroups")]
    [ProducesResponseType(typeof(IEnumerable<SubGroupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSubGroups(int mainGroupId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetSubGroupsByMainGroupQuery(mainGroupId), ct);
        return Ok(result);
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BreakupItemsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BreakupItemsController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{subGroupId:int}")]
    [ProducesResponseType(typeof(IEnumerable<BreakupItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBySubGroup(int subGroupId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetBreakupItemsBySubGroupQuery(subGroupId), ct);
        return Ok(result);
    }
}
