using MediatR;
using LocationService.Application.Commands.RoomResources;
using LocationService.Application.Queries.RoomResources;
using LocationService.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace LocationService.API.Controllers
{
    /// <summary>
    /// Room Resource API endpoints
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RoomResourcesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<RoomResourcesController> _logger;

        public RoomResourcesController(IMediator mediator, ILogger<RoomResourcesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Get resource by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RoomResourceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            var resource = await _mediator.Send(new GetRoomResourceByIdQuery(id));
            if (resource == null)
                return NotFound();
            return Ok(resource);
        }

        /// <summary>
        /// Get resources by room
        /// </summary>
        [HttpGet("room/{roomId}")]
        [ProducesResponseType(typeof(IReadOnlyList<RoomResourceDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByRoom(long roomId)
        {
            var resources = await _mediator.Send(new GetRoomResourcesByRoomQuery(roomId));
            return Ok(resources);
        }

        /// <summary>
        /// Get resources by location
        /// </summary>
        [HttpGet("location/{locationId}")]
        [ProducesResponseType(typeof(IReadOnlyList<RoomResourceDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByLocation(long locationId)
        {
            var resources = await _mediator.Send(new GetRoomResourcesByLocationQuery(locationId));
            return Ok(resources);
        }

        /// <summary>
        /// Create a new room resource
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(RoomResourceDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateRoomResourceDto dto)
        {
            var command = new CreateRoomResourceCommand
            {
                RoomId = dto.RoomId,
                LocationId = dto.LocationId,
                ResourceCode = dto.ResourceCode,
                ResourceName = dto.ResourceName,
                ResourceType = dto.ResourceType,
                ResourceQuantity = dto.ResourceQuantity,
                UserId = 1 // TODO: Get from current user
            };

            var resource = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = resource.ResourceId }, resource);
        }

        /// <summary>
        /// Update a room resource
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(RoomResourceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateRoomResourceDto dto)
        {
            var command = new UpdateRoomResourceCommand
            {
                ResourceId = id,
                ResourceName = dto.ResourceName,
                ResourceType = dto.ResourceType,
                ResourceQuantity = dto.ResourceQuantity,
                UserId = 1 // TODO: Get from current user
            };

            var resource = await _mediator.Send(command);
            return Ok(resource);
        }

        /// <summary>
        /// Delete a room resource
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            var command = new DeleteRoomResourceCommand { ResourceId = id, UserId = 1 };
            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Get resources by type
        /// </summary>
        [HttpGet("type/{resourceType}")]
        [ProducesResponseType(typeof(IReadOnlyList<RoomResourceDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByType(string resourceType)
        {
            var resources = await _mediator.Send(new GetRoomResourcesByTypeQuery(resourceType));
            return Ok(resources);
        }
    }
}
