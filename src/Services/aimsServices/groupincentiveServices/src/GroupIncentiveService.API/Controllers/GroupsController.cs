using GroupIncentiveService.Application.Commands.CreateGroupMaster;
using GroupIncentiveService.Application.Commands.UpdateGroupMaster;
using GroupIncentiveService.Application.Commands.AddEmployeeToGroup;
using GroupIncentiveService.Application.Queries.GetGroupById;
using GroupIncentiveService.Application.Queries.GetAllGroups;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroupIncentiveService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class GroupsController : ControllerBase
{
    private readonly IMediator _mediator;

    public GroupsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Gets all groups.</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] bool activeOnly = true, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetAllGroupsQuery(activeOnly), ct);
        return Ok(result);
    }

    /// <summary>Gets a group by ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetGroupByIdQuery(id), ct);
        return Ok(result);
    }

    /// <summary>Creates a new group.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateGroupMasterCommand command, CancellationToken ct)
    {
        var id = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { GroupId = id });
    }

    /// <summary>Adds an employee to a group.</summary>
    [HttpPost("{groupId:int}/employees")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddEmployee(int groupId,
        [FromBody] AddEmployeeToGroupCommand command, CancellationToken ct)
    {
        var mappingId = await _mediator.Send(command with { GroupId = groupId }, ct);
        return CreatedAtAction(nameof(GetById), new { id = groupId }, new { MappingId = mappingId });
    }
}
