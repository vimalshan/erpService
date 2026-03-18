using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using GroupManagementService.Application.Commands;
using GroupManagementService.Application.Queries;
using GroupManagementService.Application.DTOs;

namespace GroupManagementService.API.Controllers
{
    /// <summary>
    /// REST API endpoints for Group Management
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class GroupsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<GroupsController> _logger;

        public GroupsController(IMediator mediator, ILogger<GroupsController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get all groups
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GroupDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<GroupDto>>> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                var groups = await _mediator.Send(new GetAllGroupsQuery(), cancellationToken);
                return Ok(groups);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all groups");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving groups");
            }
        }

        /// <summary>
        /// Get group by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GroupDto>> GetById(long id, CancellationToken cancellationToken)
        {
            try
            {
                var group = await _mediator.Send(new GetGroupByIdQuery(id), cancellationToken);
                return Ok(group);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Group not found: {GroupId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving group {GroupId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the group");
            }
        }

        /// <summary>
        /// Get group by code
        /// </summary>
        [HttpGet("code/{code}")]
        [ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GroupDto>> GetByCode(string code, CancellationToken cancellationToken)
        {
            try
            {
                var group = await _mediator.Send(new GetGroupByCodeQuery(code), cancellationToken);
                return Ok(group);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Group not found: {GroupCode}", code);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving group with code {GroupCode}", code);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the group");
            }
        }

        /// <summary>
        /// Create a new group
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(GroupDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GroupDto>> Create(CreateGroupRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var command = new CreateGroupCommand(request.Code, request.Name, request.Description, request.CreatedBy, request.IsAdmin);
                var result = await _mediator.Send(command, cancellationToken);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error creating group: {Code}", request.Code);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating group");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the group");
            }
        }

        /// <summary>
        /// Update a group
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GroupDto>> Update(long id, UpdateGroupRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var command = new UpdateGroupCommand(id, request.Name, request.Description, request.UpdatedBy);
                var result = await _mediator.Send(command, cancellationToken);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Group not found: {GroupId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating group {GroupId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the group");
            }
        }

        /// <summary>
        /// Activate a group
        /// </summary>
        [HttpPost("{id}/activate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Activate(long id, [FromBody] long updatedBy, CancellationToken cancellationToken)
        {
            try
            {
                await _mediator.Send(new ActivateGroupCommand(id, updatedBy), cancellationToken);
                return Ok(new { message = "Group activated successfully" });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Group not found: {GroupId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activating group {GroupId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while activating the group");
            }
        }

        /// <summary>
        /// Deactivate a group
        /// </summary>
        [HttpPost("{id}/deactivate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Deactivate(long id, [FromBody] long updatedBy, CancellationToken cancellationToken)
        {
            try
            {
                await _mediator.Send(new DeactivateGroupCommand(id, updatedBy), cancellationToken);
                return Ok(new { message = "Group deactivated successfully" });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Group not found: {GroupId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deactivating group {GroupId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deactivating the group");
            }
        }

        /// <summary>
        /// Search groups
        /// </summary>
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<GroupDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GroupDto>>> Search(
            [FromQuery] string? searchTerm,
            [FromQuery] string? status,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var query = new SearchGroupsQuery(searchTerm, status, pageNumber, pageSize);
                var result = await _mediator.Send(query, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching groups");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while searching groups");
            }
        }
    }
}
