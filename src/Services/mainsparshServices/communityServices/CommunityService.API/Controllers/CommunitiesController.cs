namespace CommunityService.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Commands;
using Application.Queries;
using Application.DTOs;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CommunitiesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CommunitiesController> _logger;

    public CommunitiesController(IMediator mediator, ILogger<CommunitiesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommunityDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CommunityDto>> GetCommunity(long id)
    {
        _logger.LogInformation("Getting community with ID: {CommunityId}", id);
        var result = await _mediator.Send(new GetCommunityByIdQuery(id));
        
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CommunityListDto>))]
    public async Task<ActionResult<IEnumerable<CommunityListDto>>> GetAllCommunities(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("Getting all communities - Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);
        var result = await _mediator.Send(new GetAllCommunitiesQuery(pageNumber, pageSize));
        return Ok(result);
    }

    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CommunityListDto>))]
    public async Task<ActionResult<IEnumerable<CommunityListDto>>> SearchCommunities(
        [FromQuery] string searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("Searching communities with term: {SearchTerm}", searchTerm);
        var result = await _mediator.Send(new SearchCommunitiesQuery(searchTerm, pageNumber, pageSize));
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CommunityDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CommunityDto>> CreateCommunity([FromBody] CreateCommunityDto dto)
    {
        _logger.LogInformation("Creating new community: {CommunityCode}", dto.CommunityCode);
        var result = await _mediator.Send(new CreateCommunityCommand(dto));
        return CreatedAtAction(nameof(GetCommunity), new { id = result.CommunityId }, result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommunityDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CommunityDto>> UpdateCommunity(long id, [FromBody] UpdateCommunityDto dto)
    {
        var updateDto = dto with { CommunityId = id };
        _logger.LogInformation("Updating community with ID: {CommunityId}", id);
        var result = await _mediator.Send(new UpdateCommunityCommand(updateDto));
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ArchiveCommunity(long id)
    {
        _logger.LogInformation("Archiving community with ID: {CommunityId}", id);
        var result = await _mediator.Send(new ArchiveCommunityCommand(id));
        
        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpPost("{communityId}/members")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CommunityMemberDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CommunityMemberDto>> AddMember(
        long communityId,
        [FromBody] AddMemberDto dto)
    {
        var addDto = dto with { CommunityId = communityId };
        _logger.LogInformation("Adding member {UserId} to community {CommunityId}", dto.UserId, communityId);
        var result = await _mediator.Send(new AddCommunityMemberCommand(addDto));
        return CreatedAtAction(nameof(GetCommunityMembers), new { communityId }, result);
    }

    [HttpGet("{communityId}/members")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CommunityMemberDto>))]
    public async Task<ActionResult<IEnumerable<CommunityMemberDto>>> GetCommunityMembers(long communityId)
    {
        _logger.LogInformation("Getting members for community {CommunityId}", communityId);
        var result = await _mediator.Send(new GetCommunityMembersQuery(communityId));
        return Ok(result);
    }

    [HttpDelete("{communityId}/members/{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveMember(long communityId, long userId)
    {
        _logger.LogInformation("Removing member {UserId} from community {CommunityId}", userId, communityId);
        var result = await _mediator.Send(new RemoveCommunityMemberCommand(
            new Application.DTOs.RemoveMemberDto(communityId, userId)));
        
        if (!result)
            return NotFound();

        return NoContent();
    }
}
