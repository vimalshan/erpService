using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using GroupManagementService.Application.Commands;
using GroupManagementService.Application.DTOs;

namespace GroupManagementService.API.Controllers
{
    /// <summary>
    /// REST API endpoints for Group Menu Management
    /// </summary>
    [ApiController]
    [Route("api/v1/groups/{groupId}/[controller]")]
    [Authorize]
    public class MenuMapsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<MenuMapsController> _logger;

        public MenuMapsController(IMediator mediator, ILogger<MenuMapsController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Add menu mapping to a group
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(GroupMenuMapDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GroupMenuMapDto>> AddMenuMap(long groupId, AddMenuMapRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var command = new AddMenuMapCommand(groupId, request.MenuCode, request.MenuName, 
                    request.Permissions, request.CreatedBy, request.MenuSequence);
                var result = await _mediator.Send(command, cancellationToken);
                return CreatedAtAction(nameof(AddMenuMap), new { groupId }, result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error adding menu map: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding menu map to group {GroupId}", groupId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding the menu map");
            }
        }

        /// <summary>
        /// Remove menu mapping from a group
        /// </summary>
        [HttpDelete("{menuCode}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RemoveMenuMap(long groupId, string menuCode, [FromBody] long updatedBy, CancellationToken cancellationToken)
        {
            try
            {
                await _mediator.Send(new RemoveMenuMapCommand(groupId, menuCode, updatedBy), cancellationToken);
                return Ok(new { message = "Menu removed from group successfully" });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error removing menu map: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing menu {MenuCode} from group {GroupId}", menuCode, groupId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while removing the menu map");
            }
        }

        /// <summary>
        /// Update menu permissions for a group
        /// </summary>
        [HttpPut("{menuCode}/permissions")]
        [ProducesResponseType(typeof(GroupMenuMapDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GroupMenuMapDto>> UpdateMenuPermissions(long groupId, string menuCode, 
            UpdateMenuPermissionsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var command = new UpdateMenuPermissionsCommand(groupId, menuCode, request.Permissions, request.UpdatedBy);
                var result = await _mediator.Send(command, cancellationToken);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error updating menu permissions: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating permissions for menu {MenuCode} in group {GroupId}", menuCode, groupId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating menu permissions");
            }
        }
    }
}
